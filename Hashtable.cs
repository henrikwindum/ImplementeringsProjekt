using System;
using System.Numerics;

namespace Implementeringsprojekt {
    public class Hashtable {
            class hashentry
            {
                ulong key;
                ulong data;
                public hashentry(ulong key, ulong data)
                {
                    this.key = key;
                    this.data = data;
                }
                public ulong getkey()
                {
                    return key;
                }
                public ulong getdata()
                {
                    return data;
                }

                public void incrementdata(ulong d) {
                    data = data + d;
                }
            }
            private int maxSize;
            private int lSize;
            private string hashMethod;
            private HashFunctions hashFunctions;
            
            hashentry[] table;
            
            public Hashtable(HashFunctions hashing, string hasher, int l) {
                maxSize = (int) Math.Pow(2, l);
                lSize = l;
                hashMethod = hasher;
                hashFunctions = hashing;
                table = new hashentry[maxSize];
                for (int i = 0; i < maxSize; i++)
                {
                    table[i] = null;
                }
            }
            public ulong get(ulong key) {
                int hash = hasher(key);
                while (table[hash] != null && table[hash].getkey() != key)
                {
                    hash = (hash + 1);
                }
                return table[hash] == null ? 0 : table[hash].getdata();
            }
            public void set(ulong key, ulong data) {
                int hash = hasher(key);
                while (table[hash] != null && table[hash].getkey() != key)
                {
                    hash = (hash + 1);
                }
                table[hash] = new hashentry(key, data);
            }

            public void increment(ulong key, ulong d) {
                int hash = hasher(key);
                while (table[hash] != null && table[hash].getkey() != key)
                {
                    hash = (hash + 1);
                }

                if (table[hash] == null) {
                    table[hash] = new hashentry(key, d);
                } else {
                    table[hash].incrementdata(d);  
                }
            }
            private int hasher(ulong key) {
                int hash = 0;
                switch (hashMethod) {
                case "Shift":
                    hash = (int)hashFunctions.MultiplyShift(key, lSize);
                    break;
                case "ModPrime":
                    hash = (int)hashFunctions.MultiplyModPrime(key, lSize);
                    break;
                }

                return hash;
            }
        }
}
