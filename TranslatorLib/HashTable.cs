using System;
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

    public class HashTableSubList
    {
        public readonly int KeyHash;
        public readonly List<HashTableItem> Items;

        public HashTableSubList(int keyHash)
        {
            KeyHash = keyHash;
            Items = new();
        }

        public void AddItem(string Key, int khash, object Object)
        {
            if (khash != KeyHash)
            {
                throw new ArgumentException("Invalid key hash");
            }

            HashTableItem? existing = GetItemUnchecked(Key, khash);
            if (existing != null)
            {
                throw new InvalidOperationException("Key already exists");
            }

            HashTableItem newItem = new(Key, KeyHash, Object);
            Items.Add(newItem);
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
    }

    public class HashTable
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

        public void AddItem(string Key, object obj)
        {
            int keyHash = CalculateHash(Key);
            HashTableSubList? subList = Sublists[keyHash];
            if (subList is null)
            {
                subList = new(keyHash);
                Sublists.Add(keyHash, subList);
            }

            subList.AddItem(Key, keyHash, obj);
        }

        public object GetItem(string Key)
        {
            int keyHash = CalculateHash(Key);
            HashTableSubList? subList = Sublists[keyHash];
            return subList?.GetItem(Key, keyHash) ?? throw new KeyNotFoundException();
        }

        public object this[string key]
        {
            get { return GetItem(key); }
            set { AddItem(key, value); }
        }

        public void RemoveItem(string Key)
        {
            int keyHash = CalculateHash(Key);
            HashTableSubList? subList = Sublists[keyHash];
            subList?.RemoveItem(Key, keyHash);
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
