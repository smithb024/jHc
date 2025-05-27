namespace jHCVMUI.ViewModels.ViewModels.Windows.Results
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using HandicapModel.AthletesModel;
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using HandicapModel.SeasonModel.EventModel;
    using jHCVMUI.ViewModels.ViewModels.Types.Athletes;
    using NynaeveLib.Commands;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// View model which supports the dialog which is used to manually enter results.
    /// </summary>
    public class EventRawResultsDlgViewModel : ViewModelBase
    {
        /// <summary>
        /// Associated handicap event model.
        /// </summary>
        private readonly IHandicapEvent handicapEventModel;

        /// <summary>
        /// 
        /// </summary>
        private List<RawResults> allAthletes = new List<RawResults>();

        /// <summary>
        /// A list of all athletes known to the model. They are used to create the unregistered
        /// athletes.
        /// </summary>
        private List<AthleteRegistrationViewModel> athleteList = new List<AthleteRegistrationViewModel>();

        private string raceNumberUsed = "";
        private int minutes = 0;
        private int seconds = 0;

        /// <summary>
        /// Indiciates if the athlete did not finish.
        /// </summary>
        private bool dnf = false;

        /// <summary>
        /// Indiciates if the athlete was taking part in a relay, so normal scoring rules do not 
        /// apply.
        /// </summary>
        private bool relay = false;

        /// <summary>
        /// Indicates whether to filter on active athletes.
        /// </summary>
        private bool isActive;

        /// <summary>
        /// The index of the currently selected unregistered athlete.
        /// </summary>
        private int unregisteredAthletesIndex;

        /// <summary>
        /// Initialises a new instance of the <see cref="EventRawResultsDlgViewModel"/> class.
        /// </summary>
        /// <param name="handicapEventModel">junior handicap model</param>
        /// <param name="athletesModel">athletes model</param>
        public EventRawResultsDlgViewModel(
            IHandicapEvent handicapEventModel,
            Athletes athletesModel)
        {
            this.athleteList = new List<AthleteRegistrationViewModel>();
            this.RegisteredAthletes = new ObservableCollection<RawResults>();
            this.handicapEventModel = handicapEventModel;
            this.isActive = true;

            // Get the list of athletes registered for the current season from the Business layer.
            // This doesn't include the raw results, so read this directly from a file and add
            // to the the list.
            this.LoadRegisteredInformation(athletesModel.AthleteDetails);
            this.LoadRawEventResults();

            this.AddNewResultCommand =
              new SimpleCommand(
                this.AddRawTimeCmd,
                this.AddRawTimeCmdAvailable);
            this.SaveCommand =
              new SimpleCommand(
                this.SaveRawResultsCmd);

            this.unregisteredAthletesIndex = -1;
        }

        /// <summary>
        /// Gets the list of unregistered athletes to display on the dialog.
        /// </summary>
        public List<AthleteRegistrationViewModel> UnregisteredAthletes =>
            new List<AthleteRegistrationViewModel>(
                this.athleteList.FindAll(
                    a => 
                    !a.IsRegisteredForCurrentEvent));

        /// <summary>
        /// Gets and sets the currently selected object in the athlete collection.
        /// </summary>
        /// <remarks>
        /// Setting the index causes the <see cref="RaceNumberUsed"/> to be set to the number of the
        /// chosen athlete. Maybe this should be done via a click event.
        /// </remarks>
        public int UnregisteredAthletesIndex
        {
            get => this.unregisteredAthletesIndex;

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
            get => this.raceNumberUsed; 
            set
            {
                if (this.raceNumberUsed == value)
                {
                    return;
                }

                this.raceNumberUsed = value;
                this.RaisePropertyChangedEvent(nameof(this.RaceNumberUsed));
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Gets and sets the minutes taken.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public int TotalMinutes
        {
            get => this.minutes;
            set
            {
                if (this.minutes == value)
                {
                    return;
                }

                this.minutes = value;
                this.RaisePropertyChangedEvent(nameof(this.TotalMinutes));
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Gets and sets the seconds taken.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public int TotalSeconds
        {
            get => this.seconds;
            set
            {
                if (this.seconds == value)
                {
                    return;
                }

                this.seconds = value;
                this.RaisePropertyChangedEvent(nameof(this.TotalSeconds));
            }
        }

        /// <summary>
        /// Gets and sets a flag indicating if the athlete finished.
        /// </summary>
        /// <remarks>
        /// If set, then <see cref="Relay"/> can't be
        /// </remarks>
        public bool DNF
        {
            get => this.dnf;
            set
            {
                if (this.dnf == value)
                {
                    return;
                }

                this.dnf = value;
                this.RaisePropertyChangedEvent(nameof(this.DNF));

                if (this.dnf)
                {
                    this.relay = false;
                    this.RaisePropertyChangedEvent(nameof(this.Relay));
                }

                this.RaisePropertyChangedEvent(nameof(this.TimeIsValid));
            }
        }

        /// <summary>
        /// Gets and sets a flag indicating if the athlete is in a relay.
        /// </summary>
        /// <remarks>
        /// If set, then <see cref="DNF"/> can't be
        /// </remarks>
        public bool Relay
        {
            get => this.relay;
            set
            {
                if (this.relay == value)
                {
                    return;
                }

                this.relay = value;
                this.RaisePropertyChangedEvent(nameof(this.Relay));

                if (this.relay)
                {
                    this.dnf = false;
                    this.RaisePropertyChangedEvent(nameof(this.DNF));
                }

                this.RaisePropertyChangedEvent(nameof(this.TimeIsValid));
            }
        }

        /// <summary>
        /// Gets and sets a flag indicating whether to only provide active athletes to the view.
        /// </summary>
        public bool IsActive
        {
            get => this.isActive;
            set
            {
                if (this.isActive == value)
                {
                    return;
                }

                this.isActive = value;
                this.RaisePropertyChangedEvent(nameof(this.IsActive));
            }
        }

        /// <summary>
        /// Gets a value indicating if the time is a valid input. 
        /// </summary>
        public bool TimeIsValid => !this.DNF || !this.Relay;

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>AthleteCollection</name>
        /// <date>02/03/15</date>
        /// <summary>
        /// Gets and sets the athlete collection
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public ObservableCollection<RawResults> RegisteredAthletes { get; }

        /// <summary>
        /// Gets a command which saves the raw results.
        /// </summary>
        public ICommand SaveCommand { get; private set; }

        /// <summary>
        /// Gets a command which adds a new result to the list of raw results.
        /// </summary>
        public ICommand AddNewResultCommand { get; private set; }

        /// <summary>
        /// Check to see if the race number is available for selection because it is in the unregistered
        /// athletes list.
        /// </summary>
        /// <returns>
        /// Flag indicating whether the add time command should be enabled.
        /// </returns>
        public bool AddRawTimeCmdAvailable()
        {
            foreach (AthleteRegistrationViewModel athlete in this.athleteList)
            {
                foreach (string number in athlete.AthleteNumbers)
                {
                    if (string.Equals(number, this.RaceNumberUsed))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Command received to register a new result from the GUI data.
        /// </summary>
        public void AddRawTimeCmd()
        {
            RaceTimeType raceTime = this.CalculateRaceTime();

            ObservableCollection <string> newNumber =
                new ObservableCollection<string>
                {
                    this.RaceNumberUsed 
                };

            AthleteRegistrationViewModel foundAthlete =
                this.athleteList.Find(
                    a => a.AthleteNumbers.Contains(this.RaceNumberUsed));

            if (foundAthlete == null)
            {
                return;
            }

            RawResults newResult =
                new RawResults(
                    foundAthlete.Key,
                    foundAthlete.Name,
                    newNumber)
                {
                    RaceTime = raceTime,
                    RaceNumber = this.RaceNumberUsed
                };

            // Determine the finish order if two or more athletes share the same finishing time.
            List<RawResults> filteredList = 
                this.FindAll(
                    raceTime);
            newResult.Order = filteredList.Count();

            this.RegisteredAthletes.Add(newResult);
            this.RegisterAthlete(foundAthlete.Key);
            this.ResetMemberData();

            this.RaisePropertyChangedEvent(nameof(this.UnregisteredAthletes));
            this.RaisePropertyChangedEvent(nameof(this.RegisteredAthletes));
            this.UnregisteredAthletesIndex = -1;
        }

        /// <summary>
        /// Save the raw results.
        /// </summary>
        public void SaveRawResultsCmd()
        {
            this.SaveRawEventResults();

            CommonMessenger.Default.Send(
                new HandicapProgressMessage(
                    "Raw results saved"));
        }

        /// <summary>
        /// Reset the fields which are used to enter new results.
        /// </summary>
        private void ResetMemberData()
        {
            this.RaceNumberUsed = string.Empty;
            this.TotalMinutes = 59;
            this.TotalSeconds = 59;
            this.DNF = false;
            this.Relay = false;
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
                    AthleteRegistrationViewModel newAthlete =
                        new AthleteRegistrationViewModel(
                            athlete.Key,
                            athlete.Name,
                            new ObservableCollection<string>(
                                athlete.RunningNumbers));
                    this.athleteList.Add(newAthlete);

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
        /// Load the raw event results into the registered athletes.
        /// </summary>
        /// <remarks>
        /// This is a set up method, so should only be called from the constructor.
        /// </remarks>
        private void LoadRawEventResults()
        {
            List<IRaw> rawResultsData = this.handicapEventModel.LoadRawResults();

            foreach (IRaw raw in rawResultsData)
            {
                AthleteRegistrationViewModel foundAthlete =
                    this.athleteList.Find(
                        a => a.AthleteNumbers.Contains(raw.RaceNumber));

                if (foundAthlete != null) 
                {
                    RawResults rawResult =
                        new RawResults(
                            foundAthlete.Key,
                            foundAthlete.Name,
                            foundAthlete.AthleteNumbers)
                        {
                            RaceTime = raw.TotalTime,
                            RaceNumber = raw.RaceNumber,
                            Order = raw.Order
                        };
                    this.RegisteredAthletes.Add(rawResult);
                    foundAthlete.SetRegistered(foundAthlete.Key);
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
        private void RegisterNewResult(
            string raceNumber,
            RaceTimeType raceTime)
        {
            RawResults result = this.FindAthlete(raceNumber);

            if (result != null)
            {
                result.RaceNumber = raceNumber;
                result.RaceTime = raceTime;

                // Determine the finish order if two or more athletes share the same finishing time.
                List<RawResults> filteredList = this.allAthletes.FindAll(athlete => athlete.RaceTime == result.RaceTime);
                result.Order = filteredList.Count();

                this.ResetMemberData();

                this.RaisePropertyChangedEvent(nameof(this.UnregisteredAthletes));
                this.RaisePropertyChangedEvent(nameof(this.RegisteredAthletes));
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
                List<RawResults> filteredList = this.allAthletes.FindAll(athlete => athlete.RaceTime == raceTime);
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
            //foreach (RawResults athlete in this.UnregisteredAthletes)
            //{
            //    foreach (string number in athlete.AthleteNumbers)
            //    {
            //        if (number == raceNumber)
            //        {
            //            return athlete;
            //        }
            //    }
            //}

            return null;
        }

        /// <summary>
        /// Go through all the athletes in <see cref="athleteList"/> and attempt to register them.
        /// As soon as the correct one is found, registration will be successful, so return from 
        /// the method.
        /// </summary>
        /// <param name="key">The key of the athlete to register</param>
        private void RegisterAthlete(int key)
        {
            foreach (AthleteRegistrationViewModel athlete in this.athleteList)
            {
                if (athlete.SetRegistered(key))
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Create a <see cref="RaceTimeType"/> object from the input values.
        /// </summary>
        /// <returns>A <see cref="RaceTimeType"/> object</returns>
        private RaceTimeType CalculateRaceTime()
        {
            RaceTimeType raceTimeType;

            if (this.DNF)
            {
                raceTimeType =
                    new RaceTimeType(
                        RaceTimeDescription.Dnf);
            }
            else if (this.Relay)
            {
                raceTimeType =
                    new RaceTimeType(
                        RaceTimeDescription.Relay);
            }
            else
            {
                raceTimeType =
                    new RaceTimeType(
                        this.TotalMinutes,
                        this.TotalSeconds);
            }

            return raceTimeType;
        }

        /// <summary>
        /// Determine all registered results with the same time. 
        /// </summary>
        /// <param name="raceTime">The time to search for</param>
        /// <returns>A list of all results.</returns>
        private List<RawResults> FindAll(RaceTimeType raceTime)
        {
            List<RawResults> filteredList = new List<RawResults>();

            foreach (RawResults rawResults in this.RegisteredAthletes)
            {
                if (rawResults.RaceTime == raceTime)
                {
                    filteredList.Add(rawResults);
                }
            }

            return filteredList;
        }
    }
}