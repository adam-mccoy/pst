using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using Pst.Internal;
using Pst.Internal.Ltp;
using Pst.Internal.Ndb;

namespace Pst.Tests
{
    [TestFixture]
    public class BTreeTests
    {
        private static readonly byte[] SingleLevel = TestHelper.GetTestDataBytes("BTree.SingleLevel.bin");
        private static readonly byte[] MultiLevel  = TestHelper.GetTestDataBytes("BTree.MultiLevel.bin");

        [Test]
        public void Validates_Header()
        {
            var block = Block.Create(SingleLevel);
            var reader = new Mock<IPstReader>();
            reader.Setup(r => r.FindBlock(0x04)).Returns(block);
            var heap = new Heap(new Node(0x0102, 0x04, 0x00, reader.Object), reader.Object);
            new BTree<ulong, ushort>(heap, null, null);
        }

        [Test]
        public void Find_Nonexistent_Element_Returns_Null()
        {
            var block = Block.Create(SingleLevel);
            var reader = new Mock<IPstReader>();
            reader.Setup(r => r.FindBlock(0x04)).Returns(block);
            var heap = new Heap(new Node(0x0102, 0x04, 0x00, reader.Object), reader.Object);
            var btree = new BTree<byte[], ushort>(
                heap,
                b => BitConverter.ToUInt16(b.Array, b.Offset),
                b => b.ToArray());

            var result = btree.Find(1234);
            Assert.IsNull(result);
        }

        [Test]
        public void Finds_Element_Single_Level()
        {
            var expectedBytes = new byte[]
            {
                0xe5, 0x35, 0x02, 0x01,
                0x60, 0x01, 0x00, 0x00
            };

            var block = Block.Create(SingleLevel);
            var reader = new Mock<IPstReader>();
            reader.Setup(r => r.FindBlock(0x04)).Returns(block);
            var heap = new Heap(new Node(0x0102, 0x04, 0x00, reader.Object), reader.Object);
            var btree = new BTree<byte[], ushort>(
                heap,
                b => BitConverter.ToUInt16(b.Array, b.Offset),
                b => b.ToArray());

            var result = btree.Find(0x35e5);
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(expectedBytes, result);
        }

        [Test]
        public void Finds_Element_MultiLevel_Level()
        {
            var expectedBytes = new byte[]
            {
                0xe5, 0x35, 0x02, 0x01,
                0xa0, 0x01, 0x00, 0x00
            };

            var block = Block.Create(MultiLevel);
            var reader = new Mock<IPstReader>();
            reader.Setup(r => r.FindBlock(0x04)).Returns(block);
            var heap = new Heap(new Node(0x0102, 0x04, 0x00, reader.Object), reader.Object);
            var btree = new BTree<byte[], ushort>(
                heap,
                b => BitConverter.ToUInt16(b.Array, b.Offset),
                b => b.ToArray());

            var result = btree.Find(0x35e5);
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(expectedBytes, result);
        }

        [Test]
        public void Gets_All_Elements_Single_Level()
        {
            var block = Block.Create(SingleLevel);
            var reader = new Mock<IPstReader>();
            reader.Setup(r => r.FindBlock(0x04)).Returns(block);
            var heap = new Heap(new Node(0x0102, 0x04,0x00, reader.Object), reader.Object);
            var btree = new BTree<byte[], ushort>(
                heap,
                b => BitConverter.ToUInt16(b.Array, b.Offset),
                b => b.ToArray());

            var items = btree.GetAll();

            Assert.AreEqual(17, items.Count());
            var item1 = items.ElementAt(0);
            Assert.AreEqual(0xe34, item1.Key);
            CollectionAssert.AreEqual(
                new byte[] { 0x34, 0x0e, 0x02, 0x01, 0xa0, 0x00, 0x00, 0x00 },
                item1.Value);
            var item6 = items.ElementAt(5);
            Assert.AreEqual(0x35df, item6.Key);
            CollectionAssert.AreEqual(
                new byte[] { 0xdf, 0x35, 0x03, 0x00, 0xff, 0x00, 0x00, 0x00 },
                item6.Value);
        }

        [Test]
        public void Gets_All_Elements_Multi_Level()
        {
            var block = Block.Create(MultiLevel);
            var reader = new Mock<IPstReader>();
            reader.Setup(r => r.FindBlock(0x04)).Returns(block);
            var heap = new Heap(new Node(0x0102, 0x04,0x00, reader.Object), reader.Object);
            var btree = new BTree<byte[], ushort>(
                heap,
                b => BitConverter.ToUInt16(b.Array, b.Offset),
                b => b.ToArray());

            var items = btree.GetAll();

            Assert.AreEqual(17, items.Count());
            var item1 = items.ElementAt(0);
            Assert.AreEqual(0xe34, item1.Key);
            CollectionAssert.AreEqual(
                new byte[] { 0x34, 0x0e, 0x02, 0x01, 0xe0, 0x00, 0x00, 0x00 },
                item1.Value);
            var item6 = items.ElementAt(5);
            Assert.AreEqual(0x35df, item6.Key);
            CollectionAssert.AreEqual(
                new byte[] { 0xdf, 0x35, 0x03, 0x00, 0xff, 0x00, 0x00, 0x00 },
                item6.Value);
        }
    }
}
