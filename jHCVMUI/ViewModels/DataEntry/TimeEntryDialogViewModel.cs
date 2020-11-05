namespace jHCVMUI.ViewModels.DataEntry
{
  using System;
  using System.IO;
  using System.Windows.Forms;

  using CommonHandicapLib.Types;
  using NynaeveLib.Commands;

  public class TimeEntryDialogViewModel : DialogViewModelBase
  {
    private string timeEntryContents;

    private string faultString;

    private string pathOfCurrentFile;

    /// <summary>
    /// Initialises a new instance of the <see cref="TimeEntryDialogViewModel"/> class.
    /// </summary>
    public TimeEntryDialogViewModel()
    {
      this.timeEntryContents = string.Empty;
      this.faultString = string.Empty;
      this.pathOfCurrentFile = string.Empty;

      this.OpenCommand =
        new SimpleCommand(
          this.OpenFile,
          this.CanOpenFile);
      this.SaveCommand =
        new SimpleCommand(
          this.SaveFile,
          this.CanSaveFile);
      this.SaveAsCommand =
        new SimpleCommand(
          this.SaveAsFile,
          this.CanSaveAsFile);
    }

    /// <summary>
    /// Gets or sets the contents of the time entry file.
    /// </summary>
    public string TimeEntryContents
    {
      get
      {
        return this.timeEntryContents;
      }

      set
      {
        if (this.timeEntryContents != value)
        {
          this.timeEntryContents = value;
          this.RaisePropertyChangedEvent(nameof(this.TimeEntryContents));
        }
      }
    }

    /// <summary>
    /// Gets or sets the current fault string.
    /// </summary>
    public string FaultString
    {
      get
      {
        return this.faultString;
      }

      set
      {
        if (this.faultString != value)
        {
          this.faultString = value;
          this.RaisePropertyChangedEvent(nameof(this.FaultString));
        }
      }
    }

    /// <summary>
    /// Use a dialog to choose a file then open it.
    /// </summary>
    private void OpenFile()
    {
      OpenFileDialog dialog = new OpenFileDialog();

      if (dialog.ShowDialog() == DialogResult.OK)
      {
        this.pathOfCurrentFile = dialog.FileName;

        string readContents;
        using (StreamReader streamReader = new StreamReader(this.pathOfCurrentFile))
        {
          readContents = streamReader.ReadToEnd();
        }

        //List<string> rawResults = CommonIO.ReadFile(dialog.FileName);

        //if (rawResults == null ||
        //  rawResults.Count == 0)
        //{
        //  return;
        //}

        //foreach (string rawTime in rawResults)
        //{
        //}

        this.TimeEntryContents = readContents;
      }
    }

    /// <summary>
    /// Save the current file using the current path/file name
    /// </summary>
    private void SaveFile()
    {
      this.SaveFile(this.pathOfCurrentFile);
    }

    /// <summary>
    /// Save the current file.
    /// </summary>
    /// <param name="filename">name of the file to save to</param>
    private void SaveFile(string filename)
    {
      using (StreamWriter streamWriter = new StreamWriter(filename))
      {
        streamWriter.Write(this.TimeEntryContents);
      }
    }

    /// <summary>
    /// Indicates whether the <see cref="SaveCommand"/> is available.
    /// </summary>
    /// <returns>can save file flag</returns>
    protected override bool CanSaveFile()
    {
      return !string.IsNullOrEmpty(this.pathOfCurrentFile) && this.ContentsValid();
    }

    private void SaveAsFile()
    {
      string path =
        this.SaveAsTxtFile(
          this.SaveFile);

      if (!string.IsNullOrEmpty(path))
      {
        this.pathOfCurrentFile = path;
      }
    }

    /// <summary>
    /// Indicates whether the <see cref="SaveAsCommand"/> is available.
    /// </summary>
    /// <returns>can save as file flag</returns>
    protected override bool CanSaveAsFile()
    {
      return this.ContentsValid();
    }

    /// <summary>
    /// Determine whether each line returns a valid time. Note the fault if at least one line
    /// is rubbish.
    /// </summary>
    /// <remarks>
    /// This is constantly being evaluated, I think this is because I'm not using MVVM light
    /// to evaluate property changed events. 
    /// </remarks>
    /// <returns>contants valid flag</returns>
    private bool ContentsValid()
    {
      if (string.IsNullOrEmpty(this.TimeEntryContents))
      {
        return false;
      }

      string[] lines =
        this.TimeEntryContents.Split(
          new[] { Environment.NewLine },
          StringSplitOptions.None);

      foreach (string line in lines)
      {
        if (string.IsNullOrWhiteSpace(line))
        {
          this.FaultString = "Empty Value";
          return false;
        }

        RaceTimeType time = new RaceTimeType(line);
        if ((time.Minutes == 59 && time.Seconds == 59) || time.Seconds >= 60)
        {
          this.FaultString = "Invalid Data";
          return false;
        }
      }

      this.FaultString = string.Empty;
      return true;
    }
  }
}
