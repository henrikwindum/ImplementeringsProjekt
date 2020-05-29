using System;
using System.Numerics;

namespace Implementeringsprojekt {
    public class Hashtable {
            class hashentry
            {
                ulong key;
                string data;
                public hashentry(ulong key, string data)
                {
                    this.key = key;
                    this.data = data;
                }
                public int getkey()
                {
                    return key;
                }
                public string getdata()
                {
                    return data;
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
            public string get(ulong key) {
                int hash = 0;
                if (hashMethod == "Shift") {
                    hash = (int)hashFunctions.MultiplyShift(key, lSize);
                } else if (hashMethod == "ModPrime") {
                    hash = (int)hashFunctions.MultiplyModPrime(key, lSize);
                }
                while (table[hash] != null && (ulong) table[hash].getkey() != key)
                {
                    hash = (hash + 1) % maxSize;
                }
                if (table[hash] == null)
                {
                    return "No entry";
                }
                else
                {
                    return table[hash].getdata();
                }
            }
            public void set(int key, string data)
            {
                int hash = (key % maxSize);
                while (table[hash] != null && table[hash].getkey() != key)
                {
                    hash = (hash + 1) % maxSize;
                }
                table[hash] = new hashentry(key, data);
            }
            public void quadraticHashInsert(int key, string data)
            {
                //quadratic probing method
                int j = 0;
                int hash = key % maxSize;
                while(table[hash] != null && table[hash].getkey() != key)
                {
                    j++;
                    hash = (hash + j * j) % maxSize;
                }
                if (table[hash] == null)
                {
                    table[hash] = new hashentry(key, data);
                    return;
                }
            }
            public void doubleHashInsert(int key, string data)
            {
                //double probing method
                int hashVal = hash1(key);
                int stepSize = hash2(key);
        
                while(table[hashVal] != null && table[hashVal].getkey() != key)
                {
                    hashVal = (hashVal + stepSize * hash2(key)) % maxSize;
                }
                table[hashVal] = new hashentry(key, data);
                return;
            }
            private int hash1(int key)
            {
                return key % maxSize;
            }
            private int hash2(int key)
            {
                //must be non-zero, less than array size, ideally odd
                return 5 - key % 5;
            } 
        }
}
