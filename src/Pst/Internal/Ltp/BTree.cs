using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pst.Internal.Ltp
{
    internal class BTree
    {
        private readonly Heap _heap;

        public BTree(Heap heap)
        {
            _heap = heap;
        }
    }
}
