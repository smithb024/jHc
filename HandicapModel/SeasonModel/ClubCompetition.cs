namespace HandicapModel.SeasonModel
{
    using System.Collections.Generic;

    using HandicapModel.Common;
    using Interfaces.Common;
    using Interfaces.SeasonModel;
    using CommonLib.Types;

    /// <summary>
    /// The club competition. This is the original competition which involves a full team trying 
    /// to score as many points as is possible.
    /// </summary>
    public class ClubCompetition : IClubCompetition
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ClubCompetition"/> class.
        /// </summary>
        public ClubCompetition()
        {
            this.Points = new List<ICommonPoints>();
        }

        /// <summary>
        /// Gets the total points awarded to the club.
        /// </summary>
        public int TotalPoints
        {
            get
            {
                int totalPoints = 0;

                foreach (ICommonPoints points in this.Points)
                {
                    totalPoints += points.TotalPoints;
                }

                return totalPoints;
            }
        }

        /// <summary>
        /// Gets the total finishing points awarded to the club.
        /// </summary>
        public int TotalFinishingPoints
        {
            get
            {
                int totalPoints = 0;

                foreach (ICommonPoints points in this.Points)
                {
                    totalPoints += points.FinishingPoints;
                }

                return totalPoints;
            }
        }

        /// <summary>
        /// Gets the total position points awarded to the club.
        /// </summary>
        public int TotalPositionPoints
        {
            get
            {
                int totalPoints = 0;

                foreach (ICommonPoints points in this.Points)
                {
                    totalPoints += points.PositionPoints;
                }

                return totalPoints;
            }
        }

        /// <summary>
        /// Gets the total best points awarded to the club.
        /// </summary>
        public int TotalBestPoints
        {
            get
            {
                int totalPoints = 0;

                foreach (ICommonPoints points in this.Points)
                {
                    totalPoints += points.BestPoints;
                }

                return totalPoints;
            }
        }

        /// <summary>
        /// Gets all the points received over the current season.
        /// </summary>
        public List<ICommonPoints> Points { get; }

        /// <summary>
        /// Add a new event.
        /// </summary>
        /// <param name="newPoints">points received</param>
        public void AddNewEvent(ICommonPoints newPoints)
        {
            this.Points.Add(newPoints);
        }

        /// <summary>
        /// Set points to an existing entry
        /// </summary>
        /// <param name="eventIndex">event index</param>
        /// <param name="updatedPoints">points received</param>
        public void SetPoints(
            int eventIndex,
            ICommonPoints updatedPoints)
        {
            if (eventIndex < this.Points.Count)
            {
                this.Points[eventIndex] = updatedPoints;
            }
        }

        /// <summary>
        /// Remove all points corresponding to the argument date. Do nothing if there are no points for the date.
        /// </summary>
        /// <param name="date"></param>
        public void RemovePoints(DateType date)
        {
            if (this.Points.Exists(dateCheck => dateCheck.Date == date))
            {
                this.Points.Remove(this.Points.Find(dateCheck => dateCheck.Date == date));
            }
        }
    }
}