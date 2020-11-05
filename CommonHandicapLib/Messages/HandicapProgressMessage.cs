namespace CommonHandicapLib.Messages
{
    /// <summary>
    /// Message used by the MVVM Light messenger service to pass progress messages around
    /// </summary>
    public class HandicapProgressMessage
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="HandicapProgressMessage"/> class.
        /// </summary>
        /// <param name="message">progress message</param>
        public HandicapProgressMessage(string message)
        {
            this.ProgressMessage = message;
        }

        /// <summary>
        /// Gets the progress message.
        /// </summary>
        public string ProgressMessage { get; }
    }
}