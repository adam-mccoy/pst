namespace Pst.Internal.Ltp
{
    internal class Heap
    {
        internal byte ClientSignature { get; private set; }
        internal uint UserRoot { get; private set; }
        internal int AllocatedCount { get; private set; }
        internal int FreedCount { get; private set; }
        internal uint[] Allocations { get; private set; }
    }
}
