namespace HandicapModel.SeasonModel.EventModel
{
    using CommonHandicapLib.Types;
    using HandicapModel.Interfaces.SeasonModel.EventModel;

    /// <summary>
    /// A single entry of the raw event data.
    /// </summary>
    public class Raw : IRaw
    {
        /// <summary>
        /// Creates a new instance of the Raw clas.
        /// </summary>
        /// <param name="number">Number used in the event</param>
        /// <param name="time">total time taken, including the handicap</param>
        /// <param name="timeOrder">order within the same time bracket</param>
        public Raw(
            string number,
            RaceTimeType time,
            int timeOrder)
        {
            this.RaceNumber = number;
            this.TotalTime = time;
            this.Order = timeOrder;
        }

        /// <summary>
        /// Gets the order value, this is important when multiple athletes have the same time.
        /// </summary>
        public int Order { get; private set; }

        /// <summary>
        /// Gets the number used for the race.
        /// </summary>
        public string RaceNumber { get; private set; }

        /// <summary>
        /// Gets the total time taken, including handicap.
        /// </summary>
        public RaceTimeType TotalTime { get; private set; }
    }
}