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

        internal int Fours { get; private set; }
        internal int Twos { get; private set; }
        internal int Ones { get; private set; }
        internal int Bm { get; private set; }
    }
}
