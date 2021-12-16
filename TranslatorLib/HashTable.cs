using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TranslatorLib
{
    internal static class Hashing
    {
        public static int Hash(string str)
        {
            int hash = 5381;
            foreach (int c in str)
            {
                unchecked
                {
                    hash = (hash << 5) + hash + c;
                }
            }
            return hash;
        }
    }

    [Serializable]
    public class HashTableItem
    {
        public readonly string Key;
        public int KeyHash;
        public object Object;

        public HashTableItem(string Key, int KeyHash, object Object)
        {
            this.Key = Key;
            this.KeyHash = KeyHash;
            this.Object = Object;
        }

    }

    public class HashTableSubList : IEnumerable<HashTableItem>
    {
        public readonly int KeyHash;
        public readonly List<HashTableItem> Items;

        public HashTableSubList(int keyHash)
        {
            KeyHash = keyHash;
            Items = new();
        }

        public bool TryAddItem(string Key, int khash, object Object)
        {
            if (khash != KeyHash)
            {
                throw new ArgumentException("Invalid key hash");
            }

            HashTableItem? existing = GetItemUnchecked(Key, khash);
            if (existing != null)
            {
                return false;
            }

            HashTableItem newItem = new(Key, KeyHash, Object);
            Items.Add(newItem);
            return true;
        }

        public void AddItem(string Key, int khash, object Object)
        {
            bool added = TryAddItem(Key, khash, Object);
            if (!added)
            {
                throw new InvalidOperationException("Key already exists");
            }
        }

        private HashTableItem? GetItemUnchecked(string Key, int khash)
        {
            return (from item in Items where item.Key == Key && item.KeyHash == khash select item).FirstOrDefault();
        }

        public HashTableItem GetItem(string Key, int khash)
        {
            if (khash != KeyHash)
            {
                throw new ArgumentException("Invalid key hash");
            }

            return GetItemUnchecked(Key, khash) ?? throw new KeyNotFoundException();
        }

        public void RemoveItem(string Key, int khash)
        {
            if (khash != KeyHash)
            {
                throw new ArgumentException("Invalid key hash");
            }

            HashTableItem? item = GetItemUnchecked(Key, khash);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        public bool HasKey(string Key, int khash)
        {
            if (khash != KeyHash)
            {
                throw new ArgumentException("Invalid key hash");
            }

            HashTableItem? item = GetItemUnchecked(Key, khash);
            return item != null;
        }

        public IEnumerator<HashTableItem> GetEnumerator()
        {
            return ((IEnumerable<HashTableItem>)Items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Items).GetEnumerator();
        }
    }

    public class HashTable : IEnumerable<HashTableItem>
    {
        private SortedList<int, HashTableSubList> Sublists;

        public HashTable()
        {
            Sublists = new();
        }

        private static int CalculateHash(string key)
        {
            return Hashing.Hash(key);
        }

        public bool TryAddItem(string Key, object obj)
        {
            int keyHash = CalculateHash(Key);
            bool sublistExists = Sublists.TryGetValue(keyHash, out HashTableSubList? sublist);
            if (!sublistExists || sublist is null)
            {
                sublist = new(keyHash);
                Sublists.Add(keyHash, sublist);
            }

            return sublist.TryAddItem(Key, keyHash, obj);
        }

        public void AddItem(string Key, object obj)
        {
            bool added = TryAddItem(Key, obj);
            if (!added)
            {
                throw new InvalidOperationException("Key already exists");
            }
        }

        public object GetItem(string Key)
        {
            int keyHash = CalculateHash(Key);
            _ = Sublists.TryGetValue(keyHash, out HashTableSubList? sublist);
            return sublist?.GetItem(Key, keyHash) ?? throw new KeyNotFoundException();
        }

        public object this[string key]
        {
            get { return GetItem(key); }
            set { AddItem(key, value); }
        }

        public void RemoveItem(string Key)
        {
            int keyHash = CalculateHash(Key);
            _ = Sublists.TryGetValue(keyHash, out HashTableSubList? sublist);
            sublist?.RemoveItem(Key, keyHash);
        }

        public bool HasKey(string Key)
        {
            int keyHash = CalculateHash(Key);
            _ = Sublists.TryGetValue(keyHash, out HashTableSubList? sublist);
            return sublist?.HasKey(Key, keyHash) ?? false;
        }

        IEnumerator<HashTableItem> IEnumerable<HashTableItem>.GetEnumerator()
        {
            foreach (var sublist in Sublists)
            {
                foreach (var item in sublist.Value)
                {
                    yield return item;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var sublist in Sublists)
            {
                foreach (var item in sublist.Value)
                {
                    yield return item;
                }
            }
        }
    }

    public class HashTableList
    {
        public HashTable PrimaryHashTable { get; }
        public HashTable SecondaryHashTable { get; }
        public HashTable SignHashTable { get; }

        public HashTableList()
        {
            PrimaryHashTable = new();
            SecondaryHashTable = new();
            SignHashTable = new();
        }

    }
}
