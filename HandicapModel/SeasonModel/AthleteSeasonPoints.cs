namespace HandicapModel.SeasonModel
{
    using System.Collections.Generic;
    using Admin.Manage;
    using CommonHandicapLib.Types;
    using CommonLib.Types;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.SeasonModel;

    //// TODO Am not sure about this part of the model
    //// Is this class needed, can it all go in to the main AthleteSeasonDetails Class.
    //// The events should include the name/date/key information
    //// 13/06/20: Kept note, this keeps these details separate and avoids clutter in 
    //// the main class.

    /// <summary>
    /// Records all the points received over the course of a season.
    /// </summary>
    /// <remarks>
    /// This class contains a list of Athlete points, there will be one per event in the current season. 
    /// </remarks>
    public class AthleteSeasonPoints : IAthleteSeasonPoints
    {
        /// <summary>
        /// The configuration manager
        /// </summary>
        private readonly IResultsConfigMngr resultsConfigurationManager;

        /// <summary>
        /// Initialise a new instance of the <see cref="AthleteSeasonPoints"/> class.
        /// </summary>
        /// <param name="config">Instructions on how to read the scores</param>
        public AthleteSeasonPoints(IResultsConfigMngr resultsConfigurationManager)
        {
            this.resultsConfigurationManager = resultsConfigurationManager;
            this.AllPoints = new List<CommonPoints>();
        }

        /// <summary>
        /// Gets all the points received.
        /// </summary>
        public List<CommonPoints> AllPoints { get; }

        /// <summary>
        /// Returns all the points the athlete has earnt.
        /// </summary>
        public int TotalPoints => this.FinishingPoints + this.PositionPoints + this.BestPoints;

        /// <summary>
        /// Returns all the finishing points the athlete has earnt.
        /// </summary>
        public int FinishingPoints
        {
            get
            {
                int finishingPoints = 0;

                foreach (CommonPoints points in this.AllPoints)
                {
                    finishingPoints += points.FinishingPoints;
                }

                return finishingPoints;
            }
        }

        /// <summary>
        /// Returns all the position points the athlete has earnt.
        /// </summary>
        public int PositionPoints { get; set; }

        /// <summary>
        /// Returns all the best points the athlete has earnt.
        /// </summary>
        public int BestPoints
        {
            get
            {
                int bestPoints = 0;

                foreach (CommonPoints points in this.AllPoints)
                {
                    bestPoints += points.BestPoints;
                }

                return bestPoints;
            }
        }

        /// <summary>
        /// Add a new entry to represent a new event.
        /// </summary>
        /// <param name="date">Date of the event</param>
        public void AddNewEvent(CommonPoints newPoints)
        {
            AllPoints.Add(newPoints);

            this.PositionPoints = this.CalculatePositionPoints();
        }

        /// <summary>
        /// Set points to an existing entry.
        /// </summary>
        /// <param name="eventIndex">event Id</param>
        /// <param name="finishingPoints">ponts received for finishing</param>
        /// <param name="positionPoints">points scoring postion</param>
        /// <param name="bestPoints">points received for running a seasons best.</param>
        public void SetPoints(
            int eventIndex,
            int finishingPoints,
            int positionPoints,
            int bestPoints)
        {
            if (eventIndex < AllPoints.Count)
            {
                AllPoints[eventIndex].FinishingPoints = finishingPoints;
                AllPoints[eventIndex].PositionPoints = positionPoints;
                AllPoints[eventIndex].BestPoints = bestPoints;
            }

            this.PositionPoints = this.CalculatePositionPoints();
        }

        /// <summary>
        /// Update the points earnt for position for the indicated athlete on the indicated date.
        /// </summary>
        /// <param name="date">date of the event</param>
        /// <param name="points">earned points</param>
        public void UpdatePositionPoints(DateType date, int points)
        {
            AllPoints.Find(allPoints => allPoints.Date == date).PositionPoints = points;
            this.PositionPoints = this.CalculatePositionPoints();
        }

        /// <summary>
        /// Remove all points corresponding to the argument date. Do nothing if there are no appearances for the date.
        /// </summary>
        /// </summary>
        /// <param name="date">date to remove.</param>
        public void RemovePoints(DateType date)
        {
            if (AllPoints.Exists(dateCheck => dateCheck.Date == date))
            {
                AllPoints.Remove(AllPoints.Find(dateCheck => dateCheck.Date == date));
            }

            this.PositionPoints = this.CalculatePositionPoints();
        }

        /// <summary>
        /// Determine the new position points
        /// </summary>
        /// <returns></returns>
        private int CalculatePositionPoints()
        {
            // Get working set.
            List<int> points = new List<int>();
            foreach (CommonPoints common in this.AllPoints)
            {
                points.Add(common.PositionPoints);
            }

            // If all scores used, just return.
            ResultsConfigType resultsConfigurationDetails =
                this.resultsConfigurationManager.ResultsConfigurationDetails;

            if (resultsConfigurationDetails.AllResults)
            {
                return this.SumCollection(points);
            }

            // Order scores to make analysis easier.
            points.Sort();

            if (this.resultsConfigurationManager.ResultsConfigurationDetails.ScoresAreDescending)
            {
                points.Reverse();
            }
            else
            {
                // Make up for shortfall in scores.
                while (points.Count < this.resultsConfigurationManager.ResultsConfigurationDetails.ScoresToCount)
                {
                    points.Add(this.resultsConfigurationManager.ResultsConfigurationDetails.MissingScore);
                }
            }

            // Get rid of non counting scores
            while (points.Count > this.resultsConfigurationManager.ResultsConfigurationDetails.ScoresToCount)
            {
                points.RemoveAt(points.Count);
            }

            return this.SumCollection(points);
        }

        /// <summary>
        /// Sum all the values in an integer enumerable collection.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private int SumCollection(IEnumerable<int> collection)
        {
            int sum = 0;

            foreach (int value in collection)
            {
                sum += value;
            }

            return sum;
        }
    }
}