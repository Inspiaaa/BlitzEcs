using System.Collections;
using System.Collections.Generic;

namespace BlitzEcs {
    public struct Entity {
        private int id;
        private World world;

        public int Id => id;

        public Entity(World world, int id) {
            this.id = id;
            this.world = world;
        }

        public Entity Add<TComponent>() where TComponent : struct {
            world.GetComponentPool<TComponent>().Add(id);
            return this;
        }

        public Entity Add<TComponent>(TComponent component) where TComponent : struct {
            world.GetComponentPool<TComponent>().Add(id, component);
            return this;
        }

        public ref TComponent Get<TComponent>() where TComponent : struct {
            return ref world.GetComponentPool<TComponent>().Get(id);
        }

        public ref TComponent GetUnsafe<TComponent>() where TComponent : struct {
            return ref world.GetComponentPool<TComponent>().GetUnsafe(id);
        }

        public Entity Remove<TComponent>() where TComponent : struct {
            world.GetComponentPool<TComponent>().Remove(id);
            return this;
        }

        public bool Has<TComponent>() where TComponent : struct {
            return world.GetComponentPool<TComponent>().Contains(id);
        }

        public void Despawn() {
            world.Despawn(id);
        }

        public static implicit operator int(Entity e) => e.id;
    }
}
