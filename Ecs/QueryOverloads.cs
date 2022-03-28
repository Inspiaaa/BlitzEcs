using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ecs {
    public class Query<C1> : Query
        where C1 : struct {

        public Query(World world) : base(world) {
            Inc<C1>();
        }

        public delegate void ForEachAction(Entity e, ref C1 c1);

        public void ForEach(ForEachAction action) {
            if (! hot) Fetch();

            ComponentPool<C1> pool1 = world.GetComponentPool<C1>();
            foreach (int id in matchedEntities.Keys) {
                action(
                    new Entity(world, id),
                    ref pool1.GetUnsafe(id));
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
            if (! hot) Fetch();

            ComponentPool<C1> pool1 = world.GetComponentPool<C1>();
            ComponentPool<C2> pool2 = world.GetComponentPool<C2>();
            foreach (int id in matchedEntities.Keys) {
                action(
                    new Entity(world, id),
                ref pool1.GetUnsafe(id),
                ref pool2.GetUnsafe(id));
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
            if (! hot) Fetch();

            ComponentPool<C1> pool1 = world.GetComponentPool<C1>();
            ComponentPool<C2> pool2 = world.GetComponentPool<C2>();
            ComponentPool<C3> pool3 = world.GetComponentPool<C3>();
            foreach (int id in matchedEntities.Keys) {
                action(
                    new Entity(world, id),
                    ref pool1.GetUnsafe(id),
                    ref pool2.GetUnsafe(id),
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
            if (! hot) Fetch();

            ComponentPool<C1> pool1 = world.GetComponentPool<C1>();
            ComponentPool<C2> pool2 = world.GetComponentPool<C2>();
            ComponentPool<C3> pool3 = world.GetComponentPool<C3>();
            ComponentPool<C4> pool4 = world.GetComponentPool<C4>();
            foreach (int id in matchedEntities.Keys) {
                action(
                    new Entity(world, id),
                    ref pool1.GetUnsafe(id),
                    ref pool2.GetUnsafe(id),
                    ref pool3.GetUnsafe(id),
                    ref pool4.GetUnsafe(id));
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
            if (! hot) Fetch();

            ComponentPool<C1> pool1 = world.GetComponentPool<C1>();
            ComponentPool<C2> pool2 = world.GetComponentPool<C2>();
            ComponentPool<C3> pool3 = world.GetComponentPool<C3>();
            ComponentPool<C4> pool4 = world.GetComponentPool<C4>();
            ComponentPool<C5> pool5 = world.GetComponentPool<C5>();
            foreach (int id in matchedEntities.Keys) {
                action(
                    new Entity(world, id),
                    ref pool1.GetUnsafe(id),
                    ref pool2.GetUnsafe(id),
                    ref pool3.GetUnsafe(id),
                    ref pool4.GetUnsafe(id),
                    ref pool5.GetUnsafe(id));
            }
        }
    }
}