namespace CommonHandicapLib.Types
{
    /// <summary>
    /// Stores the position and race number details as imported from a text file.
    /// </summary>
    public class RawPositionResults
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="RawPositionResults"/> class.
        /// </summary>
        /// <param name="raceNumber"></param>
        /// <param name="position"></param>
        public RawPositionResults(
            string raceNumber,
            int position)
        {
            this.RaceNumber = raceNumber;
            this.Position = position;
        }

        /// <summary>
        /// Gets the race number.
        /// </summary>
        public string RaceNumber { get; }

        /// <summary>
        /// Gets the position of the race number.
        /// </summary>
        public int Position { get; }
    }
}