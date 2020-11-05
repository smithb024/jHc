namespace jHCVMUI.ViewModels.DataEntry
{
  using jHCVMUI.ViewModels.ViewModels;

  public class StopwatchP610EditorRawItem : ViewModelBase
  {
    /// <summary>
    /// Initialises a new instance of the <see cref="StopwatchP610EditorRawItem"/> class
    /// </summary>
    /// <param name="input"></param>
    public StopwatchP610EditorRawItem(string eventDetails)
    {
      this.Event = eventDetails;
    }

    /// <summary>
    /// Gets the event details information.
    /// </summary>
    public string Event { get; }
  }
}