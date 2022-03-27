using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ecs;

public class Example : MonoBehaviour
{
    public struct Position {
        float x, y, z;

        public Position(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString() {
            return $"Pos({x}, {y}, {z})";
        }

        public static Position Zero => new Position(0, 0, 0);
    }

    public struct Velocity {
        float x, y, z;

        public Velocity(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString() {
            return $"Vel({x}, {y}, {z})";
        }
    }

    void Start() {
        Application.targetFrameRate = 20;

        World world = new World();
        Entity e1 = world
            .Spawn()
            .Add<Position>(Position.Zero);

        Entity e2 = world
            .Spawn()
            .Add<Position>(new Position(1, 2, 3))
            .Add<Velocity>(new Velocity(5, 6, 7));

        Entity e3 = world
            .Spawn()
            .Add<Position>(new Position(11, 12, 13))
            .Add<Velocity>(new Velocity(15, 16, 17));

        var query = new Query<Position, Velocity>(world);

        query.ForEach((Entity e, ref Position pos, ref Velocity vel) => {
            print($"{e.Id}: {pos} {vel}");
        });

        print("########");

        world.Despawn(e2);
        query.ForEach((Entity e, ref Position pos, ref Velocity vel) => {
            print($"{e.Id}: {pos} {vel}");
        });
    }
}
