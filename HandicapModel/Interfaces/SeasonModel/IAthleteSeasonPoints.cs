namespace HandicapModel.Interfaces.SeasonModel
{
    using CommonLib.Types;
    using HandicapModel.Common;
    using System.Collections.Generic;

    /// <summary>
    /// Defines all the points scored by an athlete in a specific season.
    /// This is for the normal Points Competition.
    /// </summary>
    public interface IAthleteSeasonPoints
    {
        /// <summary>
        /// Gets a collection of all the points received.
        /// </summary>
        List<CommonPoints> AllPoints { get; }

        /// <summary>
        /// Returns all the points the athlete has earnt.
        /// </summary>
        int TotalPoints { get; }

        /// <summary>
        /// Returns all the finishing points the athlete has earnt.
        /// </summary>
        int FinishingPoints { get; }

        /// <summary>
        /// Returns all the position points the athlete has earnt.
        /// </summary>
        int PositionPoints { get; }

        /// <summary>
        /// Returns all the best points the athlete has earnt.
        /// </summary>
        int BestPoints { get; }

        /// <summary>
        /// Add a new entry to represent a new event.
        /// </summary>
        /// <param name="date">Date of the event</param>
        void AddNewEvent(CommonPoints newPoints);

        /// <summary>
        /// Set points to an existing entry.
        /// </summary>
        /// <param name="eventIndex">Event Id</param>
        /// <param name="finishingPoints">ponts received for finishing</param>
        /// <param name="positionPoints">points scoring postion</param>
        /// <param name="bestPoints">points received for running a seasons best.</param>
        void SetPoints(
            int eventIndex,
            int finishingPoints,
            int positionPoints,
            int bestPoints);

        /// <summary>
        /// Update the points earnt for position for the indicated athlete on the indicated date.
        /// </summary>
        /// <param name="date">date of the event</param>
        /// <param name="points">earned points</param>
        void UpdatePositionPoints(
            DateType date,
            int points);

        /// <summary>
        /// Remove all points corresponding to the argument date. Do nothing if there are no appearances for the date.
        /// </summary>
        /// </summary>
        /// <param name="date">date to remove.</param>
        void RemovePoints(DateType date);
    }
}