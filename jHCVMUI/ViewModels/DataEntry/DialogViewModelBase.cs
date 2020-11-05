using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jHCVMUI.ViewModels.DataEntry
{
  using System.IO;
  using System.Windows.Forms;
  using System.Windows.Input;

  using CommonHandicapLib.Types;
  using HandicapModel.Admin.IO.TXT;
  using jHCVMUI.ViewModels.ViewModels;
  using NynaeveLib.Commands;

  public class DialogViewModelBase : ViewModelBase
  {
    /// <summary>
    /// Initialises a new instance of the <see cref="DialogViewModelBase"/> class.
    /// </summary>
    public DialogViewModelBase()
    {
    }

    /// <summary>
    /// Command used to open a new file.
    /// </summary>
    public ICommand OpenCommand { get; protected set; }

    /// <summary>
    /// Command used to save the time data entry values to a known file.
    /// </summary>
    public ICommand SaveCommand { get; protected set; }

    /// <summary>
    /// Command used to save the time data entry values to a user specified file.
    /// </summary>
    public ICommand SaveAsCommand { get; protected set; }

    /// <summary>
    /// Virtual method which indicates when the <see cref="OpenCommand"/> can be run. In the base
    /// class it is always true.
    /// </summary>
    /// <returns>can open flag</returns>
    protected virtual bool CanOpenFile()
    {
      return true;
    }

    /// <summary>
    /// Virtual method which indicates when the <see cref="SaveCommand"/> can be run. In the base
    /// class it is always true.
    /// </summary>
    /// <returns>can save flag</returns>
    protected virtual bool CanSaveFile()
    {
      return true;
    }

    /// <summary>
    /// Virtual method which indicates when the <see cref="SaveAsCommand"/> can be run. In the base
    /// class it is always true.
    /// </summary>
    /// <returns>can save as flag</returns>
    protected virtual bool CanSaveAsFile()
    {
      return true;
    }

    /// <summary>
    /// Use the <see cref="SaveFileDialog"/> to to obtain a filename. Use the filename to invoke
    /// <paramref name="saveCommand"/> and return the filename.
    /// </summary>
    /// <param name="saveCommand">command to invoke with the file name</param>
    /// <returns>obtained file name</returns>
    protected string SaveAsTxtFile(Action<string> saveCommand)
    {
      SaveFileDialog dialog = new SaveFileDialog();
      dialog.Filter = "Normal text file (*.txt)|*.txt";
      dialog.DefaultExt = "txt";
      dialog.AddExtension = true;

      if (dialog.ShowDialog() == DialogResult.OK)
      {
        saveCommand.Invoke(dialog.FileName);
        return dialog.FileName;
      }

      return string.Empty;
    }
  }
}