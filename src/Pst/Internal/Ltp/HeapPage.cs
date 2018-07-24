namespace Pst.Internal.Ltp
{
    internal class HeapPage
    {
        private ushort _mapOffset;
        private ushort[] _allocations;

        public HeapPage(Segment<byte> data)
        {
            Data = data;
            Initialize();
        }

        public int AllocatedCount => Data.ToUInt16(_mapOffset);
        public int FreedCount => Data.ToUInt16(_mapOffset + 2);

        public Segment<byte> Data { get; private set; }
        public Segment<byte> this[int index] => Data.Derive(_allocations[index], _allocations[index + 1] - _allocations[index]);

        private void Initialize()
        {
            _mapOffset = Data.ToUInt16();
            _allocations = new ushort[AllocatedCount + 1];
            for (var i = 0; i < AllocatedCount + 1; i++)
                _allocations[i] = Data.ToUInt16(_mapOffset + 4 + i * 2);
        }
    }
}
