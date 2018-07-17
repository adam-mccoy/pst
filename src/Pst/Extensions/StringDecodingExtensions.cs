using System.Text;

namespace Pst.Internal
{
    internal static class StringDecodingExtensions
    {
        public static string DecodeString(this IPstReader reader, Segment<byte> encodedString)
        {
            var encoding = reader.IsAnsi ? Encoding.UTF8 : Encoding.Unicode;
            return encoding.GetString(encodedString.Array, encodedString.Offset, encodedString.Count);
        }
    }
}
