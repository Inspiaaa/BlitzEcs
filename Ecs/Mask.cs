using System.Collections;
using System.Collections.Generic;
using System;

namespace BlitzEcs {
    public class Mask : IEquatable<Mask> {
        private World world;
        internal List<int> componentsToInclude;
        internal List<int> componentsToExclude;

        public Mask(World world) {
            this.world = world;
            componentsToInclude = new List<int>();
            componentsToExclude = new List<int>();

            componentsToExclude.GetHashCode();
        }

        public Mask Inc<TComponent>() where TComponent : struct {
            componentsToInclude.Add(world.GetComponentPool<TComponent>().PoolId);
            return this;
        }

        public Mask Exc<TComponent>() where TComponent : struct {
            componentsToExclude.Add(world.GetComponentPool<TComponent>().PoolId);
            return this;
        }

        public bool IsCompatibleAfterAdddingComponent(int componentPoolId) {
            // Assumes that the entity was previously compatible and has now added a new component.
            return !componentsToExclude.Contains(componentPoolId);
        }

        public bool IsCompatibleAfterRemovingComponent(int componentPoolId) {
            // Assumes that the entity was previously compatible and has now added a new component.
            return ! componentsToInclude.Contains(componentPoolId);
        }

        public bool IsCompatible(int entityId) {
            foreach (int poolId in componentsToInclude) {
                IComponentPool pool;
                bool poolExists = world.TryGetIComponentPool(poolId, out pool);

                if (! poolExists) {
                    return false;
                }

                if (! pool.Contains(entityId)) {
                    return false;
                }
            }

            foreach (int poolId in componentsToExclude) {
                IComponentPool pool;
                bool poolExists = world.TryGetIComponentPool(poolId, out pool);

                if (! poolExists) {
                    continue;
                }

                if (pool.Contains(entityId)) {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 0;

            foreach (int poolId in componentsToInclude)
                hash ^= poolId.GetHashCode();

            foreach (int poolId in componentsToExclude)
                hash ^= poolId.GetHashCode();

            return hash;
        }

        public bool Equals(Mask other) {
            if (componentsToInclude.Count != other.componentsToInclude.Count)
                return false;

            if (componentsToExclude.Count != other.componentsToExclude.Count)
                return false;

            for (int i = 0; i < componentsToInclude.Count; i ++) {
                if (componentsToInclude[i] != other.componentsToInclude[i])
                    return false;
            }

            for (int i = 0; i < componentsToExclude.Count; i ++) {
                if (componentsToExclude[i] != other.componentsToExclude[i])
                    return false;
            }

            return true;
        }
    }
}
