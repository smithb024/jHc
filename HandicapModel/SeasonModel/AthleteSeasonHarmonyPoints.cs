namespace HandicapModel.SeasonModel
{
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AthleteSeasonHarmonyPoints : IAthleteSeasonHarmonyPoints
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="AthleteSeasonHarmonyPointsq"/> class.
        /// </summary>
        public AthleteSeasonHarmonyPoints()
        {
            this.AllPoints = new List<IAthleteHarmonyPoints>();
        }

        /// <summary>
        /// Gets a collection of all the points received.
        /// </summary>
        public List<IAthleteHarmonyPoints> AllPoints { get; }

        /// <summary>
        /// Gets the total number of points scored for the team.
        /// </summary>
        public int TotalPoints
        {
            get
            {
                int points = 0;

                foreach (CommonHarmonyPoints point in this.AllPoints)
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

                foreach (CommonHarmonyPoints point in this.AllPoints)
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
        public void AddNewEvent(IAthleteHarmonyPoints newPoints)
        {
            IAthleteHarmonyPoints foundPoints =
                this.AllPoints.Find(p => p.Date == newPoints.Date);

            if (foundPoints != null)
            {
                this.AllPoints.Remove(foundPoints);
            }

            this.AllPoints.Add(newPoints);
            //this.AllPoints.OrderBy(p => p.Date).ToList();
        }
    }
}
