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
    public class HarmonyCompetition : IHarmonyCompetition
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="HarmonyCompetition"/> class.
        /// </summary>
        public HarmonyCompetition()
        {
            this.Events = new List<IHarmonyEvent>();
        }

        /// <summary>
        /// Gets a collection of all the events this team has entered.
        /// </summary>
        public List<IHarmonyEvent> Events { get; }

        /// <summary>
        /// Gets the total points scored by this team.
        /// </summary>
        public int TotalPoints
        {
            get
            {
                int points = 0;

                foreach (IHarmonyEvent race in this.Events)
                {
                    points += race.TotalPoints;
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
            IHarmonyEvent newEvent)
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
