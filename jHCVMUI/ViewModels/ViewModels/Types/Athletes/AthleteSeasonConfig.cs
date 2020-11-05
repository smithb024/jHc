namespace jHCVMUI.ViewModels.ViewModels.Types.Athletes
{
  // TODO This class is being made redundant because the register to season idea
  // is going. Registration will be automatic.
  public class AthleteSeasonConfig : AthleteSeasonBase
  {
    private bool                      m_new            = false;
    private bool                      m_updated        = false;

    /// <summary>
    /// Initialises a new instance of the <see cref="AthleteSeasonConfig"/> class.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="name"></param>
    public AthleteSeasonConfig(
      int key,
      string name)
      : base (
          key,
          name,
          new System.Collections.ObjectModel.ObservableCollection<string>())
    {
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>IsNew</name>
    /// <date>04/05/15</date>
    /// <summary>
    /// Gets and sets the new flag.
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public bool IsNew
    {
      get { return m_new; }
      set
      {
        m_new = value;
        RaisePropertyChangedEvent("IsNew");
      }
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>IsUpdated</name>
    /// <date>04/05/15</date>
    /// <summary>
    /// Gets and sets the updated flag.
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public bool IsUpdated
    {
      get { return m_updated; }
      set
      {
        m_updated = value;
        RaisePropertyChangedEvent("IsUpdated");
      }
    }
  }
}
