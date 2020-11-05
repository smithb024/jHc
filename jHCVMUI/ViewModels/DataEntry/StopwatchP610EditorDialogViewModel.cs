using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jHCVMUI.ViewModels.DataEntry
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.IO;
  using System.Windows.Forms;

  using HandicapModel.Admin.IO.TXT;

  using NynaeveLib.Commands;

  public class StopwatchP610EditorDialogViewModel : DialogViewModelBase
  {
    /// <summary>
    /// Indicates the index of the currently selected event.
    /// </summary>
    private int eventsIndex;

    /// <summary>
    /// Initialises a new instance of the <see cref="StopwatchP610EditorDialogViewModel"/> class.
    /// </summary>
    public StopwatchP610EditorDialogViewModel()
    {
      this.eventsIndex= -1;

      this.OpenCommand =
        new SimpleCommand(
          this.OpenFile,
          this.CanOpenFile);
      this.SaveAsCommand =
        new SimpleCommand(
          this.SaveAsFile,
          this.CanSaveAsFile);
    }

    /// <summary>
    /// Gets the barcode data which is to be manipulated by this editor view model.
    /// </summary>
    public ObservableCollection<StopwatchP610EditorRawItem> Events { get; private set; }

    /// <summary>
    /// Gets or sets the index of the selected barcode.
    /// </summary>
    public int EventsIndex
    {
      get
      {
        return this.eventsIndex;
      }
      set
      {
        this.eventsIndex = value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OpenFile()
    {
      this.Events = new ObservableCollection<StopwatchP610EditorRawItem>();
      OpenFileDialog dialog = new OpenFileDialog();

      if (dialog.ShowDialog() == DialogResult.OK)
      {
        string readContents;
        using (StreamReader streamReader = new StreamReader(dialog.FileName))
        {
          readContents = streamReader.ReadToEnd();
        }

        string[] splitContents = readContents.Split(new string[] { "------------------------------------------------------------------------" }, StringSplitOptions.None);

        foreach (string content in splitContents)
        {
          if (!string.IsNullOrWhiteSpace(content))
          {
            this.Events.Add(new StopwatchP610EditorRawItem(content));
          }
        }
      }

      this.RaisePropertyChangedEvents();
    }

    /// <summary>
    /// Save the contents of the dialog to a file.
    /// </summary>
    private void SaveAsFile()
    {
      this.SaveAsTxtFile(this.SaveFile);
    }

    private new bool CanSaveAsFile()
    {
      return this.IndexIsValid();
    }

    /// <summary>
    /// Save the current file.
    /// </summary>
    /// <param name="filename">name of the file to save to</param>
    private void SaveFile(string filename)
    {
      using (StreamWriter streamWriter = new StreamWriter(filename))
      {
        streamWriter.WriteLine(this.Events[this.EventsIndex].Event);
      }
    }

    private void RaisePropertyChangedEvents()
    {
      this.RaisePropertyChangedEvent(nameof(this.Events));
    }

    private bool IndexIsValid()
    {
      return
        this.Events != null &&
        this.Events.Count > 0 &&
        this.EventsIndex >= 0 && 
        this.EventsIndex < this.Events.Count;
    }
  }
}