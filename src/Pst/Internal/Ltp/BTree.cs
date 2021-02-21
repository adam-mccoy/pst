using System;
using System.Collections.Generic;
using System.Linq;

namespace Pst.Internal.Ltp
{
    internal class BTree<T, TKey>
    {
        private readonly Heap _heap;
        private readonly uint _headerHid;
        private readonly Func<Segment<byte>, TKey> _keyFactory;
        private readonly Func<Segment<byte>, T> _valueFactory;

        private int _keyLength;
        private int _valueLength;
        private int _indexLevels;
        private Hid _rootHid;

        internal BTree(
            Heap heap,
            Hid headerHid,
            Func<Segment<byte>, TKey> keyFactory,
            Func<Segment<byte>, T> valueFactory)
        {
            _heap = heap;
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

        private int ElementLength => _keyLength + _valueLength;
        private int IntermediateLength => _keyLength + 4;

        internal T Find(TKey key) => _rootHid == Hid.Zero ? default : FindInternal(key, _heap[_rootHid], _indexLevels);

        internal IEnumerable<KeyValuePair<TKey, T>> GetAll()
        {
            if (_rootHid == Hid.Zero)
                return Enumerable.Empty<KeyValuePair<TKey, T>>();

            var items = new List<KeyValuePair<TKey, T>>();
            GetAllInternal(items, _heap[_rootHid], _indexLevels);
            return items;
        }

        private T FindInternal(TKey key, Segment<byte> heapItem, int level)
        {
            var keys = ReadKeys(heapItem, level == 0 ? ElementLength : IntermediateLength);
            var keyIndex = keys.BinarySearch(key);

            if (level == 0)
            {
                return keyIndex < 0 ? default : _valueFactory(heapItem.Slice(keyIndex * ElementLength, ElementLength));
            }

            if (keyIndex < 0)
                keyIndex = ~keyIndex - 1;

            var nextLevel = _heap[heapItem.Slice(keyIndex * IntermediateLength + _keyLength, 4).ToUInt32()];
            return FindInternal(key, nextLevel, level - 1);
        }

        private void GetAllInternal(List<KeyValuePair<TKey, T>> items, Segment<byte> heapItem, int level)
        {
            var recordLength = level == 0 ? ElementLength : IntermediateLength;
            var itemCount = heapItem.Count / recordLength;
            for (var i = 0; i < itemCount; i++)
            {
                var key = _keyFactory(heapItem.Slice(i * recordLength, _keyLength));

                if (level == 0)
                {
                    var value = _valueFactory(heapItem.Slice(i * ElementLength, ElementLength));
                    items.Add(new KeyValuePair<TKey, T>(key, value));
                }
                else
                {
                    var value = heapItem.Slice(i * IntermediateLength + _keyLength, 4);
                    GetAllInternal(items, _heap[value.ToUInt32()], level - 1);
                }
            }
        }

        private List<TKey> ReadKeys(Segment<byte> data, int recordLength)
        {
            var keyCount = data.Count / recordLength;
            var keys = new List<TKey>(keyCount);
            for (var i = 0; i < keyCount; i++)
                keys.Add(_keyFactory(data.Slice(i * recordLength, _keyLength)));
            return keys;
        }

        private void ProcessHeader()
        {
            var header = _heap[_headerHid];
            Validate.Match(header[0], 0xb5, "Invalid type");
            _keyLength = header[1];
            _valueLength = header[2];
            _indexLevels = header[3];
            _rootHid = header.ToUInt32(4);
        }
    }
}
