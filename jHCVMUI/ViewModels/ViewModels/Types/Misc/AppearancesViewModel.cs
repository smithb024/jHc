namespace jHCVMUI.ViewModels.ViewModels.Types.Misc
{
  public class AppearancesViewModel : ViewModelBase
  {
    private string time;
    private string date;

    /// <summary>
    /// Initialises a new instance of the <see cref="AppearancesViewModel"/> class.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="date"></param>
    public AppearancesViewModel(
      string time,
      string date)
    {
      this.time = time;
      this.date = date;
    }

    /// <summary>
    /// Gets the appearance time
    /// </summary>
    public string Time => this.time;

    /// <summary>
    /// Gets the appearance date as a string.
    /// </summary>
    public string Date => this.date;
  }
}
