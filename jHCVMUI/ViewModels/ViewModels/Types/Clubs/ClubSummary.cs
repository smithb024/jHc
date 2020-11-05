namespace jHCVMUI.ViewModels.ViewModels.Types.Clubs
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  /// <summary>
  /// Used to present club summary information to the user.
  /// </summary>
  public class ClubSummary : ViewModelBase
  {
    private string name             = string.Empty;
    private int    numberRegistered = 0;

    /// <summary>
    /// Initialises a new instance of the <see cref="ClubSummary"/> class.
    /// </summary>
    /// <param name="clubName">club name</param>
    /// <param name="numberRegistered">number of registered athletes</param>
    public ClubSummary(string clubName,
                       int    numberRegistered)
    {
      Name             = clubName;
      NumberRegistered = numberRegistered;
    }

    /// <summary>
    /// Gets or sets the name of the current club.
    /// </summary>
    public string Name
    {
      get
      {
        return name;
      }

      private set
      {
        name = value;
        RaisePropertyChangedEvent("Name");
      }
    }

    /// <summary>
    /// Gets or sets the number of athletes registed to the club.
    /// </summary>
    public int NumberRegistered
    {
      get
      {
        return numberRegistered;
      }

      private set
      {
        numberRegistered = value;
        RaisePropertyChangedEvent("NumberRegistered");
      }
    }
  }
}