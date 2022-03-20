namespace HandicapModel.SeasonModel
{
    using System.Collections.Generic;

    using Interfaces.SeasonModel;

    /// <summary>
    /// Contains the details of a specific club over the course of a season.
    /// </summary>
    /// <remarks>
    /// This part of the model pertains to the team trophy which limits the size of the team.
    /// The team size will be hardcoded to be 10, if there are not enough runners then points will
    /// be appended for missing runners, otherwise it works like a mob match. Lowest points win.
    /// </remarks>
    public class TeamTrophyCompetition : ITeamTrophyCompetition
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="TeamTrophyCompetition"/> class.
        /// </summary>
        public TeamTrophyCompetition()
        {
            this.Events = new List<ITeamTrophyEvent>();
        }

        /// <summary>
        /// Gets a collection of all the events this team has entered.
        /// </summary>
        public List<ITeamTrophyEvent> Events { get; }

        /// <summary>
        /// Gets the total score for this team.
        /// </summary>
        public int TotalScore
        {
            get
            {
                int points = 0;

                foreach (ITeamTrophyEvent race in this.Events)
                {
                    points += race.Score;
                }

                return points;
            }
        }

        /// <summary>
        /// Add a new events results for this season.
        /// </summary>
        /// <remarks>
        /// If there is already an event on the <paramref name="date"/> then it should be overwritten
        /// instead of adding a new event.
        /// </remarks>
        /// <param name="newEvent">new event</param>
        public void AddEvent(
            ITeamTrophyEvent newEvent)
        {
            if (this.Events.Count > 0)
            {
                for (int index = 0; index < this.Events.Count; ++index)
                {
                    if (this.Events[index].Date == newEvent.Date)
                    {
                        this.Events[index] =
                            newEvent;
                        return;
                    }
                }
            }

            this.Events.Add(newEvent);
        }
    }
}
