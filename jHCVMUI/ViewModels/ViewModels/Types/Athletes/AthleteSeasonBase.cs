namespace jHCVMUI.ViewModels.ViewModels.Types.Athletes
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using System.Text;

  public class AthleteSeasonBase : AthleteBase
  {
    private string                    m_handicap       = string.Empty;
    private bool                      m_firstTimer     = true;
    private ObservableCollection<string> runningNumbers;

    public AthleteSeasonBase(
      int key,
      string name,
      ObservableCollection<string> runningNumbers) 
      : base (
          key,
          name)
    {
      this.runningNumbers = runningNumbers;
    }

    /// <summary>
    /// Gets and sets the race number.
    /// </summary>
    /// <remarks>
    /// This is the number used by the athlete in the race.
    /// </remarks>
    public ObservableCollection<string> AthleteNumbers
    {
      get { return runningNumbers; }
      set
      {
        runningNumbers = value;
        RaisePropertyChangedEvent("AthleteNumbers");
      }
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>Handicap</name>
    /// <date>04/05/15</date>
    /// <summary>
    /// Gets and sets the handicap.
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public string Handicap
    {
      get { return m_handicap; }
      set
      {
        m_handicap = value;
        RaisePropertyChangedEvent("Handicap");
      }
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>FirstTimer</name>
    /// <date>04/05/15</date>
    /// <summary>
    /// Gets and sets the first timer flag.
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public bool FirstTimer
    {
      get { return m_firstTimer; }
      set
      {
        m_firstTimer = value;
        RaisePropertyChangedEvent("FirstTimer");
      }
    }
  }
}
