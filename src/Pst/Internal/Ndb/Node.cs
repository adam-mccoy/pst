using System;
using System.Diagnostics;
using System.IO;

namespace Pst.Internal.Ndb
{
    [DebuggerDisplay("Nid: {Nid.Type} {Nid.Index}, DataBid: {DataBid}, SubnodeBid: {SubnodeBid}")]
    internal class Node
    {
        private readonly IPstReader _reader;
        private readonly SubnodeReader _subnodeReader;

        internal Node(Nid nid, ulong dataBid, ulong subnodeBid, IPstReader reader)
        {
            Nid = nid;
            DataBid = dataBid;
            SubnodeBid = subnodeBid;

            _reader = reader;
            if (SubnodeBid != 0)
                _subnodeReader = new SubnodeReader(SubnodeBid, reader);
        }

        internal Nid Nid { get; private set; }
        internal ulong DataBid { get; private set; }
        internal ulong SubnodeBid { get; private set; }

        internal Stream GetDataStream() => new NodeDataStream(DataBid, _reader);

        internal Node FindSubnode(Nid subnodeNid)
        {
            if (SubnodeBid == 0)
                throw new Exception("No subnode block found for this node.");

            return _subnodeReader.FindSubnode(subnodeNid);
        }
    }
}
