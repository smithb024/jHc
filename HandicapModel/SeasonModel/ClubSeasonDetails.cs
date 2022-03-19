namespace HandicapModel.SeasonModel
{
    using System.Collections.Generic;
    using HandicapModel.Interfaces.Common;
    using Interfaces.SeasonModel;
    using CommonLib.Types;

    /// <summary>
    /// Contains the details of a specific club over the course of a season.
    /// </summary>
    public class ClubSeasonDetails : IClubSeasonDetails
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ClubSeasonDetails"/> class.
        /// </summary>
        /// <param name="clubName">name of this team.</param>
        public ClubSeasonDetails(string clubName)
        {
            this.Name = clubName;
            this.MobTrophy = new MobTrophy();
            this.HarmonyCompetition = new HarmonyCompetition();
        }

        /// <summary>
        /// Gets the club name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the details of the mob trophy for this team.
        /// </summary>
        public IMobTrophy MobTrophy { get; }

        /// <summary>
        /// Gets the details of the team harmony competition for this team.
        /// </summary>
        public IHarmonyCompetition HarmonyCompetition { get; }

        /// <summary>
        /// Add a new event.
        /// </summary>
        /// <param name="newPoints">points received</param>
        public void AddNewEvent(
            ICommonPoints newPoints)
        {
            this.MobTrophy.AddNewEvent(newPoints);
        }

        /// <summary>
        /// Add a new event.
        /// </summary>
        /// <param name="newPoints">points harmony received</param>
        public void AddNewEvent(
            IHarmonyEvent newEvent)
        {
            this.HarmonyCompetition.AddEvent(newEvent);
        }

        /// <summary>
        /// Set points to an existing entry
        /// </summary>
        /// <param name="eventIndex">event index</param>
        /// <param name="updatedPoints">points received</param>
        public void SetMobTrophyPoints(
            int eventIndex,
            ICommonPoints updatedPoints)
        {
            this.MobTrophy.SetPoints(
                eventIndex,
                updatedPoints);
        }

        /// <summary>
        /// Remove all points corresponding to the argument date. Do nothing if there are no points for the date.
        /// </summary>
        /// <param name="date"></param>
        public void RemoveMobTrophyPoints(DateType date)
        {
            this.MobTrophy.RemovePoints(date);
        }
    }
}