using System;
using System.IO;
using Pst.Internal.Ndb;

namespace Pst.Internal
{
    internal class PstReader : IPstReader
    {
        private const int RootOffset = 180;
        private const int CryptMethodOffset = 0x201;

        private static readonly byte[] MagicBytes = new byte[] { 0x21, 0x42, 0x44, 0x4e };
        private static readonly byte[] MagicClientBytes = new byte[] { 0x53, 0x4d };
        private static readonly ushort[] SupportedVersions = new ushort[] { 14, 15, 23 };

        private const int UnicodeHeaderLength = 564;
        private const int AnsiHeaderLength = 512;

        private readonly Stream _stream;
        private ushort _fileVersion;
        private CryptMethod _cryptMethod;
        private BTreeReader<BbtEntry> _bbtReader;
        private BTreeReader<NbtEntry> _nbtReader;

        internal PstReader(Stream input)
        {
            ValidateStream(input);
            _stream = input;
            VerifyHeader();
        }

        public bool IsAnsi => _fileVersion == 14 || _fileVersion == 15;

        public BbtEntry LookupBlock(Bid bid) => _bbtReader.Find(bid);

        public Block FindBlock(Bid bid)
        {
            var entry = _bbtReader.Find(bid);
            if (entry == null)
                return null;

            var blockSize = 64 * ((entry.Length + Block.TrailerLength + 63) / 64);
            var block = new byte[blockSize];
            _stream.Seek((long)entry.Bref.Ib, SeekOrigin.Begin);
            ReadBytes(block, 0, blockSize);

            var result = Block.Create(block);
            Validate.Match(result.Crc, Crc32.Calculate(result.Data.Slice(0, result.Length)), "Block CRC doesn't match.");

            if (_cryptMethod != CryptMethod.None && bid.Type == BlockType.External)
                DecryptBlock(result.Data, result.Bid);

            return result;
        }

        public Node FindNode(Nid nid)
        {
            var entry = _nbtReader.Find(nid);
            if (entry == null)
                return null;

            return new Node(entry.Nid, entry.DataBid, entry.SubnodeBid, this);
        }

        private void VerifyHeader()
        {
            var buffer = new byte[UnicodeHeaderLength];
            ReadBytes(buffer, 0, 28);
            Validate.Match(buffer.Slice(0, 4), MagicBytes, "Magic value invalid.");
            Validate.Match(buffer.Slice(8, 2), MagicClientBytes, "Magic client value invalid.");
            _fileVersion = BitConverter.ToUInt16(buffer, 10);
            Validate.Any(_fileVersion, SupportedVersions, "Found unsupported version.");
            ReadBytes(buffer, 28, (IsAnsi ? AnsiHeaderLength : UnicodeHeaderLength) - 28);
            var crcPartial = BitConverter.ToUInt32(buffer, 4);
            Validate.Match(crcPartial, Crc32.Calculate(buffer.Slice(8, 471)), "Partial CRC invalid.");

            var nbt = new Bref(
                buffer.Slice(RootOffset + 36).ToUInt64(),
                buffer.Slice(RootOffset + 44).ToUInt64());
            var bbt = new Bref(
                buffer.Slice(RootOffset + 52).ToUInt64(),
                buffer.Slice(RootOffset + 60).ToUInt64());

            _nbtReader = new BTreeReader<NbtEntry>(_stream, (long)nbt.Ib, seg => new NbtEntry(seg));
            _bbtReader = new BTreeReader<BbtEntry>(_stream, (long)bbt.Ib, seg => new BbtEntry(seg));

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
                bytesRead += _stream.Read(buffer, offset + bytesRead, count - bytesRead);
        }

        private static void ValidateStream(Stream input)
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
