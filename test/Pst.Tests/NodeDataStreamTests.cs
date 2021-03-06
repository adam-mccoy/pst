using System.IO;
using Moq;
using NUnit.Framework;
using Pst.Internal;
using Pst.Internal.Ndb;

namespace Pst.Tests
{
    [TestFixture]
    public class NodeDataStreamTests
    {
        #region Test Data
        private static readonly byte[] L2Data = new byte[]
        {
            0x01, 0x02, 0x02, 0x00, 0x28, 0x00, 0x00, 0x00, 0x56, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x4c, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x46, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        private static readonly byte[] L1B1Data = new byte[]
        {
            0x01, 0x01, 0x02, 0x00, 0x14, 0x00, 0x00, 0x00, 0x5c, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x64, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x56, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        private static readonly byte[] L1B2Data = new byte[]
        {
            0x01, 0x01, 0x02, 0x00, 0x14, 0x00, 0x00, 0x00, 0x6c, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x74, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4c, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        private static readonly byte[] LeafBlock1Data = new byte[]
        {
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5c, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        private static readonly byte[] LeafBlock2Data = new byte[]
        {
            0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10, 0x11, 0x12, 0x13, 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        private static readonly byte[] LeafBlock3Data = new byte[]
        {
            0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x6c, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        private static readonly byte[] LeafBlock4Data = new byte[]
        {
            0x1f, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x0a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x74, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };
        #endregion

        private static readonly IPstReader _reader = CreateReader();

        private static IPstReader CreateReader()
        {
            var reader = new Mock<IPstReader>();
            reader.Setup(r => r.FindBlock(0x146)).Returns(Block.Create(L2Data));
            reader.Setup(r => r.FindBlock(0x156)).Returns(Block.Create(L1B1Data));
            reader.Setup(r => r.FindBlock(0x14c)).Returns(Block.Create(L1B2Data));
            reader.Setup(r => r.FindBlock(0x15c)).Returns(Block.Create(LeafBlock1Data));
            reader.Setup(r => r.FindBlock(0x164)).Returns(Block.Create(LeafBlock2Data));
            reader.Setup(r => r.FindBlock(0x16c)).Returns(Block.Create(LeafBlock3Data));
            reader.Setup(r => r.FindBlock(0x174)).Returns(Block.Create(LeafBlock4Data));
            reader.Setup(r => r.LookupBlock(0x15c)).Returns(new BbtEntry(new Bref(), 10, 1));
            reader.Setup(r => r.LookupBlock(0x164)).Returns(new BbtEntry(new Bref(), 10, 1));
            reader.Setup(r => r.LookupBlock(0x16c)).Returns(new BbtEntry(new Bref(), 10, 1));
            reader.Setup(r => r.LookupBlock(0x174)).Returns(new BbtEntry(new Bref(), 10, 1));
            return reader.Object;
        }

        [Test]
        public void Reads_Single_Data_Block()
        {
            var expectedBytes = new[]
            {
                0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a
            };
            var stream = new NodeDataStream(0x15c, _reader);
            var br = new BinaryReader(stream);

            var bytes = br.ReadBytes((int)stream.Length);

            CollectionAssert.AreEqual(expectedBytes, bytes);
        }

        [Test]
        public void Reads_Single_Level_Data_Tree()
        {
            var expectedBytes = new[]
            {
                0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a,
                0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10, 0x11, 0x12, 0x13, 0x14
            };
            var stream = new NodeDataStream(0x156, _reader);
            var br = new BinaryReader(stream);

            var bytes = br.ReadBytes((int)stream.Length);

            CollectionAssert.AreEqual(expectedBytes, bytes);
        }

        [Test]
        public void Reads_Multi_Level_Data_Tree()
        {
            var expectedBytes = new[]
            {
                0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a,
                0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10, 0x11, 0x12, 0x13, 0x14,
                0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e,
                0x1f, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28,
            };
            var stream = new NodeDataStream(0x146, _reader);
            var br = new BinaryReader(stream);

            var bytes = br.ReadBytes((int)stream.Length);

            CollectionAssert.AreEqual(expectedBytes, bytes);
        }

        [Test]
        public void Reads_Across_Blocks()
        {
            var expectedBytes = new[]
            {
                0x0f, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19
            };
            var stream = new NodeDataStream(0x146, _reader)
            {
                Position = 14
            };
            var br = new BinaryReader(stream);
            var bytes = br.ReadBytes(11);

            CollectionAssert.AreEqual(expectedBytes, bytes);
        }
    }
}
