using System;

namespace Implementeringsprojekt
{
    class Program
    {
        static void Main(string[] args)
        {

            var mulShift = new MultiplyShift();
            ulong a = 0b10001000_01110110_10110000_01001011_10100011_00000000_10100001_11110001;
            Random rnd = new Random(26);
            int l = rnd.Next(64);    

            // creates a tuple of streams (TEST)
            var tuple = StreamGenerator.CreateStream(100, l);
            foreach (var stream in tuple)
            {
                Console.WriteLine(mulShift.Hash(a, stream.Item1, l));
                //Console.WriteLine(stream.Item1);
            }
        }
    }
}
