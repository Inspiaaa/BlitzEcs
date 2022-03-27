using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Ecs {
    public class ComponentPool<TComponent> : IComponentPool where TComponent : struct {
        // Uses a sparse set to map the entity IDs to the components.

        private IEntityManager world;

        // Sparse array
        private int[] entityIdToComponentIdx;
        // Dense array
        private int[] componentIdxToEntityId;
        // Aligned to the componentIdxToEntityId list
        private TComponent[] components;

        private int count;
        public int Count => count;

        public ComponentPool(IEntityManager world) {
            this.world = world;
            entityIdToComponentIdx = new int[1];
            componentIdxToEntityId = new int[1];
            components = new TComponent[1];
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
                int newCapacity = 2 * components.Length;
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
                world.IncComponentCount(entityId);
            }
        }

        public void Remove(int entityId) {
            if (!Contains(entityId)) {
                return;
            }

            int lastCompIdx = count - 1;
            int compIdxToBeDeleted = entityIdToComponentIdx[entityId];
            int lastEntityId = componentIdxToEntityId[lastCompIdx];
            TComponent lastComponent = components[lastCompIdx];

            // Replace the item to be deleted with the last item.
            components[compIdxToBeDeleted] = lastComponent;
            componentIdxToEntityId[compIdxToBeDeleted] = lastEntityId;
            // Reset the last component to free any GC references.
            components[lastCompIdx] = default;
            entityIdToComponentIdx[lastEntityId] = compIdxToBeDeleted;

            count --;

            world.DecComponentCount(entityId);
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

        public TComponent[] RawComponents => components;
        public int[] RawEntityIds => componentIdxToEntityId;

        public void Clear() {
            // Remove any GC references.
            Array.Clear(components, 0, count);
            count = 0;
        }

        public PoolEntityIdEnumerator EntityIds => new PoolEntityIdEnumerator(this);
        public ComponentEnumerator Components => new ComponentEnumerator(this);

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
