namespace CommonHandicapLib.Types
{
    /// <summary>
    /// Enumeration describing the type of race time being presented.
    /// </summary>
    public enum RaceTimeDescription
    {
        /// <summary>
        /// The event was completed normally. The time represents the finishing time.
        /// </summary>
        Finished,

        /// <summary>
        /// The event was started, but not completed.
        /// </summary>
        Dnf,

        /// <summary>
        /// The event is a relay event. Everyone would get a consistent score.
        /// </summary>
        Relay,

        /// <summary>
        /// Default value, the time is not known.
        /// </summary>
        Unknown
    }
}