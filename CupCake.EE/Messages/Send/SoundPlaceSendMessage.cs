using CupCake.EE.Blocks;
using PlayerIOClient;

namespace CupCake.EE.Messages.Send
{
    public sealed class SoundPlaceSendMessage : BlockPlaceSendMessage
    {
        public readonly int SoundId;

        public SoundPlaceSendMessage(string encryption, Layer layer, int x, int y, SoundBlock block, int soundId)
            : base(encryption, layer, x, y, (Block)block)
        {
            this.SoundId = soundId;
        }

        internal override Message GetMessage()
        {
            if (IsSound(this.Block))
            {
                Message message = base.GetMessage();
                message.Add(this.SoundId);
                return message;
            }
            return base.GetMessage();
        }
    }
}