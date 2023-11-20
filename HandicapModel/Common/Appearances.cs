namespace HandicapModel.Common
{
    using CommonHandicapLib.Types;
    using CommonLib.Types;

    /// <summary>
    /// Records the date and run time of a specific appearance.
    /// </summary>
    public class Appearances
    {
        /// <summary>
        /// The appearance time.
        /// </summary>
        private RaceTimeType time;

        /// <summary>
        /// The date of the appearance.
        /// </summary>
        private DateType date;

        /// <summary>
        /// Initialises a new instance of the <see cref="Appearances"/> class.
        /// </summary>
        /// <param name="time">The time taken</param>
        /// <param name="date">The date of the appearance</param>
        public Appearances(
            RaceTimeType time,
            DateType date)
        {
            Time = time;
            Date = date;
        }

        /// <summary>
        /// Gets and sets the appearance time.
        /// </summary>
        public RaceTimeType Time
        {
            get { return time; }
            set { time = value; }
        }

        /// <summary>
        /// Gets the appearance time as a string.
        /// </summary>
        public string TimeString => this.time.ToString();

        /// <summary>
        /// Gets and sets the appearance date.
        /// </summary>
        public DateType Date
        {
            get { return date; }
            set { date = value; }
        }

        /// <summary>
        /// Gets the appearance date as a string.
        /// </summary>
        public string DateString => this.date.ToString();

        /// <summary>
        /// Is the time valid.
        /// </summary>
        /// <remarks>
        /// Checks to see if the time is not null. 
        /// Then it checks to see if the time is not DNF or unknown.
        /// </remarks>
        /// <returns>Time valid flag</returns>
        public bool IsTimeValid()
        {
            if (time == null)
            {
                return false;
            }

            return !time.DNF && !time.Unknown;
        }
    }
}