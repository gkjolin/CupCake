using System.Collections.Generic;
using CupCake.EE;
using PlayerIOClient;

public sealed class TeleportEveryoneReceiveMessage : ReceiveMessage
{
    //0

    public readonly Dictionary<int, Point> Coordinates = new Dictionary<int, Point>();
    public readonly bool ResetCoins;

    internal TeleportEveryoneReceiveMessage(Message message)
        : base(message)
    {
        this.ResetCoins = message.GetBoolean(0);

        for (uint i = 1; i <= message.Count - 1u; i += 3)
        {
            this.Coordinates.Add(message.GetInteger(i),
                new Point(message.GetInteger(i + 1u), message.GetInteger(i + 2u)));
        }
    }
}