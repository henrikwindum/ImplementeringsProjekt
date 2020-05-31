using System;
using System.Collections.Generic;
using System.Numerics;

namespace Implementeringsprojekt
{
    public class HashFunctions {
        private ulong shiftA =(ulong)BitGenerator.RandomInRange(0, BigInteger.Pow(2, 63) - 1);
        
        private BigInteger bigA = BitGenerator.RandomArray(1)[0];
        private BigInteger bigB = BitGenerator.RandomArray(1)[0];
        private BigInteger bigP = BitGenerator.RandomArray(1)[0];
        private BigInteger r;
        private BigInteger[] bigAValues = BitGenerator.RandomArray(4);

        public HashFunctions(int l) {
            while (bigA >= bigP) {
                bigA = BitGenerator.RandomArray(1)[0];
            }
            while (bigB >= bigP) {
                bigB = BitGenerator.RandomArray(1)[0];
            }

            for (int i = 0; i < 4; i++) {
                while (bigAValues[i] >= bigP) {
                    bigAValues[i] = BitGenerator.RandomArray(1)[0];
                }
            }
            
            r = new BigInteger(Math.Pow(2,l));
        }

        public ulong MultiplyShift(ulong x, int l){
            return (shiftA * x) >> (64 - l);
        }


        private BigInteger Modulos(BigInteger x, BigInteger p, int q){
            BigInteger y = (x&p) + (x>>q);
            if (y>=p){
                y -= p;
            }
            return y;
        }
        public BigInteger MultiplyModPrime(ulong x){
            BigInteger newX = (bigA * x + bigB);
            BigInteger y = Modulos(newX,bigP, 89);
            
            return y % r;
        }

        public BigInteger FourUniversal(ulong x) {
            BigInteger y = bigAValues[3];
            for (int i = 2; i >= 0; i--) {
                y = y * (ulong)Math.Pow(x, i+1) + bigAValues[i];
                y = (y&bigP) + (y>>89);
            }
            if (y >= bigP) {
                y -= bigP;
            }
            return y;
        }

        public BigInteger HCalc(ulong x, int m) {
            return FourUniversal(x) % m;
        }

        public int SCalc(ulong x) {
            int b = (int)(FourUniversal(x) >> 88);
            return 1 - 2*b;
        }

        public long[] CountSketch(IEnumerable<Tuple<ulong, int>> stream, int m) {
            long[] c = new long[m];
            for (int i = 0; i < c.Length; i++) {
                c[i] = 0;
            }
            foreach (var (x, delta) in stream) {
                c[(int) HCalc(x, m)] += delta * SCalc(x);
            }
            return c;
        }

        public long Estimate(IEnumerable<Tuple<ulong, int>> stream, int m) {
            long sum = 0;
            foreach (var count in CountSketch(stream, m)) {
                sum += (long)Math.Pow(count, 2);
            }
            return sum;
        }
    }
}