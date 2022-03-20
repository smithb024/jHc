namespace CommonHandicapLib.Types
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class contains all the configuration information used to work out the results.
    /// </summary>
    public class ResultsConfigType
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ResultsConfigType"/> class.
        /// </summary>
        /// <param name="finishingPoints">points scored for finishing</param>
        /// <param name="seasonBestPoints">points scored for a season best</param>
        /// <param name="scoringPositions">number of scoring positions</param>
        /// <param name="teamFinishingPoints">team points scored for finishing</param>
        /// <param name="teamSize">size of a team</param>
        /// <param name="teamSeasonBestPoints">points scored by team for a season best</param>
        /// <param name="scoresToCount"></param>
        /// <param name="allResults"></param>
        /// <param name="useTeams"></param>
        /// <param name="scoresAreDescending"></param>
        /// <param name="excludeFirstTimers">
        /// First timers are excluded from scoring points for finishing position
        /// </param>
        /// <param name="teamTrophyTeamSize">
        /// The number of members of a Team Trophy team.
        /// </param>
        /// <param name="teamTrophyPointsScoring">
        /// A comma separated list detailing the points scored per position in the Team Trophy team
        /// </param>
        public ResultsConfigType(
          int finishingPoints,
          int seasonBestPoints,
          int scoringPositions,
          int teamFinishingPoints,
          int teamSize,
          int teamSeasonBestPoints,
          int scoresToCount,
          bool allResults,
          bool useTeams,
          bool scoresAreDescending,
          bool excludeFirstTimers,
          int teamTrophyTeamSize,
          string teamTrophyPointsScoring)
        {
            this.FinishingPoints = finishingPoints;
            this.SeasonBestPoints = seasonBestPoints;
            this.NumberOfScoringPositions = scoringPositions;
            this.TeamFinishingPoints = teamFinishingPoints;
            this.NumberInTeam = teamSize;
            this.TeamSeasonBestPoints = teamSeasonBestPoints;
            this.ScoresToCount = scoresToCount;
            this.AllResults = allResults;
            this.UseTeams = useTeams;
            this.ScoresAreDescending = scoresAreDescending;
            this.ExcludeFirstTimers = excludeFirstTimers;
            this.NumberInTeamTrophyTeam = teamTrophyTeamSize;
            this.TeamTrophyPointsScoring = teamTrophyPointsScoring;

            // Decode the Team Trophy points from the input values into a collection of integers
            this.TeamTrophyPoints = new List<int>();
            List<string> teamTrophyPointStrings =
                this.TeamTrophyPointsScoring.Split(',').ToList();

            foreach(string pointString in teamTrophyPointStrings)
            {
                int point;

                if(!int.TryParse(pointString, out point))
                {
                    this.TeamTrophyPoints = null;
                    break;
                }

                this.TeamTrophyPoints.Add(point);
            }
        }

        /// <summary>
        /// Gets or sets the number of points scored for finishing.
        /// </summary>
        public int FinishingPoints { get; set; }

        /// <summary>
        /// Gets or sets the number of team points scored for finishing.
        /// </summary>
        public int TeamFinishingPoints { get; set; }

        /// <summary>
        /// Gets or sets the number of points scored for running a season best time.
        /// </summary>
        public int SeasonBestPoints { get; set; }

        /// <summary>
        /// Gets or sets the number of positions which score points. 
        /// </summary>
        /// <remarks>
        /// First timers will not be scored.
        /// </remarks>
        public int NumberOfScoringPositions { get; set; }

        /// <summary>
        /// Gets or sets the number of people in a team, used in the Mob Trophy.
        /// </summary>
        public int NumberInTeam { get; set; }

        /// <summary>
        /// Gets or sets the number of points awarded to a team for running the season best time.
        /// </summary>
        public int TeamSeasonBestPoints { get; set; }

        /// <summary>
        /// Gets or sets the number of scores in a season to count towards the final points table.
        /// </summary>
        public int ScoresToCount { get; set; }

        /// <summary>
        /// Gets or sets a value which indicates whether all the results in a season counts towards
        /// the final points table.
        /// </summary>
        public bool AllResults { get; set; }

        /// <summary>
        /// Gets or sets a value which indicates whether teams are to be used in the competition.
        /// </summary>
        public bool UseTeams { get; set; }

        /// <summary>
        /// Gets or sets a value which indicates whether the athelete scores are decending. Ie
        /// 1st is high and decends to zero. Highest score at the end of the season wins.
        /// Ascending is 1pt for 1st, 2pts for 2nd. Lowest score at the end of the season wins.
        /// </summary>
        public bool ScoresAreDescending { get; set; }

        /// <summary>
        /// Gets or sets a value which indicates whether first timers should be excluded 
        /// from scoring position points.
        /// </summary>
        public bool ExcludeFirstTimers { get; set; }

        /// <summary>
        /// Gets the value used for missing scores when going for low scores.
        /// </summary>
        public int MissingScore => 1000;

        /// <summary>
        /// Gets or sets the number of members of a Team Trophy team.
        /// </summary>
        public int NumberInTeamTrophyTeam { get; set; }

        /// <summary>
        /// Gets or sets a comma separated list detailing the points scored per position in the
        /// Team Trophy team competition. 
        /// </summary>
        public string TeamTrophyPointsScoring { get; set; }

        /// <summary>
        /// Gets a collection of the Team Trophy club points.
        /// </summary>
        public List<int> TeamTrophyPoints { get; }
    }
}