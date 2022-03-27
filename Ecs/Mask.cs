using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ecs {
    public class Mask {
        private World world;
        internal List<int> componentsToInclude;
        internal List<int> componentsToExclude;

        public Mask(World world) {
            this.world = world;
            componentsToInclude = new List<int>();
            componentsToExclude = new List<int>();
        }

        public Mask Inc<TComponent>() where TComponent : struct {
            componentsToInclude.Add(world.GetComponentPool<TComponent>().PoolId);
            return this;
        }

        public Mask Exc<TComponent>() where TComponent : struct {
            componentsToExclude.Add(world.GetComponentPool<TComponent>().PoolId);
            return this;
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
    }
}
