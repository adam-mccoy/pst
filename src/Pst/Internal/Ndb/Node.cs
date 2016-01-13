namespace Pst.Internal.Ndb
{
    internal class Node
    {
        internal Node(uint nid, ulong dataBid, ulong subnodeBid)
        {
            Nid = nid;
            DataBid = dataBid;
            SubnodeBid = subnodeBid;
        }

        internal uint Nid { get; private set; }
        internal ulong DataBid { get; private set; }
        internal ulong SubnodeBid { get; private set; }
    }
}
