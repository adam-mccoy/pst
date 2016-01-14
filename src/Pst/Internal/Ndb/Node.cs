namespace Pst.Internal.Ndb
{
    internal class Node
    {
        internal Node(Nid nid, ulong dataBid, ulong subnodeBid)
        {
            Nid = nid;
            DataBid = dataBid;
            SubnodeBid = subnodeBid;
        }

        internal Nid Nid { get; private set; }
        internal ulong DataBid { get; private set; }
        internal ulong SubnodeBid { get; private set; }
    }
}
