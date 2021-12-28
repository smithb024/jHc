namespace CommonHandicapLib.Messages
{
    /// <summary>
    /// Message class. This is used when a new season has been selected and is to be loaded
    /// into the model.
    /// </summary>
    public class LoadNewSeasonMessage
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="LoadNewSeasonMessage"/> class.
        /// </summary>
        /// <param name="seasonName">season name</param>
        public LoadNewSeasonMessage(string seasonName)
        {
            this.Name = seasonName;
        }

        /// <summary>
        /// Gets the name of the new season.
        /// </summary>
        public string Name { get; }
    }
}