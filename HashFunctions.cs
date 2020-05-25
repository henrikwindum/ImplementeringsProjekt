using System;
using System.Numerics;

namespace Implementeringsprojekt
{
    public class HashFunctions{
        

        public double MultiplyShift(ulong x, int l){
            ulong a = 0b10001000_01110110_10110000_01001011_10100011_00000000_10100001_11110001;
            return ((a * x) >> (64 - l));
        }


        private BigInteger Modulos(BigInteger x, BigInteger p, int q){
            BigInteger y = (x&p) + (x>>q);
            if (y>=p){
                y -= p;
            }
            return y;
        }

        public BigInteger MultiplyModPrime(ulong x, int l){
            string a = "82164261376181154113155191";
            BigInteger Biga = 0;
            Biga = BigInteger.Parse(a);
            string b = "14815078981061821961915417";
            BigInteger Bigb = 0;
            Bigb = BigInteger.Parse(b);
            string p = "618970019642690137449562111";
            BigInteger Bigp = 0;
            Bigp = BigInteger.Parse(p);


            BigInteger r = new BigInteger(Math.Pow(2,l)-1);
            BigInteger X = (Biga * x + Bigb);
            BigInteger y = Modulos(X,Bigp, 89);
            
            return Modulos(y, r, l);
            /*
            BigInteger y = ((Biga * x + Bigb) &Bigp) +  ((Biga*x+Bigb)>>89);
            if(y>=Bigp){
                y -= Bigp;
            }*/
        } 
    }
}