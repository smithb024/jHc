﻿namespace jHCVMUI.ViewModels.ViewModels.Windows.Results
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
        /// A string which is used to filter the <see cref="UnregisteredAthletes"/>.
        /// </summary>
        private string filterString;

        /// <summary>
        /// Indicates whether to filter on active athletes.
        /// </summary>
        private bool isActiveFilter;

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
            this.isActiveFilter = true;

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
                    !a.IsRegisteredForCurrentEvent &&
                    this.IncludeBasedOnActiveStatus(a.IsActive) &&
                    this.IncludeBasedOnFilterString(a.Name)));

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
        /// Gets and sets a string which is used to filter the <see cref="UnregisteredAthletes"/>.
        /// </summary>
        public string FilterString
        {
            get => this.filterString;
            set
            {
                if (this.filterString == value)
                {
                    return;
                }

                this.filterString = value;
                this.RaisePropertyChangedEvent(nameof(this.FilterString));
                this.UnregisteredAthletesIndex = -1;
                this.RaisePropertyChangedEvent(nameof(this.UnregisteredAthletes));
            }
        }

        /// <summary>
        /// Gets and sets a flag indicating whether to only provide active athletes to the view.
        /// </summary>
        public bool IsActive
        {
            get => this.isActiveFilter;
            set
            {
                if (this.isActiveFilter == value)
                {
                    return;
                }

                this.isActiveFilter = value;
                this.RaisePropertyChangedEvent(nameof(this.IsActive));
                this.UnregisteredAthletesIndex = -1;
                this.RaisePropertyChangedEvent(nameof(this.UnregisteredAthletes));
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
            newResult.Order = filteredList.Count() + 1;

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
                                athlete.RunningNumbers),
                            athlete.Active);
                    this.athleteList.Add(newAthlete);
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

            foreach (RawResults result in this.RegisteredAthletes)
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

        /// <summary>
        /// Determine whether to include an athlete in the <see cref="UnregisteredAthletes"/> 
        /// collection, based on the is active filter flag and the athlete's own active state.
        /// </summary>
        /// <param name="isActive">the athlete's active state.</param>
        /// <returns>Include flag</returns>
        private bool IncludeBasedOnFilterString(string name)
        {
            if (string.IsNullOrEmpty(this.FilterString))
            {
                return true;
            }

            return name.Contains(this.FilterString);
        }

        /// <summary>
        /// Determine whether to include an athlete in the <see cref="UnregisteredAthletes"/> 
        /// collection, based on the is active filter flag and the athlete's own active state.
        /// </summary>
        /// <param name="isActive">the athlete's active state.</param>
        /// <returns>Include flag</returns>
        private bool IncludeBasedOnActiveStatus(bool isActive)
        {
            if (!this.isActiveFilter)
            {
                return true;
            }

            return isActive;
        }
    }
}