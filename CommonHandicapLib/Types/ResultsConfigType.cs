namespace CommonHandicapLib.Types
{
  using System;

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
    /// <param name="excludeFirstTimers">first timers are excluded from scoring points for finishing position</param>
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
      bool excludeFirstTimers)
    {
      FinishingPoints = finishingPoints;
      SeasonBestPoints = seasonBestPoints;
      NumberOfScoringPositions = scoringPositions;
      TeamFinishingPoints = teamFinishingPoints;
      NumberInTeam = teamSize;
      TeamSeasonBestPoints = teamSeasonBestPoints;
      ScoresToCount = scoresToCount;
      AllResults = allResults;
      UseTeams = useTeams;
      this.ScoresAreDescending = scoresAreDescending;
      this.ExcludeFirstTimers = excludeFirstTimers;
    }

    /// <summary>
    /// Gets or sets the number of points scored for finishing.
    /// </summary>
    public int FinishingPoints
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the number of team points scored for finishing.
    /// </summary>
    public int TeamFinishingPoints
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the number of points scored for running a season best time.
    /// </summary>
    public int SeasonBestPoints
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the number of positions which score points. 
    /// </summary>
    /// <remarks>
    /// First timers will not be scored.
    /// </remarks>
    public int NumberOfScoringPositions
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the number of people in a team, for purpose of scoring club points.
    /// </summary>
    public int NumberInTeam
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the number of points awarded to a team for running the season best time.
    /// </summary>
    public int TeamSeasonBestPoints
    {
      get;
      set;
    }

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
  }
}