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
        public MessageImportance Importance => (MessageImportance)_properties.Value.Get(PropertyKey.Importance)?.ToInt32();
        public MessagePriority Priority => (MessagePriority)_properties.Value.Get(PropertyKey.Priority)?.ToInt32();
        public string MessageClass => _properties.Value.Get(PropertyKey.MessageClass)?.ToString(_pstReader);
        public bool? ReadReceiptRequested => _properties.Value.Get(PropertyKey.ReadReceiptsRequested)?.ToBoolean();
        public MessageSensitivity Sensitivity => (MessageSensitivity)_properties.Value.Get(PropertyKey.Sensitivity)?.ToInt32();

        private PropertyContext Initialize()
        {
            var node = _pstReader.FindNode(_nid);
            return new PropertyContext(node, _pstReader);
        }
    }
}
