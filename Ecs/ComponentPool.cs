using System.Collections;
using System.Collections.Generic;
using System;

using BlitzEcs.Util;

namespace BlitzEcs {
    public class ComponentPool<TComponent> : IComponentPool where TComponent : struct {
        // Uses a sparse set to map the entity IDs to the components.

        private IEntityManager world;
        private int poolId;
        public int PoolId => poolId;

        // Sparse array
        private int[] entityIdToComponentIdx;
        // Dense array
        private int[] componentIdxToEntityId;
        // Aligned to the componentIdxToEntityId list
        private TComponent[] components;

        private int count;
        public int Count => count;

        private SparseSet<bool> entitiesToRemove;
        // Used for running custom clean-up logic when a component is destroyed.
        private IEcsDestroyHandler<TComponent> destroyHandler;

        public TComponent[] RawComponents => components;
        public int[] RawEntityIds => componentIdxToEntityId;
        public int[] RawEntityIdsToComponentIdx => entityIdToComponentIdx;

        public PoolEntityIdEnumerator EntityIds => new PoolEntityIdEnumerator(this);
        public ComponentEnumerator Components => new ComponentEnumerator(this);

        public ComponentPool(IEntityManager world, int poolId) {
            this.world = world;
            this.poolId = poolId;

            entityIdToComponentIdx = new int[1];
            componentIdxToEntityId = new int[1];
            components = new TComponent[1];
            count = 0;

            entitiesToRemove = new SparseSet<bool>();
            // To avoid boxing and heap allocations when destroying a component, only one
            // component is boxed and then used to destroy the other components, which
            // therefore don't have to be individually boxed.
            destroyHandler = default(TComponent) as IEcsDestroyHandler<TComponent>;
        }

        public void SetDestroyHandler(IEcsDestroyHandler<TComponent> destroyHandler) {
            this.destroyHandler = destroyHandler;
        }

        private int GetComponentIdx(int entityId) {
            if (entityId >= entityIdToComponentIdx.Length)
                return -1;

            int componentIdx = entityIdToComponentIdx[entityId];
            if (componentIdx >= count)
                return -1;
            if (entityId != componentIdxToEntityId[componentIdx])
                return -1;

            return componentIdx;
        }

        public bool Contains(int entityId) => GetComponentIdx(entityId) != -1;

        public void Add(int entityId) => Add(entityId, default);

        public void Add(int entityId, TComponent component) {
            bool isAlreadyContained = Contains(entityId);

            int componentIdx = count;

            if (componentIdx >= components.Length) {
                int newCapacity = MathUtil.NextPowerOf2(componentIdx + 1);
                Array.Resize(ref components, newCapacity);
                Array.Resize(ref componentIdxToEntityId, newCapacity);
            }

            if (entityId >= entityIdToComponentIdx.Length) {
                Array.Resize(ref entityIdToComponentIdx, MathUtil.NextPowerOf2(entityId + 1));
            }

            components[componentIdx] = component;
            componentIdxToEntityId[componentIdx] = entityId;
            entityIdToComponentIdx[entityId] = componentIdx;

            count ++;

            if (! isAlreadyContained) {
                world.OnAddComponentToEntity(entityId, poolId);
            }

            // If the component is queued to be deleted, but in the meantime we add the component again,
            // there is no need to remove the new one.
            if (entitiesToRemove.Contains(entityId)) {
                entitiesToRemove.Remove(entityId);
            }
        }

        private void ActuallyRemove(int entityId) {
            if (!Contains(entityId)) {
                return;
            }

            int lastCompIdx = count - 1;
            int compIdxToBeDeleted = entityIdToComponentIdx[entityId];
            int lastEntityId = componentIdxToEntityId[lastCompIdx];
            TComponent deletedComponent = components[compIdxToBeDeleted];
            TComponent lastComponent = components[lastCompIdx];

            // Replace the item to be deleted with the last item.
            components[compIdxToBeDeleted] = lastComponent;
            componentIdxToEntityId[compIdxToBeDeleted] = lastEntityId;
            // Reset the last component to free any GC references.
            components[lastCompIdx] = default;
            entityIdToComponentIdx[lastEntityId] = compIdxToBeDeleted;

            count --;

            if (count <= components.Length / 4) {
                Shrink();
            }

            destroyHandler?.OnDestroy(ref deletedComponent);
            world.OnRemoveComponentFromEntity(entityId, poolId);
        }

