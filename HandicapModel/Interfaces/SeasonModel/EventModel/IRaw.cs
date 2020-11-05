namespace HandicapModel.Interfaces.SeasonModel.EventModel
{
    using CommonHandicapLib.Types;

    /// <summary>
    /// Interface describing the raw event data.
    /// </summary>
    public interface IRaw
    {
        /// <summary>
        /// Gets the order value, this is important when multiple athletes have the same time.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Gets the number used for the event.
        /// </summary>
        string RaceNumber { get; }

        /// <summary>
        /// Gets the total time taken, including handicap.
        /// </summary>
        RaceTimeType TotalTime { get; }
    }
}