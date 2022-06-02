namespace jHCVMUI.ViewModels.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Input;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Helpers.EventRawResults;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel.Admin.IO.RawResults;
    using HandicapModel.AthletesModel;
    using HandicapModel.Interfaces.Admin.IO.TXT;
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using HandicapModel.SeasonModel.EventModel;
    using jHCVMUI.ViewModels.ViewModels.Types.Athletes;
    using jHCVMUI.ViewModels.ViewModels.Types.Misc;

    using NynaeveLib.Commands;

    // Need to know all athletes who are registered for the current season.
    // Put together a list results:
    // Time
    // Race Number
    // Name
    // Club
    // Handicap
    // Order (for those with the same time).

    // Will want to tick off any registered athletes when they are given a time.
    // Prevent an athlete from getting two times.

    // Edit time and race number

    /// <summary>
    /// This view model manages inputting raw results.
    /// </summary>
    /// <remarks>
    /// This seems to be used for manually entering raw results and for importing results from a file,
    /// it may or may not be ideal solution. Be careful when changing, because of unintended 
    /// consequences on the other view. 
    /// Views are EventRawResultsDlg and ImportEventRawResultsDialog.
    /// </remarks>
    public class EventRawResultsViewModel : ViewModelBase
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
        /// Associate athlete model..
        /// </summary>
        private Athletes athletesModel;

        /// <summary>
        /// Associated handicap event model.
        /// </summary>
        private IHandicapEvent handicapEventModel;

        // TODO Originally a neat little view model which handled both data entry windows
        // It checked athletes to ensure that they were registered.
        // Have needed to allow it to handle unknown athletes, the adds ons to help it
        // do that give the class a hacky feel.
        private List<RawResults> allAthletes = new List<RawResults>();
        private string raceNumberUsed = "";
        private int minutes = 0;
        private int seconds = 0;
        private bool dnf = false;

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
        /// The index of the currently selected unregistered athlete.
        /// </summary>
        private int unregisteredAthletesIndex;

        /// <summary>
        /// View model which manages raw results view.
        /// </summary>
        /// <param name="handicapEventModel">junior handicap model</param>
        /// <param name="athletesModel">athletes model</param>
        /// <param name="commonIo">common IO manager</param>
        /// <param name="logger">application logger</param>
        public EventRawResultsViewModel(
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
            LoadRawEventResults();

            this.canSaveImported = false;
            this.importedState = string.Empty;

            AddNewResultCommand =
              new SimpleCommand(
                this.AddRawTimeCmd,
                this.AddRawTimeCmdAvailable);
            SaveImportedResultsCommand =
              new SimpleCommand(
                this.SaveImportResults,
                this.CanSaveImported);
            SaveCommand =
              new SimpleCommand(
                this.SaveRawResultsCmd,
                this.SaveRawResultsCmdAvailable);
            ImportPositionsCommand =
              new SimpleCommand(
                this.ImportPositionDataDialog);
            ImportTimesCommand =
              new SimpleCommand(
                this.ImportTimesDataDialog);

            this.unregisteredAthletesIndex = -1;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>AthleteCollection</name>
        /// <date>02/03/15</date>
        /// <summary>
        /// Gets and sets the athlete collection
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public List<RawResults> AllAthletes
        {
            get { return allAthletes; }
            set { allAthletes = value; }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>AthleteCollection</name>
        /// <date>02/03/15</date>
        /// <summary>
        /// Gets and sets the athlete collection
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public List<RawResults> UnregisteredAthletes
        {
            get
            {
                return new List<RawResults>(allAthletes.FindAll(RawResults => RawResults.RaceTime == null));
            }
        }

        /// <summary>
        /// Gets and sets the currently selected object in the athlete collection.
        /// </summary>
        /// <remarks>
        /// Setting the index causes the <see cref="RaceNumberUsed"/> to be set to the number of the
        /// chosen athlete. Maybe this should be done via a click event.
        /// </remarks>
        public int UnregisteredAthletesIndex
        {
            get
            {
                return this.unregisteredAthletesIndex;
            }

            set
            {
                this.unregisteredAthletesIndex = value;
                this.RaisePropertyChangedEvent(nameof(this.UnregisteredAthletesIndex));

                if (this.UnregisteredAthletes.Count > 0 && this.UnregisteredAthletesIndex >= 0)
                {
                    if (this.UnregisteredAthletes[this.unregisteredAthletesIndex].AthleteNumbers.Count > 0)
                    {
                        this.RaceNumberUsed = this.UnregisteredAthletes[this.unregisteredAthletesIndex].AthleteNumbers[0];
                    }
                    else
                    {
                        this.RaceNumberUsed = string.Empty;
                    }
                }
                else
                {
                    this.RaceNumberUsed = string.Empty;
                }
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Gets and sets race number used by an athlete.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string RaceNumberUsed
        {
            get { return this.raceNumberUsed; }
            set
            {
                this.raceNumberUsed = value;
                RaisePropertyChangedEvent("RaceNumberUsed");
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Gets and sets the minutes taken.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public int TotalMinutes
        {
            get { return minutes; }
            set
            {
                minutes = value;
                RaisePropertyChangedEvent("TotalMinutes");
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Gets and sets the seconds taken.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public int TotalSeconds
        {
            get { return seconds; }
            set
            {
                seconds = value;
                RaisePropertyChangedEvent("TotalSeconds");
            }
        }

        /// <summary>
        /// Gets and sets a flag indicating if the athlete finished.
        /// </summary>
        public bool DNF
        {
            get { return dnf; }
            set
            {
                dnf = value;
                RaisePropertyChangedEvent("DNF");
                RaisePropertyChangedEvent("TimeIsValid");
            }
        }

        /// <summary>
        /// Gets a value indicating if the time is a valid input. 
        /// </summary>
        public bool TimeIsValid => !DNF;

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>AthleteCollection</name>
        /// <date>02/03/15</date>
        /// <summary>
        /// Gets and sets the athlete collection
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public ObservableCollection<RawResults> RegisteredAthletes
        {
            get
            {
                return new ObservableCollection<RawResults>(allAthletes.FindAll(RawResults => RawResults.RaceTime != null));
            }
        }

        /// <summary>
        /// Gets or sets the raw position data.
        /// </summary>
        public List<RawPositionResults> RawImportedPostions
        {
            get
            {
                return rawImportedPostions;
            }

            set
            {
                rawImportedPostions = value;
            }
        }

        /// <summary>
        /// Gets or sets the raw sets of times.
        /// </summary>
        public List<RaceTimeType> RawImportedTimes
        {
            get
            {
                return rawImportedTimes;
            }

            set
            {
                rawImportedTimes = value;
            }
        }

        /// <summary>
        /// Note any problems with the import.
        /// </summary>
        public string ImportedState
        {
            get
            {
                return this.importedState;
            }

            set
            {
                this.importedState = value;
                this.RaisePropertyChangedEvent(nameof(this.ImportedState));
            }
        }

        /// <summary>
        /// Gets all the possible time input sources as a collection of strings.
        /// </summary>
        public List<string> TimeInputSources => Enum.GetNames(typeof(TimeSources)).ToList();

        /// <summary>
        /// Gets the currently selected time input source.
        /// </summary>
        public TimeSources CurrentTimeInputSource =>
                this.timeInputSourceIndex < 0 || this.timeInputSourceIndex > this.TimeInputSources.Count
                    ? TimeSources.Manual
                    : (TimeSources)this.timeInputSourceIndex;

        /// <summary>
        /// Gets or sets the current index of the currently selected time input source.
        /// </summary>
        public int TimeInputSourceIndex
        {
            get
            {
                return this.timeInputSourceIndex;
            }

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
        /// Gets the currently selected position input source.
        /// </summary>
        public PositionSources CurrentPositionInputSource =>
            (this.positionInputSourceIndex < 0 || this.positionInputSourceIndex > this.PositionInputSources.Count)
                ? PositionSources.OPN200
                :(PositionSources)this.positionInputSourceIndex;

        /// <summary>
        /// Gets or sets the current index of the currently selected position input source.
        /// </summary>
        public int PositionInputSourceIndex
        {
            get
            {
                return this.positionInputSourceIndex;
            }

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

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public ICommand SaveImportedResultsCommand
        {
            get;
            private set;
        }

        public ICommand AddNewResultCommand
        {
            get;
            private set;
        }

        public ICommand ImportTimesCommand
        {
            get;
            private set;
        }

        public ICommand ImportPositionsCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Check to see if the race number is available for selection because it is in the unregistered
        /// athletes list.
        /// </summary>
        /// <returns></returns>
        public bool AddRawTimeCmdAvailable()
        {
            return this.RaceNumberPresent(UnregisteredAthletes, RaceNumberUsed);
        }

        /// <summary>
        /// Command received to register a new result from the GUI data.
        /// </summary>
        public void AddRawTimeCmd()
        {
            RegisterNewResult(RaceNumberUsed,
                              DNF ? new RaceTimeType(DNF, false) : new RaceTimeType(TotalMinutes, TotalSeconds));

            this.UnregisteredAthletesIndex = -1;
        }

        public bool SaveRawResultsCmdAvailable()
        {
            return true;
        }

        /// <summary>
        /// Save the raw results.
        /// </summary>
        public void SaveRawResultsCmd()
        {
            SaveRawEventResults();

            Messenger.Default.Send(
                new HandicapProgressMessage(
                    "Raw results saved"));
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveImportResults()
        {
            try
            {
                LoadRegisteredInformation(this.athletesModel.AthleteDetails);

                for (int index = 0; index < RawImportedPostions.Count; ++index)
                {
                    // Should have already checked that these two arrays are the same size.
                    RegisterNewResult(RawImportedPostions[index].RaceNumber, RawImportedTimes[index]);
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(
                    new HandicapProgressMessage(
                        "Results inport failed"));
                this.logger.WriteLog("Failed on import of results: " + ex.ToString());
            }

            bool saveSuccess = SaveRawEventResults();

            RaisePropertyChangedEvent("UnregisteredAthletes");
            RaisePropertyChangedEvent("RegisteredAthletes");

            if (saveSuccess)
            {
                Messenger.Default.Send(
                    new HandicapProgressMessage(
                        "Results inport completed"));
            }
            else
            {
                Messenger.Default.Send(
                    new HandicapProgressMessage(
                        "Results inport failed"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ImportPositionsData(string fileName)
        {
            switch (this.CurrentPositionInputSource)
            {
                case PositionSources.OPN200:
                    this.RawImportedPostions =
                      ImportOpn200PositionFactory.Import(
                        fileName,
                        this.commonIo,
                        this.logger);
                    break;

                case PositionSources.Parkrun:
                    break;
                default:
                    break;
            }

            //        this.RawImportedPostions = new List<RawPositionResults>();
            //List<List<string>> rawPositions = this.commonIo.ReadPairedStringListFomFile(fileName);

            //foreach (List<string> positionAthleteData in rawPositions)
            //{
            //    // Ensure 2 results present
            //    if (positionAthleteData.Count == 2)
            //    {
            //        int? position = null;
            //        string raceNumber = null;

            //string result1 = ResultsDecoder.OpnScannerResultsBarcode(positionAthleteData[0]);
            //        string result2 = ResultsDecoder.OpnScannerResultsBarcode(positionAthleteData[1]);

            //        this.UpdatePositionAthleteData(result1, ref position, ref raceNumber);
            //        this.UpdatePositionAthleteData(result2, ref position, ref raceNumber);

            //        if (position != null && raceNumber != null)
            //        {
            //            RawImportedPostions.Add(new RawPositionResults(raceNumber, (int)position));
            //        }
            //        else
            //        {
            //            string errorString = string.Format("Can't decode {0}/{1}", positionAthleteData[0], positionAthleteData[1]);
            //Messenger.Default.Send(
            //    new HandicapProgressMessage(
            //        errorString));
            //this.logger.WriteLog(errorString);
            //        }
            //    }
            //    else
            //    {
            //        string errorString = "Please check results, result/barcode pair invalid";
            //        Messenger.Default.Send(
            //            new HandicapProgressMessage(
            //                errorString));
            //        this.logger.WriteLog(errorString);
            //    }
            //}

            //RawImportedPostions = RawImportedPostions.OrderBy(position => position.Position).ToList();
            this.RaisePropertyChangedEvent(nameof(this.RawImportedPostions));
            this.DetermineImportedState();
        }

        /// <summary>
        /// Import and analyse the contents of a raw times file.
        /// </summary>
        public void ImportTimesData(string fileName)
        {
            switch (this.CurrentTimeInputSource)
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
        /// Resets all the resetable member data.
        /// </summary>
        private void ResetMemberData()
        {
            RaceNumberUsed = string.Empty;
            TotalMinutes = 59;
            TotalSeconds = 59;
            DNF = false;
        }

        /// <summary>
        /// Load all athletes and sort by race number.
        /// </summary>
        /// <param name="athletes">registered athletes</param>
        private void LoadRegisteredInformation(List<AthleteDetails> athletes)
        {
            List<AthleteDetails> orderedList = athletes.OrderBy(athlete => athlete.PrimaryNumber).ToList();
            AllAthletes = new List<RawResults>();

            foreach (AthleteDetails athlete in orderedList)
            {
                if (athlete.RunningNumbers != null &&
                  athlete.RunningNumbers.Count > 0)
                {
                    AllAthletes.Add(
                      new RawResults(
                        athlete.Key,
                        athlete.Name,
                        new ObservableCollection<string>(
                          athlete.RunningNumbers)));
                }
            }
        }

        /// <summary>
        /// Load the raw event results into the registered athletes.
        /// </summary>
        private void LoadRawEventResults()
        {
            List<IRaw> rawResultsData = this.handicapEventModel.LoadRawResults();

            foreach (IRaw raw in rawResultsData)
            {
                bool found = false;

                foreach (RawResults results in UnregisteredAthletes)
                {
                    if (results.AthleteNumbers.Contains(raw.RaceNumber))
                    {
                        results.RaceNumber = raw.RaceNumber;
                        results.RaceTime = raw.TotalTime;
                        results.Order = raw.Order;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    ObservableCollection<string> newNumber = new ObservableCollection<string> { raw.RaceNumber };
                    RawResults newResult = new RawResults(0, string.Empty, newNumber);
                    newResult.RaceTime = raw.TotalTime;
                    newResult.RaceNumber = raw.RaceNumber;
                    newResult.Order = raw.Order;

                    this.AllAthletes.Add(newResult);
                }
            }
        }

        /// <summary>
        /// Apply the raw results to the model and save to disk.
        /// </summary>
        private bool SaveRawEventResults()
        {
            List<IRaw> rawList = new List<IRaw>();

            foreach (RawResults result in AllAthletes.FindAll(athlete => athlete.RaceNumber != string.Empty))
            {
                rawList.Add(new Raw(result.RaceNumber, result.RaceTime, result.Order));
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
            foreach (RawPositionResults raw in this.RawImportedPostions)
            {
                if (RawImportedPostions.FindAll(raceNumber => raceNumber.RaceNumber == raw.RaceNumber).Count > 1)
                {
                    this.ImportedState = string.Format("Error: Duplicate number {0}", raw.RaceNumber);
                    return true;
                }

                if (RawImportedPostions.FindAll(racePosition => racePosition.Position == raw.Position).Count > 1)
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

            foreach (RawPositionResults raw in this.RawImportedPostions)
            {
                if (!this.RaceNumberPresent(AllAthletes, raw.RaceNumber))
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
                List<RawResults> filteredList = AllAthletes.FindAll(athlete => athlete.RaceTime == result.RaceTime);
                result.Order = filteredList.Count();

                ResetMemberData();

                RaisePropertyChangedEvent("UnregisteredAthletes");
                RaisePropertyChangedEvent("RegisteredAthletes");
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
                List<RawResults> filteredList = AllAthletes.FindAll(athlete => athlete.RaceTime == raceTime);
                newResult.Order = filteredList.Count();

                this.AllAthletes.Add(newResult);
            }
        }

        /// <summary>
        /// Search throught the unregistered athlete to find the athlete with the race number
        /// </summary>
        /// <returns>requested athlete</returns>
        private RawResults FindAthlete(string raceNumber)
        {
            foreach (RawResults athlete in UnregisteredAthletes)
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

        ///// <summary>
        ///// Takes a result and populate the position or race number depending on what it is.
        ///// </summary>
        ///// <param name="result">position / athlete result</param>
        ///// <param name="position">position of result</param>
        ///// <param name="raceNumber">race number of result</param>
        //private void UpdatePositionAthleteData(string result,
        //                                       ref int? position,
        //                                       ref string raceNumber)
        //{
        //    if (ResultsDecoder.IsPositionValue(result))
        //    {
        //        position = ResultsDecoder.ConvertPositionValue(result);
        //    }
        //    else
        //    {
        //        raceNumber = result;
        //    }
        //}

        private void DetermineImportedState()
        {
            this.canSaveImported = false;

            if (this.TestForImportedCounts())
            {
                return;
            }

            if (TestForDuplicateNumbersAndPositions())
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
            if (RawImportedPostions.Count == 0)
            {
                this.ImportedState = "Please import position data";
                return true;
            }
            else if (RawImportedTimes.Count == 0)
            {
                this.ImportedState = "Please import time data";
                return true;
            }
            else if (RawImportedPostions.Count < RawImportedTimes.Count)
            {
                this.ImportedState = "More times than finishers.";
                return true;
            }
            else if (RawImportedPostions.Count > RawImportedTimes.Count)
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