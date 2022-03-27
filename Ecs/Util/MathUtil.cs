using System.Collections;
using System.Collections.Generic;
using System;

namespace Ecs {
    public static class MathUtil {
        public static int NextMultipleOfN(int num, int b)
            => ((num + b - 1) / b) * b;

        public static int NextMultipleOf2(int num)
            => ((num + 1) / 2) * 2;
    }
}
