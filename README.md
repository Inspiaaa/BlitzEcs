# BlitzEcs

BlitzEcs is a lightning-fast, memory-efficient, sparse set based ECS (Entity Component System) framework written in C# for making games. It is bare bones, can be easily integrated into existing systems / games, and can be used side by side with object oriented design to allow for a hybrid approach.

### Why use BlitzEcs?

- Lightweight and efficient

- Minimal amount of GC allocations

- Game engine agnostic (can be used with Unity, Monogame, your own engine, ...)

- Memory efficient:
  
  - Internal pools shrink to maintain a minimal memory footprint (with amortised `O(1)` operations)
  - Smart entity id recycling

- Multi-threading support that is incredibly easy to use

- Simple API

- No runtime code generation required (can be used with AOT compilation)

- Pure C# code

- No external dependencies

### Quick overview

##### Importing the framework:

```csharp
using BlitzEcs;
```

##### Creating components

```csharp
public struct Transform {
    public float x, y, angle;
}

public struct Velocity {
    public float x, y;
}

// Works like a flag
public struct EnemyTeam { }
```

##### Creating a world

```csharp
var world = new World();
```

##### Spawning in entities

```csharp
// First entity
world
    .Spawn()
    .Add<Transform>(new Transform { x = 10, y = 15, angle = 0})
    .Add<Velocity>();

// Second entity
Entity entity = world.Spawn();
entity.Add<EnemyTeam>();
entity.Add<Transform>();

ref Transform transform = ref entity.Get<Transform>();
transform.x = 2;
transform.y = 10;
```

##### Removing components

```csharp
entity.Remove<EnemyTeam>();
```

##### Despawning entities

```csharp
entity.Despawn();
```

Entities are also despawned when they have no components left:

```csharp
var entity = world.Spawn().Add<Transform>();
entity.Remove<Transform>();
```

Don't use the `Entity` instance after despawning, as this may lead to errors or unexpected behaviour when the entity id is recycled.

##### Creating systems

```csharp
var query = new Query<Transform>(world);
query.ForEach((ref Transform t) => {
    // ...
});

// Has an overload that lets you access the entity
query.ForEach((Entity e, ref Transform t) => {
    // ...
});

var query2 = new Query<Transform, Velocity>(world);
query2.ForEach((ref Transform t, ref Velocity vel) => {
    // ...
});
```

As the `Query` class allocates memory the first time it is created and queried, it is recommended to reuse the same query instance many times. This allows the query to reuse the memory that it allocated the first time it was run.

This works very nicely when you create custom systems, e.g.

```csharp
public class ApplyVelocityToTransformsSys {
    private Query<Transform, Velocity> query;

    public ApplyVelocityToTransformsSys(World world) {
        query = new Query<Transform, Velocity>(world);
    }

    public void Update(float deltaTime) {
        query.ForEach((ref Transform transform, ref Velocity vel) => {
            transform.x += vel.x * deltaTime;
            transform.y += vel.y * deltaTime;
        });
    }
}
```

Then the main game loop could look like this:

```csharp
public class Game {
    // ...
    private World world;

    // Systems
    private ApplyVelocityToTransformsSys sys;

    public Game() {
        world = new World();
        // ...

        sys = new ApplyVelocityToTransformsSys(world);
    }

    public void Update(float deltaTime) {
        sys.Update(deltaTime);
    }
}
```

You can still add and remove components during iteration. To avoid any problems with the iterator, components are automatically removed after iteration.

##### Parallel processing

To use multi-threading to process the entities in parallel, you only have to change the `ForEach` call to `ParallelForEach`:

```csharp
query.ParallelForEach((ref Transform transform, ref Velocity vel) => {
    transform.x += vel.x * deltaTime;
    transform.y += vel.y * deltaTime;
});
```

You can also pass in the chunk size to define how many entities are processed on each thread. 64 is the default.

```csharp
query.ParallelForEach(
    (ref Transform transform, ref Velocity vel) => {
        transform.x += vel.x * deltaTime;
        transform.y += vel.y * deltaTime;
    },
    chunkSize: 64
);
```

Warning: Most of the API of BlitzEcs is not thread-safe. To avoid any problems with parallel processing, it is recommended to only use this feature for systems that process the components (i.e. change values, read values, ...) but don't create new entities, new components, remove components, ... as shown in the example above.

##### Excluding and including components

BlitzEcs allows you to also filter the entities of a query further by letting you exclude and include more components. These are however not directly included in the parameters of the `ForEach` method.

```csharp
// Matches entities that have a Transform and a Velocity component
// but not an EnemyTeam component.

var query = new Query<Transform>(world);
query.Inc<Velocity>();
query.Exc<EnemyTeam>();

query.ForEach((ref Transform transform) => {});
```

##### Iterating over entities manually

```csharp
// This method is automatically called when you use query.ForEach(...).
query.Fetch();

foreach (Entity entity in query) {
    if (entity.Has<Velocity>()) {
        // ...
    }

    ref Transform transform = ref entity.Get<Transform>();
}
```

Be aware that the `ForEach` method is usually faster for iterating over the matched entities than using a `foreach` loop that manually gets the components.

BlitzEcs uses component pools internally and it can sometimes be useful to interact with them directly (e.g. to achieve greater performance when using `foreach` loops). But to be honest, this is not a feature you would normally need to use to make a game.

```csharp
ComponentPool<Transform> transforms = world.GetComponentPool<Transform>();
ComponentPool<Velocity> velocities = world.GetComponentPool<Velocity>();

// If we want to remove components from the entities we are iterating over,
// it is recommended to lock the component pools so that the components are only
// removed after iteration. (Not necessary for adding components)
world.LockComponentPools();

query.Fetch();

foreach (Entity entity in query) {
    if (velocities.Contains(entity.Id)) {
        // ...
    }

    // As we can be sure that every matched entity has a transform component
    // we don't have to do checks. (i.e. use GetUnsafe() instead of Get()).
    ref Transform transform = ref transforms.GetUnsafe(entity.Id);
}

world.UnlockComponentPools();
```



### Credits

This project is greatly inspired by [Byteron/ecs](https://github.com/Byteron/ecs).
