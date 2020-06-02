using System;
using System.Collections.Generic;
using System.Numerics;

namespace Implementeringsprojekt
{
    public class HashFunctions {
        private ulong shiftA =(ulong)BitGenerator.RandomInRange(0, BigInteger.Pow(2, 63) - 1);
        
        private BigInteger bigA = BitGenerator.RandomArray(1)[0];
        private BigInteger bigB = BitGenerator.RandomArray(1)[0];
        private BigInteger bigP = new BigInteger(Math.Pow(2,89)-1);
        private BigInteger r;
        private BigInteger[] bigAValues = BitGenerator.RandomArray(4);
        
        
        // Constructor for hash functions. Generates and checks randomized values
        public HashFunctions(int l) {
            while (bigA >= bigP) {
                bigA = BitGenerator.RandomArray(1)[0];
            }
            while (bigB >= bigP) {
                bigB = BitGenerator.RandomArray(1)[0];
            }

            while (shiftA % 2 == 0) {
                shiftA = (ulong)BitGenerator.RandomInRange(0, BigInteger.Pow(2, 63) - 1);
            }

            for (int i = 0; i < 4; i++) {
                while (bigAValues[i] >= bigP) {
                    bigAValues[i] = BitGenerator.RandomArray(1)[0];
                }
            }
            
            r = new BigInteger(Math.Pow(2,l));
        }
        // Implementation of simple multiply-shift function, where shiftA is a random ulong value.
        public ulong MultiplyShift(ulong x, int l){
            return (shiftA * x) >> (64 - l);
        }

        // Modulos function based on implementation from Exercise 2.7-2.8 in notes.
        private BigInteger Modulos(BigInteger x, BigInteger p, int q){
            BigInteger y = (x&p) + (x>>q);
            if (y>=p){
                y -= p;
            }
            return y;
        }
        // Multiply-mod-prime implementation using modulus with Mersenne prime p.
        public BigInteger MultiplyModPrime(ulong x){
            BigInteger newX = (bigA * x + bigB);
            BigInteger y = Modulos(newX,bigP, 89);
            
            return y % r;
        }
        // Four universal hashing based on second-moment estimation providing an 
        // approximate version of full universal 4-way hashing.
        public BigInteger FourUniversal(ulong x) {
            BigInteger y = bigAValues[3];
            for (int i = 2; i >= 0; i--) {
                y = (y * BigInteger.Pow(x, i+1) + bigAValues[i]);
                //y = (y&bigP) + (y>>89);
            }
/*            if (y >= bigP) {
                y -= bigP;
            }*/
            return y % bigP;
        }
        // Calculation of hashing value based on the Four universal output
        public int HCalc(ulong x, int m) {
            return (int) (FourUniversal(x) % m);
        }
        // Calculation of s, which returns either 1 og -1 based on most significant bit
        public int SCalc(ulong x) {
            int b = (int)(FourUniversal(x) >> 88);
            return 1 - 2*b;
        }
        // Implementation of a basic Count Sketch algorithm based on version in hashing notes.
        public long[] CountSketch(IEnumerable<Tuple<ulong, int>> stream, int m) {
            long[] c = new long[(int) m];
            for (int i = 0; i < c.Length; i++) {
                c[i] = 0;
            }
            foreach (var (x, delta) in stream) {
                c[HCalc(x, m)] = c[HCalc(x, m)] + delta * SCalc(x);
            }
            return c;
        }
        // Calculation of estimate based on Count Sketch result
        public long Estimate(long[] sketches) {
            long sum = 0;
            foreach (var count in sketches) {
                sum += (long)Math.Pow(count, 2);
            }
            return sum;
        }
    }
}