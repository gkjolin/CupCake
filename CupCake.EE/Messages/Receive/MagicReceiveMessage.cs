using PlayerIOClient;

namespace CupCake.EE.Messages.Receive
{
    public class MagicReceiveMessage : ReceiveMessage
    {
        public MagicReceiveMessage(Message message)
            : base(message)
        {
            this.UserId = message.GetInteger(0);
        }

        public int UserId { get; private set; }
    }
}