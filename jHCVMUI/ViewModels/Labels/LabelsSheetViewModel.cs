namespace jHCVMUI.ViewModels.Labels
{
  using System.Collections.ObjectModel;
  using jHCVMUI.ViewModels.ViewModels;

  /// <summary>
  /// Describes a sheet of labels to be saved to a png file.
  /// </summary>
  public class LabelsSheetViewModel : ViewModelBase
  {
    /// <summary>
    /// All the athlete labels.
    /// </summary>
    private ObservableCollection<AthleteLabel> athleteDetails;

    /// <summary>
    /// All the labels on the left hand column of the view.
    /// </summary>
    private ObservableCollection<AthleteLabel> leftColumn;

    /// <summary>
    /// All the labels on the right hand column of the view.
    /// </summary>
    private ObservableCollection<AthleteLabel> rightColumn;

    /// <summary>
    /// Height of the page.
    /// </summary>
    private int height;

    /// <summary>
    /// Width of the page
    /// </summary>
    private int width;

    /// <summary>
    /// Margin on the top and bottom of the page
    /// </summary>
    private int topMargin;

    /// <summary>
    /// Margin of each side of the page.
    /// </summary>
    private int sideMargin;

    /// <summary>
    /// Gets the margin around everything.
    /// </summary>
    private string labelMargin;

    /// <summary>
    /// Initialises a new instance of the <see cref="LabelsSheetViewModel"/> class.
    /// </summary>
    /// <param name="athleteDetails">athletes to show labels for</param>
    public LabelsSheetViewModel(ObservableCollection<AthleteLabel> athleteDetails)
    {
      // Check that the input is valid.
      if (athleteDetails == null || athleteDetails.Count == 0)
      {
        return;
      }

      bool isOdd = athleteDetails.Count % 2 != 0;
      int firstRightIndex =
        isOdd ?
        ((athleteDetails.Count - 1) / 2) + 1:
        athleteDetails.Count / 2;
      this.leftColumn = new ObservableCollection<AthleteLabel>();
      this.rightColumn = new ObservableCollection<AthleteLabel>();
      this.athleteDetails = athleteDetails;

      for (int index = 0; index < athleteDetails.Count; ++index)
      {
        if (index < firstRightIndex)
        {
          leftColumn.Add(athleteDetails[index]);
        }
        else
        {
          rightColumn.Add(athleteDetails[index]);
        }
      }

      this.height = A4Details.A4Height96DPI;
      this.width = A4Details.A4Width96DPI;
      this.topMargin = A4Details.LabelStandardTopMargin96DPI;
      this.sideMargin = A4Details.LabelStandardSideMargin96DPI;
      this.labelMargin = 
        string.Format(
          "{0},{1},{0},{1}",
          SideMargin.ToString(),
          TopMargin.ToString());
    }

    /// <summary>
    /// Gets the details of the athletes to draw labels for.
    /// </summary>
    public ObservableCollection<AthleteLabel> AthleteDetails => this.athleteDetails;

    /// <summary>
    /// Gets the details of the athletes to draw labels for. This is the list for the right hand
    /// column.
    /// </summary>
    public ObservableCollection<AthleteLabel> RightColumn => this.rightColumn;

    /// <summary>
    /// Gets the details of the athletes to draw labels for. This is the list for the right hand
    /// column.
    /// </summary>
    public ObservableCollection<AthleteLabel> LeftColumn => this.leftColumn;

    /// <summary>
    /// Gets the height of a A4 sheet assuming printed at 96 dpi.
    /// </summary>
    public int A4Height => this.height;

    /// <summary>
    /// Gets the width of a A4 sheet assuming printed at 96 dpi.
    /// </summary>
    public int A4Width => this.width;

    /// <summary>
    /// Gets the label margin
    /// </summary>
    public string LabelMargin => this.labelMargin;

    /// <summary>
    /// Gets the top and bottom margin used.
    /// </summary>
    public int TopMargin => this.topMargin;

    /// <summary>
    /// Gets the side margin used.
    /// </summary>
    public int SideMargin => this.sideMargin;
  }
}