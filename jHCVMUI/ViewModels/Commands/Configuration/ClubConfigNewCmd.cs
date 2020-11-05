namespace jHCVMUI.ViewModels.Commands.Configuration
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows.Input;
  using jHCVMUI.ViewModels;
  using jHCVMUI.ViewModels.ViewModels;

  public class ClubConfigNewCmd : ICommand
  {
    private ClubConfigurationViewModel m_viewModel = null;

    public ClubConfigNewCmd(ClubConfigurationViewModel viewModel)
    {
      m_viewModel = viewModel;
    }

    public event System.EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
      return m_viewModel.CanAdd();
    }

    public void Execute(object parameter)
    {
      m_viewModel.AddNewClub();
    }
  }
}
