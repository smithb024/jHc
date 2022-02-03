namespace jHCVMUI.ViewModels.ViewModels.Types.Clubs
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using CommonLib.Types;
    using HandicapModel.Interfaces.Common;

    /// <summary>
    /// View model which is used to show all the points scored by a team for a specific event 
    /// within the harmony team event.
    /// </summary>
    public class HarmonyPointsTypeViewModel : ViewModelBase
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="HarmonyPointsTypeViewModel"/> class.
        /// </summary>
        /// <param name="score">The score for this team in this event</param>
        /// <param name="aggregatePoints">
        /// Aggregate of all contributing points.
        /// </param>
        /// <param name="contributingSize">number of members of the team</param>
        /// <param name="inputPoints">collection of all points in the event</param>
        /// <param name="date">event date</param>
        public HarmonyPointsTypeViewModel(
            int score,
            int aggregatePoints,
            int contributingSize,
            List<ICommonHarmonyPoints> inputPoints,
            DateType date)
        {
            this.Score = score;
            this.AggregatePoints = aggregatePoints;
            this.ContributingSize = contributingSize;
            this.PointsDate = date;

            string allPoints = string.Empty;

            for (int index = 0; index < inputPoints.Count; index++)
            {
                if (index == 0)
                {
                    allPoints = inputPoints[index].Point.ToString();
                }
                else
                {
                    allPoints = $"{allPoints}, {inputPoints[index].Point}";
                }
            }

            this.AllPoints = allPoints;
        }

        /// <summary>
        /// Gets the score by this team for this event.
        /// </summary>
        public int Score { get; }

        /// <summary>
        /// Gets a list of all the points scored in this event.
        /// </summary>
        public string AllPoints { get; }

        /// <summary>
        /// Gets the aggregate of all individual points scored by the team.
        /// </summary>
        public int AggregatePoints { get; }

        /// <summary>
        /// Gets the number of athletes who scored in this event.
        /// </summary>
        public int ContributingSize { get; }

        /// <summary>
        /// Gets the date of this event.
        /// </summary>
        public DateType PointsDate { get; }
    }
}