using System.Collections;
using System.Collections.Generic;

namespace Ecs {
    public interface IEntityManager {
        void DecComponentCount(int entityId);
        void IncComponentCount(int entityId);
    }
}
