namespace HandicapModel.SeasonModel
{
    using System.Collections.Generic;

    using CommonLib.Types;
    using HandicapModel.Common;
    using Interfaces.Common;
    using Interfaces.SeasonModel;

    /// <summary>
    /// A single event for a team within the harmony competition.
    /// </summary>
    public class HarmonyEvent : IHarmonyEvent
    {
        /// <summary>
        /// The size of a team. To be valid, the points collection should equal this size.
        /// </summary>
        public const int TeamSize = 10;

        /// <summary>
        /// Initialises a new instance of the <see cref="HarmonyEvent"/> class.
        /// </summary>
        /// <param name="date">date of the event</param>
        /// <param name="points">event points</param>
        public HarmonyEvent(
            DateType date,
            List<ICommonHarmonyPoints> points)
        {
            this.Date = date;
            this.Points = points;
            this.VirtualAthletePoints = 0;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="HarmonyEvent"/> class.
        /// </summary>
        /// <param name="date">date of the event</param>
        public HarmonyEvent(
            DateType date)
        {
            this.Date = date;
            this.Points = new List<ICommonHarmonyPoints>();
            this.VirtualAthletePoints = 0;
        }

        /// <summary>
        /// Gets the date of the event.
        /// </summary>
        public DateType Date { get; }

        /// <summary>
        /// Gets the total points scored in this event.
        /// </summary>
        public int TotalPoints 
        {
            get
            {
                int points = 0;

                foreach(ICommonHarmonyPoints point in this.Points)
                {
                    points += point.Point;
                }

                return points;
            }
        }

        /// <summary>
        /// Gets the number of athletes present in the team to a maximum of 10.
        /// </summary>
        public int NumberOfAthletes => this.Points.FindAll(p => p.IsReal).Count;

        /// <summary>
        /// Gets a collection containing the break down of points.
        /// </summary>
        public List<ICommonHarmonyPoints> Points { get; }

        /// <summary>
        /// Gets a value indicating whether the points collection is valid.
        /// </summary>
        public bool PointsValid => this.Points.Count == TeamSize;

        /// <summary>
        /// Gets a value which states the number of points scored by any virtual athletes which 
        /// need to be created to make up the team to a full team. 
        /// This value will be one greater than the highest scoring real athlete.
        /// </summary>
        public int VirtualAthletePoints { get; private set; }

        /// <summary>
        /// Add a new point to the <see cref="Points"/> collection.
        /// </summary>
        /// <remarks>
        /// The point is only added if there is space in the team.
        /// </remarks>
        /// <param name="newPoint"><see cref="ICommonHarmonyPoints"/> to add</param>
        /// <returns>success flag</returns>
        public bool AddPoint(ICommonHarmonyPoints newPoint)
        {
            bool success = false;

            if (this.Points.Count < TeamSize)
            {
                this.Points.Add(newPoint);
                success = true;
            }
            else if (this.Points.Count > TeamSize)
            {
                this.Points.RemoveRange(TeamSize, this.Points.Count - TeamSize);
            }

            return success;
        }

        /// <summary>
        /// Complete the <see cref="Points"/> collection with dummy values with the <paramref name="pointsValue"/>
        /// as the points value of the remaining points up to the team size.
        /// </summary>
        /// <param name="teamSize">size of the team</param>
        /// <param name="pointsValue">value of remaining points</param>
        public void Complete(
            int teamSize,
            int pointsValue)
        {
            this.VirtualAthletePoints = pointsValue;

            if (this.Points.Count < teamSize)
            {
                while (this.Points.Count < teamSize)
                {
                    ICommonHarmonyPoints virtualPoint =
                        new CommonHarmonyPoints(
                            pointsValue,
                            string.Empty,
                            -1,
                            false,
                            this.Date);
                    this.Points.Add(virtualPoint);
                }
            }
            else
            {
                // TODO Trim long points totals?
            }
        }
    }
}