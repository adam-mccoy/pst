using System;
using System.IO;
using Pst.Extensions;
using Pst.Internal.Ndb;

namespace Pst.Internal
{
    internal class PstReader
    {
        private const int RootOffset = 180;
        private const int CryptMethodOffset = 0x201;

        private static readonly byte[] MagicBytes = new byte[] { 0x21, 0x42, 0x44, 0x4e };
        private static readonly byte[] MagicClientBytes = new byte[] { 0x53, 0x4d };
        private static readonly ushort[] SupportedVersions = new ushort[] { 14, 15, 23 };

        private const int UnicodeHeaderLength = 564;
        private const int AnsiHeaderLength = 512;

        private readonly Stream _input;
        private ushort _fileVersion;
        private CryptMethod _cryptMethod;
        private BTreeReader<BbtEntry> _bbtReader;
        private BTreeReader<NbtEntry> _nbtReader;

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

        internal Block ReadBlock(ulong bid)
        {
            var entry = _bbtReader.Find(bid);
            if (entry == null)
                return null;

            var blockSize = 64 * ((entry.Length + 64) / 64);
            var block = new byte[blockSize];
            _input.Seek((long)entry.Bref.Ib, SeekOrigin.Begin);
            ReadBytes(block, 0, blockSize);

            var result = Block.Create(block);

            if (_cryptMethod != CryptMethod.None)
                DecryptBlock(block, result.Bid);

            return result;
        }

        internal Block ReadBlock(uint nid)
        {
            var entry = _nbtReader.Find(nid);
            if (entry == null)
                return null;

            return ReadBlock(entry.DataBid);
        }

        private void VerifyHeader()
        {
            var buffer = new byte[UnicodeHeaderLength];
            ReadBytes(buffer, 0, 28);
            Validate.Match(buffer.Segment(0, 4), MagicBytes, "Magic value invalid.");
            Validate.Match(buffer.Segment(8, 2), MagicClientBytes, "Magic client value invalid.");
            _fileVersion = BitConverter.ToUInt16(buffer, 10);
            Validate.Any(_fileVersion, SupportedVersions, "Found unsupported version.");
            ReadBytes(buffer, 28, (IsAnsi ? AnsiHeaderLength : UnicodeHeaderLength) - 28);
            var crcPartial = BitConverter.ToUInt32(buffer, 4);
            Validate.Match(crcPartial, Crc32.Calculate(buffer.Segment(8, 471)), "Partial CRC invalid.");

            var nbt = new Bref(
                BitConverter.ToUInt64(buffer, RootOffset + 36),
                BitConverter.ToUInt64(buffer, RootOffset + 44));
            var bbt = new Bref(
                BitConverter.ToUInt64(buffer, RootOffset + 52),
                BitConverter.ToUInt64(buffer, RootOffset + 60));

            _nbtReader = new BTreeReader<NbtEntry>(_input, (long)nbt.Ib);
            _bbtReader = new BTreeReader<BbtEntry>(_input, (long)bbt.Ib);

            _cryptMethod = (CryptMethod)buffer[CryptMethodOffset];
        }

        private void DecryptBlock(byte[] block, ulong bid)
        {
            switch (_cryptMethod)
            {
                case CryptMethod.Permute:
                    Crypt.CryptPermute(block, false);
                    break;

                case CryptMethod.Cyclic:
                    Crypt.CryptCyclic(block, (uint)bid);
                    break;
            }
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
