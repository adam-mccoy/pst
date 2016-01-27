namespace Pst.Internal
{
    internal struct Segment<T>
    {
        private T[] _array;
        private int _offset;
        private int _count;

        internal Segment(T[] array)
            : this(array, 0, array.Length)
        {
        }

        internal Segment(T[] array, int offset, int count)
        {
            _array = array;
            _offset = offset;
            _count = count;
        }

        public static implicit operator Segment<T>(T[] value)
        {
            return new Segment<T>(value);
        }

        public static implicit operator T[](Segment<T> value)
        {
            return value.ToArray();
        }

        internal T[] Array
        {
            get { return _array; }
        }

        internal int Offset
        {
            get { return _offset; }
        }

        internal int Count
        {
            get { return _count; }
        }

        internal T this[int index]
        {
            get { return _array[_offset + index]; }
            set { _array[_offset + index] = value; }
        }

        internal Segment<T> Derive(int offset, int count)
        {
            return new Segment<T>(_array, _offset + offset, count);
        }

        internal T[] ToArray()
        {
            var array = new T[_count];
            System.Array.Copy(_array, _offset, array, 0, _count);
            return array;
        }
    }
}
