using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using BlitzEcs;
using System;

namespace BlitzEcs.Tests {
    public class TestComponentPool {
        private class MockEntityManager : IEntityManager {
            public void OnAddComponentToEntity(int entityId, int poolId) { }
            public void OnRemoveComponentFromEntity(int entityId, int poolId) { }
            public bool ArePoolsLocked => false;
        }

        private ComponentPool<int> pool;

        [SetUp]
        public void Setup() {
            pool = new ComponentPool<int>(new MockEntityManager(), 0);
        }

        [Test]
        public void Test_pool_should_start_empty() {
            Assert.AreEqual(pool.Count, 0);
        }

        [Test]
        public void Test_adding_changes_count() {
            pool.Add(5, 0);
            pool.Add(3, 0);
            pool.Add(10, 0);
            Assert.AreEqual(pool.Count, 3);
        }

        [Test]
        public void Test_get_works_after_add() {
            pool.Add(5, 10);
            pool.Add(0, 50);
            pool.Add(3, 20);

            Assert.AreEqual(10, pool.Get(5));
            Assert.AreEqual(50, pool.Get(0));
            Assert.AreEqual(20, pool.Get(3));
        }

        [Test]
        public void Test_getting_non_existant_entity_fails() {
            Assert.Throws<InvalidOperationException>(() => pool.Get(0));
            Assert.Throws<InvalidOperationException>(() => pool.Get(1));
            Assert.Throws<InvalidOperationException>(() => pool.Get(2));
        }

        [Test]
        public void Test_contains_is_false_for_non_existant_entities() {
            pool.Add(1, 10);
            pool.Add(10, 10);

            Assert.IsFalse(pool.Contains(0));
            Assert.IsFalse(pool.Contains(2));
            Assert.IsFalse(pool.Contains(50));
        }

        [Test]
        public void Test_contains_is_true_for_existing_entities() {
            pool.Add(0, 10);
            pool.Add(1, 10);
            pool.Add(10, 10);

            Assert.IsTrue(pool.Contains(0));
            Assert.IsTrue(pool.Contains(1));
            Assert.IsTrue(pool.Contains(10));
        }

        [Test]
        public void Test_remove_reduces_count() {
            pool.Add(1);
            pool.Add(2);
            pool.Add(3);
            pool.Remove(1);
            pool.Remove(2);

            Assert.AreEqual(1, pool.Count);
        }

        [Test]
        public void Test_getting_last_entity_works_after_removing_other_entity() {
            pool.Add(1, 10);
            pool.Add(2, 20);
            pool.Add(3, 30);
            pool.Remove(2);

            Assert.AreEqual(30, pool.Get(3));
        }

        [Test]
        public void Test_getting_middle_entity_works_after_removing_other_entity() {
            pool.Add(1, 10);
            pool.Add(2, 20);
            pool.Add(3, 30);
            pool.Remove(1);

            Assert.AreEqual(20, pool.Get(2));
        }

        [Test]
        public void Test_getting_entity_after_removing_it_fails() {
            pool.Add(1, 10);
            pool.Add(2, 20);
            pool.Remove(1);

            Assert.Throws<InvalidOperationException>(() => pool.Get(1));
        }

        private struct MyComponent : IEcsDestroyHandler<MyComponent> {
            Action action;

            public MyComponent(Action action) => this.action = action;

            public void OnDestroy(ref MyComponent component) {
                component.action();
            }
        }

        [Test]
        public void Test_auto_destroyer_is_called_when_a_component_is_removed() {
            var pool = new ComponentPool<MyComponent>(new MockEntityManager(), 0);

            bool wasE0Destroyed = false;
            bool wasE1Destroyed = false;

            pool.Add(0, new MyComponent(() => wasE0Destroyed = true));
            pool.Add(1, new MyComponent(() => wasE1Destroyed = true));

            pool.Remove(0);
            pool.Remove(1);

            Assert.True(wasE0Destroyed);
            Assert.True(wasE1Destroyed);
        }
    }
}
