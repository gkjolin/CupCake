using CupCake.EE.Blocks;
using PlayerIOClient;

namespace CupCake.EE.Messages.Send
{
    public sealed class LabelPlaceSendMessage : BlockPlaceSendMessage
    {
        public readonly string Text;

        public LabelPlaceSendMessage(string encryption, Layer layer, int x, int y, LabelBlock block, string text)
            : base(encryption, layer, x, y, (Block)block)
        {
            this.Text = text;
        }

        internal override Message GetMessage()
        {
            if (IsLabel(this.Block))
            {
                Message message = base.GetMessage();
                message.Add(this.Text);
                return message;
            }
            return base.GetMessage();
        }
    }
}