        public void Remove(int entityId) {
            if (world.ArePoolsLocked) {
                entitiesToRemove.Add(entityId);
            }
            else {
                ActuallyRemove(entityId);
            }
        }

        public void ExecuteBufferedRemoves() {
            int[] entityIds = entitiesToRemove.DirectKeys;
            for (int i = 0, max = entitiesToRemove.Count; i < max; i++ ) {
                // TODO: If autoPurge, only do the purge after doing all removes.
                ActuallyRemove(entityIds[i]);
            }
            entitiesToRemove.Clear();
        }

        public ref TComponent Get(int entityId) {
            int componentIdx = GetComponentIdx(entityId);
            if (componentIdx == -1) {
                throw new InvalidOperationException(
                    $"Entity {entityId} does not have a component of type {typeof(TComponent)}."
                );
            }
            return ref components[componentIdx];
        }

        public ref TComponent GetUnsafe(int entityId) {
            return ref components[entityIdToComponentIdx[entityId]];
        }

        public int HighestEntityId {
            get {
                int highestId = 0;

                for (int i = 0; i < count; i ++) {
                    int entityId = componentIdxToEntityId[i];
                    if (entityId > highestId) {
                        highestId = entityId;
                    }
                }

                return highestId;
            }
        }

        public void SetCountUnsafe(int newCount) => count = newCount;

        public void SetMinCapacity(int maxEntityIdInclusive, int count) {
            if (maxEntityIdInclusive >= entityIdToComponentIdx.Length) {
                Array.Resize(ref entityIdToComponentIdx, MathUtil.NextPowerOf2(maxEntityIdInclusive + 1));
            }

            if (count > this.count) {
                int newCapacity = MathUtil.NextPowerOf2(count);
                Array.Resize(ref componentIdxToEntityId, newCapacity);
                Array.Resize(ref components, newCapacity);
            }
        }

        public void Clear() {
            for (int entityId = 0; entityId < count; entityId ++) {
                world.OnRemoveComponentFromEntity(componentIdxToEntityId[entityId], poolId);
            }
            // Remove any GC references.
            Array.Clear(components, 0, count);
            count = 0;
        }

        public void Shrink() {
            // Reduces the unnecessary buffer space to save memory.

            int highestEntityId = HighestEntityId;
            if (highestEntityId <= entityIdToComponentIdx.Length / 4) {
                Array.Resize(ref entityIdToComponentIdx, MathUtil.NextPowerOf2(highestEntityId + 1));
            }

            if (count <= components.Length / 4) {
                int newCapacity = MathUtil.NextPowerOf2(count);
                Array.Resize(ref componentIdxToEntityId, newCapacity);
                Array.Resize(ref components, newCapacity);
            }
        }

        public struct ComponentEnumerator {
            private int idx;
            private int count;
            private TComponent[] components;

            public ComponentEnumerator(ComponentPool<TComponent> pool) {
                count = pool.count;
                components = pool.components;
                idx = -1;
            }

            public ComponentEnumerator GetEnumerator() => this;

            public TComponent Current => components[idx];

            public bool MoveNext() {
                return ++idx < count;
            }
        }
    }

    public struct PoolEntityIdEnumerator {
        private int idx;
        private int count;
        private int[] entityIds;

        public PoolEntityIdEnumerator(IComponentPool pool) {
            count = pool.Count;
            entityIds = pool.RawEntityIds;
            idx = -1;
        }

        public PoolEntityIdEnumerator GetEnumerator() => this;

        public int Current => entityIds[idx];

        public bool MoveNext() {
            return ++idx < count;
        }
    }
}
