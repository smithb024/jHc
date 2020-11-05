namespace jHCVMUI.ViewModels.Labels
{
  using System;
  using System.Collections.Generic;
  using System.Drawing;
  using System.Linq;
  using System.Runtime.InteropServices;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows;
  using System.Windows.Media.Imaging;
  using BarcodeLib;
  using CommonLib.Types;
  using jHCVMUI.ViewModels.ViewModels;

  public class AthleteLabel : ViewModelBase
  {
    private string       athleteName    = string.Empty;
    private string       athleteTeam    = string.Empty;
    private string       athleteNumber  = string.Empty;
    private TimeType     handicap       = null;
    private string       eventDetails   = "Handicap - [Date]";
    private int          labelWidth     = 0;
    private int          labelHeight    = 0;
    private bool         firstTimer     = false;

    public AthleteLabel(string   name,
                        string   team,
                        string   number,
                        TimeType handicap,
                        bool     firstTimer)
    {
      AthleteName     = name;
      AthleteTeam     = team;
      AthleteNumber   = number;
      AthleteHandicap = handicap;
      FirstTimer      = firstTimer;

      RaisePropertyChangedEvent("AthleteLabelWidth");
    }

    /// <summary>
    /// Gets or sets the athlete's name.
    /// </summary>
    public string AthleteName
    {
      get { return athleteName; }
      private set
      {
        athleteName = value;
        RaisePropertyChangedEvent("AthleteName");
      }
    }

    /// <summary>
    /// Gets the athlete's forename
    /// </summary>
    public string Forename
    {
      get
      {
        return this.AthleteName.Contains(' ') ?
          AthleteName.Substring(0, AthleteName.LastIndexOf(' ')) :
          this.AthleteName;
      }
    }

    /// <summary>
    /// Gets the athlete's surname
    /// </summary>
    public string Surname
    {
      get
      {
        return this.AthleteName.Contains(' ') ?
        AthleteName.Substring(AthleteName.LastIndexOf(' ') + 1) :
        string.Empty;
      }
    }

    /// <summary>
    /// Gets or sets the athlete's team.
    /// </summary>
    public string AthleteTeam
    {
      get { return athleteTeam; }
      private set
      {
        athleteTeam = value;
        RaisePropertyChangedEvent("AthleteTeam");
      }
    }

    /// <summary>
    /// Gets or sets the athlete's number.
    /// </summary>
    public string AthleteNumber
    {
      get { return athleteNumber; }
      private set
      {
        athleteNumber = value;
        RaisePropertyChangedEvent("AthleteNumber");
      }
    }

    /// <summary>
    /// Gets or sets the athlete's handicap.
    /// </summary>
    public TimeType AthleteHandicap
    {
      get
      {
        return handicap;
      }
      private set
      {
        handicap = value;
        RaisePropertyChangedEvent("AthleteHandicap");
      }
    }

    /// <summary>
    /// Gets or sets a value which indicates whether the athlete is a first timer.
    /// </summary>
    public bool FirstTimer
    {
      get
      {
        return firstTimer;
      }
      private set
      {
        firstTimer = value;
        RaisePropertyChangedEvent("FirstTimer");
      }
    }

    /// <summary>
    /// Gets the event details.
    /// </summary>
    public string EventDetails
    {
      get {return eventDetails;}
      set
      {
        eventDetails = value;
        RaisePropertyChangedEvent("EventDetails");
      }
    }

    /// <summary>
    /// Gets the athlete labels width
    /// </summary>
    public int AthleteLabelWidth
    {
      get { return labelWidth; }
      set
      {
        labelWidth = value;
        RaisePropertyChangedEvent("AthleteLabelWidth");
      }
    }

    /// <summary>
    /// Gets the athlete labels width
    /// </summary>
    public int AthleteLabelHeight
    {
      get { return labelHeight; }
      set
      {
        labelHeight = value;
        RaisePropertyChangedEvent("AthleteLabelHeight");
      }
    }
  }
}