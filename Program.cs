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
            var function = new HashFunctions();
            int l = 20;    

            // creates a tuple of streams (TEST)
            var tuple = StreamGenerator.CreateStream(20000, l);
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
                var res = function.MultiplyModPrime(stream2.Item1, l);
                valueList2.Add(res);
                //Console.WriteLine(res);
            }
            var modResult = valueList2.Aggregate((currentSum, item)=> currentSum + item);
            Console.WriteLine(modResult);   
            Console.WriteLine(watch.ElapsedMilliseconds);
            watch.Stop();
            
            var tableShift = new Hashtable(function, "Shift", l);
            var tableMod = new Hashtable(function, "ModPrime", l);
            
            
        }
    }
}
