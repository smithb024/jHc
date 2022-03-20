namespace HandicapModel.Common
{
    using CommonLib.Types;
    using Interfaces.Common;

    /// <summary>
    /// Class used to store a point for an individual athlete in the Team Trophy.
    /// </summary>
    public class CommonTeamTrophyPoints : AthleteTeamTrophyPoints, ICommonTeamTrophyPoints
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="CommonTeamTrophyPoints"/> class.
        /// </summary>
        /// <param name="point">points scored</param>
        /// <param name="name">name of the points scorer</param>
        /// <param name="key">key of the points scorer</param>
        /// <param name="isReal">
        /// indicates whether the points scorer is real or artifically used to make up the numbers
        /// </param>
        public CommonTeamTrophyPoints(
            int point,
            string name,
            int key,
            bool isReal,
            DateType date)
            : base (point, date)
        {
            this.Name = name;
            this.AthleteKey = key;
            this.IsReal = isReal;
        }

        /// <summary>
        /// Gets a value indicated whether this is a real athlete, or just filler.
        /// </summary>
        public bool IsReal { get; }

        /// <summary>
        /// Gets the name of the athlete
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the athletes key
        /// </summary>
        public int AthleteKey { get; }
    }
}
