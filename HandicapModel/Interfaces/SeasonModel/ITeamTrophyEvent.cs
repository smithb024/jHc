﻿namespace HandicapModel.Interfaces.SeasonModel
{
    using System.Collections.Generic;
    using CommonLib.Types;
    using Interfaces.Common;

    /// <summary>
    /// Interface which details a single event for a team within the Team Trophy.
    /// </summary>
    public interface ITeamTrophyEvent
    {
        /// <summary>
        /// Gets the date of the event.
        /// </summary>
        DateType Date { get; }

        /// <summary>
        /// Gets the total points scored in this event.
        /// </summary>
        /// <remarks>
        /// This is the total number of points scored by all the athletes (real and virtual) in 
        /// this event.
        /// </remarks>
        int TotalAthletePoints { get; }

        /// <summary>
        /// Gets or sets the score which is scored by this team for this event.
        /// </summary>
        int Score { get; set; }

        /// <summary>
        /// Gets the number of athletes present in the team to a maximum of 10.
        /// </summary>
        int NumberOfAthletes { get; }

        /// <summary>
        /// Gets a collection containing the break down of points.
        /// </summary>
        List<ICommonTeamTrophyPoints> Points { get; }

        /// <summary>
        /// Gets a value indicating whether the points collection is valid.
        /// </summary>
        bool PointsValid { get; }

        /// <summary>
        /// Gets a value which states the number of points scored by any virtual athletes which 
        /// need to be created to make up the team to a full team. 
        /// This value will be one greater than the highest scoring real athlete.
        /// </summary>
        int VirtualAthletePoints { get; }

        /// <summary>
        /// Gets the size of a Team Trophy team.
        /// </summary>
        int TeamSize { get; }

        /// <summary>
        /// Add a new point to the <see cref="Points"/> collection.
        /// </summary>
        /// <remarks>
        /// The point is only added if there is space in the team.
        /// </remarks>
        /// <param name="newPoint"><see cref="ICommonTeamTrophyPoints"/> to add</param>
        /// <returns>success flag</returns>
        bool AddPoint(ICommonTeamTrophyPoints newPoint);

        /// <summary>
        /// Complete the <see cref="Points"/> collection with dummy values with the <paramref name="pointsValue"/>
        /// as the points value of the remaining points up to the team size.
        /// </summary>
        /// <param name="teamSize">size of the team</param>
        /// <param name="pointsValue">value of remaining points</param>
        void Complete(
            int teamSize,
            int pointsValue);
    }
}