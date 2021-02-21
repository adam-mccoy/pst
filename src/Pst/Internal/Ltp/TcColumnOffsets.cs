namespace Pst.Internal.Ltp
{
    internal class TcColumnOffsets
    {
        internal TcColumnOffsets(int fours, int twos, int ones, int bm)
        {
            Fours = fours;
            Twos = twos;
            Ones = ones;
            Bm = bm;
        }

        internal int Fours { get; }
        internal int Twos { get; }
        internal int Ones { get; }
        internal int Bm { get; }
    }
}
