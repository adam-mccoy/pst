using System;
using System.Collections.Generic;
using System.IO;

namespace Pst.Internal.Ndb
{
    internal class BTreeReader<T>
        where T : class
    {
        private const int MetaDataOffset = 488;
        private const int TrailerOffset = 496;

        private readonly Stream _input;
        private readonly long _rootOffset;
        private readonly Func<Segment<byte>, T> _factory;
        private readonly byte[] _buffer = new byte[512];

        private int _entries;
        private int _maxEntries;
        private int _entrySize;
        private int _currentLevel;
        private PageTrailer _trailer;

        internal BTreeReader(Stream input, long offset, Func<Segment<byte>, T> factory)
        {
            _input = input;
            _rootOffset = offset;
            _factory = factory;
        }

        internal T Find(ulong key)
        {
            MoveTo(_rootOffset);

            while (_currentLevel > 0)
            {
                var keys = ReadKeys();
                var i = keys.BinarySearch(key);

                if (i < 0)
                    i = ~i - 1;

                var offset = _buffer.Slice(i * _entrySize + 16).ToInt64();
                MoveTo(offset);
            }

            var leafKeys = ReadKeys();
            var leafIndex = leafKeys.BinarySearch(key);
            return leafIndex < 0 ? null : CreateNode(leafIndex);
        }

        private T CreateNode(int index)
        {
            var segment = _buffer.Slice(index * _entrySize, _entrySize);
            var node = _factory(segment);
            return node;
        }

        private List<ulong> ReadKeys()
        {
            var list = new List<ulong>(_entries);
            var offset = 0;
            for (var i = 0; i < _entries; i++)
            {
                list.Add(_buffer.Slice(offset).ToUInt64());
                offset += _entrySize;
            }

            return list;
        }

        private void MoveTo(long offset)
        {
            _input.Seek(offset, SeekOrigin.Begin);
            _input.Read(_buffer, 0, 512);

            _entries = _buffer[MetaDataOffset];
            _maxEntries = _buffer[MetaDataOffset + 1];
            _entrySize = _buffer[MetaDataOffset + 2];
            _currentLevel = _buffer[MetaDataOffset + 3];

            var type = (PageType)_buffer[TrailerOffset];
            var sig = _buffer.Slice(TrailerOffset + 2).ToUInt16();
            var crc = _buffer.Slice(TrailerOffset + 4).ToUInt32();
            var bid = _buffer.Slice(TrailerOffset + 8).ToUInt64();
            _trailer = new PageTrailer(type, sig, crc, bid);
        }
    }
}
