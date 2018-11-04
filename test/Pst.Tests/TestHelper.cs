using System.IO;
using System.Reflection;

namespace Pst.Tests
{
    public static class TestHelper
    {
        public static Stream GetTestDataStream(string fileName) =>
            Assembly.GetExecutingAssembly().GetManifestResourceStream("Pst.Tests.TestData." + fileName);

        public static byte[] GetTestDataBytes(string fileName)
        {
            using (var stream = GetTestDataStream(fileName))
            {
                return new BinaryReader(stream).ReadBytes((int)stream.Length);
            }
        }
    }
}
