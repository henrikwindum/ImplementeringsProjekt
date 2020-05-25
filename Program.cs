using System;

namespace Implementeringsprojekt
{
    class Program
    {
        static void Main(string[] args)
        {

            var function = new HashFunctions();
            //ulong a = 0b10001000_01110110_10110000_01001011_10100011_00000000_10100001_11110001;

            Random rnd = new Random(42);
            var a = rnd.Next((int)Math.Pow(2,89)-1);
            var b = rnd.Next((int)Math.Pow(2,89)-1);
            int l = 5;    

            // creates a tuple of streams (TEST)
            var tuple = StreamGenerator.CreateStream(20, l);
            foreach (var stream in tuple)
            {
                Console.WriteLine(function.MultiplyModPrime(stream.Item1, l));
                Console.WriteLine(stream);
            }
            
        }
    }
}
