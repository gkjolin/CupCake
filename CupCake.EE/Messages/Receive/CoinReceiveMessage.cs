using PlayerIOClient;

namespace CupCake.EE.Messages.Receive
{
    public sealed class CoinReceiveMessage : ReceiveMessage
    {
        public CoinReceiveMessage(Message message)
            : base(message)
        {
            this.UserId = message.GetInteger(0);
            this.Coins = message.GetInteger(1);
        }

        public int Coins { get; private set; }
        public int UserId { get; private set; }
    }
}