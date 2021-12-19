namespace jHCVMUI.ViewModels.Labels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using CommonHandicapLib;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Types;
    using CommonLib.Types;
    using HandicapModel.Admin.Manage;
    using HandicapModel.AthletesModel;
    using HandicapModel.Interfaces;
    using jHCVMUI.ViewModels.Commands.Label;
    using jHCVMUI.ViewModels.ViewModels;

    /// <summary>
    /// 
    /// </summary>
    public class LabelGenerationViewModel : ViewModelBase
    {
        private ISeriesConfigMngr seriesConfigManager;

        private IJHcLogger logger;

        /// <summary>
        /// Junior handicap model
        /// </summary>
        private IModel model;

        private List<AthleteLabel> athleteDetails = new List<AthleteLabel>();
        private string sampleName = "Sample Name";
        private string sampleTeam = "Sample Team";
        private int sampleNumber = 439;
        private TimeType sampleHandicap = new TimeType(88, 88);
        private string sampleEvent = "Handicap";

        private int numberColumns = 3;
        private int numberRows = 7;
        private int numberSpareSheets = 2;

        private string saveDirectory = string.Empty;

        /// <summary>
        /// Initialises a new instance of the <see cref="LabelGenerationViewModel"/> class.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="normalisationConfigManager">normalisation configuration manager</param>
        /// <param name="seriesConfigManager">series configuration manager</param>
        /// <param name="logger">application logger</param>
        /// <param name="saveFolder">folder to save the output to</param>
        public LabelGenerationViewModel(
            IModel model,
            INormalisationConfigMngr normalisationConfigManager,
            ISeriesConfigMngr seriesConfigManager,
            IJHcLogger logger,
            string saveFolder)
        {
            this.model = model;
            this.seriesConfigManager = seriesConfigManager;
            this.logger = logger;
            SaveFolder = saveFolder;
            NormalisationConfigType hcConfiguration = 
                normalisationConfigManager.ReadNormalisationConfiguration();

            // TODO, this is repeated code, see HandicapWriter.cs
            foreach (AthleteDetails athlete in model.Athletes.AthleteDetails)
            {
                TimeType newHandicap = model.CurrentSeason.GetAthleteHandicap(athlete.Key, hcConfiguration);

                // Only look at active athletes.
                if (athlete.Active)
                {
                    List<string> runningNumbers = model.Athletes.GetAthleteRunningNumbers(athlete.Key);

                    // Use default handicap, if the athlete is not registered for the current season.
                    // TODO, I suspect this should never happen???
                    if (newHandicap == null)
                    {
                        newHandicap = athlete.RoundedHandicap;
                    }

                    // Ensure that the athlete is registered for the season.
                    if (runningNumbers.Count > 0)
                    {
                        AthleteLabel modelAthlete = new AthleteLabel(athlete.Name,
                                                                     athlete.Club,
                                                                     runningNumbers[0],
                                                                     newHandicap,
                                                                     athlete.Appearances == 0);
                        modelAthlete.AthleteLabelWidth = A4Details.GetLabelWidth96DPI(NoColumns);
                        modelAthlete.AthleteLabelHeight = A4Details.GetLabelHeight96DPI(NoRows);
                        AthleteDetails.Add(modelAthlete);
                    }
                }
            }

            // Order the athletes alphabetically.
            AthleteDetails = AthleteDetails.OrderBy(athlete => athlete.Forename).ToList();
            AthleteDetails = AthleteDetails.OrderBy(athlete => athlete.Surname).ToList();

            this.saveDirectory = saveFolder;

            CreateRaceLabelsCommand = new CreateAndSaveRaceLabelsCmd(this);
            CreateSpareLabelsCommand = new CreateAndSaveSpareLabelsCmd(this);
            CreateAllLabelsCommand = new CreateAndSaveAllLabelsCmd(this);

            // Force the GUI to update.
            RaisePropertyChangedEvent("AthleteName");
            RaisePropertyChangedEvent("AthleteTeam");
            RaisePropertyChangedEvent("AthleteNumber");
            RaisePropertyChangedEvent("AthleteHandicap");
            RaisePropertyChangedEvent("EventDetails");
            RaisePropertyChangedEvent("NoColumns");
            RaisePropertyChangedEvent("NoRows");
        }

        public ICommand CreateRaceLabelsCommand { get; private set; }
        public ICommand CreateSpareLabelsCommand { get; private set; }
        public ICommand CreateAllLabelsCommand { get; private set; }

        /// <summary>
        /// Gets the details of all athletes.
        /// </summary>
        public List<AthleteLabel> AthleteDetails
        {
            get { return athleteDetails; }
            private set
            {
                athleteDetails = value;
            }
        }

        /// <summary>
        /// Gets the sample name
        /// </summary>
        public string AthleteName
        {
            get { return sampleName; }
            private set
            {
                sampleName = value;
                RaisePropertyChangedEvent("AthleteName");
            }
        }

        /// <summary>
        /// Gets the same team
        /// </summary>
        public string AthleteTeam
        {
            get { return sampleTeam; }
            private set
            {
                sampleTeam = value;
                RaisePropertyChangedEvent("AthleteTeam");
            }
        }

        /// <summary>
        /// Gets the sample number
        /// </summary>
        public int AthleteNumber
        {
            get { return sampleNumber; }
            private set
            {
                sampleNumber = value;
                RaisePropertyChangedEvent("AthleteNumber");
            }
        }

        /// <summary>
        /// Gets the sample handicap.
        /// </summary>
        public TimeType AthleteHandicap
        {
            get { return sampleHandicap; }
            private set
            {
                sampleHandicap = value;
                RaisePropertyChangedEvent("AthleteHandicap");
            }
        }

        /// <summary>
        /// Gets the sample handicap.
        /// </summary>
        public string SaveFolder
        {
            get { return saveDirectory; }
            private set
            {
                saveDirectory = value;
                RaisePropertyChangedEvent("SaveFolder");
            }
        }

        /// <summary>
        /// Gets the sample event details
        /// </summary>
        public string EventDetails
        {
            get { return sampleEvent; }
            private set
            {
                sampleEvent = value;

                foreach (AthleteLabel label in AthleteDetails)
                {
                    label.EventDetails = sampleEvent;
                }

                RaisePropertyChangedEvent("EventDetails");
            }
        }

        /// <summary>
        /// Gets the number of columns on the printed sheet.
        /// </summary>
        public int NoColumns
        {
            get { return numberColumns; }
            set
            {
                numberColumns = value;

                foreach (AthleteLabel label in AthleteDetails)
                {
                    label.AthleteLabelWidth = A4Details.GetLabelWidth96DPI(numberColumns);
                }

                RaisePropertyChangedEvent("NoColumns");
            }
        }

        /// <summary>
        /// Gets the number of rows on the printed sheet.
        /// </summary>
        public int NoRows
        {
            get { return numberRows; }
            set
            {
                numberRows = value;

                foreach (AthleteLabel label in AthleteDetails)
                {
                    label.AthleteLabelHeight = A4Details.GetLabelHeight96DPI(numberRows);
                }

                RaisePropertyChangedEvent("NoRows");
            }
        }

        /// <summary>
        /// Gets the number of spare sheets to create.
        /// </summary>
        public int NoSpareSheets
        {
            get { return numberSpareSheets; }
            set
            {
                numberSpareSheets = value;
                RaisePropertyChangedEvent("NoSpareSheets");
            }
        }

        /// <summary>
        /// Create images for all race and spare labels and their crib sheets.
        /// </summary>
        public void CreateAllLabels()
        {
            CreateSpareLabels();
            CreateRaceLabels();
        }

        /// <summary>
        /// Create images for all race labels and their crib sheets.
        /// </summary>
        public void CreateRaceLabels()
        {
            LabelImageGenerator.CreateRaceLabels(AthleteDetails, SaveFolder, NoColumns, NoRows);
            LabelImageGenerator.CreateRaceLabelsCribSheet(AthleteDetails, SaveFolder);

            this.logger.WriteLog("Race labels created");
        }

        /// <summary>
        /// Cenerate images for all spare labels and their crib sheets.
        /// </summary>
        public void CreateSpareLabels()
        {
            LabelImageGenerator.CreateSpareLabels(
                this.model,
                this.seriesConfigManager,
                SaveFolder,
                NoSpareSheets,
                NoColumns, 
                NoRows, 
                EventDetails);
            LabelImageGenerator.CreateSpareLabelsCribSheet(
                this.model,
                this.seriesConfigManager,
                SaveFolder, 
                NoSpareSheets, 
                NoColumns,
                NoRows);

            this.logger.WriteLog("Race labels created");
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public void CreateRaceLabels()
        //{
        //  ObservableCollection<AthleteLabel> labels        = new ObservableCollection<AthleteLabel>();
        //  int                                labelsOnSheet = 0;
        //  int                                sheetNumber   = 1;

        //  // loop through all athletedetails.
        //  // Every (columns * rows) call create sheet.

        //  foreach (AthleteLabel athlete in AthleteDetails)
        //  {
        //    ++labelsOnSheet;

        //    labels.Add(athlete);

        //    if (labelsOnSheet == numberColumns * numberRows)
        //    {
        //      CreateSingleSheet(labels, sheetNumber.ToString() + "Racetest.png");

        //      ++sheetNumber;
        //      labelsOnSheet = 0;
        //      labels = new ObservableCollection<AthleteLabel>();
        //    }
        //  }

        //  if (labelsOnSheet > 0)
        //  {
        //    CreateSingleSheet(labels, sheetNumber.ToString() + "Racetest.png");
        //  }

        //  // Create the summary sheets TODO, this is ugly
        //  labels = new ObservableCollection<AthleteLabel>();
        //  labelsOnSheet = 0;

        //  foreach (AthleteLabel athlete in AthleteDetails)
        //  {
        //    ++labelsOnSheet;

        //    labels.Add(athlete);

        //    if (labelsOnSheet == 50)
        //    {
        //      CreateSingleSummarySheet(labels, sheetNumber.ToString() + "Summarytest.png");

        //      ++sheetNumber;
        //      labelsOnSheet = 0;
        //      labels = new ObservableCollection<AthleteLabel>();
        //    }
        //  }

        //  if (labelsOnSheet > 0)
        //  { 
        //      CreateSingleSummarySheet(labels, sheetNumber.ToString() + "Summarytest.png");
        //  }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public void CreateSpareLabels()
        //{
        //  int number = Model.Instance.CurrentSeason.NextAvailableRaceNumber;

        //  for (int sheetNumber = 0; sheetNumber < NoSpareSheets; ++sheetNumber)
        //  {
        //    ObservableCollection<AthleteLabel> sheet = new ObservableCollection<AthleteLabel>();

        //    for (int labelIndex = 0; labelIndex < numberColumns * numberRows; ++labelIndex)
        //    {
        //      AthleteLabel newLabel = new AthleteLabel(string.Empty, string.Empty, number, null);
        //      newLabel.EventDetails = EventDetails;
        //      newLabel.AthleteLabelWidth = A4Details.GetLabelWidth96DPI(numberColumns);
        //      newLabel.AthleteLabelHeight = A4Details.GetLabelHeight96DPI(numberRows);

        //      sheet.Add(newLabel);

        //      ++number;
        //    }

        //    CreateSingleSheet(sheet, sheetNumber.ToString() + "test.png");
        //  }

        //  // Crib sheet TODO, all this is ugly.
        //  number = Model.Instance.CurrentSeason.NextAvailableRaceNumber;
        //  ObservableCollection<AthleteLabel> labels = new ObservableCollection<AthleteLabel>();
        //  int summarySheetNumber = 1;

        //  for (int labelIndex = 0; labelIndex < numberColumns * numberRows * NoSpareSheets; ++labelIndex)
        //  {
        //    labels.Add(new AthleteLabel("________________________________________________________________________", string.Empty, number, null));

        //    if (labels.Count == 50)
        //    {
        //      CreateSingleSummarySheet(labels, summarySheetNumber.ToString() + "SummaryCribtest.png");

        //      ++summarySheetNumber;
        //      labels = new ObservableCollection<AthleteLabel>();
        //    }

        //    ++number;
        //  }

        //  if (labels.Count > 0)
        //  {
        //      CreateSingleSummarySheet(labels, summarySheetNumber.ToString() + "SummaryCribtest.png");
        //  }
        //}

        ///// <summary>
        ///// Creates a single sheet of labels.
        ///// </summary>
        ///// <param name="labels">list of label images</param>
        ///// <param name="imageName">file name</param>
        //public void CreateSingleSheet(ObservableCollection<AthleteLabel> labels, string imageName)
        //{
        //  LabelSheetDialog     labelDialog    = new LabelSheetDialog();
        //  LabelsSheetViewModel labelViewModel = new LabelsSheetViewModel(labels);

        //  labelDialog.DataContext = labelViewModel;

        //  labelDialog.Show();

        //  labelDialog.SaveMyWindow(96, SaveFolder + Path.DirectorySeparatorChar + imageName);

        //  labelDialog.Close();
        //}

        ///// <summary>
        ///// Creates a single label summary sheet.
        ///// </summary>
        ///// <param name="labels">list of label images</param>
        ///// <param name="imageName">file name</param>
        //public void CreateSingleSummarySheet(ObservableCollection<AthleteLabel> labels, string imageName)
        //{
        //  SummarySheetDialog   summaryDialog  = new SummarySheetDialog();
        //  LabelsSheetViewModel labelViewModel = new LabelsSheetViewModel(labels);

        //  summaryDialog.DataContext = labelViewModel;

        //  summaryDialog.Show();

        //  summaryDialog.SaveMyWindow(96, SaveFolder + Path.DirectorySeparatorChar + imageName);

        //  summaryDialog.Close();
        //}
    }
}