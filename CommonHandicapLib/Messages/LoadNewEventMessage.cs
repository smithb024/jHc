namespace CommonHandicapLib.Messages
{
    /// <summary>
    /// Message class. This is used when a new event has been selected and is to be loaded
    /// into the model.
    /// </summary>
    public class LoadNewEventMessage
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LoadNewEventMessage"/> class.
        /// </summary>
        /// <param name="seasonName">event name</param>
        public LoadNewEventMessage(string eventName)
        {
            this.Name = eventName;
        }

        /// <summary>
        /// Gets the name of the new season.
        /// </summary>
        public string Name { get; }
    }
}