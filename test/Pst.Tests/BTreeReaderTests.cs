using NUnit.Framework;
using Pst.Internal.Ndb;

namespace Pst.Tests
{
    [TestFixture]
    public class BTreeReaderTests
    {
        [Test]
        public void Finds_NBT_Node()
        {
            var stream = TestHelper.GetTestDataStream("nbt.bin");
            var reader = new BTreeReader<NbtEntry>(stream, 0);

            var node = reader.Find(3);

            Assert.IsNotNull(node);
            Assert.AreEqual(3, node.Nid);
            Assert.AreEqual(0xffee, node.DataBid);
            Assert.AreEqual(0, node.SubnodeBid);
            Assert.AreEqual(0, node.ParentNid);
        }

        [Test]
        public void Nonexistent_NBT_Node_Returns_Null()
        {
            var stream = TestHelper.GetTestDataStream("nbt.bin");
            var reader = new BTreeReader<NbtEntry>(stream, 0);

            var node = reader.Find(6);

            Assert.IsNull(node);
        }
    }
}
