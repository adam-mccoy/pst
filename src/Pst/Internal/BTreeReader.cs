using System;
using System.IO;
using Pst.Internal.Ndb;

namespace Pst.Internal
{
    internal class BTreeReader
    {
        private const int MetaDataOffset = 488;
        private const int TrailerOffset = 496;

        private readonly Stream _input;
        private readonly long _offset;
        private readonly byte[] _buffer = new byte[512];

        private int _entries;
        private int _maxEntries;
        private int _entrySize;
        private int _level;
        private PageTrailer _trailer;

        internal BTreeReader(Stream input, long offset)
        {
            _input = input;
            _offset = offset;
        }

        private void ReadPage(Stream input)
        {
            input.Read(_buffer, 0, 512);

            _entries = _buffer[MetaDataOffset];
            _maxEntries = _buffer[MetaDataOffset + 1];
            _entrySize = _buffer[MetaDataOffset + 2];
            _level = _buffer[MetaDataOffset + 3];

            var type = (PageType)_buffer[TrailerOffset];
            var sig = BitConverter.ToUInt16(_buffer, TrailerOffset + 2);
            var crc = BitConverter.ToUInt32(_buffer, TrailerOffset + 4);
            var bid = BitConverter.ToUInt64(_buffer, TrailerOffset + 8);
            _trailer = new PageTrailer(type, sig, crc, bid);
        }
    }
}
