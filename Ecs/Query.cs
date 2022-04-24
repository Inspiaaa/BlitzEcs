using System.Collections;
using System.Collections.Generic;
using System;

using BlitzEcs.Util;

namespace BlitzEcs {
    public class Query {
        public struct Enumerator {
            private Query query;
            private int idx;

            public Enumerator(Query query) {
                this.query = query;
                idx = 0;
            }

            public Entity Current => new Entity(query.world, query.matchedEntities.DirectKeys[idx]);

            public bool MoveNext() {
                return ++idx < query.matchedEntities.Count;
            }
        }

        protected World world;

        // Hot queries are cached. That means that the matchedEntities table is set up once and
        // then only updated when components are added or removed.
        protected bool hot;
        public bool IsCached => hot;

        private Mask mask;
        public Mask Mask => mask;

        // Set of all entities that match the query. The key is the entity id, and the value
        // is not important (bool to save memory).
        protected SparseSet<bool> matchedEntities;
        public (int[] entityIds, int count) MatchedEntityIds => (matchedEntities.DirectKeys, matchedEntities.Count);

        public Query(World world) {
            this.world = world;

            mask = new Mask(world);
            matchedEntities = new SparseSet<bool>();
            matchedEntities.AutoShrink = false;
        }

        internal void OnCache() {
            // Called when the World caches this query.

            if (hot)
                return;

            Fetch();
            hot = true;
            matchedEntities.AutoShrink = true;
        }

        internal void MirrorCachedQuery(Query sourceQuery) {
            matchedEntities = sourceQuery.matchedEntities;
            hot = true;
        }

        public Query Inc<TComponent>() where TComponent : struct {
            if (hot)
                throw new InvalidOperationException("A query's mask cannot be changed after caching.");
            mask.Inc<TComponent>();
            return this;
        }

        public Query Exc<TComponent>() where TComponent : struct {
            if (hot)
                throw new InvalidOperationException("A query's mask cannot be changed after caching.");
            mask.Exc<TComponent>();
            return this;
        }

        public void OnAddComponentToEntity(int entityId, int poolId) {
            if (matchedEntities.Contains(entityId)) {
                // The entity previously matched the query.
                // Checks whether the query still matches after adding the component.
                if (! mask.IsCompatibleAfterAdddingComponent(poolId)) {
                    matchedEntities.Remove(entityId);
                }
            }
            else {
                // The entity previously did not match / was just created.
                // Checks whether the query fully matches after adding the component.
                if (mask.IsCompatible(entityId)) {
                    matchedEntities.Add(entityId);
                }
            }
        }

        public void OnRemoveComponentFromEntity(int entityId, int poolId) {
            if (matchedEntities.Contains(entityId)) {
                // The entity previously matched the query.
                // Checks whether the query still matches after removing the component.
                if (! mask.IsCompatibleAfterRemovingComponent(poolId)) {
                    matchedEntities.Remove(entityId);
                }
            }
            else {
                // The entity previously did not match / was just created.
                // Checks whether the query fully matches after removing the component.
                if (mask.IsCompatible(entityId)) {
                    matchedEntities.Add(entityId);
                }
            }
        }

        public void Fetch() {
            matchedEntities.Clear();
            if (mask.componentsToInclude.Count == 0) {
                return;
            }

            // Find the pool with the least items.

            IComponentPool smallestPool = null;
            int smallestCount = int.MaxValue;

            foreach (int poolId in mask.componentsToInclude) {
                if (! world.TryGetIComponentPool(poolId, out IComponentPool pool)) {
                    // One of the required component pools does not exist, i.e. has 0 items.
                    // => There are no entities that match the query.
                    return;
                }
                if (pool.Count < smallestCount) {
                    smallestPool = pool;
                    smallestCount = pool.Count;
                }
            }

            if (smallestCount == 0) {
                return;
            }

            // Find the components.

            // Assume that all entities in the smallest pool have all components.
            // Directly copy the entity id data from the smallest pool to the matchedEntities set.

            int highestEntityId = smallestPool.HighestEntityId;
            matchedEntities.SetMinCapacity(highestEntityId, smallestCount);

            // Copy the sparse array.
            Array.Copy(
                smallestPool.RawEntityIdsToComponentIdx,
                matchedEntities.DirectKeysToValueIdx,
                highestEntityId + 1);

            // Copy the dense array. As the values are not important, the dense values / data array needn't be copied.
            Array.Copy(smallestPool.RawEntityIds, matchedEntities.DirectKeys, smallestCount);
            matchedEntities.SetCountUnsafe(smallestCount);

            // Then go over every individual component type and remove the entities that
            // do not match.
            foreach (int poolId in mask.componentsToInclude) {
                if (smallestPool.PoolId == poolId) continue;

                IComponentPool pool;
                world.TryGetIComponentPool(poolId, out pool);

                int[] entityIds = matchedEntities.DirectKeys;
                for (int i = 0; i < matchedEntities.Count; i ++) {
                    int id = entityIds[i];

                    if (! pool.Contains(id)) {
                        matchedEntities.Remove(id);
                        // When an item is deleted from a sparse set, the resulting "hole" is filled with the last
                        // item. As we want to check every entity again, we have to check the entity that
                        // just filled the hole, i.e. go back one more time.
                        i --;
                    }
                }
            }

            foreach (int poolId in mask.componentsToExclude) {
                IComponentPool pool;
                world.TryGetIComponentPool(poolId, out pool);

                int[] entityIds = matchedEntities.DirectKeys;
                for (int i = 0; i < matchedEntities.Count; i ++) {
                    int id = entityIds[i];

                    if (pool.Contains(id)) {
                        matchedEntities.Remove(id);
                        i --;
                    }
                }
            }

            matchedEntities.Shrink();
        }

        public Enumerator GetEnumerator() => new Enumerator(this);
    }
}
