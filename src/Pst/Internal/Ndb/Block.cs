using System;
using System.Diagnostics;

namespace Pst.Internal.Ndb
{
    [DebuggerDisplay("Bid: {Bid.Type} {Bid.Value}, Length: {Length}")]
    internal class Block
    {
        public const int TrailerLength = 16;

        private Block()
        {
        }

        public Bid Bid { get; private set; }
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

            var dataSize = data.Slice(blockSize - 16).ToUInt16();
            var sig = data.Slice(blockSize - 14).ToUInt16();
            var crc = data.Slice(blockSize - 12).ToUInt32();
            var bid = data.Slice(blockSize - 8).ToUInt64();

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
