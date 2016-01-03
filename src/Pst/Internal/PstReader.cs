using System;
using System.IO;
using Pst.Extensions;

namespace Pst.Internal
{
    internal class PstReader
    {
        private static readonly byte[] MagicBytes = new byte[] { 0x21, 0x42, 0x44, 0x4e };
        private static readonly byte[] MagicClientBytes = new byte[] { 0x53, 0x4d };
        private static readonly ushort[] SupportedVersions = new ushort[] { 14, 15, 23 };

        private const int UnicodeHeaderLength = 564;
        private const int AnsiHeaderLength = 512;

        private readonly Stream _input;
        private ushort _fileVersion;

        internal PstReader(Stream input)
        {
            ValidateStream(input);
            _input = input;
            VerifyHeader();
        }

        internal bool IsAnsi
        {
            get { return _fileVersion == 14 || _fileVersion == 15; }
        }

        internal Stream Stream
        {
            get { return _input; }
        }

        internal void VerifyHeader()
        {
            var buffer = new byte[UnicodeHeaderLength];
            ReadBytes(buffer, 0, 28);
            Validate.Match(buffer.Segment(0, 4), MagicBytes, "Magic value invalid.");
            Validate.Match(buffer.Segment(8, 2), MagicClientBytes, "Magic client value invalid.");
            _fileVersion = BitConverter.ToUInt16(buffer, 4);
            Validate.Any(_fileVersion, SupportedVersions, "Found unsupported version.");
            ReadBytes(buffer, 28, (IsAnsi ? AnsiHeaderLength : UnicodeHeaderLength) - 28);
            var crcPartial = BitConverter.ToUInt32(buffer, 4);
            Validate.Match(crcPartial, Crc32.Calculate(buffer.Segment(8, 471)), "Partial CRC invalid.");
        }

        private void ReadBytes(byte[] buffer, int offset, int count)
        {
            var bytesRead = 0;
            while (bytesRead < count)
                bytesRead += _input.Read(buffer, offset + bytesRead, count - bytesRead);
        }

        private void ValidateStream(Stream input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (!input.CanRead)
                throw new ArgumentException("Stream cannot be read.", nameof(input));
            if (!input.CanSeek)
                throw new ArgumentException("Stream cannot be seeked.", nameof(input));
        }
    }
}
