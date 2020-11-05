namespace jHCVMUI.ViewModels.Config
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows.Input;
  using CommonHandicapLib;
  using CommonHandicapLib.Types;
  using CommonLib.Enumerations;
  using HandicapModel.Admin.Manage;
  using jHCVMUI.ViewModels;
  using jHCVMUI.ViewModels.Commands.Configuration;
  using jHCVMUI.ViewModels.ViewModels;

  public class ResultsConfigViewModel : ViewModelBase
  {
    private string finishingPoints          = string.Empty;
    private string finishingPointsOrig      = string.Empty;
    private string seasonBestPoints         = string.Empty;
    private string seasonBestPointsOrig     = string.Empty;
    private bool scoresAreDescending = true;
    private string scoresToCount         = string.Empty;
    private string scoresToCountOrig     = string.Empty;
    private bool allResults = false;
    private string scoringPositions         = string.Empty;
    private string scoringPositionsOrig     = string.Empty;
    private bool useTeams = false;
    private bool exludeFirstTimers = false;
    private string teamFinishingPoints      = string.Empty;
    private string teamFinishingPointsOrig  = string.Empty;
    private string teamSize                 = string.Empty;
    private string teamSizeOrig             = string.Empty;
    private string teamSeasonBestPoints     = string.Empty;
    private string teamSeasonBestPointsOrig = string.Empty;

    private IResultsConfigMngr resultsConfigurationManager;

    /// <summary>
    /// Initialises a new instance of the <see cref="ResultsConfigViewModel"/> class.
    /// </summary>
    public ResultsConfigViewModel(
      IResultsConfigMngr resultsConfigurationManager)
    {
      this.resultsConfigurationManager = resultsConfigurationManager;

      finishingPoints      = resultsConfigurationManager.ResultsConfigurationDetails.FinishingPoints.ToString();
      seasonBestPoints     = resultsConfigurationManager.ResultsConfigurationDetails.SeasonBestPoints.ToString();
      scoringPositions     = resultsConfigurationManager.ResultsConfigurationDetails.NumberOfScoringPositions.ToString();
      teamFinishingPoints  = resultsConfigurationManager.ResultsConfigurationDetails.TeamFinishingPoints.ToString();
      teamSize             = resultsConfigurationManager.ResultsConfigurationDetails.NumberInTeam.ToString();
      teamSeasonBestPoints = resultsConfigurationManager.ResultsConfigurationDetails.TeamSeasonBestPoints.ToString();
      this.scoresToCount = resultsConfigurationManager.ResultsConfigurationDetails.ScoresToCount.ToString();
      this.allResults = resultsConfigurationManager.ResultsConfigurationDetails.AllResults;
      this.useTeams = resultsConfigurationManager.ResultsConfigurationDetails.UseTeams;
      this.scoresAreDescending = resultsConfigurationManager.ResultsConfigurationDetails.ScoresAreDescending;
      this.exludeFirstTimers = resultsConfigurationManager.ResultsConfigurationDetails.ExcludeFirstTimers;

      finishingPointsOrig      = finishingPoints;
      seasonBestPointsOrig     = seasonBestPoints;
      scoringPositionsOrig     = scoringPositions;
      teamFinishingPointsOrig  = teamFinishingPoints;
      teamSizeOrig             = teamSize;
      teamSeasonBestPointsOrig = teamSeasonBestPoints;
      this.scoresToCountOrig = this.scoresToCount;

      SaveCommand   = new ResultsConfigSaveCmd(this);
    }

    public ICommand SaveCommand
    {
      get;
      private set ;
    }

    /// <summary>
    /// Gets or sets the number of points scored for finishing.
    /// </summary>
    public string FinishingPoints
    {
      get { return finishingPoints; }
      set
      {
        finishingPoints = value;
        RaisePropertyChangedEvent("FinishingPoints");
        RaisePropertyChangedEvent("FinishingPointsChanged");
      }
    }

    /// <summary>
    /// Gets the state of the finishing points property
    /// </summary>
    public FieldUpdatedType FinishingPointsChanged
    {
      get
      {
        if (!this.StringIsValidAsInt(this.FinishingPoints))
        {
          return FieldUpdatedType.Invalid;
        }
        else if (string.Compare(FinishingPoints, this.finishingPointsOrig) != 0)
        {
          return FieldUpdatedType.Changed;
        }
        else
        {
          return FieldUpdatedType.Unchanged;
        }
      }
    }

    /// <summary>
    /// Gets or sets the number of points scored for running a season best time.
    /// </summary>
    public string SeasonBestPoints
    {
      get { return seasonBestPoints; }
      set
      {
        seasonBestPoints = value;
        RaisePropertyChangedEvent("SeasonBestPoints");
        RaisePropertyChangedEvent("SeasonBestPointsChanged");
      }
    }

    /// <summary>
    /// Gets the state of the season best points property
    /// </summary>
    public FieldUpdatedType SeasonBestPointsChanged
    {
      get
      {
        if (!this.StringIsValidAsInt(this.SeasonBestPoints))
        {
          return FieldUpdatedType.Invalid;
        }
        else if (string.Compare(SeasonBestPoints, this.seasonBestPointsOrig) != 0)
        {
          return FieldUpdatedType.Changed;
        }
        else
        {
          return FieldUpdatedType.Unchanged;
        }
      }
    }

    /// <summary>
    /// Gets or sets the number of points scored for running a season best time.
    /// </summary>
    public string ScoresToCount
    {
      get { return this.scoresToCount; }
      set
      {
        this.scoresToCount = value;
        RaisePropertyChangedEvent(nameof(this.ScoresToCount));
        RaisePropertyChangedEvent(nameof(this.ScoresToCountChanged));
      }
    }

    /// <summary>
    /// Gets the state of the season best points property
    /// </summary>
    public FieldUpdatedType ScoresToCountChanged
    {
      get
      {
        if (!this.StringIsValidAsInt(this.ScoresToCount))
        {
          return FieldUpdatedType.Invalid;
        }
        else if (string.Compare(this.ScoresToCount, this.scoresToCountOrig) != 0)
        {
          return FieldUpdatedType.Changed;
        }
        else
        {
          return FieldUpdatedType.Unchanged;
        }
      }
    }

    /// <summary>
    /// Gets or sets the all scores in a season flag.
    /// </summary>
    public bool AllResults
    {
      get { return this.allResults; }
      set
      {
        this.allResults = value;
        this.RaisePropertyChangedEvent(nameof(this.AllResults));
      }
    }

    /// <summary>
    /// Gets or sets the number of positions which score points. 
    /// </summary>
    /// <remarks>
    /// First timers will not be scored.
    /// </remarks>
    public string NumberOfScoringPositions
    {
      get { return scoringPositions; }
      set
      {
        scoringPositions = value;
        RaisePropertyChangedEvent("NumberOfScoringPositions");
        RaisePropertyChangedEvent("NumberOfScoringPositionsChanged");
      }
    }

    /// <summary>
    /// Gets the state of the number of scoring positions points property
    /// </summary>
    public FieldUpdatedType NumberOfScoringPositionsChanged
    {
      get
      {
        if (!this.StringIsValidAsInt(this.NumberOfScoringPositions))
        {
          return FieldUpdatedType.Invalid;
        }
        else if (string.Compare(NumberOfScoringPositions, this.scoringPositionsOrig) != 0)
        {
          return FieldUpdatedType.Changed;
        }
        else
        {
          return FieldUpdatedType.Unchanged;
        }
      }
    }

    /// <summary>
    /// Gets or sets the number of people in a team, for purpose of scoring club points.
    /// </summary>
    public string NumberInTeam
    {
      get { return teamSize; }
      set
      {
        teamSize = value;
        RaisePropertyChangedEvent("NumberInTeam");
        RaisePropertyChangedEvent("TeamSizeChanged");
      }
    }

    /// <summary>
    /// Gets or sets the number of points scored for finishing (team points).
    /// </summary>
    public string TeamFinishingPoints
    {
      get { return teamFinishingPoints; }
      set
      {
        teamFinishingPoints = value;
        RaisePropertyChangedEvent("TeamFinishingPoints");
        RaisePropertyChangedEvent("TeamFinishingPointsChanged");
      }
    }

    /// <summary>
    /// Gets the state of the (team) finishing points property
    /// </summary>
    public FieldUpdatedType TeamFinishingPointsChanged
    {
      get
      {
        if (!this.StringIsValidAsInt(this.TeamFinishingPoints))
        {
          return FieldUpdatedType.Invalid;
        }
        else if (string.Compare(TeamFinishingPoints, this.teamFinishingPointsOrig) != 0)
        {
          return FieldUpdatedType.Changed;
        }
        else
        {
          return FieldUpdatedType.Unchanged;
        }
      }
    }

    /// <summary>
    /// Gets the state of the team size property
    /// </summary>
    public FieldUpdatedType TeamSizeChanged
    {
      get
      {
        if (!this.StringIsValidAsInt(this.NumberInTeam))
        {
          return FieldUpdatedType.Invalid;
        }
        else if (string.Compare(NumberInTeam, this.teamSizeOrig) != 0)
        {
          return FieldUpdatedType.Changed;
        }
        else
        {
          return FieldUpdatedType.Unchanged;
        }
      }
    }

    /// <summary>
    /// Gets or sets the number of points awarded to a team for running the season best time.
    /// </summary>
    public string TeamSeasonBestPoints
    {
      get { return teamSeasonBestPoints; }
      set
      {
        teamSeasonBestPoints = value;
        RaisePropertyChangedEvent("TeamSeasonBestPoints");
        RaisePropertyChangedEvent("TeamSeasonBestPointsChanged");
      }
    }

    /// <summary>
    /// Gets the state of the season best (team) points property
    /// </summary>
    public FieldUpdatedType TeamSeasonBestPointsChanged
    {
      get
      {
        if (!this.StringIsValidAsInt(this.TeamSeasonBestPoints))
        {
          return FieldUpdatedType.Invalid;
        }
        else if (string.Compare(TeamSeasonBestPoints, this.teamSeasonBestPointsOrig) != 0)
        {
          return FieldUpdatedType.Changed;
        }
        else
        {
          return FieldUpdatedType.Unchanged;
        }
      }
    }

    /// <summary>
    /// Gets or sets a value indicating wh
    /// </summary>
    public bool ScoresAreDescending
    {
      get { return this.scoresAreDescending; }
      set
      {
        this.scoresAreDescending = value;
        this.RaisePropertyChangedEvent(nameof(this.ScoresAreDescending));
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether first timers are excluded from scoring points for
    /// finishing position.
    /// </summary>
    public bool ExcludeFirstTimers
    {
      get { return this.exludeFirstTimers; }
      set
      {
        this.exludeFirstTimers = value;
        this.RaisePropertyChangedEvent(nameof(this.ExcludeFirstTimers));
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether teams are used in the series.
    /// </summary>
    public bool UseTeams
    {
      get { return this.useTeams; }
      set
      {
        this.useTeams = value;
        this.RaisePropertyChangedEvent(nameof(this.UseTeams));
      }
    }

    /// <summary>
    /// Indicates if all the entered values are valid.
    /// </summary>
    /// <returns>Entries valid</returns>
    public bool ValidEntries()
    {
      return this.StringIsValidAsInt(this.FinishingPoints) &&
        this.StringIsValidAsInt(this.SeasonBestPoints) &&
        this.StringIsValidAsInt(this.NumberOfScoringPositions) &&
        this.StringIsValidAsInt(this.NumberInTeam) &&
        this.StringIsValidAsInt(this.TeamSeasonBestPoints) &&
        this.StringIsValidAsInt(this.ScoresToCount);
    }

    /// <summary>
    /// Save the files.
    /// </summary>
    public void SaveConfig()
    {
      try
      {
        int saveFinishingPoints      = 0;
        int saveSeasonBestPoints     = 0;
        int saveScoringPostions      = 0;
        int saveTeamFinishingPoints  = 0;
        int saveTeamSize             = 0;
        int saveTeamSeasonBestPoints = 0;
        int saveScoresToCount = 0;

        if (!int.TryParse(FinishingPoints, out saveFinishingPoints))
        {
          Logger.GetInstance().WriteLog("Can't save results config, invalid finishing points");
          return;
        }

        if (!int.TryParse(SeasonBestPoints, out saveSeasonBestPoints))
        {
          Logger.GetInstance().WriteLog("Can't save results config, invalid season best points");
          return;
        }

        if (!int.TryParse(NumberOfScoringPositions, out saveScoringPostions))
        {
          Logger.GetInstance().WriteLog("Can't save results config, invalid scoring positions");
          return;
        }

        if (!int.TryParse(NumberInTeam, out saveTeamSize))
        {
          Logger.GetInstance().WriteLog("Can't save results config, invalid team size");
          return;
        }

        if (!int.TryParse(TeamFinishingPoints, out saveTeamFinishingPoints))
        {
          Logger.GetInstance().WriteLog("Can't save results config, invalid (team) finishing points");
          return;
        }

        if (!int.TryParse(TeamSeasonBestPoints, out saveTeamSeasonBestPoints))
        {
          Logger.GetInstance().WriteLog("Can't save results config, invalid season best (team) points");
          return;
        }

        if (!int.TryParse(this.ScoresToCount, out saveScoresToCount))
        {
          Logger.GetInstance().WriteLog("Can't save results config, invalid season scores to count points");
          return;
        }

        this.resultsConfigurationManager.SaveResultsConfiguration(
          saveFinishingPoints,
          saveSeasonBestPoints,
          saveScoringPostions,
          saveTeamFinishingPoints,
          saveTeamSize,
          saveTeamSeasonBestPoints,
          saveScoresToCount,
          this.AllResults,
          this.UseTeams,
          this.ScoresAreDescending,
          this.ExcludeFirstTimers);
      }
      catch (Exception ex)
      {
          Logger.GetInstance().WriteLog("Error saving results config: " + ex.ToString());
      }
    }

    /// <summary>
    /// Check that the string is an integer
    /// </summary>
    /// <param name="orig">origin string</param>
    /// <returns>valid flag</returns>
    private bool StringIsValidAsInt(string orig)
    {
        int testNum;
        return int.TryParse(SeasonBestPoints, out testNum);
    }
  }
}