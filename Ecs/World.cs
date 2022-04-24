using System.Collections;
using System.Collections.Generic;
using System;

using BlitzEcs.Util;

namespace BlitzEcs {
    public class World : IEntityManager {
        private Dictionary<Type, IComponentPool> componentPoolsByType;
        private List<IComponentPool> allComponentPools;

        // Cached "hot" queries that are frequently used.
        private List<List<Query>> queriesByComponents;
        private Dictionary<Mask, Query> cachedQueriesByMask;

        private MinHeap recycledEntities;
        private int maxEntityCount;

        // Maps the entity id to its component count. Also useful for checking which entities
        // are still alive (if they are not, then the Contains() method will return false).
        private SparseSet<int> entityComponentCounts;
        public int EntityCount => entityComponentCounts.Count;

        private int activePoolLocks;

        public World() {
            componentPoolsByType = new Dictionary<Type, IComponentPool>();
            allComponentPools = new List<IComponentPool>();

            recycledEntities = new MinHeap();
            maxEntityCount = 0;

            entityComponentCounts = new SparseSet<int>();

            queriesByComponents = new List<List<Query>>();
            cachedQueriesByMask = new Dictionary<Mask, Query>();
        }

        public TQuery GetCached<TQuery>(TQuery query) where TQuery : Query {
            if (cachedQueriesByMask.TryGetValue(query.Mask, out Query cachedQuery)) {
                if (cachedQuery is TQuery targetQuery) {
                    return targetQuery;
                }

                // When two queries have the same mask, but different types, like e.g.
                // Query().Inc<C> and Query<C>(), the same query instance can obviously
                // not be reused.
                // Solution: Cache one of the queries and make the other queries
                // with the same mask "mirror" the source.
                // (They share the same matchedEntities sparse set)
                query.MirrorCachedQuery(cachedQuery);
                return query;
            }

            CacheQuery(query);
            return query;
        }

        public void Cache(Query query) {
            if (cachedQueriesByMask.TryGetValue(query.Mask, out Query cachedQuery)) {
                query.MirrorCachedQuery(cachedQuery);
            }
            else {
                CacheQuery(query);
            }
        }

        private void CacheQuery(Query query) {
            // Adds the query to the list of hot queries that are actively updated about
            // component adds / removes, so that they don't have to do a Fetch() on every execution.

            query.OnCache();

            List<int> includes = query.Mask.componentsToInclude;
            List<int> excludes = query.Mask.componentsToExclude;

            foreach (int poolId in includes) {
                while (queriesByComponents.Count <= poolId)
                    queriesByComponents.Add(null);

                if (queriesByComponents[poolId] == null)
                    queriesByComponents[poolId] = new List<Query>();

                queriesByComponents[poolId].Add(query);
            }

            foreach (int poolId in excludes) {
                if (includes.Contains(poolId))
                    continue;

                while (queriesByComponents.Count <= poolId)
                    queriesByComponents.Add(null);

                if (queriesByComponents[poolId] == null)
                    queriesByComponents[poolId] = new List<Query>();

                queriesByComponents[poolId].Add(query);
            }

            cachedQueriesByMask[query.Mask] = query;
        }

        public Entity Spawn() {
            int id = 0;

            if (recycledEntities.Count > 0) {
                id = recycledEntities.Pop();
            }
            else {
                id = maxEntityCount;
                maxEntityCount ++;
            }
            entityComponentCounts.Add(id, 0);
            return new Entity(this, id);
        }

        private void RecycleEntity(int entityId) {
            entityComponentCounts.Remove(entityId);
            recycledEntities.Push(entityId);
        }

        public void Despawn(Entity entity) => Despawn(entity.Id);

        public void Despawn(int entityId) {
            int componentCount = entityComponentCounts.Get(entityId);

            // If the entity has no components, it can be removed instantly.
            if (componentCount == 0) {
                RecycleEntity(entityId);
                return;
            }

            // Removes all components from the entity. When the component count finally
            // hits 0, the entity will be removed.
            // If the pools are locked, the entity will therefore only be deleted once the
            // pools have been unlocked.
            foreach (IComponentPool pool in componentPoolsByType.Values) {
                pool.Remove(entityId);
            }
        }

        public ComponentPool<TComponent> GetComponentPool<TComponent>() where TComponent : struct {
            Type type = typeof(TComponent);
            if (componentPoolsByType.TryGetValue(type, out IComponentPool pool)) {
                return (ComponentPool<TComponent>)pool;
            }

            ComponentPool<TComponent> newPool = new ComponentPool<TComponent>(this, allComponentPools.Count);
            componentPoolsByType[type] = newPool;
            allComponentPools.Add(newPool);
            return newPool;
        }

        public bool TryGetIComponentPool(Type componentType, out IComponentPool pool) {
            return componentPoolsByType.TryGetValue(componentType, out pool);
        }

        public bool TryGetIComponentPool(int poolId, out IComponentPool pool) {
            if (poolId >= allComponentPools.Count) {
                pool = null;
                return false;
            }

            pool = allComponentPools[poolId];
            return true;
        }

        public void SetDestroyHandler<TComponent>(IEcsDestroyHandler<TComponent> destroyHandler)
        where TComponent : struct {
            GetComponentPool<TComponent>().SetDestroyHandler(destroyHandler);
        }

        private void UpdateHotQueriesAfterAddingComponent(int entityId, int poolId) {
            if (poolId >= queriesByComponents.Count)
                return;

            List<Query> pool = queriesByComponents[poolId];
            if (pool == null)
                return;

            foreach (Query query in pool) {
                query.OnAddComponentToEntity(entityId, poolId);
            }
        }

        private void UpdateHotQueriesAfterRemovingComponent(int entityId, int poolId) {
            if (poolId >= queriesByComponents.Count)
                return;

            List<Query> pool = queriesByComponents[poolId];
            if (pool == null)
                return;

            foreach (Query query in pool) {
                query.OnRemoveComponentFromEntity(entityId, poolId);
            }
        }

        public void OnRemoveComponentFromEntity(int entityId, int poolId) {
            // Reduce the component count.
            int count = --entityComponentCounts.Get(entityId);

            // Despawn when the entity has no components left.
            if (count <= 0) {
                RecycleEntity(entityId);
            }

            UpdateHotQueriesAfterRemovingComponent(entityId, poolId);
        }

        public void OnAddComponentToEntity(int entityId, int poolId) {
            entityComponentCounts.Get(entityId) ++;

            // Update the hot queries.
            UpdateHotQueriesAfterAddingComponent(entityId, poolId);
        }

        public int GetComponentCount(Entity entity) => entityComponentCounts.Get(entity.Id);

        public bool IsEntityAlive(Entity entity) => entityComponentCounts.Contains(entity.Id);

        public void LockComponentPools() {
            activePoolLocks ++;
        }

        public void UnlockComponentPools() {
            activePoolLocks --;

            if (activePoolLocks < 0) {
                throw new InvalidOperationException("Invalid component pool lock balance. (Unlocks > locks)");
            }

            if (activePoolLocks == 0) {
                foreach (IComponentPool pool in allComponentPools) {
                    pool.ExecuteBufferedRemoves();
                }
            }
        }

        public bool ArePoolsLocked => activePoolLocks > 0;
    }
}
