using System;
using System.Collections.Generic;
using System.Linq;

namespace Pst.Internal.Ltp
{
    internal class BTree<T, TKey>
    {
        private readonly Heap _heap;
        private readonly Func<IList<byte>, TKey> _keyFactory;
        private readonly Func<IList<byte>, T> _valueFactory;

        private int _keySize;
        private int _valueSize;
        private int _elementSize;
        private int _indexLevels;
        private uint _rootHid;

        internal BTree(
            Heap heap,
            Func<IList<byte>, TKey> keyFactory,
            Func<IList<byte>, T> valueFactory)
        {
            _heap = heap;
            _keyFactory = keyFactory;
            _valueFactory = valueFactory;
            ProcessHeader();
        }

        internal T Find(TKey key)
        {
            var root = _heap[_rootHid];
            var keys = ReadKeys(root);
            var keyIndex = Array.BinarySearch(keys, key);
            if (keyIndex < 0)
                return default(T);

            return _valueFactory(root.Skip(keyIndex * _elementSize).Take(_elementSize).ToList());
        }

        internal Heap Heap
        {
            get { return _heap; }
        }

        private TKey[] ReadKeys(IList<byte> data)
        {
            var keyCount = data.Count / _elementSize;
            var keys = new TKey[keyCount];
            for (var i = 0; i < keyCount; i++)
                keys[i] = _keyFactory(data.Skip(i * _elementSize).Take(_keySize).ToList());
            return keys;
        }

        private void ProcessHeader()
        {
            var header = _heap[_heap.UserRoot];
            Validate.Equals(header[0], 0xb5);
            _keySize = header[1];
            _valueSize = header[2];
            _elementSize = _keySize + _valueSize;
            _indexLevels = header[3];
            // TODO: Optimize
            _rootHid = BitConverter.ToUInt32(header.Skip(4).Take(4).ToArray(), 0);
        }
    }
}
