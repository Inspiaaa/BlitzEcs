using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BlitzEcs.Benchmarks {
    public class ExampleBenchmark : MonoBehaviour
    {
        public struct Position {
            float x, y, z;

            public Position(float x, float y, float z) {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }

        public struct Flag {

        }

        public struct Rotation {
            float angle;

            public Rotation(float angle) {
                this.angle = angle;
            }
        }


        public class BenchQueryForeachOneComponent : Benchmark {
            private World world;

            public BenchQueryForeachOneComponent(int entityCount) {
                world = new World();

                for (int i = 0; i < entityCount; i ++) {
                    world.Spawn().Add<Position>();
                }
            }

            public void Run(int n) {
                var query = new Query<Position>(world);

                for (int i = 0; i < n; i ++){
                    query.ForEach((Entity e, ref Position pos) => {});
                }
            }
        }

        public class BenchQueryFetchOneComponent : Benchmark {
            private World world;

            public BenchQueryFetchOneComponent(int entityCount) {
                world = new World();

                for (int i = 0; i < entityCount; i ++) {
                    world.Spawn().Add<Position>();
                }
            }

            public void Run(int n) {
                var query = new Query<Position>(world);

                for (int i = 0; i < n; i ++){
                    query.Fetch();
                }
            }
        }

        public class BenchParallelQueryForeachOneComponent : Benchmark {
            private World world;

            public BenchParallelQueryForeachOneComponent(int entityCount) {
                world = new World();

                for (int i = 0; i < entityCount; i ++) {
                    world.Spawn().Add<Position>();
                }
            }

            public void Run(int n) {
                var query = new Query<Position>(world);

                for (int i = 0; i < n; i ++){
                    query.ParallelForEach((Entity e, ref Position pos) => {});
                }
            }
        }

        public class BenchQueryForeachTwoComponents : Benchmark {
            private World world;

            public BenchQueryForeachTwoComponents(int entityCount) {
                world = new World();

                for (int i = 0; i < entityCount; i ++) {
                    world.Spawn().Add<Position>().Add<Rotation>();
                }
            }

            public void Run(int n) {
                var query = new Query<Position, Rotation>(world);

                for (int i = 0; i < n; i ++){
                    query.ForEach((Entity e, ref Position pos, ref Rotation rot) => {});
                }
            }
        }

        public class BenchHotQueryForeachTwoComponents : Benchmark {
            private World world;

            public BenchHotQueryForeachTwoComponents(int entityCount) {
                world = new World();

                for (int i = 0; i < entityCount; i ++) {
                    world.Spawn().Add<Position>().Add<Rotation>();
                }
            }

            public void Run(int n) {
                var query = world.GetCachedQuery(new Query<Position, Rotation>(world));

                for (int i = 0; i < n; i ++){
                    query.ForEach((Entity e, ref Position pos, ref Rotation rot) => { });
                }
            }
        }


        private void Start() {
            Application.targetFrameRate = 10;
            StartCoroutine(RunBenchmarks());
        }

        public IEnumerator RunBenchmarks() {
            yield return null;

            int entityCount = 10000;
            int iterations = 100;

            Benchmark.LogProfile(
                $"Foreach on {entityCount} entities with Position",
                new BenchQueryForeachOneComponent(entityCount).Run, iterations
            );
            yield return null;

            Benchmark.LogProfile(
                $"Query.Fetch for {entityCount} entities with Position",
                new BenchQueryFetchOneComponent(entityCount).Run, iterations
            );
            yield return null;

            Benchmark.LogProfile(
                $"Foreach on {entityCount} entities (parallel) with Position",
                new BenchParallelQueryForeachOneComponent(entityCount).Run, iterations
            );
            yield return null;

            Benchmark.LogProfile(
                $"Foreach on {entityCount} entities with Position and Rotation",
                new BenchQueryForeachTwoComponents(entityCount).Run, iterations
            );
            yield return null;

            Benchmark.LogProfile(
                $"Foreach on {entityCount} entities (hot query) with Position and Rotation",
                new BenchHotQueryForeachTwoComponents(entityCount).Run, iterations
            );
            yield return null;
        }
    }
}
