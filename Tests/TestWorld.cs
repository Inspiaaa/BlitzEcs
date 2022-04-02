using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Ecs;
using System;

namespace Ecs.Tests {
    public class TestWorld {

        private World world;

        [SetUp]
        public void Setup() {
            world = new World();
        }

        [Test]
        public void Test_spawn_should_create_entities_with_sequential_ids() {
            Entity e0 = world.Spawn();
            Entity e1 = world.Spawn();
            Entity e2 = world.Spawn();

            Assert.AreEqual(0, e0.Id);
            Assert.AreEqual(1, e1.Id);
            Assert.AreEqual(2, e2.Id);
        }

        [Test]
        public void Test_spawn_should_use_recycled_entity_ids() {
            Entity e0 = world.Spawn();
            Entity e1 = world.Spawn();
            Entity e2 = world.Spawn();

            world.Despawn(e1);
            Entity newE1 = world.Spawn();
            Assert.AreEqual(1, newE1.Id);

            world.Despawn(e2);
            Entity newE2 = world.Spawn();
            Assert.AreEqual(2, newE2.Id);

            world.Despawn(e0);
            Entity newE0 = world.Spawn();
            Assert.AreEqual(0, newE0.Id);
        }

        [Test]
        public void Test_GetComponentPool_should_create_new_pool() {
            ComponentPool<int> pool = world.GetComponentPool<int>();
            Assert.NotNull(pool);
        }

        [Test]
        public void Test_GetComponentPool_should_return_existing_pool_after_create() {
            ComponentPool<int> createdPool = world.GetComponentPool<int>();
            ComponentPool<int> gotPool = world.GetComponentPool<int>();
            Assert.AreEqual(createdPool, gotPool);
        }

        [Test]
        public void Test_adding_component_increments_world_entity_component_count() {
            Entity entity = world.Spawn();
            entity.Add<int>();
            Assert.AreEqual(1, world.GetComponentCount(entity));
        }

        [Test]
        public void Test_overwriting_component_with_add_does_not_change_world_entity_component_count() {
            Entity entity = world.Spawn();
            entity.Add<int>(0);
            entity.Add<int>(1);
            Assert.AreEqual(1, world.GetComponentCount(entity));
        }

        [Test]
        public void Test_removing_decrements_world_entity_component_count() {
            Entity entity = world.Spawn();
            entity.Add<int>();
            entity.Add<bool>();
            entity.Remove<int>();

            Assert.AreEqual(1, world.GetComponentCount(entity));
        }

        [Test]
        public void Test_spawned_entity_should_be_alive() {
            Entity entity = world.Spawn();
            Assert.True(world.IsEntityAlive(entity));
        }

        [Test]
        public void Test_removing_all_components_deletes_entity() {
            Entity entity = world.Spawn();
            entity.Add<int>();
            entity.Remove<int>();
            Assert.False(world.IsEntityAlive(entity));
        }
    }
}
