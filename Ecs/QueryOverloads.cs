using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

using BlitzEcs.Util;

// This code was automatically generated from a template.
// Manual changes will be overwritten when the code is regenerated.

namespace BlitzEcs {
    public class Query<C1> : Query
        where C1 : struct {

        public Query(World world) : base(world) {
            Inc<C1>();
        }

        public delegate void RefAction(
            ref C1 c1);

        public delegate void EntityRefAction(
            Entity e,
            ref C1 c1);

        public void ForEach(RefAction action) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();

            world.LockComponentPools();
            try {
                foreach (int id in matchedEntities.Keys) {
                    action(
                        ref pool1.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ForEach(EntityRefAction action) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();

            world.LockComponentPools();
            try {
                foreach (int id in matchedEntities.Keys) {
                    action(
                        new Entity(world, id),
                        ref pool1.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ParallelForEach(RefAction action, int chunkSize=64) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount - 1, chunkNum => {
                int start = chunkNum * chunkSize;
                int end = (chunkNum == chunkCount - 1) ? count : (chunkNum + 1) * chunkSize;

                for (int id = start; id < end; id++) {
                    action(
                        ref pool1.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
        }

        public void ParallelForEach(EntityRefAction action, int chunkSize=64) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount - 1, chunkNum => {
                int start = chunkNum * chunkSize;
                int end = (chunkNum == chunkCount - 1) ? count : (chunkNum + 1) * chunkSize;

                for (int id = start; id < end; id++) {
                    action(
                        new Entity(world, id),
                        ref pool1.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
        }
    }

public class Query<C1, C2> : Query
        where C1 : struct
        where C2 : struct {

        public Query(World world) : base(world) {
            Inc<C1>();
            Inc<C2>();
        }

        public delegate void RefAction(
            ref C1 c1,
            ref C2 c2);

        public delegate void EntityRefAction(
            Entity e,
            ref C1 c1,
            ref C2 c2);

        public void ForEach(RefAction action) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();

            world.LockComponentPools();
            try {
                foreach (int id in matchedEntities.Keys) {
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ForEach(EntityRefAction action) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();

            world.LockComponentPools();
            try {
                foreach (int id in matchedEntities.Keys) {
                    action(
                        new Entity(world, id),
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ParallelForEach(RefAction action, int chunkSize=64) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount - 1, chunkNum => {
                int start = chunkNum * chunkSize;
                int end = (chunkNum == chunkCount - 1) ? count : (chunkNum + 1) * chunkSize;

                for (int id = start; id < end; id++) {
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
        }

        public void ParallelForEach(EntityRefAction action, int chunkSize=64) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount - 1, chunkNum => {
                int start = chunkNum * chunkSize;
                int end = (chunkNum == chunkCount - 1) ? count : (chunkNum + 1) * chunkSize;

                for (int id = start; id < end; id++) {
                    action(
                        new Entity(world, id),
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
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

        public delegate void RefAction(
            ref C1 c1,
            ref C2 c2,
            ref C3 c3);

        public delegate void EntityRefAction(
            Entity e,
            ref C1 c1,
            ref C2 c2,
            ref C3 c3);

        public void ForEach(RefAction action) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();

            world.LockComponentPools();
            try {
                foreach (int id in matchedEntities.Keys) {
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ForEach(EntityRefAction action) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();

            world.LockComponentPools();
            try {
                foreach (int id in matchedEntities.Keys) {
                    action(
                        new Entity(world, id),
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ParallelForEach(RefAction action, int chunkSize=64) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount - 1, chunkNum => {
                int start = chunkNum * chunkSize;
                int end = (chunkNum == chunkCount - 1) ? count : (chunkNum + 1) * chunkSize;

                for (int id = start; id < end; id++) {
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
        }

        public void ParallelForEach(EntityRefAction action, int chunkSize=64) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount - 1, chunkNum => {
                int start = chunkNum * chunkSize;
                int end = (chunkNum == chunkCount - 1) ? count : (chunkNum + 1) * chunkSize;

                for (int id = start; id < end; id++) {
                    action(
                        new Entity(world, id),
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
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

        public delegate void RefAction(
            ref C1 c1,
            ref C2 c2,
            ref C3 c3,
            ref C4 c4);

        public delegate void EntityRefAction(
            Entity e,
            ref C1 c1,
            ref C2 c2,
            ref C3 c3,
            ref C4 c4);

        public void ForEach(RefAction action) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();

            world.LockComponentPools();
            try {
                foreach (int id in matchedEntities.Keys) {
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ForEach(EntityRefAction action) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();

            world.LockComponentPools();
            try {
                foreach (int id in matchedEntities.Keys) {
                    action(
                        new Entity(world, id),
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ParallelForEach(RefAction action, int chunkSize=64) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount - 1, chunkNum => {
                int start = chunkNum * chunkSize;
                int end = (chunkNum == chunkCount - 1) ? count : (chunkNum + 1) * chunkSize;

                for (int id = start; id < end; id++) {
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
        }

        public void ParallelForEach(EntityRefAction action, int chunkSize=64) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount - 1, chunkNum => {
                int start = chunkNum * chunkSize;
                int end = (chunkNum == chunkCount - 1) ? count : (chunkNum + 1) * chunkSize;

                for (int id = start; id < end; id++) {
                    action(
                        new Entity(world, id),
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
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

        public delegate void RefAction(
            ref C1 c1,
            ref C2 c2,
            ref C3 c3,
            ref C4 c4,
            ref C5 c5);

        public delegate void EntityRefAction(
            Entity e,
            ref C1 c1,
            ref C2 c2,
            ref C3 c3,
            ref C4 c4,
            ref C5 c5);

        public void ForEach(RefAction action) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();

            world.LockComponentPools();
            try {
                foreach (int id in matchedEntities.Keys) {
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id),
                        ref pool5.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ForEach(EntityRefAction action) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();

            world.LockComponentPools();
            try {
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
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ParallelForEach(RefAction action, int chunkSize=64) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount - 1, chunkNum => {
                int start = chunkNum * chunkSize;
                int end = (chunkNum == chunkCount - 1) ? count : (chunkNum + 1) * chunkSize;

                for (int id = start; id < end; id++) {
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id),
                        ref pool5.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
        }

        public void ParallelForEach(EntityRefAction action, int chunkSize=64) {
            if (! hot) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount - 1, chunkNum => {
                int start = chunkNum * chunkSize;
                int end = (chunkNum == chunkCount - 1) ? count : (chunkNum + 1) * chunkSize;

                for (int id = start; id < end; id++) {
                    action(
                        new Entity(world, id),
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id),
                        ref pool5.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
        }
    }

}