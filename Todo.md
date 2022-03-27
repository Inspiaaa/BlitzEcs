- [x] world.ForEach( (Entity e, ref Position a, ...))
- [x] Queries
- [ ] Masks
- [x] Entity manager in World
- [x] Keep track of how many components an entity has and automatically despawn when the count hits 0
- [ ] Add excludes to queries
- [ ] Handle delete / add component during iteration => Maybe create a system to buffer the changes.
    => Add component does not change the query, only delete does => buffer deletes in ComponentPool
- [ ] Reduce indirection of Enumerators by caching the array in the struct enumerator.