using System;

namespace Pst.Internal.Ndb
{
    internal class Node
    {
        private readonly IPstReader _reader;

        internal Node(Nid nid, ulong dataBid, ulong subnodeBid, IPstReader reader)
        {
            Nid = nid;
            DataBid = dataBid;
            SubnodeBid = subnodeBid;
            _reader = reader;
        }

        internal Nid Nid { get; private set; }
        internal ulong DataBid { get; private set; }
        internal ulong SubnodeBid { get; private set; }

        internal Block GetDataBlock()
        {
            return _reader.FindBlock(DataBid);
        }

        internal Node FindSubnode(Nid subnodeNid)
        {
            if (SubnodeBid == 0)
                throw new Exception("No subnode block found for this node.");

            var subnode = new SubnodeReader(SubnodeBid, _reader);
            return subnode.FindSubnode(subnodeNid);
        }
    }
}
