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
            int l = 12;    
            var function = new HashFunctions(l);

            // creates a tuple of streams (TEST)
            IEnumerable<Tuple<ulong,int>> tuple = StreamGenerator.CreateStream(100000, l);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var valueList = new List<ulong>(); 
            foreach (var stream in tuple) {
                var res = function.MultiplyShift(stream.Item1, l);
                valueList.Add(res); 
                //Console.WriteLine(res);
            }
            var shiftResult = valueList.Aggregate((currentSum, item)=> currentSum + item);
            //Console.WriteLine(shiftResult);                
            //Console.WriteLine(watch.ElapsedMilliseconds);
            watch.Reset();
            watch.Start();
            var valueList2 = new List<System.Numerics.BigInteger>(); 
            foreach (var stream2 in tuple) {
                var res = function.MultiplyModPrime(stream2.Item1);
                valueList2.Add(res);
                //Console.WriteLine(res);
            }
            var modResult = valueList2.Aggregate((currentSum, item)=> currentSum + item);
            //Console.WriteLine(modResult);   
            //Console.WriteLine(watch.ElapsedMilliseconds);
            watch.Reset();
            watch.Start();
            var valueList3 = new List<System.Numerics.BigInteger>();
            foreach (var stream2 in tuple) {
                var res = function.HCalc(stream2.Item1, 16);
                valueList3.Add(res);
                //Console.WriteLine(res);
            }
            var modResult2 = valueList3.Aggregate((currentSum2, item)=> currentSum2 + item);
            //Console.WriteLine(modResult2); 
            //Console.WriteLine(watch.ElapsedMilliseconds);
            watch.Stop();
            
            //Console.WriteLine(function.Estimate(tuple, 16));
/*            Program.TestHashTable(3200000, 2);*/
/*            Program.TestHashTable(3200000, 4);*/
//            Program.TestHashTable(3200000, 8);
//            Program.TestHashTable(3200000, 12);
//            Program.TestHashTable(3200000, 16);
            //Program.TestHashTable(3200000, 20);
