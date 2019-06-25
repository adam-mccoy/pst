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
        // NID: 0x62000000
        // BID: 0x02000000
        private static readonly byte[] FolderBlock = TestHelper.GetTestDataBytes("Folder.FolderPC.bin");
        // NID: 0x6e000000
        // BID: 0x03000000
        private static readonly byte[] FolderContentTableBlock = TestHelper.GetTestDataBytes("Folder.ContentsTC.bin");
        // BID: 0x22000000
        private static readonly byte[] FolderContentSubnodeTree = TestHelper.GetTestDataBytes("Folder.ContentsSubnodeTree.bin");
        // NID: 0x3f000000
        // BID: 0x04000000
        private static readonly byte[] FolderContentSubnodeData = TestHelper.GetTestDataBytes("Folder.ContentsSubnodeData.bin");
        #endregion

        private readonly IPstReader _testReader = CreateTestReader();

        [Test]
        public void Gets_Display_Name()
        {
            var folder = new Folder(0x62, _testReader);

            Assert.AreEqual("Personal", folder.Name);
        }

        [Test]
        public void Gets_Content_Count()
        {
            var folder = new Folder(0x62, _testReader);

            Assert.AreEqual(4, folder.ItemCount);
        }

        [Test]
        public void Gets_Unread_Count()
        {
            var folder = new Folder(0x62, _testReader);

            Assert.AreEqual(2, folder.UnreadCount);
        }

        [Test]
        public void Gets_Has_Subfolders()
        {
            var folder = new Folder(0x62, _testReader);

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
            var folder = new Folder(0x62, _testReader);
            var messages = folder.Messages;

            Assert.AreEqual(4, messages.Count);
        }

        private static IPstReader CreateTestReader()
        {
            var reader = new Mock<IPstReader>();
            reader.Setup(r => r.FindNode(0x62))
                  .Returns(new Node(0x62, 0x04, 0x00, reader.Object));
            reader.Setup(r => r.FindBlock(0x04))
                  .Returns(Block.Create(FolderBlock));
            reader.Setup(r => r.FindNode(0x6e))
                  .Returns(new Node(0x6e, 0x0c, 0x14, reader.Object));
            reader.Setup(r => r.FindBlock(0x0c))
                  .Returns(Block.Create(FolderContentTableBlock));
            reader.Setup(r => r.FindBlock(0x14))
                  .Returns(Block.Create(FolderContentSubnodeTree));
            reader.Setup(r => r.FindBlock(0x1c))
                  .Returns(Block.Create(FolderContentSubnodeData));

            return reader.Object;
        }
    }
}
