using System;
using System.Collections.Generic;

namespace BlitzEcs {
    public class SystemGroup {
        private List<ISystem> systems = new List<ISystem>();

        public SystemGroup Add(ISystem system) {
            systems.Add(system);
            return this;
        }

        public void Update() {
            foreach (ISystem system in systems) {
                system.Update();
            }
        }
    }
}
