using Pst.Internal;
using System;
using System.IO;

namespace Pst
{
    public class PstFile
    {
        private readonly Stream _input;
        private readonly PstReader _pstReader;

        private bool _isOpen;

        public PstFile(Stream input)
        {
            _input = input;
            _pstReader = new PstReader(_input);
            _isOpen = true;
        }

        public MessageStore MessageStore
        {
            get
            {
                if (!_isOpen)
                    throw new InvalidOperationException("File is not open.");

                return new MessageStore(0x21, _pstReader);
            }
        }
    }
}
