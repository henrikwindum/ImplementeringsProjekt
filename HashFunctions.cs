using System;
using System.Collections.Generic;
using System.Numerics;

namespace Implementeringsprojekt
{
    public class HashFunctions {
        private ulong shiftA =
            0b10001000_01110110_10110000_01001011_10100011_00000000_10100001_11110001;

        private string a = "6925115517784187226122207";
        private string b = "583421377714931531722877";
        private string p = "618970019642690137449562111";
        private List<string> aVals = new List<string>{
            "109156851151214535168186206",
            "555719711119132272362531120",
            "501570207502331918116846222",
            "20156117174049194196186188226"
        };

        private BigInteger bigA;
        private BigInteger bigB;
        private BigInteger bigP;
        private BigInteger r;
        private List<BigInteger> BigaVals = new List<BigInteger>();

        public HashFunctions(int l) {
            bigA = BigInteger.Parse(a);
            bigB = BigInteger.Parse(b);
            bigP = BigInteger.Parse(p);
            r = new BigInteger(Math.Pow(2,l));
            foreach (var element in aVals) {
                BigaVals.Add(BigInteger.Parse(element));
            }
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
            BigInteger y = BigaVals[3];
            for (int i = 2; i >= 0; i--) {
                y = y * (ulong)Math.Pow(x, i+1) + BigaVals[i];
                y = (y&bigP) + (y>>89);
            }
            if (y >= bigP) {
                y -= bigP;
            }
            return y;
        }

        public BigInteger HCalc(ulong x) {
            return FourUniversal(x) % r;
        }

        public int SCalc(ulong x) {
            int b = (int) FourUniversal(x) >> 88;
            return 1 - 2*b;
        }
    }
}