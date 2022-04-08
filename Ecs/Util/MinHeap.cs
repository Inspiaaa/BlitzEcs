using System.Collections;
using System.Collections.Generic;
using System;

namespace BlitzEcs.Util {
    public class MinHeap {
        /*
        Indices of nodes:
        |     0
        |  1     2
        | 3 4   5 6

        Every layer is full (except the last one which is filled from the left)
        */

        private int[] elements;
        private int count;

        public int Count => count;

        public MinHeap() {
            elements = new int[8];
        }

        private int GetLeftChildIndex(int parentIndex) => 2 * parentIndex + 1;
        private int GetRightChildIndex(int parentIndex) => 2 * parentIndex + 2;
        private int GetParentIndex(int childIndex) => (childIndex - 1) / 2;

        private bool HasLeftChild(int parentIndex) => GetLeftChildIndex(parentIndex) < count;
        private bool HasRightChild(int parentIndex) => GetRightChildIndex(parentIndex) < count;
        private bool HasParent(int childIndex) => GetParentIndex(childIndex) >= 0;

        private int GetLeftChild(int parentIndex) => elements[GetLeftChildIndex(parentIndex)];
        private int GetRightChild(int parentIndex) => elements[GetRightChildIndex(parentIndex)];
        private int GetParent(int childIndex) => elements[GetParentIndex(childIndex)];

        private void Swap(int indexA, int indexB) {
            int temp = elements[indexA];
            elements[indexA] = elements[indexB];
            elements[indexB] = temp;
        }

        private void EnsureExtraCapacity() {
            if (count == elements.Length) {
                Array.Resize(ref elements, count * 2);
            }
        }

        public int Peek() {
            // Returns the smallest item in the heap.
            if (count == 0) throw new IndexOutOfRangeException("Peek from empty MinHeap.");
            return elements[0];
        }

        public int Pop() {
            if (count == 0) throw new IndexOutOfRangeException("Pop from empty MinHeap.");

            int value = elements[0];

            // Move the last element to the root of the tree.
            elements[0] = elements[count - 1];
            count --;
            // Restore the heap property.
            HeapifyDown();

            return value;
        }

        public void Push(int value) {
            EnsureExtraCapacity();
            elements[count] = value;
            count ++;
            HeapifyUp();
        }

        private void HeapifyDown() {
            // Restores the heap property after changing the root node.

            int currentNode = 0;

            while (HasLeftChild(currentNode)) {
                int smallerChildNode = GetLeftChildIndex(currentNode);
                if (HasRightChild(currentNode) && GetRightChild(currentNode) < GetLeftChild(currentNode)) {
                    smallerChildNode = GetRightChildIndex(currentNode);
                }

                if (elements[currentNode] < elements[smallerChildNode]) {
                    break;
                }
                else {
                    Swap(currentNode, smallerChildNode);
                }
                currentNode = smallerChildNode;
            }
        }

        private void HeapifyUp() {
            // Restores the heap property after pushing an item (to the leftmost node in the last level).

            int currentNode = count - 1;

            while (HasParent(currentNode) && GetParent(currentNode) > elements[currentNode]) {
                int parentNode = GetParentIndex(currentNode);
                Swap(parentNode, currentNode);
                currentNode = parentNode;
            }
        }
    }
}