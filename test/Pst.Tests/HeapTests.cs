using NUnit.Framework;
using Pst.Internal.Ltp;
using Pst.Internal.Ndb;

namespace Pst.Tests
{
    [TestFixture]
    public class HeapTests
    {
        private static readonly byte[] HeapData = new byte[]
        {
            0xa6, 0x01, 0xec, 0xbc, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xb5, 0x02, 0x06, 0x00,
            0x40, 0x00, 0x00, 0x00, 0x34, 0x0e, 0x02, 0x01, 0xa0, 0x00, 0x00, 0x00, 0x38, 0x0e, 0x03, 0x00,
            0x00, 0x00, 0x00, 0x00, 0xf9, 0x0f, 0x02, 0x01, 0x60, 0x00, 0x00, 0x00, 0x01, 0x30, 0x1f, 0x00,
            0x80, 0x00, 0x00, 0x00, 0x16, 0x34, 0x02, 0x01, 0xa0, 0x01, 0x00, 0x00, 0xdf, 0x35, 0x03, 0x00,
            0xff, 0x00, 0x00, 0x00, 0xe0, 0x35, 0x02, 0x01, 0xc0, 0x00, 0x00, 0x00, 0xe2, 0x35, 0x02, 0x01,
            0x20, 0x01, 0x00, 0x00, 0xe3, 0x35, 0x02, 0x01, 0x00, 0x01, 0x00, 0x00, 0xe4, 0x35, 0x02, 0x01,
            0x40, 0x01, 0x00, 0x00, 0xe5, 0x35, 0x02, 0x01, 0x60, 0x01, 0x00, 0x00, 0xe6, 0x35, 0x02, 0x01,
            0x80, 0x01, 0x00, 0x00, 0xe7, 0x35, 0x02, 0x01, 0xe0, 0x00, 0x00, 0x00, 0x33, 0x66, 0x0b, 0x00,
            0x01, 0x00, 0x00, 0x00, 0xfa, 0x66, 0x03, 0x00, 0x11, 0x00, 0x0e, 0x00, 0xfc, 0x66, 0x03, 0x00,
            0xde, 0x89, 0x02, 0x00, 0xff, 0x67, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x97, 0x7b, 0x45, 0xcb,
            0xdb, 0xc2, 0x82, 0x43, 0x92, 0x10, 0x55, 0x19, 0xe9, 0x93, 0x28, 0xfa, 0x4f, 0x00, 0x75, 0x00,
            0x74, 0x00, 0x6c, 0x00, 0x6f, 0x00, 0x6f, 0x00, 0x6b, 0x00, 0x20, 0x00, 0x44, 0x00, 0x61, 0x00,
            0x74, 0x00, 0x61, 0x00, 0x20, 0x00, 0x46, 0x00, 0x69, 0x00, 0x6c, 0x00, 0x65, 0x00, 0x01, 0x00,
            0x00, 0x00, 0x77, 0xc3, 0x5d, 0xd5, 0x33, 0x8a, 0x5f, 0x44, 0x9d, 0x2d, 0x4b, 0x44, 0xdd, 0xcc,
            0xbd, 0x48, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x97, 0x7b, 0x45, 0xcb, 0xdb, 0xc2,
            0x82, 0x43, 0x92, 0x10, 0x55, 0x19, 0xe9, 0x93, 0x28, 0xfa, 0x22, 0x80, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x97, 0x7b, 0x45, 0xcb, 0xdb, 0xc2, 0x82, 0x43, 0x92, 0x10, 0x55, 0x19, 0xe9, 0x93,
            0x28, 0xfa, 0x42, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x97, 0x7b, 0x45, 0xcb, 0xdb, 0xc2,
            0x82, 0x43, 0x92, 0x10, 0x55, 0x19, 0xe9, 0x93, 0x28, 0xfa, 0x62, 0x80, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x97, 0x7b, 0x45, 0xcb, 0xdb, 0xc2, 0x82, 0x43, 0x92, 0x10, 0x55, 0x19, 0xe9, 0x93,
            0x28, 0xfa, 0xa2, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x97, 0x7b, 0x45, 0xcb, 0xdb, 0xc2,
            0x82, 0x43, 0x92, 0x10, 0x55, 0x19, 0xe9, 0x93, 0x28, 0xfa, 0xc2, 0x80, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x97, 0x7b, 0x45, 0xcb, 0xdb, 0xc2, 0x82, 0x43, 0x92, 0x10, 0x55, 0x19, 0xe9, 0x93,
            0x28, 0xfa, 0xe2, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x97, 0x7b, 0x45, 0xcb, 0xdb, 0xc2,
            0x82, 0x43, 0x92, 0x10, 0x55, 0x19, 0xe9, 0x93, 0x28, 0xfa, 0x02, 0x81, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x97, 0x7b, 0x45, 0xcb, 0xdb, 0xc2, 0x82, 0x43, 0x92, 0x10, 0x55, 0x19, 0xe9, 0x93,
            0x28, 0xfa, 0x43, 0x00, 0x08, 0x00, 0x0d, 0x00, 0x00, 0x00, 0x0c, 0x00, 0x14, 0x00, 0x9c, 0x00,
            0xac, 0x00, 0xce, 0x00, 0xe6, 0x00, 0xfe, 0x00, 0x16, 0x01, 0x2e, 0x01, 0x46, 0x01, 0x5e, 0x01,
            0x76, 0x01, 0x8e, 0x01, 0xa6, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0xc6, 0x01, 0xa0, 0x8a, 0x27, 0x21, 0x63, 0xc8, 0x20, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        [Test]
        public void Heap_Gives_Correct_Client_Signature()
        {
            var block = Block.Create(HeapData);
            var heap = new Heap(block);

            Assert.AreEqual(0xbc, heap.ClientSignature);
        }

        [Test]
        public void Heap_Gives_Correct_User_Root()
        {
            var block = Block.Create(HeapData);
            var heap = new Heap(block);

            Assert.AreEqual(0x20, heap.UserRoot);
        }

        [Test]
        public void Heap_Gives_Correct_Allocated_Count()
        {
            var block = Block.Create(HeapData);
            var heap = new Heap(block);

            Assert.AreEqual(13, heap.AllocatedCount);
        }

        [Test]
        public void Heap_Gives_Correct_Freed_Count()
        {
            var block = Block.Create(HeapData);
            var heap = new Heap(block);

            Assert.AreEqual(0, heap.FreedCount);
        }

        [Test]
        public void Heap_Retrieves_Item()
        {
            var expectedBytes = new byte[]
            {
                0xb5, 0x02, 0x06, 0x00,
                0x40, 0x00, 0x00, 0x00
            };
            var block = Block.Create(HeapData);
            var heap = new Heap(block);

            var item = heap[0x20];

            CollectionAssert.AreEqual(expectedBytes, item.ToArray());
        }
    }
}
