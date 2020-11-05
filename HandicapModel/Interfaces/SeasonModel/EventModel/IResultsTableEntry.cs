namespace HandicapModel.Interfaces.SeasonModel.EventModel
{
    using CommonHandicapLib.Types;
    using CommonLib.Enumerations;
    using HandicapModel.Interfaces.Common;

    /// <summary>
    /// Interface which describes a completed result for a single athlete.
    /// </summary>
    public interface IResultsTableEntry
    {
        /// <summary>
        /// Gets the athlete age grading
        /// </summary>
        string AgeGrading { get; }

        /// <summary>
        /// Gets the athlete club.
        /// </summary>
        string Club { get; }

        /// <summary>
        /// Gets any extra information associated with the result.
        /// </summary>
        string ExtraInfo { get; set; }

        /// <summary>
        /// Gets a value indicating whether this was the first event for the athlete.
        /// </summary>
        bool FirstTimer { get; set; }

        /// <summary>
        /// Gets the handicap for the athlete in this event.
        /// </summary>
        RaceTimeType Handicap { get; }

        /// <summary>
        /// Gets the athlete id.
        /// </summary>
        int Key { get; }

        /// <summary>
        /// Gets the name of the athlete.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the result notes.
        /// </summary>
        string Notes { get; }

        /// <summary>
        /// Gets the order value, used to differentiate between results with the same overall time.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Gets or sets a value which indicates whether this is a PB.
        /// </summary>
        bool PB { get; set; }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        ICommonPoints Points { get; set; }

        /// <summary>
        /// Gets or sets the points associated with the harmony competition.
        /// </summary>
        int HarmonyPoints { get; set; }

        /// <summary>
        /// Gets the finishing position.
        /// </summary>
        int Position { get; }

        /// <summary>
        /// Gets the number used to enter this event.
        /// </summary>
        string RaceNumber { get; }

        /// <summary>
        /// Gets or sets the speed order (traditional way of calculating results)
        /// </summary>
        int RunningOrder { get; set; }

        /// <summary>
        /// Gets the total time spent running. (Time excluding handicap).
        /// </summary>
        RaceTimeType RunningTime { get; }

        /// <summary>
        /// Gets or sets a value which indicates whether this is a SB.
        /// </summary>
        bool SB { get; set; }

        /// <summary>
        /// Gets the sex of the athlete.
        /// </summary>
        SexType Sex { get; }

        /// <summary>
        /// Gets the total time (Time including handicap).
        /// </summary>
        RaceTimeType Time { get; }
    }
}