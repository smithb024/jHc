namespace HandicapModel.Interfaces.SeasonModel
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface which describes the team harmony competition. This is the second competition
    /// and involves the first ten athletes from a team attempting to score the fewest points.
    /// </summary>
    public interface IHarmonyCompetition
    {
        /// <summary>
        /// Gets the total points scored by the team during the competition.
        /// </summary>
        int TotalPoints { get; }

        /// <summary>
        /// Gets a collection of all the events recorded for this team in this season's competition.
        /// </summary>
        List<IHarmonyEvent> Events { get; }

        /// <summary>
        /// Add a new events results for this season.
        /// </summary>
        /// <param name="newEvent">new harmony event</param>
        void AddEvent(IHarmonyEvent newEvent);
    }
}
