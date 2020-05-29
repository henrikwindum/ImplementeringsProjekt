using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Implementeringsprojekt
{
    class Program
    {
        static void Main(string[] args)
        {
            int l = 20;    
            var function = new HashFunctions(l);

            // creates a tuple of streams (TEST)
            var tuple = StreamGenerator.CreateStream(100000, l);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var valueList = new List<ulong>(); 
            foreach (var stream in tuple) {
                var res = function.MultiplyShift(stream.Item1, l);
                valueList.Add(res); 
                //Console.WriteLine(res);
            }
            var shiftResult = valueList.Aggregate((currentSum, item)=> currentSum + item);
            Console.WriteLine(shiftResult);                
            Console.WriteLine(watch.ElapsedMilliseconds);
            watch.Reset();
            watch.Start();
            var valueList2 = new List<System.Numerics.BigInteger>(); 
            foreach (var stream2 in tuple) {
                var res = function.MultiplyModPrime(stream2.Item1);
                valueList2.Add(res);
                //Console.WriteLine(res);
            }
            var modResult = valueList2.Aggregate((currentSum, item)=> currentSum + item);
            Console.WriteLine(modResult);   
            Console.WriteLine(watch.ElapsedMilliseconds);
            watch.Reset();
            watch.Start();
            var valueList3 = new List<System.Numerics.BigInteger>();
            foreach (var stream2 in tuple) {
                var res = function.FourUniversal(stream2.Item1);
                valueList3.Add(res);
                //Console.WriteLine(res);
            }
            var modResult2 = valueList3.Aggregate((currentSum2, item)=> currentSum2 + item);
            Console.WriteLine(modResult2); 
            Console.WriteLine(watch.ElapsedMilliseconds);
            watch.Stop();
            /*Program.tester(3200000, 2);
            Program.tester(3200000, 4);
            Program.tester(3200000, 8);
            Program.tester(3200000, 12);
            Program.tester(3200000, 16);
            Program.tester(3200000, 20);
            Program.tester(3200000, 24);
            Program.tester(3200000, 26);
            Program.tester(3200000, 28);
            Program.tester(3200000, 29); */     
        }
        public static void tester(int n, int l) {
            var tuple = StreamGenerator.CreateStream(n, l);  
            var function = new HashFunctions(l);
            var tableShift = new Hashtable(function, "Shift", l);
            var tableMod = new Hashtable(function, "ModPrime", l);
            
            var watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (var (item1, item2) in tuple) {
                tableShift.increment(item1, item2);
                
            }
            Console.WriteLine("Time passed - shift: {0}, S: {1}, n: {2}, l: {3}", 
                watch.ElapsedMilliseconds, tableShift.calcQuadraticSum(), n, l);
            watch.Reset();
            watch.Start();
            foreach (var (item1, item2) in tuple) {
                tableMod.increment(item1, item2);
            }
            Console.WriteLine("Time passed - mod: {0}, S: {1}, n: {2}, l: {3}", 
                watch.ElapsedMilliseconds, tableMod.calcQuadraticSum(), n, l);
            watch.Stop();
            
        }
    }
}
