using PlayerIOClient;

public sealed class AutoTextReceiveMessage : ReceiveMessage
{
    //0
    //1

    public readonly string AutoText;
    public readonly int UserID;

    internal AutoTextReceiveMessage(Message message)
        : base(message)
    {
        this.UserID = message.GetInteger(0);
        this.AutoText = message.GetString(1);
    }
}