using System.Collections;
using System.Collections.Generic;
using System;

namespace Ecs {
    public interface IEntityManager {
        void OnAddComponentToEntity(int entityId, int poolId);
        void OnRemoveComponentFromEntity(int entityId, int poolId);
    }
}
