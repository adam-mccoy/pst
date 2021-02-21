using System;
using System.Collections.Generic;
using System.Linq;
using Pst.Internal;
using Pst.Internal.Ltp;
using Pst.Internal.Ndb;

namespace Pst
{
    public class Message
    {
        private const uint RecipientTableNid = 0x692;

        private readonly Nid _nid;
        private readonly IPstReader _pstReader;
        private readonly Lazy<Node> _node;
        private readonly Lazy<PropertyContext> _properties;
        private readonly Lazy<TableContext> _recipients;

        internal Message(Nid nid, IPstReader reader)
        {
            _nid = nid;
            _pstReader = reader;
            _node = new Lazy<Node>(LoadNode);
            _properties = new Lazy<PropertyContext>(GetPropertyContext);
            _recipients = new Lazy<TableContext>(GetRecipientsTable);
        }

        public string Subject => _properties.Value.Get(PropertyKey.Subject)?.ToString(_pstReader);
        public MessageImportance Importance => (MessageImportance)_properties.Value.Get(PropertyKey.Importance)?.ToInt32();
        public MessagePriority Priority => (MessagePriority)_properties.Value.Get(PropertyKey.Priority)?.ToInt32();
        public string MessageClass => _properties.Value.Get(PropertyKey.MessageClass)?.ToString(_pstReader);
        public bool? ReadReceiptRequested => _properties.Value.Get(PropertyKey.ReadReceiptsRequested)?.ToBoolean();
        public MessageSensitivity Sensitivity => (MessageSensitivity)_properties.Value.Get(PropertyKey.Sensitivity)?.ToInt32();
        public IEnumerable<Recipient> Recipients => _recipients.Value?.Rows.Select(row => new Recipient(row, _pstReader));

        private Node LoadNode() => _pstReader.FindNode(_nid);
        private PropertyContext GetPropertyContext() => new PropertyContext(_node.Value, _pstReader);
        private TableContext GetRecipientsTable() => new TableContext(_node.Value.FindSubnode(RecipientTableNid), _pstReader);
    }
}
