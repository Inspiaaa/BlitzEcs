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

        public void ForEach(RefAction action, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();

            world.LockComponentPools();
            try {
                int[] ids = matchedEntities.DirectKeys;
                int count = matchedEntities.Count;

                for(int idx = 0; idx < count; idx ++) {
                    int id = ids[idx];
                    action(
                        ref pool1.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ForEach(EntityRefAction action, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();

            world.LockComponentPools();
            try {
                int[] ids = matchedEntities.DirectKeys;
                int count = matchedEntities.Count;

                for(int idx = 0; idx < count; idx ++) {
                    int id = ids[idx];
                    action(
                        new Entity(world, id),
                        ref pool1.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ParallelForEach(RefAction action, int chunkSize = 64, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();

            int[] entityIds = matchedEntities.DirectKeys;

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount, chunkIdx => {
                int start = chunkIdx * chunkSize;
                int end = (chunkIdx == chunkCount - 1) ? count : (chunkIdx + 1) * chunkSize;

                for (int entityIdx = start; entityIdx < end; entityIdx++) {
                    int id = entityIds[entityIdx];
                    action(
                        ref pool1.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
        }

        public void ParallelForEach(EntityRefAction action, int chunkSize = 64, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();

            int[] entityIds = matchedEntities.DirectKeys;

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount, chunkIdx => {
                int start = chunkIdx * chunkSize;
                int end = (chunkIdx == chunkCount - 1) ? count : (chunkIdx + 1) * chunkSize;

                for (int entityIdx = start; entityIdx < end; entityIdx++) {
                    int id = entityIds[entityIdx];
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

        public void ForEach(RefAction action, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();

            world.LockComponentPools();
            try {
                int[] ids = matchedEntities.DirectKeys;
                int count = matchedEntities.Count;

                for(int idx = 0; idx < count; idx ++) {
                    int id = ids[idx];
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ForEach(EntityRefAction action, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();

            world.LockComponentPools();
            try {
                int[] ids = matchedEntities.DirectKeys;
                int count = matchedEntities.Count;

                for(int idx = 0; idx < count; idx ++) {
                    int id = ids[idx];
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

        public void ParallelForEach(RefAction action, int chunkSize = 64, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();

            int[] entityIds = matchedEntities.DirectKeys;

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount, chunkIdx => {
                int start = chunkIdx * chunkSize;
                int end = (chunkIdx == chunkCount - 1) ? count : (chunkIdx + 1) * chunkSize;

                for (int entityIdx = start; entityIdx < end; entityIdx++) {
                    int id = entityIds[entityIdx];
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
        }

        public void ParallelForEach(EntityRefAction action, int chunkSize = 64, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();

            int[] entityIds = matchedEntities.DirectKeys;

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount, chunkIdx => {
                int start = chunkIdx * chunkSize;
                int end = (chunkIdx == chunkCount - 1) ? count : (chunkIdx + 1) * chunkSize;

                for (int entityIdx = start; entityIdx < end; entityIdx++) {
                    int id = entityIds[entityIdx];
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

        public void ForEach(RefAction action, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();

            world.LockComponentPools();
            try {
                int[] ids = matchedEntities.DirectKeys;
                int count = matchedEntities.Count;

                for(int idx = 0; idx < count; idx ++) {
                    int id = ids[idx];
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

        public void ForEach(EntityRefAction action, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();

            world.LockComponentPools();
            try {
                int[] ids = matchedEntities.DirectKeys;
                int count = matchedEntities.Count;

                for(int idx = 0; idx < count; idx ++) {
                    int id = ids[idx];
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

        public void ParallelForEach(RefAction action, int chunkSize = 64, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();

            int[] entityIds = matchedEntities.DirectKeys;

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount, chunkIdx => {
                int start = chunkIdx * chunkSize;
                int end = (chunkIdx == chunkCount - 1) ? count : (chunkIdx + 1) * chunkSize;

                for (int entityIdx = start; entityIdx < end; entityIdx++) {
                    int id = entityIds[entityIdx];
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
        }

        public void ParallelForEach(EntityRefAction action, int chunkSize = 64, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();

            int[] entityIds = matchedEntities.DirectKeys;

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount, chunkIdx => {
                int start = chunkIdx * chunkSize;
                int end = (chunkIdx == chunkCount - 1) ? count : (chunkIdx + 1) * chunkSize;

                for (int entityIdx = start; entityIdx < end; entityIdx++) {
                    int id = entityIds[entityIdx];
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

        public void ForEach(RefAction action, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();

            world.LockComponentPools();
            try {
                int[] ids = matchedEntities.DirectKeys;
                int count = matchedEntities.Count;

                for(int idx = 0; idx < count; idx ++) {
                    int id = ids[idx];
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

        public void ForEach(EntityRefAction action, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();

            world.LockComponentPools();
            try {
                int[] ids = matchedEntities.DirectKeys;
                int count = matchedEntities.Count;

                for(int idx = 0; idx < count; idx ++) {
                    int id = ids[idx];
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

        public void ParallelForEach(RefAction action, int chunkSize = 64, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();

            int[] entityIds = matchedEntities.DirectKeys;

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount, chunkIdx => {
                int start = chunkIdx * chunkSize;
                int end = (chunkIdx == chunkCount - 1) ? count : (chunkIdx + 1) * chunkSize;

                for (int entityIdx = start; entityIdx < end; entityIdx++) {
                    int id = entityIds[entityIdx];
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
        }

        public void ParallelForEach(EntityRefAction action, int chunkSize = 64, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();

            int[] entityIds = matchedEntities.DirectKeys;

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount, chunkIdx => {
                int start = chunkIdx * chunkSize;
                int end = (chunkIdx == chunkCount - 1) ? count : (chunkIdx + 1) * chunkSize;

                for (int entityIdx = start; entityIdx < end; entityIdx++) {
                    int id = entityIds[entityIdx];
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

        public void ForEach(RefAction action, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();

            world.LockComponentPools();
            try {
                int[] ids = matchedEntities.DirectKeys;
                int count = matchedEntities.Count;

                for(int idx = 0; idx < count; idx ++) {
                    int id = ids[idx];
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

        public void ForEach(EntityRefAction action, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();

            world.LockComponentPools();
            try {
                int[] ids = matchedEntities.DirectKeys;
                int count = matchedEntities.Count;

                for(int idx = 0; idx < count; idx ++) {
                    int id = ids[idx];
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

        public void ParallelForEach(RefAction action, int chunkSize = 64, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();

            int[] entityIds = matchedEntities.DirectKeys;

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount, chunkIdx => {
                int start = chunkIdx * chunkSize;
                int end = (chunkIdx == chunkCount - 1) ? count : (chunkIdx + 1) * chunkSize;

                for (int entityIdx = start; entityIdx < end; entityIdx++) {
                    int id = entityIds[entityIdx];
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

        public void ParallelForEach(EntityRefAction action, int chunkSize = 64, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();

            int[] entityIds = matchedEntities.DirectKeys;

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount, chunkIdx => {
                int start = chunkIdx * chunkSize;
                int end = (chunkIdx == chunkCount - 1) ? count : (chunkIdx + 1) * chunkSize;

                for (int entityIdx = start; entityIdx < end; entityIdx++) {
                    int id = entityIds[entityIdx];
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


    public class Query<C1, C2, C3, C4, C5, C6> : Query
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct {

        public Query(World world) : base(world) {
            Inc<C1>();
            Inc<C2>();
            Inc<C3>();
            Inc<C4>();
            Inc<C5>();
            Inc<C6>();
        }

        public delegate void RefAction(
            ref C1 c1,
            ref C2 c2,
            ref C3 c3,
            ref C4 c4,
            ref C5 c5,
            ref C6 c6);

        public delegate void EntityRefAction(
            Entity e,
            ref C1 c1,
            ref C2 c2,
            ref C3 c3,
            ref C4 c4,
            ref C5 c5,
            ref C6 c6);

        public void ForEach(RefAction action, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();
            var pool6 = world.GetComponentPool<C6>();

            world.LockComponentPools();
            try {
                int[] ids = matchedEntities.DirectKeys;
                int count = matchedEntities.Count;

                for(int idx = 0; idx < count; idx ++) {
                    int id = ids[idx];
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id),
                        ref pool5.GetUnsafe(id),
                        ref pool6.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ForEach(EntityRefAction action, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();
            var pool6 = world.GetComponentPool<C6>();

            world.LockComponentPools();
            try {
                int[] ids = matchedEntities.DirectKeys;
                int count = matchedEntities.Count;

                for(int idx = 0; idx < count; idx ++) {
                    int id = ids[idx];
                    action(
                        new Entity(world, id),
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id),
                        ref pool5.GetUnsafe(id),
                        ref pool6.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ParallelForEach(RefAction action, int chunkSize = 64, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();
            var pool6 = world.GetComponentPool<C6>();

            int[] entityIds = matchedEntities.DirectKeys;

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount, chunkIdx => {
                int start = chunkIdx * chunkSize;
                int end = (chunkIdx == chunkCount - 1) ? count : (chunkIdx + 1) * chunkSize;

                for (int entityIdx = start; entityIdx < end; entityIdx++) {
                    int id = entityIds[entityIdx];
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id),
                        ref pool5.GetUnsafe(id),
                        ref pool6.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
        }

        public void ParallelForEach(EntityRefAction action, int chunkSize = 64, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();
            var pool6 = world.GetComponentPool<C6>();

            int[] entityIds = matchedEntities.DirectKeys;

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount, chunkIdx => {
                int start = chunkIdx * chunkSize;
                int end = (chunkIdx == chunkCount - 1) ? count : (chunkIdx + 1) * chunkSize;

                for (int entityIdx = start; entityIdx < end; entityIdx++) {
                    int id = entityIds[entityIdx];
                    action(
                        new Entity(world, id),
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id),
                        ref pool5.GetUnsafe(id),
                        ref pool6.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
        }
    }


    public class Query<C1, C2, C3, C4, C5, C6, C7> : Query
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
        where C7 : struct {

        public Query(World world) : base(world) {
            Inc<C1>();
            Inc<C2>();
            Inc<C3>();
            Inc<C4>();
            Inc<C5>();
            Inc<C6>();
            Inc<C7>();
        }

        public delegate void RefAction(
            ref C1 c1,
            ref C2 c2,
            ref C3 c3,
            ref C4 c4,
            ref C5 c5,
            ref C6 c6,
            ref C7 c7);

        public delegate void EntityRefAction(
            Entity e,
            ref C1 c1,
            ref C2 c2,
            ref C3 c3,
            ref C4 c4,
            ref C5 c5,
            ref C6 c6,
            ref C7 c7);

        public void ForEach(RefAction action, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();
            var pool6 = world.GetComponentPool<C6>();
            var pool7 = world.GetComponentPool<C7>();

            world.LockComponentPools();
            try {
                int[] ids = matchedEntities.DirectKeys;
                int count = matchedEntities.Count;

                for(int idx = 0; idx < count; idx ++) {
                    int id = ids[idx];
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id),
                        ref pool5.GetUnsafe(id),
                        ref pool6.GetUnsafe(id),
                        ref pool7.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ForEach(EntityRefAction action, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();
            var pool6 = world.GetComponentPool<C6>();
            var pool7 = world.GetComponentPool<C7>();

            world.LockComponentPools();
            try {
                int[] ids = matchedEntities.DirectKeys;
                int count = matchedEntities.Count;

                for(int idx = 0; idx < count; idx ++) {
                    int id = ids[idx];
                    action(
                        new Entity(world, id),
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id),
                        ref pool5.GetUnsafe(id),
                        ref pool6.GetUnsafe(id),
                        ref pool7.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ParallelForEach(RefAction action, int chunkSize = 64, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();
            var pool6 = world.GetComponentPool<C6>();
            var pool7 = world.GetComponentPool<C7>();

            int[] entityIds = matchedEntities.DirectKeys;

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount, chunkIdx => {
                int start = chunkIdx * chunkSize;
                int end = (chunkIdx == chunkCount - 1) ? count : (chunkIdx + 1) * chunkSize;

                for (int entityIdx = start; entityIdx < end; entityIdx++) {
                    int id = entityIds[entityIdx];
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id),
                        ref pool5.GetUnsafe(id),
                        ref pool6.GetUnsafe(id),
                        ref pool7.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
        }

        public void ParallelForEach(EntityRefAction action, int chunkSize = 64, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();
            var pool6 = world.GetComponentPool<C6>();
            var pool7 = world.GetComponentPool<C7>();

            int[] entityIds = matchedEntities.DirectKeys;

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount, chunkIdx => {
                int start = chunkIdx * chunkSize;
                int end = (chunkIdx == chunkCount - 1) ? count : (chunkIdx + 1) * chunkSize;

                for (int entityIdx = start; entityIdx < end; entityIdx++) {
                    int id = entityIds[entityIdx];
                    action(
                        new Entity(world, id),
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id),
                        ref pool5.GetUnsafe(id),
                        ref pool6.GetUnsafe(id),
                        ref pool7.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
        }
    }


    public class Query<C1, C2, C3, C4, C5, C6, C7, C8> : Query
        where C1 : struct
        where C2 : struct
        where C3 : struct
        where C4 : struct
        where C5 : struct
        where C6 : struct
        where C7 : struct
        where C8 : struct {

        public Query(World world) : base(world) {
            Inc<C1>();
            Inc<C2>();
            Inc<C3>();
            Inc<C4>();
            Inc<C5>();
            Inc<C6>();
            Inc<C7>();
            Inc<C8>();
        }

        public delegate void RefAction(
            ref C1 c1,
            ref C2 c2,
            ref C3 c3,
            ref C4 c4,
            ref C5 c5,
            ref C6 c6,
            ref C7 c7,
            ref C8 c8);

        public delegate void EntityRefAction(
            Entity e,
            ref C1 c1,
            ref C2 c2,
            ref C3 c3,
            ref C4 c4,
            ref C5 c5,
            ref C6 c6,
            ref C7 c7,
            ref C8 c8);

        public void ForEach(RefAction action, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();
            var pool6 = world.GetComponentPool<C6>();
            var pool7 = world.GetComponentPool<C7>();
            var pool8 = world.GetComponentPool<C8>();

            world.LockComponentPools();
            try {
                int[] ids = matchedEntities.DirectKeys;
                int count = matchedEntities.Count;

                for(int idx = 0; idx < count; idx ++) {
                    int id = ids[idx];
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id),
                        ref pool5.GetUnsafe(id),
                        ref pool6.GetUnsafe(id),
                        ref pool7.GetUnsafe(id),
                        ref pool8.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ForEach(EntityRefAction action, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();
            var pool6 = world.GetComponentPool<C6>();
            var pool7 = world.GetComponentPool<C7>();
            var pool8 = world.GetComponentPool<C8>();

            world.LockComponentPools();
            try {
                int[] ids = matchedEntities.DirectKeys;
                int count = matchedEntities.Count;

                for(int idx = 0; idx < count; idx ++) {
                    int id = ids[idx];
                    action(
                        new Entity(world, id),
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id),
                        ref pool5.GetUnsafe(id),
                        ref pool6.GetUnsafe(id),
                        ref pool7.GetUnsafe(id),
                        ref pool8.GetUnsafe(id));
                }
            }
            finally {
                world.UnlockComponentPools();
            }
        }

        public void ParallelForEach(RefAction action, int chunkSize = 64, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();
            var pool6 = world.GetComponentPool<C6>();
            var pool7 = world.GetComponentPool<C7>();
            var pool8 = world.GetComponentPool<C8>();

            int[] entityIds = matchedEntities.DirectKeys;

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount, chunkIdx => {
                int start = chunkIdx * chunkSize;
                int end = (chunkIdx == chunkCount - 1) ? count : (chunkIdx + 1) * chunkSize;

                for (int entityIdx = start; entityIdx < end; entityIdx++) {
                    int id = entityIds[entityIdx];
                    action(
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id),
                        ref pool5.GetUnsafe(id),
                        ref pool6.GetUnsafe(id),
                        ref pool7.GetUnsafe(id),
                        ref pool8.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
        }

        public void ParallelForEach(EntityRefAction action, int chunkSize = 64, bool shouldFetch = true) {
            if (!hot && shouldFetch) Fetch();

            var pool1 = world.GetComponentPool<C1>();
            var pool2 = world.GetComponentPool<C2>();
            var pool3 = world.GetComponentPool<C3>();
            var pool4 = world.GetComponentPool<C4>();
            var pool5 = world.GetComponentPool<C5>();
            var pool6 = world.GetComponentPool<C6>();
            var pool7 = world.GetComponentPool<C7>();
            var pool8 = world.GetComponentPool<C8>();

            int[] entityIds = matchedEntities.DirectKeys;

            int count = matchedEntities.Count;
            int chunkCount = MathUtil.CeilDivision(count, chunkSize);

            world.LockComponentPools();
            Parallel.For(0, chunkCount, chunkIdx => {
                int start = chunkIdx * chunkSize;
                int end = (chunkIdx == chunkCount - 1) ? count : (chunkIdx + 1) * chunkSize;

                for (int entityIdx = start; entityIdx < end; entityIdx++) {
                    int id = entityIds[entityIdx];
                    action(
                        new Entity(world, id),
                        ref pool1.GetUnsafe(id),
                        ref pool2.GetUnsafe(id),
                        ref pool3.GetUnsafe(id),
                        ref pool4.GetUnsafe(id),
                        ref pool5.GetUnsafe(id),
                        ref pool6.GetUnsafe(id),
                        ref pool7.GetUnsafe(id),
                        ref pool8.GetUnsafe(id));
                }
            });
            world.UnlockComponentPools();
        }
    }

}