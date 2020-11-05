namespace jHCVMUI.ViewModels.Commands.Label
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows.Input;
  using jHCVMUI.ViewModels.Labels;

  public class CreateAndSaveAllLabelsCmd : ICommand
  {
    private LabelGenerationViewModel viewModel = null;

    public CreateAndSaveAllLabelsCmd(LabelGenerationViewModel viewModel)
    {
      this.viewModel = viewModel;
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }


    public void Execute(object parameter)
    {
      viewModel.CreateAllLabels();
    }
  }
}