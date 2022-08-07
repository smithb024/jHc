namespace jHCVMUI.ViewModels.ViewModels
{
  using System.ComponentModel;

  public class ViewModelBase : INotifyPropertyChanged
  {
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>ViewModelBase</name>
    /// <date>23/02/15</date>
    /// <summary>
    ///   Creates a new instance of the ViewModelBase class
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public ViewModelBase()
    {
    }

    /// <summary>
    /// property changed event handler
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Raise an event to indicate that a property has changed.
    /// </summary>
    /// <param name="propertyName">property name</param>
    public void RaisePropertyChangedEvent(string propertyName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
