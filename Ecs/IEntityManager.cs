using System.Collections;
using System.Collections.Generic;
using System;

namespace BlitzEcs {
    public interface IEntityManager {
        void OnAddComponentToEntity(int entityId, int poolId);
        void OnRemoveComponentFromEntity(int entityId, int poolId);
        bool ArePoolsLocked { get; }
    }
}
