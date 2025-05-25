namespace jHCVMUI.ViewModels.ViewModels.Windows.Results
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Input;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using HandicapModel.Admin.IO.RawResults;
    using HandicapModel.AthletesModel;
    using HandicapModel.Interfaces.Admin.IO.TXT;
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using HandicapModel.SeasonModel.EventModel;
    using jHCVMUI.ViewModels.ViewModels.Types.Athletes;
    using NynaeveLib.Commands;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// View model which supports the dialog which is used to enter results from input files.
    /// </summary>
    public class ImportEventRawResultDialogViewModel : ViewModelBase
    {
        /// <summary>
        /// Common IO manager
        /// </summary>
        private readonly ICommonIo commonIo;

        /// <summary>
        /// aplication logger
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Associate athlete model.
        /// </summary>
        private readonly Athletes athletesModel;

        /// <summary>
        /// Associated handicap event model.
        /// </summary>
        private readonly IHandicapEvent handicapEventModel;

        /// <summary>
        /// List of all athletes
        /// </summary>
        private List<RawResults> allAthletes = new List<RawResults>();

        /// <summary>
        /// Index used to select a input source from the <see cref="TimeInputSources"/> collection.
        /// </summary>
        private int timeInputSourceIndex = 0;

        /// <summary>
        /// Index used to select a input source from the <see cref="PositionInputSources"/> collection.
        /// </summary>
        private int positionInputSourceIndex = 0;

        private List<RawPositionResults> rawImportedPostions = new List<RawPositionResults>();
        private List<RaceTimeType> rawImportedTimes = new List<RaceTimeType>();
        private bool canSaveImported;
        private string importedState;

        /// <summary>
        /// View model which manages raw results view.
        /// </summary>
        /// <param name="handicapEventModel">junior handicap model</param>
        /// <param name="athletesModel">athletes model</param>
        /// <param name="commonIo">common IO manager</param>
        /// <param name="logger">application logger</param>
        public ImportEventRawResultDialogViewModel(
            IHandicapEvent handicapEventModel,
            Athletes athletesModel,
            ICommonIo commonIo,
            IJHcLogger logger)
        {
            this.handicapEventModel = handicapEventModel;
            this.athletesModel = athletesModel;
            this.commonIo = commonIo;
            this.logger = logger;

            // Get the list of athletes registered for the current season from the Business layer.
            // This doesn't include the raw results, so read this directly from a file and add
            // to the the list.
            this.LoadRegisteredInformation(athletesModel.AthleteDetails);

            this.canSaveImported = false;
            this.importedState = string.Empty;

            this.SaveImportedResultsCommand =
              new SimpleCommand(
                this.SaveImportResults,
                this.CanSaveImported);
            this.ImportPositionsCommand =
              new SimpleCommand(
                this.ImportPositionDataDialog);
            this.ImportTimesCommand =
              new SimpleCommand(
                this.ImportTimesDataDialog);
        }

        /// <summary>
        /// Gets or sets the raw position data.
        /// </summary>
        public List<RawPositionResults> RawImportedPositions
        {
            get => this.rawImportedPostions;
            set => this.rawImportedPostions = value;
        }

        /// <summary>
        /// Gets or sets the raw sets of times.
        /// </summary>
        public List<RaceTimeType> RawImportedTimes
        {
            get => this.rawImportedTimes;
            set => this.rawImportedTimes = value;
        }

        /// <summary>
        /// Note any problems with the import.
        /// </summary>
        public string ImportedState
        {
            get => this.importedState;

            set
            {
                if (this.importedState == value)
                {
                    return;
                }

                this.importedState = value;
                this.RaisePropertyChangedEvent(nameof(this.ImportedState));
            }
        }

        /// <summary>
        /// Gets all the possible time input sources as a collection of strings.
        /// </summary>
        public List<string> TimeInputSources => Enum.GetNames(typeof(TimeSources)).ToList();

        /// <summary>
        /// Gets or sets the current index of the currently selected time input source.
        /// </summary>
        public int TimeInputSourceIndex
        {
            get => this.timeInputSourceIndex;

            set
            {
                if (value < 0 || value > this.TimeInputSources.Count)
                {
                    this.timeInputSourceIndex = 0;
                }
                else
                {
                    this.timeInputSourceIndex = value;
                }

                this.RaisePropertyChangedEvent(nameof(this.TimeInputSourceIndex));
            }
        }

        /// <summary>
        /// Gets all the possible position input sources as a collection of strings.
        /// </summary>
        public List<string> PositionInputSources => Enum.GetNames(typeof(PositionSources)).ToList();

        /// <summary>
        /// Gets or sets the current index of the currently selected position input source.
        /// </summary>
        public int PositionInputSourceIndex
        {
            get => this.positionInputSourceIndex;

            set
            {
                if (value < 0 || value > this.PositionInputSources.Count)
                {
                    this.positionInputSourceIndex = 0;
                }
                else
                {
                    this.positionInputSourceIndex = value;
                }

                this.RaisePropertyChangedEvent(nameof(this.PositionInputSourceIndex));
            }
        }

        /// <summary>
        /// Gets a command which saves the results.
        /// </summary>
        public ICommand SaveImportedResultsCommand { get; private set; }

        /// <summary>
        /// Gets a command which starts the import times process.
        /// </summary>
        public ICommand ImportTimesCommand { get; private set; }

        /// <summary>
        /// Gets a command which starts the import positions process.
        /// </summary>
        public ICommand ImportPositionsCommand { get; private set; }

        /// <summary>
        /// Save the imported results.
        /// </summary>
        public void SaveImportResults()
        {
            try
            {
                this.LoadRegisteredInformation(this.athletesModel.AthleteDetails);

                for (int index = 0; index < this.RawImportedPositions.Count; ++index)
                {
                    // Should have already checked that these two arrays are the same size.
                    this.RegisterNewResult(
                        this.RawImportedPositions[index].RaceNumber, 
                        this.RawImportedTimes[index]);
                }
            }
            catch (Exception ex)
            {
                CommonMessenger.Default.Send(
                    new HandicapProgressMessage(
                        "Results import failed"));
                this.logger.WriteLog("Failed on import of results: " + ex.ToString());
            }

            bool saveSuccess = this.SaveRawEventResults();

            if (saveSuccess)
            {
                CommonMessenger.Default.Send(
                    new HandicapProgressMessage(
                        "Results import completed"));
            }
            else
            {
                CommonMessenger.Default.Send(
                    new HandicapProgressMessage(
                        "Results import failed"));
            }
        }

        /// <summary>
        /// Import positional data.
        /// </summary>
        public void ImportPositionsData(string fileName)
        {
            PositionSources currentPositionInputSource =
                (this.positionInputSourceIndex < 0 || this.positionInputSourceIndex > this.PositionInputSources.Count)
                ? PositionSources.OPN200
                : (PositionSources)this.positionInputSourceIndex;

            switch (currentPositionInputSource)
            {
                case PositionSources.OPN200:
                    this.RawImportedPositions =
                      ImportOpn200PositionFactory.Import(
                        fileName,
                        this.commonIo,
                        this.logger);
                    break;

                case PositionSources.Parkrun:
                    this.RawImportedPositions =
                      ImportParkrunPositionFactory.Import(
                        fileName,
                        this.commonIo,
                        this.logger);
                    break;

                default:
                    break;
            }

            this.RaisePropertyChangedEvent(nameof(this.RawImportedPositions));
            this.DetermineImportedState();
        }

        /// <summary>
        /// Import and analyse the contents of a raw times file.
        /// </summary>
        public void ImportTimesData(string fileName)
        {
            TimeSources currentTimeInputSource =
                this.timeInputSourceIndex < 0 || this.timeInputSourceIndex > this.TimeInputSources.Count
                ? TimeSources.Manual
                : (TimeSources)this.timeInputSourceIndex;

            switch (currentTimeInputSource)
            {
                case TimeSources.Manual:
                    this.RawImportedTimes =
                      ImportManualTimesFactory.Import(
                        fileName,
                        this.commonIo);
                    break;
                case TimeSources.OPN200:
                    this.RawImportedTimes =
                      ImportOpn200TimesFactory.Import(
                        fileName,
                        this.commonIo);
                    break;
                case TimeSources.Stopwatch610P:
                    this.RawImportedTimes =
                      Import610PFactory.Import(
                        fileName,
                        this.commonIo);
                    break;
                case TimeSources.Parkrun:
                    this.RawImportedTimes =
                        ImportParkrunTimerFactory.Import(
                            fileName,
                            this.commonIo);
                    break;
                default:
                    this.RawImportedTimes = new List<RaceTimeType>();
                    break;
            }

            this.DetermineImportedState();
            this.RaisePropertyChangedEvent(nameof(this.RawImportedTimes));
            this.RaisePropertyChangedEvent(nameof(this.ImportedState));
        }

        /// <summary>
        /// Load all athletes and sort by race number.
        /// </summary>
        /// <param name="athletes">registered athletes</param>
        private void LoadRegisteredInformation(List<AthleteDetails> athletes)
        {
            List<AthleteDetails> orderedList = athletes.OrderBy(athlete => athlete.PrimaryNumber).ToList();
            this.allAthletes = new List<RawResults>();

            foreach (AthleteDetails athlete in orderedList)
            {
                if (athlete.RunningNumbers != null &&
                  athlete.RunningNumbers.Count > 0)
                {
                    this.allAthletes.Add(
                      new RawResults(
                        athlete.Key,
                        athlete.Name,
                        new ObservableCollection<string>(
                          athlete.RunningNumbers)));
                }
            }
        }

        /// <summary>
        /// Apply the raw results to the model and save to disk.
        /// </summary>
        private bool SaveRawEventResults()
        {
            List<IRaw> rawList = new List<IRaw>();

            foreach (RawResults result in this.allAthletes.FindAll(athlete => athlete.RaceNumber != string.Empty))
            {
                rawList.Add(
                    new Raw(
                        result.RaceNumber,
                        result.RaceTime,
                        result.Order));
            }

            return this.handicapEventModel.SaveRawResults(rawList);
        }

        /// <summary>
        /// Ensure that the no numbers are included twice.
        /// </summary>
        /// <remarks>
        /// This would used when importing data.
        /// </remarks>
        /// <returns>string indicating any problems</returns>
        private bool TestForDuplicateNumbersAndPositions()
        {
            foreach (RawPositionResults raw in this.RawImportedPositions)
            {
                if (RawImportedPositions.FindAll(raceNumber => raceNumber.RaceNumber == raw.RaceNumber).Count > 1)
                {
                    this.ImportedState = string.Format("Error: Duplicate number {0}", raw.RaceNumber);
                    return true;
                }

                if (RawImportedPositions.FindAll(racePosition => racePosition.Position == raw.Position).Count > 1)
                {
                    this.ImportedState = string.Format("Error: Duplicate position {0}", raw.Position);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Ensure that all numbers have been registered.
        /// </summary>
        /// <remarks>
        /// This would used when importing data.
        /// </remarks>
        /// <returns>string indicating any problems</returns>
        private void TestForValidNumbers()
        {
            List<string> invalidValues = new List<string>();

            foreach (RawPositionResults raw in this.RawImportedPositions)
            {
                if (!this.RaceNumberPresent(this.allAthletes, raw.RaceNumber))
                {
                    invalidValues.Add($"{raw.RaceNumber} is not registered to an athlete");
                }
            }

            if (invalidValues.Count == 0)
            {
                this.ImportedState = string.Empty;
            }

            string outputString = string.Empty;
            for (int index = 0; index < invalidValues.Count; ++index)
            {
                if (index == 0)
                {
                    outputString = invalidValues[index];
                }
                else
                {
                    outputString = outputString + "\n" + invalidValues[index];
                }
            }

            this.ImportedState = outputString;
        }

        /// <summary>
        /// Check to see of the race number is currently allocated to an athlete.
        /// </summary>
        /// <param name="athletes">list of athletes to check</param>
        /// <param name="raceNumber">race number to check</param>
        /// <returns>flag indicating if the race number is allocated to an athlete</returns>
        private bool RaceNumberPresent(List<RawResults> athletes, string raceNumber)
        {
            foreach (RawResults athlete in athletes)
            {
                foreach (string number in athlete.AthleteNumbers)
                {
                    if (string.Equals(number, raceNumber))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void RegisterNewResult(string raceNumber, RaceTimeType raceTime)
        {
            RawResults result = this.FindAthlete(raceNumber);

            if (result != null)
            {
                result.RaceNumber = raceNumber;
                result.RaceTime = raceTime;

                // Determine the finish order if two or more athletes share the same finishing time.
                List<RawResults> filteredList = this
                    .allAthletes.FindAll(
                    athlete => athlete.RaceTime == result.RaceTime);
                result.Order = filteredList.Count();
            }
            else
            {
                // The athlete is unknown. Add the data to all athletes, all athletes is read 
                // when saving the raw results.
                ObservableCollection<string> newNumber = new ObservableCollection<string> { raceNumber };
                RawResults newResult = new RawResults(0, string.Empty, newNumber);
                newResult.RaceTime = raceTime;
                newResult.RaceNumber = raceNumber;

                // Determine the finish order if two or more athletes share the same finishing time.
                List<RawResults> filteredList = 
                    this.allAthletes.FindAll(
                        athlete => athlete.RaceTime == raceTime);
                newResult.Order = filteredList.Count();

                this.allAthletes.Add(newResult);
            }
        }

        /// <summary>
        /// Search throught the unregistered athlete to find the athlete with the race number
        /// </summary>
        /// <returns>requested athlete</returns>
        private RawResults FindAthlete(string raceNumber)
        {
            foreach (RawResults athlete in this.allAthletes)
            {
                foreach (string number in athlete.AthleteNumbers)
                {
                    if (number == raceNumber)
                    {
                        return athlete;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Ensure that valid data has been imported.
        /// </summary>
        private void DetermineImportedState()
        {
            this.canSaveImported = false;

            if (this.TestForImportedCounts())
            {
                return;
            }

            if (this.TestForDuplicateNumbersAndPositions())
            {
                return;
            }

            this.TestForValidNumbers();
            this.canSaveImported = true;
        }

        /// <summary>
        /// Tes the count numbers on the imported data. 
        /// </summary>
        /// <returns>true if there is a problem</returns>
        private bool TestForImportedCounts()
        {
            if (this.RawImportedPositions.Count == 0)
            {
                this.ImportedState = "Please import position data";
                return true;
            }
            else if (this.RawImportedTimes.Count == 0)
            {
                this.ImportedState = "Please import time data";
                return true;
            }
            else if (this.RawImportedPositions.Count < this.RawImportedTimes.Count)
            {
                this.ImportedState = "More times than finishers.";
                return true;
            }
            else if (this.RawImportedPositions.Count > this.RawImportedTimes.Count)
            {
                this.ImportedState = "More finishers than times.";
                return true;
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the save imported command can be run.
        /// </summary>
        /// <returns></returns>
        private bool CanSaveImported()
        {
            return this.canSaveImported;
        }

        /// <summary>
        /// Choose the file to import positions from, then call to import them.
        /// </summary>
        private void ImportPositionDataDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.ImportPositionsData(dialog.FileName);
            }
        }

        /// <summary>
        /// Choose the file to import times from, then call to import them.
        /// </summary>
        private void ImportTimesDataDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.ImportTimesData(dialog.FileName);
            }
        }
    }
}