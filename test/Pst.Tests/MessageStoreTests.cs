using NUnit.Framework;
using Pst.Internal;

namespace Pst.Tests
{
    [TestFixture]
    public class MessageStoreTests
    {
        [Test]
        public void Gets_Display_Name()
        {
            var stream = TestHelper.GetTestDataStream("test.pst");
            var reader = new PstReader(stream);

            var messageStore = new MessageStore(0x21, reader);

            Assert.AreEqual("Outlook Data File", messageStore.DisplayName);
        }

        [Test]
        public void Gets_Record_Key()
        {
            var expectedBytes = new byte[]
            {
                0x97, 0x7b, 0x45, 0xcb, 0xdb, 0xc2, 0x82, 0x43,
                0x92, 0x10, 0x55, 0x19, 0xe9, 0x93, 0x28, 0xfa
            };
            var stream = TestHelper.GetTestDataStream("test.pst");
            var reader = new PstReader(stream);

            var messageStore = new MessageStore(0x21, reader);

            CollectionAssert.AreEqual(expectedBytes, messageStore.RecordKey);
        }
    }
}
