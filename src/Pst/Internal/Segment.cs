namespace Pst.Internal
{
    internal struct Segment<T>
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

        internal Segment<T> Derive(int offset, int count) => new Segment<T>(Array, Offset + offset, count);

        internal T[] ToArray()
        {
            var array = new T[Count];
            System.Array.Copy(Array, Offset, array, 0, Count);
            return array;
        }
    }
}
