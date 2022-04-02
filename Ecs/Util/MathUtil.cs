using System.Collections;
using System.Collections.Generic;
using System;

namespace Ecs {
    public static class MathUtil {
        public static int NextPowerOf2(int num) {
            if (num == 0)
                return 0;

            int pow = 1;
            while (pow < num) {
                pow = pow << 1;
            }

            return pow;
        }
    }
}
