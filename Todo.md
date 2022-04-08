- [x] world.ForEach( (Entity e, ref Position a, ...))
- [x] Queries
- [x] Masks
- [x] Entity manager in World
- [x] Keep track of how many components an entity has and automatically despawn when the count hits 0
- [x] Add excludes to queries
- [x] Handle delete / add component during iteration => Maybe create a system to buffer the changes.
    => Add component does not change the query, only delete does => buffer deletes in ComponentPool
- [x] Reduce indirection of Enumerators by caching the array in the struct enumerator.
- [x] Add "Hot" queries => Queries that are used frequently and shouldn't be fetched every single frame.
- [ ] Add sorting to sparse sets

- [x] Add entity to hot queries on component add
- [x] Cache queries by their mask
- [ ] Add min capacity to sparse sets
- [ ] Add custom destroy handlers for components
- [x] Remove dependency on unity
- [ ] Move properties to the top of the file in ComponentPool.cs and SparseSet.cs