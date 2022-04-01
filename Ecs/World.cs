using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ecs {
    public class World : IEntityManager {
        private Dictionary<Type, IComponentPool> componentPoolsByType;
        private List<IComponentPool> allComponentPools;

        // Cached "hot" queries that are frequently used.
        private List<List<Query>> queriesByIncludedComponent;
        private List<List<Query>> queriesByExcludedComponent;
        private Dictionary<Mask, Query> cachedQueriesByMask;

        private Stack<int> recycledEntities;
        private int maxEntityCount;

        // Maps the entity id to its component count. Also useful for checking which entities
        // are still alive (if they are not, then the Contains() method will return false).
        private SparseSet<int> entityComponentCounts;
        public int EntityCount => entityComponentCounts.Count;

        private int activePoolLocks;

        public World() {
            componentPoolsByType = new Dictionary<Type, IComponentPool>();
            allComponentPools = new List<IComponentPool>();

            recycledEntities = new Stack<int>();
            maxEntityCount = 0;

            entityComponentCounts = new SparseSet<int>();

            queriesByIncludedComponent = new List<List<Query>>();
            queriesByExcludedComponent = new List<List<Query>>();
            cachedQueriesByMask = new Dictionary<Mask, Query>();
        }

        public TQuery GetCachedQuery<TQuery>(TQuery query) where TQuery : Query {
            // TODO: Currently does not work for Query().Inc<C> and Query<C>().

            if (cachedQueriesByMask.TryGetValue(query.Mask, out Query cachedQuery)) {
                return (TQuery)cachedQuery;
            }

            CacheQuery(query);
            return query;
        }

        public void CacheQuery(Query query) {
            // Adds the query to the list of hot queries that are actively updated about
            // component adds / removes, so that they don't have to do a Fetch() on every execution.

            query.OnCache();

            List<int> includes = query.Mask.componentsToInclude;
            List<int> excludes = query.Mask.componentsToExclude;

            foreach (int poolId in includes) {
                while (queriesByIncludedComponent.Count <= poolId)
                    queriesByIncludedComponent.Add(null);

                if (queriesByIncludedComponent[poolId] == null)
                    queriesByIncludedComponent[poolId] = new List<Query>();

                queriesByIncludedComponent[poolId].Add(query);
            }

            foreach (int poolId in excludes) {
                while (queriesByExcludedComponent.Count <= poolId)
                    queriesByExcludedComponent.Add(null);

                if (queriesByExcludedComponent[poolId] == null)
                    queriesByExcludedComponent[poolId] = new List<Query>();

                queriesByExcludedComponent[poolId].Add(query);
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

        public void Despawn(Entity entity) => Despawn(entity.Id);

        public void Despawn(int entityId) {
            foreach (IComponentPool pool in componentPoolsByType.Values) {
                pool.Remove(entityId);
            }
            entityComponentCounts.Remove(entityId);
            recycledEntities.Push(entityId);
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

        private void UpdateHotQueriesAfterAddingComponent(int entityId, int poolId) {
            if (poolId >= queriesByIncludedComponent.Count)
                return;

            List<Query> pool = queriesByIncludedComponent[poolId];
            if (pool == null)
                return;

            foreach (Query query in pool) {
                query.OnAddComponentToEntity(entityId, poolId);
            }
        }

        private void UpdateHotQueriesAfterRemovingComponent(int entityId, int poolId) {
            if (poolId >= queriesByExcludedComponent.Count)
                return;

            List<Query> pool = queriesByExcludedComponent[poolId];
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
                Despawn(entityId);
            }

            UpdateHotQueriesAfterRemovingComponent(entityId, poolId);
        }

        public void OnAddComponentToEntity(int entityId, int poolId) {
            // Update the hot queries.
            entityComponentCounts.Get(entityId) ++;

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
