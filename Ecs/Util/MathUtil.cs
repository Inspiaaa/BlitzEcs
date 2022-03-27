using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Ecs {
    public static class MathUtil {
        public static int NextPowerOf2(int num)
            => 1 << Mathf.CeilToInt(Mathf.Log(num, 2));
    }
}
