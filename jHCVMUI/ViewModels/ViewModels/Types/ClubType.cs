namespace jHCVMUI.ViewModels.ViewModels.Types
{
  using System.ComponentModel;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using CommonHandicapLib.Types;
  using jHCVMUI.ViewModels.ViewModels;

  public class ClubType : ViewModelBase
  {
    private string     m_club   = string.Empty;
    private StatusType m_status = StatusType.Ok;

    public ClubType()
    {
    }

    public string Club
    {
      get { return m_club; }
      set
      {
        m_club = value;
        RaisePropertyChangedEvent("Club");
      }
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>Status</name>
    /// <date>10/03/15</date>
    /// <summary>
    /// Gets and sets the status.
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public StatusType Status
    {
      get { return m_status; }
      set
      {
        m_status = value;
        RaisePropertyChangedEvent("Status");
      }
    }
  }
}
