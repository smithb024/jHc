namespace HandicapModel.SeasonModel
{
    using System.Collections.Generic;

    using CommonLib.Types;
    using HandicapModel.Common;
    using Interfaces.Common;
    using Interfaces.SeasonModel;

    /// <summary>
    /// A single event for a team within the Team Trophy.
    /// </summary>
    public class TeamTrophyEvent : ITeamTrophyEvent
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="TeamTrophyEvent"/> class.
        /// </summary>
        /// <param name="date">date of the event</param>
        /// <param name="points">event points</param>
        /// <param name="teamSize">the size of a Team Trophy team</param>
        /// <param name="score">the score of the team in the current event</param>
        public TeamTrophyEvent(
            DateType date,
            List<ICommonTeamTrophyPoints> points,
            int teamSize,
            int score)
        {
            this.Date = date;
            this.TeamSize = teamSize;
            this.Points = points;
            this.VirtualAthletePoints = 0;
            this.Score = score;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="TeamTrophyEvent"/> class.
        /// </summary>
        /// <param name="date">date of the event</param>
        /// <param name="teamSize">the size of a Team Trophy team</param>
        public TeamTrophyEvent(
            DateType date,
            int teamSize)
        {
            this.Date = date;
            this.TeamSize = teamSize;
            this.Points = new List<ICommonTeamTrophyPoints>();
            this.VirtualAthletePoints = 0;
            this.Score = 0;
        }

        /// <summary>
        /// Gets the date of the event.
        /// </summary>
        public DateType Date { get; }

        /// <summary>
        /// Gets the total points scored in this event.
        /// </summary>
        /// <remarks>
        /// This is the total number of points scored by all the athletes (real and virtual) in 
        /// this event.
        /// </remarks>
        public int TotalAthletePoints 
        {
            get
            {
                int points = 0;

                foreach(ICommonTeamTrophyPoints point in this.Points)
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
        /// Gets or sets the score which is scored by this team for this event.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets a collection containing the break down of points.
        /// </summary>
        public List<ICommonTeamTrophyPoints> Points { get; }

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
        /// Gets the size of a Team Trophy team.
        /// </summary>
        public int TeamSize { get; }

        /// <summary>
        /// Add a new point to the <see cref="Points"/> collection.
        /// </summary>
        /// <remarks>
        /// The point is only added if there is space in the team.
        /// </remarks>
        /// <param name="newPoint"><see cref="ICommonTeamTrophyPoints"/> to add</param>
        /// <returns>success flag</returns>
        public bool AddPoint(ICommonTeamTrophyPoints newPoint)
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
                    ICommonTeamTrophyPoints virtualPoint =
                        new CommonTeamTrophyPoints(
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