using CupCake.EE.Players;
using PlayerIOClient;

namespace CupCake.EE.Messages.Send
{
    public sealed class ChangeFaceSendMessage : SendMessage
    {
        public ChangeFaceSendMessage(string encryption, Smiley face)
        {
            this.Encryption = encryption;
            this.Face = face;
        }

        public Smiley Face { get; set; }

        public string Encryption { get; set; }

        public override Message GetMessage()
        {
            return Message.Create(this.Encryption + "f", this.Face);
        }
    }
}