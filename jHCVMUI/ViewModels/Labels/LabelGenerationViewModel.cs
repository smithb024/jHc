namespace jHCVMUI.ViewModels.Labels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Types;
    using CommonLib.Types;
    using HandicapModel.Admin.Manage;
    using HandicapModel.AthletesModel;
    using HandicapModel.Interfaces;
    using jHCVMUI.ViewModels.Commands.Label;
    using jHCVMUI.ViewModels.ViewModels;

    /// <summary>
    /// The view model which supports the label generation dialog.
    /// </summary>
    public class LabelGenerationViewModel : ViewModelBase
    {
        /// <summary>
        /// The series configuration manager.
        /// </summary>
        private ISeriesConfigMngr seriesConfigManager;

        /// <summary>
        /// The application logger.
        /// </summary>
        private IJHcLogger logger;

        /// <summary>
        /// Junior handicap model.
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
                        newHandicap = athlete.PredeclaredHandicap;
                    }

                    // Ensure that the athlete is registered for the season.
                    if (runningNumbers.Count > 0)
                    {
                        AthleteLabel modelAthlete =
                            new AthleteLabel(
                                athlete.Name,
                                athlete.Club,
                                runningNumbers[0],
                                newHandicap,
                                athlete.Appearances == 0);
                        modelAthlete.AthleteLabelWidth = A4Details.GetLabelWidth96DPI(NoColumns);
                        modelAthlete.AthleteLabelHeight = A4Details.GetLabelHeight96DPI(NoRows);
                        this.AthleteDetails.Add(modelAthlete);
                    }
                }
            }

            // Order the athletes alphabetically.
            this.AthleteDetails =
                this.AthleteDetails.OrderBy(athlete => athlete.Forename).ToList();
            this.AthleteDetails =
                this.AthleteDetails.OrderBy(athlete => athlete.Surname).ToList();

            this.saveDirectory = saveFolder;

            this.CreateRaceLabelsCommand = new CreateAndSaveRaceLabelsCmd(this);
            this.CreateSpareLabelsCommand = new CreateAndSaveSpareLabelsCmd(this);
            this.CreateAllLabelsCommand = new CreateAndSaveAllLabelsCmd(this);
        }

        /// <summary>
        /// Gets the create race labels command.
        /// </summary>
        public ICommand CreateRaceLabelsCommand { get; private set; }

        /// <summary>
        /// Gets the create spare labels command.
        /// </summary>
        public ICommand CreateSpareLabelsCommand { get; private set; }

        /// <summary>
        /// Gets the all labels command.
        /// </summary>
        public ICommand CreateAllLabelsCommand { get; private set; }

        /// <summary>
        /// Gets the details of all athletes.
        /// </summary>
        public List<AthleteLabel> AthleteDetails
        {
            get
            {
                return athleteDetails;
            }

            private set
            {
                athleteDetails = value;
            }
        }

        /// <summary>
        /// Gets or sets the sample name
        /// </summary>
        public string AthleteName
        {
            get
            {
                return this.sampleName;
            }

            set
            {
                if (this.sampleName != value)
                {
                    this.sampleName = value;
                    this.RaisePropertyChangedEvent(nameof(this.AthleteName));
                }
            }
        }

        /// <summary>
        /// Gets or sets the same team
        /// </summary>
        public string AthleteTeam
        {
            get
            {
                return this.sampleTeam;
            }

            set
            {
                this.sampleTeam = value;
                this.RaisePropertyChangedEvent(nameof(this.AthleteTeam));
            }
        }

        /// <summary>
        /// Gets or sets the sample number
        /// </summary>
        public int AthleteNumber
        {
            get
            {
                return this.sampleNumber;
            }

            set
            {
                this.sampleNumber = value;
                this.RaisePropertyChangedEvent(nameof(this.AthleteNumber));
            }
        }

        /// <summary>
        /// Gets or sets the sample handicap.
        /// </summary>
        public TimeType AthleteHandicap
        {
            get
            {
                return this.sampleHandicap;
            }

            set
            {
                this.sampleHandicap = value;
                this.RaisePropertyChangedEvent(nameof(this.AthleteHandicap));
            }
        }

        /// <summary>
        /// Gets or sets the sample handicap.
        /// </summary>
        public string SaveFolder
        {
            get
            {
                return this.saveDirectory;
            }

            set
            {
                this.saveDirectory = value;
                this.RaisePropertyChangedEvent(nameof(this.SaveFolder));
            }
        }

        /// <summary>
        /// Gets or sets the sample event details
        /// </summary>
        public string EventDetails
        {
            get
            {
                return this.sampleEvent;
            }

            set
            {
                this.sampleEvent = value;

                foreach (AthleteLabel label in this.AthleteDetails)
                {
                    label.EventDetails = this.sampleEvent;
                }

                this.RaisePropertyChangedEvent(nameof(this.EventDetails));
            }
        }

        /// <summary>
        /// Gets or sets the number of columns on the printed sheet.
        /// </summary>
        public int NoColumns
        {
            get
            {
                return this.numberColumns;
            }

            set
            {
                this.numberColumns = value;
                foreach (AthleteLabel label in this.AthleteDetails)
                {
                    label.AthleteLabelWidth = A4Details.GetLabelWidth96DPI(this.numberColumns);
                }

                this.RaisePropertyChangedEvent(nameof(this.NoColumns));
            }
        }

        /// <summary>
        /// Gets or sets the number of rows on the printed sheet.
        /// </summary>
        public int NoRows
        {
            get
            {
                return this.numberRows;
            }

            set
            {
                this.numberRows = value;

                foreach (AthleteLabel label in this.AthleteDetails)
                {
                    label.AthleteLabelHeight = A4Details.GetLabelHeight96DPI(this.numberRows);
                }

                this.RaisePropertyChangedEvent(nameof(this.NoRows));
            }
        }

        /// <summary>
        /// Gets or sets the number of spare sheets to create.
        /// </summary>
        public int NoSpareSheets
        {
            get
            {
                return this.numberSpareSheets;
            }

            set
            {
                this.numberSpareSheets = value;
                this.RaisePropertyChangedEvent(nameof(this.NoSpareSheets));
            }
        }

        /// <summary>
        /// Gets the flag to show off the first timer decorator.
        /// </summary>
        public bool FirstTimer => true;

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
        /// Generate images for all spare labels and their crib sheets.
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
    }
}