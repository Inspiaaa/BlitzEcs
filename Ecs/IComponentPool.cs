using System.Collections;
using System.Collections.Generic;

namespace Ecs {
    public interface IComponentPool {
        int Count { get; }
        int PoolId { get; }

        bool Contains(int entityId);
        void Remove(int entityId);
        void ExecuteBufferedRemoves();

        int[] RawEntityIds { get; }
        PoolEntityIdEnumerator EntityIds { get; }
    }
}
