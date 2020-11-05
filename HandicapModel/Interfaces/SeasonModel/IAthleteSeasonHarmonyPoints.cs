namespace HandicapModel.Interfaces.SeasonModel
{
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Common;
    using System.Collections.Generic;

    /// <summary>
    /// Interface which describes all the points scored by an athlete in a specific season.
    /// This is for the harmony points competition
    /// </summary>
    public interface IAthleteSeasonHarmonyPoints
    {
        /// <summary>
        /// Gets a collection of all the points received.
        /// </summary>
        List<IAthleteHarmonyPoints> AllPoints { get; }

        /// <summary>
        /// Gets the total number of points scored for the team.
        /// </summary>
        int TotalPoints { get; }

        /// <summary>
        /// Gets the number of results which were elegable for the team.
        /// </summary>
        int NumberOfScoringEvents { get; }

        /// <summary>
        /// Add a new entry to represent a new event.
        /// </summary>
        /// <remarks>
        /// Will overwrite an existing points if one already exists.
        /// </remarks>
        /// <param name="date">Date of the event</param>
        void AddNewEvent(IAthleteHarmonyPoints newPoints);
    }
}