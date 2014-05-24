using CupCake.EE.Blocks;
using PlayerIOClient;

namespace CupCake.EE.Events.Receive
{
    public class ClearReceiveEvent : ReceiveEvent
    {
        public ClearReceiveEvent(Message message)
            : base(message)
        {
            this.RoomWidth = message.GetInteger(0);
            this.RoomHeight = message.GetInteger(1);
            this.BorderBlock = (Block)message.GetInteger(2);
            this.FillBlock = (Block)message.GetInteger(3);
        }

        public Block FillBlock { get; set; }
        public Block BorderBlock { get; set; }

        public int RoomHeight { get; private set; }
        public int RoomWidth { get; private set; }
    }
}