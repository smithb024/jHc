using CommonHandicapLib.Types;
using CommonLib.Types;
using HandicapModel.Common;
using HandicapModel.SeasonModel;
using System.Collections.Generic;

namespace HandicapModel.Interfaces.SeasonModel
{
    /// <summary>
    /// Interface which describes the athletes details for a single season
    /// </summary>
    public interface IAthleteSeasonDetails
    {
        /// <summary>
        /// Gets the unique key.
        /// </summary>
        int Key { get; }

        /// <summary>
        /// Gets the athlete name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets and sets the best time this season.
        /// </summary>
        TimeType SB { get; }

        /// <summary>
        /// Gets and sets the number of points
        /// </summary>
        AthleteSeasonPoints Points { get; set; }

        /// <summary>
        /// Gets and sets the number of points assoicated with the harmnony competiion.
        /// </summary>
        IAthleteSeasonTeamTrophyPoints TeamTrophyPoints { get; set; }

        /// <summary>
        /// Gets all the times run this season.
        /// </summary>
        List<Appearances> Times { get; }

        /// <summary>
        /// Gets the number of events taken part in, in the current season.
        /// </summary>
        int NumberOfAppearances { get; }

        /// <summary>
        /// Adds a new event time to the list.
        /// </summary>
        /// <param name="runningNumber">running number</param>
        void AddNewTime(Appearances time);

        /// <summary>
        /// Update the points earnt for position for the indicated athlete on the indicated date.
        /// </summary>
        /// <param name="key">unique key</param>
        /// <param name="date">date of the event</param>
        /// <param name="points">earned points</param>
        void UpdatePositionPoints(DateType date, int points);

        /// <summary>
        /// Add some new points
        /// </summary>
        /// <param name="points">points to add</param>
        void AddNewPoints(CommonPoints points);

        /// <summary>
        /// Calculates a new handicap from the list of times.
        /// </summary>
        RaceTimeType GetRoundedHandicap(NormalisationConfigType hcConfiguration);

        /// <summary>
        /// Remove all appearances corresponding to the argument date. Do nothing if there are no appearances for the date.
        /// </summary>
        /// <param name="date">date to remove.</param>
        void RemoveAppearances(DateType date);

        /// <summary>
        /// Remove all points corresponding to the argument date. Do nothing if there are no appearances for the date.
        /// </summary>
        /// <param name="date">date to remove</param>
        void RemovePoints(DateType date);
    }
}