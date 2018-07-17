using Pst.Internal.Ndb;

namespace Pst.Internal
{
    internal interface IPstReader
    {
        bool IsAnsi { get; }
        Block FindBlock(Bid bid);
        Node FindNode(Nid nid);
    }
}
