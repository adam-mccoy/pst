using System.Linq;
using Pst;

namespace PstEditor
{
    public class MessageModel
    {
        private readonly Message _message;

        public MessageModel(Message message)
        {
            _message = message;
        }

        public string Subject => _message.Subject;
        public MessagePriority Priority => _message.Priority;
        public MessageImportance Importance => _message.Importance;
        public string MessageClass => _message.MessageClass;
        public bool? ReadReceiptsRequested => _message.ReadReceiptRequested;
        public MessageSensitivity? Sensitivity => _message.Sensitivity;
        public string To => string.Join(",", _message.Recipients.Where(r => r.Type == RecipientType.Primary).Select(r => $"{r.DisplayName} <{r.EmailAddress}>"));
        public string Cc => string.Join(",", _message.Recipients.Where(r => r.Type == RecipientType.Cc).Select(r => $"{r.DisplayName} <{r.EmailAddress}>"));
    }
}
