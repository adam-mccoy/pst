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
    }
}
