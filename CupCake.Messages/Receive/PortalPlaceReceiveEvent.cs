using System;
using CupCake.Messages.Blocks;
using PlayerIOClient;

namespace CupCake.Messages.Receive
{
    /// <summary>
    /// Occurs when a portal is placed in the world.
    /// </summary>
    public class PortalPlaceReceiveEvent : ReceiveEvent, IBlockPlaceReceiveEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PortalPlaceReceiveEvent"/> class.
        /// </summary>
        /// <param name="message">The EE message.</param>
        public PortalPlaceReceiveEvent(Message message)
            : base(message)
        {
            this.PosX = message.GetInteger(0);
            this.PosY = message.GetInteger(1);
            this.Block = (PortalBlock)message.GetInteger(2);
            this.PortalRotation = (PortalRotation)message.GetUInt(3);
            this.PortalId = message.GetUInt(4);
            this.PortalTarget = message.GetUInt(5);
        }

        /// <summary>
        /// Gets or sets the layer.
        /// </summary>
        /// <value>The layer.</value>
        public Layer Layer
        {
            get { return Layer.Foreground; }
        }

        /// <summary>
        /// Gets or sets the block.
        /// </summary>
        /// <value>The block.</value>
        public PortalBlock Block { get; set; }
        /// <summary>
        /// Gets or sets the portal identifier.
        /// </summary>
        /// <value>The portal identifier.</value>
        public uint PortalId { get; set; }
        /// <summary>
        /// Gets or sets the portal rotation.
        /// </summary>
        /// <value>The portal rotation.</value>
        public PortalRotation PortalRotation { get; set; }
        /// <summary>
        /// Gets or sets the portal target.
        /// </summary>
        /// <value>The portal target.</value>
        public uint PortalTarget { get; set; }
        /// <summary>
        /// Gets or sets the position x.
        /// </summary>
        /// <value>The position x.</value>
        public int PosX { get; set; }
        /// <summary>
        /// Gets or sets the position y.
        /// </summary>
        /// <value>The position y.</value>
        public int PosY { get; set; }

        /// <summary>
        /// Gets or sets the layer.
        /// </summary>
        /// <value>The layer.</value>
        /// <exception cref="System.NotSupportedException">Can not set Layer on this kind of block</exception>
        Layer IBlockPlaceReceiveEvent.Layer
        {
            get { return this.Layer; }
            set { throw new NotSupportedException("Can not set Layer on this kind of block"); }
        }

        /// <summary>
        /// Gets or sets the block.
        /// </summary>
        /// <value>The block.</value>
        Block IBlockPlaceReceiveEvent.Block
        {
            get { return (Block)this.Block; }
            set { this.Block = (PortalBlock)value; }
        }
    }
}