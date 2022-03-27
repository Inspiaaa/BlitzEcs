using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ecs {
    public class Query {
        public struct Enumerator {
            private Query query;
            private int idx;

            public Enumerator(Query query) {
                this.query = query;
                idx = 0;
            }

            public Entity Current => new Entity(query.world, query.matchedEntities.DirectValues[idx]);

            public bool MoveNext() {
                return ++idx < query.matchedEntities.Count;
            }
        }

        protected World world;

        private List<Type> componentsToInclude;

        protected SparseSet<int> matchedEntities;

        public Query(World world) {
            this.world = world;
            componentsToInclude = new List<Type>();
            matchedEntities = new SparseSet<int>();
        }

        public Query Inc<TComponent>() where TComponent : struct {
            componentsToInclude.Add(typeof(TComponent));
            return this;
        }

        public void Fetch() {
            matchedEntities.Clear();
            if (componentsToInclude.Count == 0) {
                return;
            }

            // Find the pool with the least items.

            IComponentPool smallestPool = null;
            int smallestCount = int.MaxValue;

            foreach (Type componentType in componentsToInclude) {
                if (! world.TryGetIComponentPool(componentType, out IComponentPool pool)) {
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
            foreach (int id in smallestPool.EntityIds) {
                matchedEntities.Add(id, id);
            }

            // Then go over every individual component type and remove the entities that
            // do not match.
            foreach (Type componentType in componentsToInclude) {
                IComponentPool pool;
                world.TryGetIComponentPool(componentType, out pool);

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
        }

        public Enumerator GetEnumerator() => new Enumerator(this);
    }

    public class Query<C1> : Query
        where C1 : struct {

        public Query(World world) : base(world) {
            Inc<C1>();
        }

        public delegate void ForEachAction(Entity e, ref C1 c1);

        public void ForEach(ForEachAction action) {
            Fetch();
            ComponentPool<C1> pool1 = world.GetComponentPool<C1>();
            foreach (int id in matchedEntities.Values) {
                action(new Entity(world, id), ref pool1.GetUnsafe(id));
            }
        }

        public void DoSomething() {}
    }

    public class Query<C1, C2> : Query
        where C1 : struct
        where C2 : struct {

        public Query(World world) : base(world) {
            Inc<C1>();
            Inc<C2>();
        }

        public delegate void ForEachAction(Entity e, ref C1 c1, ref C2 c2);

        public void ForEach(ForEachAction action) {
            Fetch();
            ComponentPool<C1> pool1 = world.GetComponentPool<C1>();
            ComponentPool<C2> pool2 = world.GetComponentPool<C2>();
            foreach (int id in matchedEntities.Values) {
                action(new Entity(world, id), ref pool1.GetUnsafe(id), ref pool2.GetUnsafe(id));
            }
        }
    }

    public class Query<C1, C2, C3> : Query
        where C1 : struct
        where C2 : struct
        where C3 : struct {

        public Query(World world) : base(world) {
            Inc<C1>();
            Inc<C2>();
            Inc<C3>();
        }

        public delegate void ForEachAction(Entity e, ref C1 c1, ref C2 c2, ref C3 c3);

        public void ForEach(ForEachAction action) {
            Fetch();
            ComponentPool<C1> pool1 = world.GetComponentPool<C1>();
            ComponentPool<C2> pool2 = world.GetComponentPool<C2>();
            ComponentPool<C3> pool3 = world.GetComponentPool<C3>();
            foreach (int id in matchedEntities.Values) {
                action(
                    new Entity(world, id), ref pool1.GetUnsafe(id), ref pool2.GetUnsafe(id),
                    ref pool3.GetUnsafe(id));
            }
        }
    }

    public class Query<C1, C2, C3, C4> : Query
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct {

        public Query(World world) : base(world) {
            Inc<C1>();
            Inc<C2>();
            Inc<C3>();
            Inc<C4>();
        }

        public delegate void ForEachAction(Entity e, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4);

        public void ForEach(ForEachAction action) {
            Fetch();
            ComponentPool<C1> pool1 = world.GetComponentPool<C1>();
            ComponentPool<C2> pool2 = world.GetComponentPool<C2>();
            ComponentPool<C3> pool3 = world.GetComponentPool<C3>();
            ComponentPool<C4> pool4 = world.GetComponentPool<C4>();
            foreach (int id in matchedEntities.Values) {
                action(
                    new Entity(world, id), ref pool1.GetUnsafe(id), ref pool2.GetUnsafe(id),
                    ref pool3.GetUnsafe(id), ref pool4.GetUnsafe(id));
            }
        }
    }

    public class Query<C1, C2, C3, C4, C5> : Query
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct {

        public Query(World world) : base(world) {
            Inc<C1>();
            Inc<C2>();
            Inc<C3>();
            Inc<C4>();
            Inc<C5>();
        }

        public delegate void ForEachAction(Entity e, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5);

        public void ForEach(ForEachAction action) {
            Fetch();
            ComponentPool<C1> pool1 = world.GetComponentPool<C1>();
            ComponentPool<C2> pool2 = world.GetComponentPool<C2>();
            ComponentPool<C3> pool3 = world.GetComponentPool<C3>();
            ComponentPool<C4> pool4 = world.GetComponentPool<C4>();
            ComponentPool<C5> pool5 = world.GetComponentPool<C5>();
            foreach (int id in matchedEntities.Values) {
                action(
                    new Entity(world, id), ref pool1.GetUnsafe(id), ref pool2.GetUnsafe(id),
                    ref pool3.GetUnsafe(id), ref pool4.GetUnsafe(id), ref pool5.GetUnsafe(id));
            }
        }
    }
}
