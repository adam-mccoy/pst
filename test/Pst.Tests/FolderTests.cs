using Moq;
using NUnit.Framework;
using Pst.Internal;
using Pst.Internal.Ndb;

namespace Pst.Tests
{
    [TestFixture]
    public class FolderTests
    {
        #region Test Data

        private static byte[] FolderBlock = new byte[]
        {
            0x72, 0x00, 0xec, 0xbc, 0x20, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xb5, 0x02, 0x06, 0x00,
            0x40, 0x00, 0x00, 0x00, 0x01, 0x30, 0x1f, 0x00, 0x60, 0x00, 0x00, 0x00, 0x02, 0x36, 0x03, 0x00,
            0x06, 0x00, 0x00, 0x00, 0x03, 0x36, 0x03, 0x00, 0x08, 0x00, 0x00, 0x00, 0x0a, 0x36, 0x0b, 0x00,
            0x01, 0x00, 0x00, 0x00, 0xda, 0x36, 0x02, 0x01, 0x80, 0x00, 0x00, 0x00, 0x54, 0x00, 0x6f, 0x00,
            0x70, 0x00, 0x20, 0x00, 0x6f, 0x00, 0x66, 0x00, 0x20, 0x00, 0x4f, 0x00, 0x75, 0x00, 0x74, 0x00,
            0x6c, 0x00, 0x6f, 0x00, 0x6f, 0x00, 0x6b, 0x00, 0x20, 0x00, 0x64, 0x00, 0x61, 0x00, 0x74, 0x00,
            0x61, 0x00, 0x20, 0x00, 0x66, 0x00, 0x69, 0x00, 0x6c, 0x00, 0x65, 0x00, 0x06, 0x04, 0x03, 0x00,
            0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x0c, 0x00, 0x14, 0x00, 0x3c, 0x00, 0x6c, 0x00, 0x72, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x80, 0x00, 0xf8, 0x6d, 0x06, 0xf0, 0xc5, 0xf5, 0xf8, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        private static byte[] FolderContentTableBlock = new byte[]
        {
        };

        private static byte[] FolderContentSubnodeTree = new byte[]
        {
        };

        private static byte[] FolderContentSubnodeBlock = new byte[]
        {
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

            Assert.AreEqual("Top of Outlook data file", folder.Name);
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

            Assert.AreEqual(6, folder.ItemCount);
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

            Assert.AreEqual(8, folder.UnreadCount);
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
            reader.Setup(r => r.FindNode(0x22))
                  .Returns(new Node(0x22, 0xabc, 0x00, reader.Object));
            reader.Setup(r => r.FindBlock(0xabc))
                  .Returns(Block.Create(FolderBlock));
            reader.Setup(r => r.FindNode(0x2e))
                  .Returns(new Node(0x2e, 0xdef, 0x00, reader.Object));
            reader.Setup(r => r.FindBlock(0xdef))
                  .Returns(Block.Create(FolderContentTableBlock));

            var folder = new Folder(0x22, reader.Object);
            var messages = folder.Messages;

            Assert.AreEqual(1, messages.Count);
        }

        private IPstReader CreateTestReader()
        {
            var reader = new Mock<IPstReader>();

            return reader.Object;
        }
    }
}
