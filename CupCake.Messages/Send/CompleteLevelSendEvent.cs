using PlayerIOClient;

namespace CupCake.Messages.Send
{
    /// <summary>
    ///     Class Complete Level Send Event
    /// </summary>
    public class CompleteLevelSendEvent : SendEvent
    {
        //No arguments

        /// <summary>
        ///     Gets the PlayerIO message representing the data in this <see cref="SendEvent" />.
        /// </summary>
        /// <returns></returns>
        public override Message GetMessage()
        {
            return Message.Create("levelcomplete");
        }
    }
}