using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ecs.Benchmarks {
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


        public class BenchQueryOneComponent : Benchmark {
            private World world;

            public BenchQueryOneComponent(int entityCount) {
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

        public class BenchQueryTwoComponents : Benchmark {
            private World world;

            public BenchQueryTwoComponents(int entityCount) {
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


        private void Start() {
            Application.targetFrameRate = 10;
            StartCoroutine(RunBenchmarks());
        }

        public IEnumerator RunBenchmarks() {
            yield return null;

            int entityCount = 1000;
            int iterations = 100;

            Benchmark.LogProfile(
                $"Querying {entityCount} entities with Position",
                new BenchQueryOneComponent(entityCount).Run, iterations
            );
            yield return null;

            Benchmark.LogProfile(
                $"Querying {entityCount} entities with Position and Rotation",
                new BenchQueryTwoComponents(entityCount).Run, iterations
            );
            yield return null;
        }
    }
}
