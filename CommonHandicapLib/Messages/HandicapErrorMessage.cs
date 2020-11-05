namespace CommonHandicapLib.Messages
{
    /// <summary>
    /// Message used by the MVVM Light messenger service to pass error messages around
    /// </summary>
    public class HandicapErrorMessage
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="HandicapErrorMessage"/> class.
        /// </summary>
        /// <param name="message">error message</param>
        public HandicapErrorMessage(string message)
        {
            this.ErroMessage = message;
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string ErroMessage { get; }
    }
}