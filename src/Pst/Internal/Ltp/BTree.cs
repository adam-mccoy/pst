using System;
using System.Collections.Generic;

namespace Pst.Internal.Ltp
{
    internal class BTree<T, TKey>
    {
        private readonly uint _headerHid;
        private readonly Func<Segment<byte>, TKey> _keyFactory;
        private readonly Func<Segment<byte>, T> _valueFactory;

        private int _keySize;
        private int _valueSize;
        private int _elementSize;
        private int _indexLevels;
        private Hid _rootHid;

        internal BTree(
            Heap heap,
            Hid headerHid,
            Func<Segment<byte>, TKey> keyFactory,
            Func<Segment<byte>, T> valueFactory)
        {
            Heap = heap;
            _headerHid = headerHid;
            _keyFactory = keyFactory;
            _valueFactory = valueFactory;
            ProcessHeader();
        }

        internal BTree(
            Heap heap,
            Func<Segment<byte>, TKey> keyFactory,
            Func<Segment<byte>, T> valueFactory)
            : this(heap, heap.UserRoot, keyFactory, valueFactory)
        {
        }

        internal T Find(TKey key)
        {
            var root = Heap[_rootHid];
            var keys = ReadKeys(root);
            var keyIndex = Array.BinarySearch(keys, key);
            if (keyIndex < 0)
                return default(T);

            return _valueFactory(root.Derive(keyIndex * _elementSize, _elementSize));
        }

        internal Heap Heap { get; }

        internal IEnumerable<KeyValuePair<TKey, T>> GetAll()
        {
            var root = Heap[_rootHid];
            var itemCount = root.Count / _elementSize;
            var items = new KeyValuePair<TKey, T>[itemCount];
            for (var i = 0; i < itemCount; i++)
            {
                var key = _keyFactory(root.Derive(i * _elementSize, _keySize));
                var value = _valueFactory(root.Derive(i * _elementSize, _elementSize));
                items[i] = new KeyValuePair<TKey, T>(key, value);
            }
            return items;
        }

        private TKey[] ReadKeys(Segment<byte> data)
        {
            var keyCount = data.Count / _elementSize;
            var keys = new TKey[keyCount];
            for (var i = 0; i < keyCount; i++)
                keys[i] = _keyFactory(data.Derive(i * _elementSize, _keySize));
            return keys;
        }

        private void ProcessHeader()
        {
            var header = Heap[_headerHid];
            Validate.Match(header[0], 0xb5, "Invalid type");
            _keySize = header[1];
            _valueSize = header[2];
            _elementSize = _keySize + _valueSize;
            _indexLevels = header[3];
            _rootHid = header.ToUInt32(4);
        }
    }
}
