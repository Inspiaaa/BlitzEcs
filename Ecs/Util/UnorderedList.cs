using System.Collections;
using System.Collections.Generic;
using System;

namespace Ecs {
    public class UnorderedList<T> {
        private T[] values;
        private int count;

        public int Count => count;

        public UnorderedList() {
            values = new T[1];
            count = 0;
        }

        public void Add(T value) {
            if (count + 1 > values.Length) {
                Array.Resize(ref values, MathUtil.NextMultipleOf2(count + 1));
            }

            values[count] = value;
            count ++;
        }

        public T Remove(int index) {
            int lastIdx = count - 1;
            T value = values[index];

            values[index] = values[lastIdx];
            values[lastIdx] = default;

            count --;
            return value;
        }

        public T this[int idx] {
            get => values[idx];
            set => values[idx] = value;
        }

        public Enumerator GetEnumerator() => new Enumerator(this);

        public struct Enumerator {
            private int idx;
            private UnorderedList<T> list;

            public Enumerator(UnorderedList<T> list) {
                this.list = list;
                this.idx = -1;
            }

            public T Current => list.values[idx];

            public bool MoveNext() {
                return ++idx < list.count;
            }
        }
    }
}
