using System;
using System.Collections.Generic;
using System.IO;

namespace Pst.Internal.Ndb
{
    internal class BTreeReader<T>
        where T : BTreeNode, new()
    {
        private const int MetaDataOffset = 488;
        private const int TrailerOffset = 496;

        private readonly Stream _input;
        private readonly long _rootOffset;
        private readonly byte[] _buffer = new byte[512];

        private int _entries;
        private int _maxEntries;
        private int _entrySize;
        private int _currentLevel;
        private PageTrailer _trailer;

        internal BTreeReader(Stream input, long offset)
        {
            _input = input;
            _rootOffset = offset;
        }

        internal T Find(ulong key)
        {
            MoveTo(_rootOffset);

            while (_currentLevel > 0)
            {
                var keys = ReadKeys();
                var i = Array.BinarySearch(keys, key);

                if (i < 0)
                    i = ~i - 1;

                var offset = BitConverter.ToInt64(_buffer, i * _entrySize + 16);
                MoveTo(offset);
            }

            var leafKeys = ReadKeys();
            var leafIndex = Array.BinarySearch(leafKeys, key);
            if (leafIndex < 0)
                return null;

            return CreateNode(leafIndex);
        }

        private T CreateNode(int index)
        {
            var bytes = new byte[_entrySize];
            Buffer.BlockCopy(_buffer, index * _entrySize, bytes, 0, _entrySize);
            var node = new T();
            node.Create(bytes);
            return node;
        }

        private ulong[] ReadKeys()
        {
            var list = new List<ulong>(_entries);
            var offset = 0;
            for (var i = 0; i < _entries; i++)
            {
                list.Add(BitConverter.ToUInt64(_buffer, offset));
                offset += _entrySize;
            }

            return list.ToArray();
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
            var sig = BitConverter.ToUInt16(_buffer, TrailerOffset + 2);
            var crc = BitConverter.ToUInt32(_buffer, TrailerOffset + 4);
            var bid = BitConverter.ToUInt64(_buffer, TrailerOffset + 8);
            _trailer = new PageTrailer(type, sig, crc, bid);
        }
    }
}
