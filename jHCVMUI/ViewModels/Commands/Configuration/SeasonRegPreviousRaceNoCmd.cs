namespace jHCVMUI.ViewModels.Commands.Configuration
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Windows.Input;
  using jHCVMUI.ViewModels.ViewModels;

  public class SeasonRegPreviousRaceNoCmd : ICommand
  {
    private AthleteRegisterToSeasonViewModel m_viewModel = null;

    /// <summary>
    /// Creates a new instance of the SeasonRegPreviousRaceNoCmd
    /// </summary>
    /// <param name="viewModel">view model</param>
    public SeasonRegPreviousRaceNoCmd(AthleteRegisterToSeasonViewModel viewModel)
    {
      m_viewModel = viewModel;
    }

    public bool CanExecute(object parameter)
    {
      return m_viewModel.IsPreviousNumberAvailable();
    }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object parameter)
    {
      m_viewModel.PreviousRaceNumber();
    }
  }
}
