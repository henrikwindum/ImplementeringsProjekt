using System;
using System.Numerics;

namespace Implementeringsprojekt {
    public class Hashtable {
         // Class for storing hashed entry values
        class HashEntry {
            ulong key;
            long data;
            public HashEntry(ulong key, long data) {
                this.key = key;
                this.data = data;
            }
            public ulong GetKey() {
                return key;
            }
            public long GetData() { 
                return data;
            }
            public void IncrementData(long d) {
                data = data + d;
            }
        }
        private int size;
        private int lSize;
        private string hashMethod;
        private HashFunctions hashFunctions;
        
        HashEntry[] table;
        
        public Hashtable(HashFunctions hashing, string hasher, int l) {
            size = (int) Math.Pow(2, l);
            lSize = l;
            hashMethod = hasher;
            hashFunctions = hashing;
            table = new HashEntry[size+1];
            for (int i = 0; i < size; i++)
            {
                table[i] = null;
            }
        }
        public long Get(ulong key) {
            int hash = Hasher(key);
            while (table[hash] != null && table[hash].GetKey() != key)
            {
                hash = (hash + 1) % size;
            }
            return table[hash] == null ? 0 : table[hash].GetData();
        }
        public void Set(ulong key, long data) {
            int hash = Hasher(key);
            while (table[hash] != null && table[hash].GetKey() != key)
            {
                hash = (hash + 1) % size;
            }
            table[hash] = new HashEntry(key, data);
        }

        public void Increment(ulong key, long d) {
            int hash = Hasher(key);
            while (table[hash] != null && table[hash].GetKey() != key)
            {
                hash = (hash + 1) % size;
            }

            if (table[hash] == null) {
                table[hash] = new HashEntry(key, d);
            } else {
                table[hash].IncrementData(d);  
            }
        }

        public long CalcQuadraticSum() {
            long sum = 0;
            foreach (var s in table) {
                if (s != null) {
                    sum += (long) Math.Pow(s.GetData(), 2);
                }
            }

            return sum;
        }
        private int Hasher(ulong key) {
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
