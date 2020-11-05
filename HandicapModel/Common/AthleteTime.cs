namespace HandicapModel.Common
{
    using CommonLib.Types;
    using HandicapModel.Interfaces.Common;

    /// <summary>
    /// Records a time against a specific athlete.
    /// </summary>
    public class AthleteTime : IAthleteTime
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="AthleteTime"/> class.
        /// </summary>
        /// <param name="key">new key</param>
        /// <param name="name">new name</param>
        /// <param name="time">new time</param>
        /// <param name="date">new date</param>
        public AthleteTime(
            int key,
            string name,
            TimeType time,
            DateType date)
        {
            Key = key;
            Name = name;
            Time = time;
            Date = date;
        }

        /// <summary>
        /// Gets the unique key associated with the athlete
        /// </summary>
        public int Key { get; private set; }

        /// <summary>
        /// Gets the athlete's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the athlete's time.
        /// </summary>
        public TimeType Time { get; private set; }

        /// <summary>
        /// Gets the date the time was set.
        /// </summary>
        public DateType Date { get; private set; }

        /// <summary>
        /// Updates the whole record.
        /// </summary>
        /// <param name="key">new key</param>
        /// <param name="name">new name</param>
        /// <param name="time">new time</param>
        /// <param name="date">new date</param>
        public void SetNewTime(
            int key,
            string name,
            TimeType time,
            DateType date)
        {
            Key = key;
            Name = name;
            Time = time;
            Date = date;
        }
    }
}