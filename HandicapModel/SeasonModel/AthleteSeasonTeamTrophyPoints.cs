namespace HandicapModel.SeasonModel
{
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel;
    using System.Collections.Generic;

    public class AthleteSeasonTeamTrophyPoints : IAthleteSeasonTeamTrophyPoints
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="AthleteSeasonTeamTrophyPoints"/> class.
        /// </summary>
        public AthleteSeasonTeamTrophyPoints()
        {
            this.AllPoints = new List<IAthleteTeamTrophyPoints>();
        }

        /// <summary>
        /// Gets a collection of all the points received.
        /// </summary>
        public List<IAthleteTeamTrophyPoints> AllPoints { get; }

        /// <summary>
        /// Gets the total number of points scored for the team.
        /// </summary>
        public int TotalPoints
        {
            get
            {
                int points = 0;

                foreach (CommonTeamTrophyPoints point in this.AllPoints)
                {
                    if (point.Point > 0)
                    {
                        points += point.Point;
                    }

                }

                return points;
            }
        }

        /// <summary>
        /// Gets the number of results which were elegable for the team.
        /// </summary>
        public int NumberOfScoringEvents
        {
            get
            {
                int counter = 0;

                foreach (CommonTeamTrophyPoints point in this.AllPoints)
                {
                    if (point.Point > 0)
                    {
                        ++counter;
                    }

                }

                return counter;
            }
        }

        /// <summary>
        /// Add a new entry to represent a new event.
        /// </summary>
        /// <remarks>
        /// Will overwrite an existing points if one already exists.
        /// </remarks>
        /// <param name="date">Date of the event</param>
        public void AddNewEvent(IAthleteTeamTrophyPoints newPoints)
        {
            IAthleteTeamTrophyPoints foundPoints =
                this.AllPoints.Find(p => p.Date == newPoints.Date);

            if (foundPoints != null)
            {
                this.AllPoints.Remove(foundPoints);
            }

            this.AllPoints.Add(newPoints);
        }
    }
}