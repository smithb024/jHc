namespace jHCVMUI.ViewModels.ViewModels.Types.Athletes
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using CommonHandicapLib.Helpers;

  public class AthleteBase : ViewModelBase
  {
    private int    m_key            = -1;
    private string m_name           = string.Empty;

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>AthleteSummaryType</name>
    /// <date>03/05/15</date>
    /// <summary>
    ///   Creates a new instance of the AthleteListType class
    /// </summary>
    /// <param name="key">athlete key</param>
    /// <param name="name">handicap name</param>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public AthleteBase(int key,
                              string name)
    {
      Key            = key;
      Name           = name;
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>Key</name>
    /// <date>03/05/15</date>
    /// <summary>
    /// Gets and sets the key.
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public int Key
    {
      get { return m_key; }
      set
      {
        m_key = value;
        RaisePropertyChangedEvent("Key");
      }
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>Name</name>
    /// <date>03/05/15</date>
    /// <summary>
    /// Gets and sets the name.
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public string Name
    {
      get { return m_name; }
      set
      {
        m_name = value;
        RaisePropertyChangedEvent("Name");
        RaisePropertyChangedEvent("Forename");
        RaisePropertyChangedEvent("Surname");
      }
    }

    /// <summary>
    /// Gets the Surname of the athlete.
    /// </summary>
    public string Forename
    {
      get
      {
        return NameHelper.GetForename(Name);
      }
    }

    /// <summary>
    /// Gets the Surname of the athlete.
    /// </summary>
    public string Surname
    {
      get
      {
        return NameHelper.GetSurname(Name);
      }
    }
  }
}
