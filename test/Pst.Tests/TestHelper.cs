using System.IO;
using System.Reflection;

namespace Pst.Tests
{
    public static class TestHelper
    {
        public static Stream GetTestDataStream(string fileName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("Pst.Tests.TestData." + fileName);
        }
    }
}
