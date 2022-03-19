namespace HandicapModel.Interfaces.SeasonModel
{
    using System.Collections.Generic;
    using CommonLib.Types;
    using Interfaces.Common;

    /// <summary>
    /// Interface which describes the mob trophy. This is the original competition which 
    /// involves a full team trying to score as many points as is possible.
    /// </summary>
    public interface IMobTrophy
    {
        /// <summary>
        /// Gets the total points awarded to the club.
        /// </summary>
        int TotalPoints { get; }

        /// <summary>
        /// Gets the total finishing points awarded to the club.
        /// </summary>
        int TotalFinishingPoints { get; }

        /// <summary>
        /// Gets the total position points awarded to the club.
        /// </summary>
        int TotalPositionPoints { get; }

        /// <summary>
        /// Gets the total best points awarded to the club.
        /// </summary>
        int TotalBestPoints { get; }

        /// <summary>
        /// Gets all the points received over the current season.
        /// </summary>
        List<ICommonPoints> Points { get; }

        /// <summary>
        /// Add a new event.
        /// </summary>
        /// <param name="newPoints">points received</param>
        void AddNewEvent(ICommonPoints newPoints);

        /// <summary>
        /// Set points to an existing entry
        /// </summary>
        /// <param name="eventIndex">event index</param>
        /// <param name="updatedPoints">points received</param>
        void SetPoints(
            int eventIndex,
            ICommonPoints updatedPoints);

        /// <summary>
        /// Remove all points corresponding to the argument date. Do nothing if there are no points for the date.
        /// </summary>
        /// <param name="date"></param>
        void RemovePoints(DateType date);
    }
}