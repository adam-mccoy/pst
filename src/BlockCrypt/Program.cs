using System;
using System.IO;
using System.Linq;
using Pst.Internal;

namespace BlockCrypt
{
    class Program
    {
        private static Options _options;

        static void Main(string[] args)
        {
            ProcessArguments(args);

            var data = File.ReadAllBytes(_options.FilePath);

            switch (_options.Method)
            {
                case "permute":
                    Crypt.CryptPermute(data, _options.Mode == "encrypt");
                    break;

                case "cyclic":
                    Crypt.CryptCyclic(data, (uint)_options.BlockId);
                    break;
            }

            File.WriteAllBytes(_options.FilePath, data);
        }

        static void ProcessArguments(string[] args)
        {
            string filePath = null,
                   method = null,
                   mode = null;
            ulong blockId = 0;

            var keys = args.Where(s => s.StartsWith("-")).Select(s => new { Key = s.Substring(1), Index = Array.IndexOf(args, s) });
            foreach (var k in keys)
            {
                switch (k.Key)
                {
                    case "m":
                        method = args[k.Index + 1];
                        break;

                    case "f":
                        filePath = args[k.Index + 1];
                        break;

                    case "d":
                        mode = args[k.Index + 1];
                        break;

                    case "b":
                        blockId = ulong.Parse(args[k.Index + 1]);
                        break;
                }
            }
            _options = new Options(filePath, method, mode, blockId);
        }

        private class Options
        {
            public Options(string filePath, string method, string mode, ulong blockId)
            {
                FilePath = filePath;
                Method = method;
                BlockId = blockId;
            }

            public string FilePath { get; private set; }
            public string Method { get; private set; }
            public string Mode { get; private set; }
            public ulong BlockId { get; private set; }
        }
    }
}
