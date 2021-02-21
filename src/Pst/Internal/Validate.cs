using System;
using System.Collections.Generic;
using System.Linq;

namespace Pst.Internal
{
    internal static class Validate
    {
        internal static void Match<T>(T obj1, T obj2, string message)
        {
            if (!obj1.Equals(obj2))
                throw new Exception(message);
        }

        internal static void Match<T>(IEnumerable<T> obj1, IEnumerable<T> obj2, string message)
        {
            if (!obj1.SequenceEqual(obj2))
                throw new Exception(message);
        }

        internal static void Any<T>(T item, ICollection<T> candidates, string message)
        {
            if (candidates == null)
                throw new ArgumentNullException(nameof(candidates));
            if (candidates.Count == 0)
                throw new ArgumentException("Sequence contains no elements.", nameof(candidates));

            if (!candidates.Contains(item))
                throw new Exception(message);
        }
    }
}
