using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ecs {
    public class World : IEntityManager {
        private Dictionary<Type, IComponentPool> componentPoolsByType;

        private Stack<int> recycledEntities;
        private int entityCount;
        public int EntityCount => entityCount;

        // Maps the entity id to its component count. Also useful for checking which entities
        // are still alive (if they are not, then the Contains() method will return false).
        private SparseSet<int> entityComponentCounts;

        public World() {
            componentPoolsByType = new Dictionary<Type, IComponentPool>();
            recycledEntities = new Stack<int>();
            entityCount = 0;
            entityComponentCounts = new SparseSet<int>();
        }

        public Entity Spawn() {
            int id = 0;

            if (recycledEntities.Count > 0) {
                id = recycledEntities.Pop();
            }
            else {
                id = entityCount;
                entityCount ++;
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

            ComponentPool<TComponent> newPool = new ComponentPool<TComponent>(this);
            componentPoolsByType[type] = newPool;
            return newPool;
        }

        internal bool TryGetIComponentPool(Type componentType, out IComponentPool pool) {
            return componentPoolsByType.TryGetValue(componentType, out pool);
        }

        public void DecComponentCount(int entityId) {
            int count = entityComponentCounts.Get(entityId);
            count -= 1;
            entityComponentCounts.Add(entityId, count);

            if (count <= 0) {
                Despawn(entityId);
            }
        }

        public void IncComponentCount(int entityId) => entityComponentCounts.Get(entityId) ++;
        public int GetComponentCount(Entity entity) => entityComponentCounts.Get(entity.Id);

        public bool IsEntityAlive(Entity entity) => entityComponentCounts.Contains(entity.Id);
    }
}
