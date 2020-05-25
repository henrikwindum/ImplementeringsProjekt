namespace Implementeringsprojekt
{
    public class MultiplyShift{
        public ulong Hash(ulong a, ulong x, int l){
            return ((a * x) >> (64 - l));
        } 
    }
}