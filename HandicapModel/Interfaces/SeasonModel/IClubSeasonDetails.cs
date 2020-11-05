namespace HandicapModel.Interfaces.SeasonModel
{
    using HandicapModel.Interfaces.Common;
    using CommonLib.Types;

    /// <summary>
    /// Interface which describes a season for a specific club. It manages the various competetions.
    /// </summary>
    public interface IClubSeasonDetails
    {
        /// <summary>
        /// Gets the club name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the details of the club competition for this team.
        /// </summary>
        IClubCompetition ClubCompetition { get; }

        /// <summary>
        /// Gets the details of the team harmony competition for this team.
        /// </summary>
        IHarmonyCompetition HarmonyCompetition { get; }

        /// <summary>
        /// Add a new event.
        /// </summary>
        /// <param name="newPoints">points received</param>
        void AddNewEvent(
            ICommonPoints newPoints);

        /// <summary>
        /// Add a new event.
        /// </summary>
        /// <param name="newEvent">new event</param>
        void AddNewEvent(
            IHarmonyEvent newEvent);

        /// <summary>
        /// Set points to an existing entry
        /// </summary>
        /// <param name="eventIndex">event index</param>
        /// <param name="updatedPoints">points received</param>
        void SetClubCompetitionPoints(
            int eventIndex,
            ICommonPoints updatedPoints);

        /// <summary>
        /// Remove all points corresponding to the argument date. Do nothing if there are no points for the date.
        /// </summary>
        /// <param name="date"></param>
        void RemoveClubCompetitionPoints(DateType date);
    }
}
