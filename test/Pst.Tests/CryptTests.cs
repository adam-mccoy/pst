using NUnit.Framework;
using Pst.Internal;

namespace Pst.Tests
{
    [TestFixture]
    public class CryptTests
    {
        [Test]
        public void Permute_Encrypts()
        {
            var input = new byte[]
            {
                0x01, 0x02, 0x03, 0x04, 0x05
            };
            var expected = new byte[]
            {
                0x36, 0x13, 0x62, 0xa8, 0x21
            };

            Crypt.CryptPermute(input, true);

            CollectionAssert.AreEqual(expected, input);
        }

        [Test]
        public void Permute_Decrypts()
        {
            var input = new byte[]
            {
                0x36, 0x13, 0x62, 0xa8, 0x21
            };
            var expected = new byte[]
            {
                0x01, 0x02, 0x03, 0x04, 0x05
            };

            Crypt.CryptPermute(input, false);

            CollectionAssert.AreEqual(expected, input);
        }
    }
}
