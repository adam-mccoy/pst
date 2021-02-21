using System.Collections;
using System.Collections.Generic;

namespace Pst.Internal
{
    internal struct Segment<T> : IEnumerable<T>
    {
        internal Segment(T[] array)
            : this(array, 0, array.Length)
        {
        }

        internal Segment(T[] array, int offset, int count)
        {
            Array = array;
            Offset = offset;
            Count = count;
        }

        public static implicit operator Segment<T>(T[] value) => new Segment<T>(value);

        public static implicit operator T[] (Segment<T> value) => value.ToArray();

        internal T[] Array { get; }

        internal int Offset { get; }

        internal int Count { get; }

        internal T this[int index]
        {
            get => Array[Offset + index];
            set => Array[Offset + index] = value;
        }

        internal Segment<T> Slice(int offset, int count) => new Segment<T>(Array, Offset + offset, count);

        internal T[] ToArray()
        {
            var array = new T[Count];
            System.Array.Copy(Array, Offset, array, 0, Count);
            return array;
        }

        public IEnumerator<T> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        class Enumerator : IEnumerator<T>
        {
            private readonly T[] _array;
            private readonly int _start;
            private readonly int _end;
            private int _current;

            public Enumerator(Segment<T> segment)
            {
                _array = segment.Array;
                _start = segment.Offset;
                _end = segment.Offset + segment.Count;
                _current = segment.Offset - 1;
            }

            public bool MoveNext()
            {
                if (_current < _end)
                {
                    _current++;
                    return _current < _end;
                }
                return false;
            }

            public void Reset() => _current = _start - 1;

            public T Current => _array[_current];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}
