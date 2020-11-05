namespace jHCVMUI.ViewModels.Commands.Main
{
  using System;
  using System.IO;
  using System.Windows.Input;

  /// <summary>
  /// Command class, it runs the executable which is set up in the contructor.
  /// </summary>
  public class RunApplicationCmd : ICommand
  {
    private string path;
    private bool canExecute;

    /// <summary>
    /// Initialises a new instance of the <see cref="RunApplicationCmd"/> class.
    /// </summary>
    /// <param name="path">path of the executable to run</param>
    /// <param name="canExecute">executable can be executed</param>
    public RunApplicationCmd(
      string path,
      bool canExecute)
    {
      this.path = path;
      this.canExecute = canExecute;
    }

    /// <summary>
    /// Identifies whether the executable can be executed.
    /// </summary>
    /// <param name="parameter">not used</param>
    /// <returns>boolean flag</returns>
    public bool CanExecute(object parameter)
    {
      return canExecute;
    }

    /// <summary>
    /// Can execute changed event handler, this is not currently used because the can execute 
    /// flag is set in the contructor and not updated.
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    /// <summary>
    /// Execute the executable.
    /// </summary>
    /// <param name="parameter">not used</param>
    public void Execute(object parameter)
    {
      try
      {
        string[] files = Directory.GetFiles(".//Apps//", "*");
        System.Diagnostics.Process.Start(Path.GetFullPath(this.path));
      }
      catch (Exception ex)
      {
      }
    }
  }
}