//            Program.TestHashTable(3200000, 24);
           // Program.TestHashTable(3200000, 26);
            //Program.TestHashTable(3200000, 28);
 //           Program.TestHashTable(3200000, 29);  
            //Program.TestHashTable(100000, 12);
            TestCountSketch(100000, 12);
        }
        public static void TestHashTable(int n, int l) {
            IEnumerable<Tuple<ulong,int>> tuple = StreamGenerator.CreateStream(n, l);  
            var function = new HashFunctions(l);
            var tableShift = new Hashtable(function, "Shift", l);
            var tableMod = new Hashtable(function, "ModPrime", l);
            
            var watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (var (item1, item2) in tuple) {
                tableShift.Increment(item1, item2);
                
            }

            var shiftSum = tableShift.CalcQuadraticSum();
            Console.WriteLine("Time passed - shift: {0}, S: {1}, n: {2}, l: {3}", 
                watch.ElapsedMilliseconds, shiftSum, n, l);
            watch.Reset();
            watch.Start();
            foreach (var (item1, item2) in tuple) {
                tableMod.Increment(item1, item2);
            }

            var modSum = tableMod.CalcQuadraticSum();
            Console.WriteLine("Time passed - mod: {0}, S: {1}, n: {2}, l: {3}", 
                watch.ElapsedMilliseconds, modSum, n, l);
            watch.Reset();
            
            var count = function.CountSketch(tuple, 4096);
            watch.Start();
            var estimate = function.Estimate(count);
            Console.WriteLine("Time passed - CS estimate: {0}, S: {1}, n: {2}, l: {3}",
                watch.ElapsedMilliseconds, estimate, n, l);
            watch.Stop();
        }

        public static void TestCountSketch(int n, int l) {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            IEnumerable<Tuple<ulong,int>> tuple = StreamGenerator.CreateStream(n, l);
            var function2 = new HashFunctions(l);
            var tableShift = new Hashtable(function2, "Shift", l);
            foreach (var (item1, item2) in tuple) {
                tableShift.Increment(item1, item2);
                
            }
            var shiftSum = tableShift.CalcQuadraticSum();
            List<long> estimates = new List<long>();
            for (int i = 0; i < 100; i++) {
                var function = new HashFunctions(l);
                var estimate = function.Estimate(function.CountSketch(tuple, 4096));
                estimates.Add(estimate);
            }
            
            long squaredError = 0;
            foreach (var estimate in estimates) {
                //Console.WriteLine((long)Math.Sqrt(Math.Pow((estimate - shiftSum), 2)));
                squaredError += (long)Math.Sqrt(Math.Pow((estimate - shiftSum), 2));
            }
            //Console.WriteLine("Shiftsum: {0}", shiftSum);
            //Console.WriteLine("Squared error: {0}",squaredError/100);
            
            List<long> newEstimates = new List<long>();
            for (int i = 0; i < 9; i++) {
                List<long> part = estimates.GetRange(i * 11, 11);
                part.Sort();
                newEstimates.Add(part[6]);
            }    
            long squaredError2 = 0;
            foreach (var estimate in newEstimates) {
                //Console.WriteLine((long)Math.Sqrt(Math.Pow((estimate - shiftSum), 2)));
                squaredError2 += (long)Math.Sqrt(Math.Pow((estimate - shiftSum), 2));
            }
            //Console.WriteLine("Shiftsum: {0}", shiftSum);
            //Console.WriteLine("Squared error: {0}",squaredError2/9);
            int[] mS = new[] {32, 128, 2048};
            foreach (var m in mS) {
                List<long> estimates2 = new List<long>();
                int elapsed = 0;
                for (int i = 0; i < 100; i++) {
                    
                    var function = new HashFunctions(l);
                    var counts = function.CountSketch(tuple, m);
                    watch.Start();
                    var estimate = function.Estimate(counts);
                    elapsed += (int)watch.ElapsedMilliseconds;
                    watch.Reset();
                    estimates2.Add(estimate);
                }
                Console.WriteLine(elapsed/100);
                long squaredError4 = 0;
                foreach (var estimate in estimates2) {
                    squaredError4 += (long)Math.Sqrt(Math.Pow((estimate - shiftSum), 2));
                }
                //Console.WriteLine("Shiftsum: {0}", shiftSum);
                //Console.WriteLine("Squared error: {0}",squaredError4/100);
                List<long> newEstimates2 = new List<long>();
                for (int i = 0; i < 9; i++) {
                    List<long> part = estimates2.GetRange(i * 11, 11);
                    part.Sort();
                    newEstimates2.Add(part[6]);
                }    
                long squaredError3 = 0;
                foreach (var estimate in newEstimates2) {
                    squaredError3 += (long)Math.Sqrt(Math.Pow((estimate - shiftSum), 2));
                }
                //Console.WriteLine("Shiftsum: {0}", shiftSum);
                //Console.WriteLine("Squared error: {0}",squaredError3/9);
            }
        }

        public static void TestCountSketch2(int n, int l) {
            IEnumerable<Tuple<ulong,int>> tuple = StreamGenerator.CreateStream(n, l);
            var function2 = new HashFunctions(l);
            var tableShift = new Hashtable(function2, "Shift", l);
            foreach (var (item1, item2) in tuple) {
                tableShift.Increment(item1, item2);
                
            }
            var shiftSum = tableShift.CalcQuadraticSum();
            
            int[] mS = new[] {32, 128, 2048};
            foreach (var m in mS) {
                List<long> estimates = new List<long>();
                for (int i = 0; i < 100; i++) {
                    var function = new HashFunctions(l);
                    var counts = function.CountSketch(tuple, m);
                    var estimate = function.Estimate(counts);
                    estimates.Add(estimate);
                }
                List<long> newEstimates = new List<long>();
                for (int i = 0; i < 9; i++) {
                    List<long> part = estimates.GetRange(i * 11, 11);
                    part.Sort();
                    newEstimates.Add(part[6]);
                }    
                long squaredError = 0;
                foreach (var estimate in newEstimates) {
                    squaredError += (long)Math.Sqrt(Math.Pow((estimate - shiftSum), 2));
                }
                Console.WriteLine("Shiftsum: {0}", shiftSum);
                Console.WriteLine("Squared error: {0}",squaredError/9);
            }
        }
    }
}
