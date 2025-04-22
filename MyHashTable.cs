using System;
using System.Collections.Generic;

namespace MovieLibraryApp
{
    public class HashNode<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public HashNode<TKey, TValue> Next { get; set; }
        public HashNode(TKey key, TValue value) { Key = key; Value = value; }
    }

    public class MyHashTable<TKey, TValue>
    {
        private readonly int capacity;
        private readonly HashNode<TKey, TValue>[] buckets;

        public MyHashTable(int capacity = 16)
        {
            this.capacity = capacity;
            buckets = new HashNode<TKey, TValue>[capacity];
        }

        private int GetBucketIndex(TKey key) => key.GetHashCode() & 0x7fffffff % capacity;

        public void Insert(TKey key, TValue value)
        {
            int index = GetBucketIndex(key);
            var head = buckets[index];
            while (head != null)
            {
                if (head.Key.Equals(key)) { head.Value = value; return; }
                head = head.Next;
            }
            var newNode = new HashNode<TKey, TValue>(key, value) { Next = buckets[index] };
            buckets[index] = newNode;
        }

        public TValue Get(TKey key)
        {
            var head = buckets[GetBucketIndex(key)];
            while (head != null)
            {
                if (head.Key.Equals(key)) return head.Value;
                head = head.Next;
            }
            throw new KeyNotFoundException();
        }

        public bool ContainsKey(TKey key)
        {
            var head = buckets[GetBucketIndex(key)];
            while (head != null)
            {
                if (head.Key.Equals(key)) return true;
                head = head.Next;
            }
            return false;
        }
    }
}