using System.Collections;
using System.Collections.Generic;

namespace BlitzEcs {
    public interface IComponentPool {
        int Count { get; }
        int PoolId { get; }

        int HighestEntityId { get; }

        bool Contains(int entityId);
        void Remove(int entityId);
        void ExecuteBufferedRemoves();

        // Dense array
        int[] RawEntityIds { get; }
        // Sparse array
        int[] RawEntityIdsToComponentIdx { get; }
        PoolEntityIdEnumerator EntityIds { get; }
    }
}
