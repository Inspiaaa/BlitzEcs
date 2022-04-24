using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using BlitzEcs;

namespace BlitzEcs.Tests {
    public class TestQuery
    {
        private World world;

        [SetUp]
        public void Setup() {
            world = new World();
        }

        [Test]
        public void Test_Fetch_finds_exact_matches_for_one_comp() {
            int e0 = world.Spawn().Add<int>();
            int e1 = world.Spawn().Add<int>();
            int e2 = world.Spawn().Add<int>();

            var query = new Query(world).Inc<int>();
            query.Fetch();

            (int[] ids, int count) = query.MatchedEntityIds;
            Assert.AreEqual(3, count);
            Assert.AreEqual(e0, ids[0]);
            Assert.AreEqual(e1, ids[1]);
            Assert.AreEqual(e2, ids[2]);
        }

        [Test]
        public void Test_Fetch_finds_matches_that_have_more_comps_than_the_required_one_comp() {
            int e0 = world.Spawn().Add<int>();
            int e1 = world.Spawn().Add<int>();
            int e2 = world.Spawn().Add<int>().Add<bool>();
            int e3 = world.Spawn().Add<int>().Add<bool>();

            var query = new Query(world).Inc<int>();
            query.Fetch();

            (int[] ids, int count) = query.MatchedEntityIds;
            Assert.AreEqual(4, count);
            Assert.AreEqual(e0, ids[0]);
            Assert.AreEqual(e1, ids[1]);
            Assert.AreEqual(e2, ids[2]);
            Assert.AreEqual(e3, ids[3]);
        }

        [Test]
        public void Test_Fetch_finds_exact_matches_for_two_comps() {
            int e0 = world.Spawn().Add<int>().Add<bool>();
            int e1 = world.Spawn().Add<int>().Add<bool>();
            int e2 = world.Spawn().Add<int>().Add<bool>();

            var query = new Query(world).Inc<int>().Inc<bool>();
            query.Fetch();

            (int[] ids, int count) = query.MatchedEntityIds;
            Assert.AreEqual(3, count);
            Assert.AreEqual(e0, ids[0]);
            Assert.AreEqual(e1, ids[1]);
            Assert.AreEqual(e2, ids[2]);
        }

        [Test]
        public void Test_Fetch_respects_exclude_components() {
            int e0 = world.Spawn().Add<int>();
            int e1 = world.Spawn().Add<int>().Add<bool>();
            int e2 = world.Spawn().Add<int>();

            var query = new Query(world).Inc<int>().Exc<bool>();
            query.Fetch();

            (int[] ids, int count) = query.MatchedEntityIds;
            Assert.AreEqual(2, count);
            Assert.AreEqual(e0, ids[0]);
            Assert.AreEqual(e2, ids[2]);
        }

        [Test]
        public void Test_fetching_multiple_times_does_the_same() {
            int e0 = world.Spawn().Add<int>();
            int e1 = world.Spawn().Add<int>().Add<bool>();
            int e2 = world.Spawn().Add<int>();

            var query = new Query(world).Inc<int>().Exc<bool>();
            query.Fetch();
            query.Fetch();
            query.Fetch();

            (int[] ids, int count) = query.MatchedEntityIds;
            Assert.AreEqual(2, count);
            Assert.AreEqual(e0, ids[0]);
            Assert.AreEqual(e2, ids[2]);
        }

        [Test]
        public void Test_hot_query_starts_with_all_matched_entities() {
            int e0 = world.Spawn().Add<int>();
            int e1 = world.Spawn().Add<int>().Add<bool>();
            int e2 = world.Spawn().Add<int>();

            var query = world.GetCached(new Query(world).Inc<int>().Exc<bool>());

            (int[] ids, int count) = query.MatchedEntityIds;
            Assert.AreEqual(2, count);
            Assert.AreEqual(e0, ids[0]);
            Assert.AreEqual(e2, ids[2]);
        }

        [Test]
        public void Test_hot_query_keeps_track_of_newly_added_comps() {
            var query = world.GetCached(new Query(world).Inc<int>().Exc<bool>());

            int e0 = world.Spawn().Add<int>();
            Entity e1 = world.Spawn().Add<int>();
            e1.Add<bool>();
            int e2 = world.Spawn().Add<int>();

            (int[] ids, int count) = query.MatchedEntityIds;
            Assert.AreEqual(2, count);
            Assert.AreEqual(e0, ids[0]);
            Assert.AreEqual(e2, ids[1]);
        }

        [Test]
        public void Test_hot_query_keeps_track_of_removed_comps() {
            Entity e0 = world.Spawn().Add<int>();
            Entity e1 = world.Spawn().Add<int>().Add<bool>();
            Entity e2 = world.Spawn().Add<int>();

            var query = world.GetCached(new Query(world).Inc<int>().Exc<bool>());

            world.Despawn(e0);

            (int[] ids, int count) = query.MatchedEntityIds;
            Assert.AreEqual(1, count);
            Assert.AreEqual(e2.Id, ids[0]);
        }

        [Test]
        public void Test_components_are_not_removed_during_iteration() {
            Entity e0 = world.Spawn().Add<int>();
            Entity e1 = world.Spawn().Add<int>();
            Entity e2 = world.Spawn().Add<int>();

            var query = new Query<int>(world);

            query.ForEach( (Entity e, ref int component) => {
                e.Remove<int>();
                Assert.True(e.Has<int>());
            });
        }

        [Test]
        public void Test_components_are_removed_after_iteration() {
            Entity e0 = world.Spawn().Add<int>().Add<bool>();
            Entity e1 = world.Spawn().Add<int>().Add<bool>();
            Entity e2 = world.Spawn().Add<int>().Add<bool>();

            var query = new Query<int>(world);

            query.ForEach( (Entity e, ref int component) => {
                e.Remove<int>();
            });

            Assert.False(e0.Has<int>());
            Assert.False(e1.Has<int>());
            Assert.False(e2.Has<int>());
        }

        [Test]
        public void Test_mirrored_query_stays_updated() {
            var source = world.GetCached(new Query(world).Inc<int>().Exc<bool>());
            var mirrored = new Query<int>(world);
            mirrored.Exc<bool>();
            world.Cache(mirrored);

            Entity e0 = world.Spawn().Add<int>().Add<bool>();
            Entity e1 = world.Spawn().Add<int>();
            Entity e2 = world.Spawn().Add<int>().Add<bool>();

            bool visitedE1 = false;
            mirrored.ForEach((Entity entity, ref int value) => {
                if (entity != e1) {
                    Assert.Fail($"Entity {entity} should not have been matched.");
                }
                visitedE1 = true;
            });

            Assert.True(visitedE1);
        }
    }
}
