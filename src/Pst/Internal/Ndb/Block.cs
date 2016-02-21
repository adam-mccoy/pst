using System;

namespace Pst.Internal.Ndb
{
    internal class Block
    {
        public const int TrailerLength = 16;

        private Block()
        {
        }

        public ulong Bid { get; private set; }
        public byte[] Data { get; private set; }
        public int Length { get; private set; }
        public ushort Signature { get; private set; }
        public uint Crc { get; private set; }

        public static Block Create(byte[] data)
        {
            var blockSize = data.Length;
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (blockSize % 64 != 0)
                throw new ArgumentException("Invalid block length.", nameof(data));

            var dataSize = BitConverter.ToUInt16(data, blockSize - 16);
            var sig = BitConverter.ToUInt16(data, blockSize - 14);
            var crc = BitConverter.ToUInt32(data, blockSize - 12);
            var bid = BitConverter.ToUInt64(data, blockSize - 8);

            var blockData = new byte[dataSize];
            Buffer.BlockCopy(data, 0, blockData, 0, dataSize);

            return new Block
            {
                Bid = bid,
                Length = dataSize,
                Signature = sig,
                Crc = crc,
                Data = blockData
            };
        }
    }
}
