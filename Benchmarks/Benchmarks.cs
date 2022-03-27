
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace Ecs.Benchmarks {
    public class Benchmark {
        public static double Profile(Action<int> action, int iterations) {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;

            // Warm up.
            action(1);

            Stopwatch watch = new Stopwatch();

            // Clean up.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            watch.Start();
            action(iterations);
            watch.Stop();

            return watch.Elapsed.TotalSeconds;
        }

        public static double LogProfile(String descr, Action<int> action, int iterations) {
            UnityEngine.Debug.Log(descr);
            double time = Profile(action, iterations);
            UnityEngine.Debug.Log($"Time for {iterations} iterations: {time}s");
            UnityEngine.Debug.Log($"Time per iteration: {time / iterations * 1000000}us");
            UnityEngine.Debug.Log("------");
            return time;
        }
    }
}

