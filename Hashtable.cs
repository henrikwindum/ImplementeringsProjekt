using System;
using System.Numerics;

namespace Implementeringsprojekt {
    public class Hashtable {
            class hashentry
            {
                ulong key;
                long data;
                public hashentry(ulong key, long data)
                {
                    this.key = key;
                    this.data = data;
                }
                public ulong getkey()
                {
                    return key;
                }
                public long getdata()
                {
                    return data;
                }

                public void incrementdata(long d) {
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
                table = new hashentry[maxSize+1];
                for (int i = 0; i < maxSize; i++)
                {
                    table[i] = null;
                }
            }
            public long get(ulong key) {
                int hash = hasher(key);
                while (table[hash] != null && table[hash].getkey() != key)
                {
                    hash = (hash + 1) % maxSize;
                }
                return table[hash] == null ? 0 : table[hash].getdata();
            }
            public void set(ulong key, long data) {
                int hash = hasher(key);
                while (table[hash] != null && table[hash].getkey() != key)
                {
                    hash = (hash + 1) % maxSize;
                }
                table[hash] = new hashentry(key, data);
            }

            public void increment(ulong key, long d) {
                int hash = hasher(key);
                while (table[hash] != null && table[hash].getkey() != key)
                {
                    hash = (hash + 1) % maxSize;
                }

                if (table[hash] == null) {
                    table[hash] = new hashentry(key, d);
                } else {
                    table[hash].incrementdata(d);  
                }
            }

            public long calcQuadraticSum() {
                long sum = 0;
                foreach (var s in table) {
                    if (s != null) {
                        sum += (long) Math.Pow(s.getdata(), 2);
                    }
                }

                return sum;
            }
            private int hasher(ulong key) {
                int hash = 0;
                switch (hashMethod) {
                case "Shift":
                    hash = (int)hashFunctions.MultiplyShift(key, lSize);
                    break;
                case "ModPrime":
                    hash = (int)hashFunctions.MultiplyModPrime(key);
                    break;
                }

                return hash;
            }
        }
}
