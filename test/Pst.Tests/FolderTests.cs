﻿using Moq;
using NUnit.Framework;
using Pst.Internal;
using Pst.Internal.Ndb;

namespace Pst.Tests
{
    [TestFixture]
    public class FolderTests
    {
        #region Test Data

        // NID: 0x62000000
        // BID: 0x02000000
        private static byte[] FolderBlock = new byte[]
        {
            0x44, 0x00, 0xec, 0xbc, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xb5, 0x02, 0x06, 0x00,
            0x40, 0x00, 0x00, 0x00, 0x01, 0x30, 0x1f, 0x00, 0x60, 0x00, 0x00, 0x00, 0x02, 0x36, 0x03, 0x00,
            0x04, 0x00, 0x00, 0x00, 0x03, 0x36, 0x03, 0x00, 0x02, 0x00, 0x00, 0x00, 0x0a, 0x36, 0x0b, 0x00,
            0x01, 0x00, 0x00, 0x00, 0x50, 0x00, 0x65, 0x00, 0x72, 0x00, 0x73, 0x00, 0x6f, 0x00, 0x6e, 0x00,
            0x61, 0x00, 0x6c, 0x00, 0x03, 0x00, 0x00, 0x00, 0x0c, 0x00, 0x14, 0x00, 0x34, 0x00, 0x44, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x50, 0x00, 0xff, 0xff, 0xe5, 0x14, 0xd4, 0xcb, 0xf8, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        // NID: 0x6e000000
        // BID: 0x03000000
        private static byte[] FolderContentTableBlock = new byte[]
        {
            0x34, 0x02, 0xec, 0x7c, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7c, 0x1b, 0x74, 0x00,
            0x74, 0x00, 0x76, 0x00, 0x7a, 0x00, 0x40, 0x00, 0x00, 0x00, 0x3f, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x03, 0x00, 0x17, 0x00, 0x08, 0x00, 0x04, 0x02, 0x1f, 0x00, 0x1a, 0x00, 0x0c, 0x00,
            0x04, 0x03, 0x03, 0x00, 0x36, 0x00, 0x10, 0x00, 0x04, 0x04, 0x1f, 0x00, 0x37, 0x00, 0x14, 0x00,
            0x04, 0x05, 0x40, 0x00, 0x39, 0x00, 0x18, 0x00, 0x08, 0x06, 0x1f, 0x00, 0x42, 0x00, 0x20, 0x00,
            0x04, 0x07, 0x0b, 0x00, 0x57, 0x00, 0x74, 0x00, 0x01, 0x08, 0x0b, 0x00, 0x58, 0x00, 0x75, 0x00,
            0x01, 0x09, 0x1f, 0x00, 0x70, 0x00, 0x24, 0x00, 0x04, 0x0a, 0x02, 0x01, 0x71, 0x00, 0x28, 0x00,
            0x04, 0x0b, 0x1f, 0x00, 0x03, 0x0e, 0x2c, 0x00, 0x04, 0x0c, 0x1f, 0x00, 0x04, 0x0e, 0x30, 0x00,
            0x04, 0x0d, 0x40, 0x00, 0x06, 0x0e, 0x34, 0x00, 0x08, 0x0e, 0x03, 0x00, 0x07, 0x0e, 0x3c, 0x00,
            0x04, 0x0f, 0x03, 0x00, 0x08, 0x0e, 0x40, 0x00, 0x04, 0x10, 0x03, 0x00, 0x17, 0x0e, 0x44, 0x00,
            0x04, 0x11, 0x03, 0x00, 0x30, 0x0e, 0x48, 0x00, 0x04, 0x12, 0x14, 0x00, 0x33, 0x0e, 0x4c, 0x00,
            0x08, 0x13, 0x02, 0x01, 0x34, 0x0e, 0x54, 0x00, 0x04, 0x14, 0x03, 0x00, 0x38, 0x0e, 0x58, 0x00,
            0x04, 0x15, 0x02, 0x01, 0x3c, 0x0e, 0x5c, 0x00, 0x04, 0x16, 0x02, 0x01, 0x3d, 0x0e, 0x60, 0x00,
            0x04, 0x17, 0x03, 0x00, 0x97, 0x10, 0x64, 0x00, 0x04, 0x18, 0x40, 0x00, 0x08, 0x30, 0x68, 0x00,
            0x08, 0x19, 0x03, 0x00, 0xc6, 0x65, 0x70, 0x00, 0x04, 0x1a, 0x03, 0x00, 0xf2, 0x67, 0x00, 0x00,
            0x04, 0x00, 0x03, 0x00, 0xf3, 0x67, 0x04, 0x00, 0x04, 0x01, 0xb5, 0x04, 0x04, 0x00, 0x60, 0x00,
            0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x84, 0x00, 0x00, 0x00, 0x01, 0x00,
            0x00, 0x00, 0xa4, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0xc4, 0x00, 0x00, 0x00, 0x03, 0x00,
            0x00, 0x00, 0x48, 0x00, 0x61, 0x00, 0x70, 0x00, 0x70, 0x00, 0x79, 0x00, 0x20, 0x00, 0x42, 0x00,
            0x69, 0x00, 0x72, 0x00, 0x74, 0x00, 0x68, 0x00, 0x64, 0x00, 0x61, 0x00, 0x79, 0x00, 0x20, 0x00,
            0x66, 0x00, 0x72, 0x00, 0x6f, 0x00, 0x6d, 0x00, 0x20, 0x00, 0x4d, 0x00, 0x75, 0x00, 0x6d, 0x00,
            0x4d, 0x00, 0x75, 0x00, 0x6d, 0x00, 0x6d, 0x00, 0x79, 0x00, 0x48, 0x00, 0x69, 0x00, 0x6c, 0x00,
            0x61, 0x00, 0x72, 0x00, 0x69, 0x00, 0x6f, 0x00, 0x75, 0x00, 0x73, 0x00, 0x20, 0x00, 0x70, 0x00,
            0x68, 0x00, 0x6f, 0x00, 0x74, 0x00, 0x6f, 0x00, 0x20, 0x00, 0x66, 0x00, 0x72, 0x00, 0x6f, 0x00,
            0x6d, 0x00, 0x20, 0x00, 0x55, 0x00, 0x6e, 0x00, 0x63, 0x00, 0x6c, 0x00, 0x65, 0x00, 0x20, 0x00,
            0x53, 0x00, 0x74, 0x00, 0x65, 0x00, 0x76, 0x00, 0x65, 0x00, 0x55, 0x00, 0x6e, 0x00, 0x63, 0x00,
            0x6c, 0x00, 0x65, 0x00, 0x20, 0x00, 0x53, 0x00, 0x74, 0x00, 0x65, 0x00, 0x76, 0x00, 0x65, 0x00,
            0x53, 0x00, 0x65, 0x00, 0x6e, 0x00, 0x64, 0x00, 0x20, 0x00, 0x74, 0x00, 0x68, 0x00, 0x69, 0x00,
            0x73, 0x00, 0x20, 0x00, 0x74, 0x00, 0x6f, 0x00, 0x20, 0x00, 0x32, 0x00, 0x30, 0x00, 0x20, 0x00,
            0x66, 0x00, 0x72, 0x00, 0x69, 0x00, 0x65, 0x00, 0x6e, 0x00, 0x64, 0x00, 0x73, 0x00, 0x20, 0x00,
            0x6f, 0x00, 0x72, 0x00, 0x20, 0x00, 0x64, 0x00, 0x69, 0x00, 0x65, 0x00, 0x53, 0x00, 0x63, 0x00,
            0x6f, 0x00, 0x74, 0x00, 0x74, 0x00, 0x79, 0x00, 0x47, 0x00, 0x65, 0x00, 0x74, 0x00, 0x20, 0x00,
            0x6c, 0x00, 0x61, 0x00, 0x69, 0x00, 0x64, 0x00, 0x20, 0x00, 0x74, 0x00, 0x6f, 0x00, 0x6e, 0x00,
            0x69, 0x00, 0x67, 0x00, 0x68, 0x00, 0x74, 0x00, 0x21, 0x00, 0x4d, 0x00, 0x61, 0x00, 0x78, 0x00,
            0x20, 0x00, 0x47, 0x00, 0x65, 0x00, 0x6e, 0x00, 0x74, 0x00, 0x6c, 0x00, 0x65, 0x00, 0x6d, 0x00,
            0x61, 0x00, 0x6e, 0x00, 0x0b, 0x00, 0x00, 0x00, 0x0c, 0x00, 0xfa, 0x00, 0x02, 0x01, 0x22, 0x01,
            0x50, 0x01, 0x5a, 0x01, 0x9a, 0x01, 0xb0, 0x01, 0xec, 0x01, 0xf8, 0x01, 0x1a, 0x02, 0x34, 0x02,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x50, 0x02, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        // BID: 0x22000000
        private static byte[] FolderContentSubnodeTree = new byte[]
        {
            0x02, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3f, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x20, 0x00, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x01, 0x22, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        // NID: 0x3f000000
        // BID: 0x04000000
        private static byte[] FolderContentSubnodeBlock = new byte[]
        {
            0x64, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0xa0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0xc5, 0xc0, 0x00, 0x00, 0x84, 0x00, 0x00, 0x00, 0x01, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xc0, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xe0, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00,
            0xc5, 0xc0, 0x00, 0x00, 0xa4, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x20, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0xc5, 0xc0, 0x00, 0x00, 0xc4, 0x00,
            0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x40, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x60, 0x01,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x01, 0x00, 0xc5, 0xc0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0xe8, 0x01, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        private static byte[] FolderHierarchyTableBlock = new byte[]
        {
        };

        #endregion

        [Test]
        public void Gets_Display_Name()
        {
            var reader = new Mock<IPstReader>();
            reader.Setup(r => r.FindNode(0x122))
                .Returns(new Node(0x122, 0xaa8, 0x00, reader.Object));
            reader.Setup(r => r.FindBlock(0xaa8))
                .Returns(Block.Create(FolderBlock));

            var folder = new Folder(0x122, reader.Object);

            Assert.AreEqual("Personal", folder.Name);
        }

        [Test]
        public void Gets_Content_Count()
        {
            var reader = new Mock<IPstReader>();
            reader.Setup(r => r.FindNode(0x122))
                .Returns(new Node(0x122, 0xaa8, 0x00, reader.Object));
            reader.Setup(r => r.FindBlock(0xaa8))
                .Returns(Block.Create(FolderBlock));

            var folder = new Folder(0x122, reader.Object);

            Assert.AreEqual(4, folder.ItemCount);
        }

        [Test]
        public void Gets_Unread_Count()
        {
            var reader = new Mock<IPstReader>();
            reader.Setup(r => r.FindNode(0x122))
                .Returns(new Node(0x122, 0xaa8, 0x00, reader.Object));
            reader.Setup(r => r.FindBlock(0xaa8))
                .Returns(Block.Create(FolderBlock));

            var folder = new Folder(0x122, reader.Object);

            Assert.AreEqual(2, folder.UnreadCount);
        }

        [Test]
        public void Gets_Has_Subfolders()
        {
            var reader = new Mock<IPstReader>();
            reader.Setup(r => r.FindNode(0x122))
                .Returns(new Node(0x122, 0xaa8, 0x00, reader.Object));
            reader.Setup(r => r.FindBlock(0xaa8))
                .Returns(Block.Create(FolderBlock));

            var folder = new Folder(0x122, reader.Object);

            Assert.IsTrue(folder.HasSubfolders);
        }

        [Test]
        public void Gets_Subfolders()
        {
            var stream = TestHelper.GetTestDataStream("test.pst");
            var reader = new PstReader(stream);
            var folder = new Folder(0x122, reader);

            var subfolders = folder.Folders;

            Assert.Greater(subfolders.Count, 0);
        }

        [Test]
        public void Gets_Messages()
        {
            var reader = new Mock<IPstReader>();
            reader.Setup(r => r.FindNode(0x62))
                  .Returns(new Node(0x62, 0x02, 0x00, reader.Object));
            reader.Setup(r => r.FindBlock(0x02))
                  .Returns(Block.Create(FolderBlock));
            reader.Setup(r => r.FindNode(0x6e))
                  .Returns(new Node(0x6e, 0x03, 0x22, reader.Object));
            reader.Setup(r => r.FindBlock(0x03))
                  .Returns(Block.Create(FolderContentTableBlock));
            reader.Setup(r => r.FindBlock(0x22))
                  .Returns(Block.Create(FolderContentSubnodeTree));
            reader.Setup(r => r.FindBlock(0x04))
                  .Returns(Block.Create(FolderContentSubnodeBlock));

            var folder = new Folder(0x62, reader.Object);
            var messages = folder.Messages;

            Assert.AreEqual(4, messages.Count);
        }

        private IPstReader CreateTestReader()
        {
            var reader = new Mock<IPstReader>();

            return reader.Object;
        }
    }
}
