﻿namespace HandicapModel.SeasonModel
{
    using System;
    using System.Collections.Generic;
    using Admin.Manage;
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
        /// Initialise a new instance of the <see cref="AthleteSeasonPoints"/> class.
        /// </summary>
        public AthleteSeasonPoints()
        {
            this.AllPoints = new List<CommonPoints>();
        }

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to this model.
        /// </summary>
        public event EventHandler ModelUpdateEvent;

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
        //public int PositionPoints { get; set; }
        public int PositionPoints
        {
            get
            {
                int positionPoints = 0;

                foreach (CommonPoints points in this.AllPoints)
                {
                    positionPoints += points.PositionPoints;
                }

                return positionPoints;
            }
        }

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
            this.AllPoints.Add(newPoints);
            this.ModelUpdateEvent?.Invoke(this, EventArgs.Empty);
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
            if (eventIndex < this.AllPoints.Count)
            {
                this.AllPoints[eventIndex].FinishingPoints = finishingPoints;
                this.AllPoints[eventIndex].PositionPoints = positionPoints;
                this.AllPoints[eventIndex].BestPoints = bestPoints;
                this.ModelUpdateEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Update the points earnt for position for the indicated athlete on the indicated date.
        /// </summary>
        /// <param name="date">date of the event</param>
        /// <param name="points">earned points</param>
        public void UpdatePositionPoints(DateType date, int points)
        {
            this.AllPoints.Find(allPoints => allPoints.Date == date).PositionPoints = points;
            this.ModelUpdateEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Remove all points corresponding to the argument date. Do nothing if there are no appearances for the date.
        /// </summary>
        /// </summary>
        /// <param name="date">date to remove.</param>
        public void RemovePoints(DateType date)
        {
            if (this.AllPoints.Exists(dateCheck => dateCheck.Date == date))
            {
                this.AllPoints.Remove(AllPoints.Find(dateCheck => dateCheck.Date == date));
            }

            this.ModelUpdateEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}