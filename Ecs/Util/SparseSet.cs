using System.Collections;
using System.Collections.Generic;
using System;

namespace BlitzEcs.Util {
    public class SparseSet<T> {
        private int[] sparse;
        private int[] dense;
        // Aligned to the dense list
        private T[] denseValues;

        private int count;
        public int Count => count;

        public bool AutoShrink;

        public T[] DirectValues => denseValues;
        public int[] DirectKeys => dense;
        public int[] DirectKeysToValueIdx => sparse;

        public KeyEnumerator Keys => new KeyEnumerator(this);
        public ValueEnumerator Values => new ValueEnumerator(this);

        public SparseSet(bool autoPurge = true) {
            this.AutoShrink = autoPurge;

            sparse = new int[1];
            dense = new int[1];
            denseValues = new T[1];
        }

        private int GetComponentIdx(int key) {
            if (key >= sparse.Length)
                return -1;

            int componentIdx = sparse[key];
            if (componentIdx >= count)
                return -1;
            if (key != dense[componentIdx])
                return -1;

            return componentIdx;
        }

        public bool Contains(int key) => GetComponentIdx(key) != -1;

        public void Add(int key) => Add(key, default);

        public void Add(int key, T component) {
            int valueIdx = count;

            if (valueIdx >= denseValues.Length) {
                int newCapacity = MathUtil.NextPowerOf2(valueIdx + 1);
                Array.Resize(ref denseValues, newCapacity);
                Array.Resize(ref dense, newCapacity);
            }

            if (key + 1 >= sparse.Length) {
                Array.Resize(ref sparse, MathUtil.NextPowerOf2(key + 1));
            }

            denseValues[valueIdx] = component;
            dense[valueIdx] = key;
            sparse[key] = valueIdx;

            count ++;
        }

        public void Remove(int key) {
            if (!Contains(key)) {
                return;
            }

            int lastCompIdx = count - 1;
            int valueIdxToBeDeleted = sparse[key];
            int lastKey = dense[lastCompIdx];
            T lastComponent = denseValues[lastCompIdx];

            // Replace the item to be deleted with the last item.
            denseValues[valueIdxToBeDeleted] = lastComponent;
            dense[valueIdxToBeDeleted] = lastKey;
            // Reset the last value to free any GC references.
            denseValues[lastCompIdx] = default;
            sparse[lastKey] = valueIdxToBeDeleted;

            count --;

            if (AutoShrink && count <= dense.Length / 4) {
                Shrink();
            }
        }

        public ref T Get(int key) {
            int componentIdx = GetComponentIdx(key);
            if (componentIdx == -1) {
                throw new InvalidOperationException(
                    $"Cannot find key {key}."
                );
            }
            return ref denseValues[componentIdx];
        }

        public ref T GetUnsafe(int key) {
            return ref denseValues[sparse[key]];
        }

        public void SetCountUnsafe(int newCount) => count = newCount;

        public int HighestKey {
            get {
                int highestKey = 0;

                for (int i = 0; i < count; i ++) {
                    int key = dense[i];
                    if (key > highestKey) {
                        highestKey = key;
                    }
                }

                return highestKey;
            }
        }

        public void SetMinCapacity(int maxKeyInclusive, int count) {
            // TODO: Find a better name for this method (as it doesn't actually set the min capacity).
            if (maxKeyInclusive >= sparse.Length) {
                Array.Resize(ref sparse, MathUtil.NextPowerOf2(maxKeyInclusive + 1));
            }

            if (count > this.count) {
                int newCapacity = MathUtil.NextPowerOf2(count);
                Array.Resize(ref dense, newCapacity);
                Array.Resize(ref denseValues, newCapacity);
            }
        }

        public void Clear() {
            // Remove any GC references.
            Array.Clear(denseValues, 0, count);
            count = 0;
        }

        public void Shrink() {
            // Reduces the unnecessary buffer space to save memory.

            int highestKey = HighestKey;
            if (highestKey <= sparse.Length / 4) {
                Array.Resize(ref sparse, MathUtil.NextPowerOf2(highestKey + 1));
            }

            if (count <= dense.Length / 4) {
                int newCapacity = MathUtil.NextPowerOf2(count);
                Array.Resize(ref dense, newCapacity);
                Array.Resize(ref denseValues, newCapacity);
            }
        }

        public struct KeyEnumerator {
            private int idx;
            private int count;
            private int[] keys;

            public KeyEnumerator(SparseSet<T> set) {
                idx = -1;
                count = set.count;
                keys = set.dense;
            }

            public KeyEnumerator GetEnumerator() => this;

            public int Current => keys[idx];

            public bool MoveNext() {
                return ++idx < count;
            }
        }

        public struct ValueEnumerator {
            private int idx;
            private int count;
            private T[] values;

            public ValueEnumerator(SparseSet<T> set) {
                idx = -1;
                count = set.count;
                values = set.denseValues;
            }

            public ValueEnumerator GetEnumerator() => this;

            public T Current => values[idx];

            public bool MoveNext() {
                return ++idx < count;
            }
        }
    }
}
