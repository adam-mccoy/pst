using System;
using Pst.Internal;
using Pst.Internal.Ltp;
using Pst.Internal.Ndb;

namespace Pst
{
    public class Message
    {
        private readonly Nid _nid;
        private readonly IPstReader _pstReader;
        private readonly Lazy<PropertyContext> _properties;

        internal Message(Nid nid, IPstReader reader)
        {
            _nid = nid;
            _pstReader = reader;
            _properties = new Lazy<PropertyContext>(Initialize);
        }

        public string Subject => _properties.Value.Get(PropertyKey.Subject)?.ToString(_pstReader);

        private PropertyContext Initialize()
        {
            var node = _pstReader.FindNode(_nid);
            return new PropertyContext(node, _pstReader);
        }
    }
}
