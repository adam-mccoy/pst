using Pst.Internal.Ndb;

namespace Pst.Internal
{
    internal interface IPstReader
    {
        bool IsAnsi { get; }
        BbtEntry LookupBlock(Bid bid);
        Block FindBlock(Bid bid);
        Node FindNode(Nid nid);
    }
}
