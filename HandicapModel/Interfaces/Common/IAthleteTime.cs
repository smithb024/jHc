namespace HandicapModel.Interfaces.Common
{
    using CommonLib.Types;

    /// <summary>
    /// Interface which describes a time.
    /// </summary>
    public interface IAthleteTime
    {
        /// <summary>
        /// Gets the unique key associated with the athlete
        /// </summary>
        int Key { get; }

        /// <summary>
        /// Gets the athlete's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the athlete's time.
        /// </summary>
        TimeType Time { get; }

        /// <summary>
        /// Gets the date the time was set.
        /// </summary>
        DateType Date { get; }

        /// <summary>
        /// Updates the whole record.
        /// </summary>
        /// <param name="key">new key</param>
        /// <param name="name">new name</param>
        /// <param name="time">new time</param>
        /// <param name="date">new date</param>
        void SetNewTime(
            int key,
            string name,
            TimeType time,
            DateType date);
    }
}