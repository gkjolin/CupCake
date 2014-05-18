using System;
using CupCake.EE.Blocks;
using PlayerIOClient;

namespace CupCake.EE.Messages.Send
{
    public sealed class PortalPlaceSendMessage : BlockPlaceSendMessage
    {
        public readonly int PortalId;

        public readonly PortalRotation PortalRotation;
        public readonly int PortalTarget;

        public PortalPlaceSendMessage(string encryption, Layer layer, int x, int y, PortalBlock block, int portalId,
            int portalTarget, PortalRotation portalRotation)
            : base(encryption, layer, x, y, (Block)block)
        {
            this.PortalId = portalId;
            this.PortalTarget = portalTarget;
            this.PortalRotation = portalRotation;
        }

        internal override Message GetMessage()
        {
            if (IsPortal(this.Block))
            {
                Message message = base.GetMessage();
                message.Add(Convert.ToInt32(this.PortalRotation), this.PortalId, this.PortalTarget);
                return message;
            }
            return base.GetMessage();
        }
    }
}