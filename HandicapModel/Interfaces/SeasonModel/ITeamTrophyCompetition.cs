namespace HandicapModel.Interfaces.SeasonModel
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface which describes the Team Trophy competition. This is the second competition
    /// and involves the first ten athletes from a team attempting to score the fewest points.
    /// </summary>
    public interface ITeamTrophyCompetition
    {
        /// <summary>
        /// Gets the total score for this team.
        /// </summary>
        int TotalScore { get; }

        /// <summary>
        /// Gets a collection of all the events recorded for this team in this season's competition.
        /// </summary>
        List<ITeamTrophyEvent> Events { get; }

        /// <summary>
        /// Add a new events results for this season.
        /// </summary>
        /// <param name="newEvent">new Team Trophy event</param>
        void AddEvent(ITeamTrophyEvent newEvent);
    }
}
