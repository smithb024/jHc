namespace HandicapModel.SeasonModel.Helpers
{
  public class PointsControl
  {
    private bool scoreHigh;
    private bool allScores;
    private int maximumScores;

    /// <summary>
    /// Initialises a new instance of the <see cref="PointsControl"/> class
    /// </summary>
    /// <param name="scoreHigh">Indicates whether high or low scores take precedent</param>
    /// <param name="allScores">Indicates whether all scores should be counted</param>
    /// <param name="maximumScores">Indicates the maximum number of scores in a season (not used if <paramref name="allScores"/> is true</param>
    public PointsControl(
      bool scoreHigh = true,
      bool allScores = true,
      int maximumScores = 12)
    {
      this.scoreHigh = scoreHigh;
      this.maximumScores = maximumScores;
    }

    /// <summary>
    /// Gets a value which indicates whether high or low scores take precedence.
    /// </summary>
    public bool ScoreHigh => this.scoreHigh;

    /// <summary>
    /// Gets a value which indicates whether high or low scores take precedence.
    /// </summary>
    public bool AllScore => this.allScores;

    /// <summary>
    /// Gets the maximum number of scores available in the season.
    /// </summary>
    public int MaximumScores => this.maximumScores;

    /// <summary>
    /// Gets the score to be used in the event that less than the maximum number of events have been
    /// scored. It allows those who have run all events to always beat those which have only run
    /// some of the events.
    /// </summary>
    public int MissingScore => 1000;
  }
}