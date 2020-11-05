namespace jHCVMUI.ViewModels.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;
    using CommonHandicapLib.Messages;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel;
    using HandicapModel.AthletesModel;
    using HandicapModel.Interfaces;
    using HandicapModel.SeasonModel;
    using jHCVMUI.ViewModels.Commands.Configuration;
    using jHCVMUI.ViewModels.ViewModels.Types.Athletes;

    // TODO This class is going obsolete as the concept of registering an athlete to a season is
    // going. It will be done automatically instead.
    public class AthleteRegisterToSeasonViewModel : ViewModelBase
    {
        // Need a list of all athletes in the system (minus those registered to this season)
        // Need a list of all athletes registered to this season. - They will have a list of race numbers for this season.
        // Need a number to assign to new registrations. It must be unused and can be initially calculated as the next on after the highest existing number.

        //Operations
        //Operation to register (highlight unrestered person - operation - now register with next number, number increased by one.
        //Next available number.
        //Previous available number (could be unavailable).
        //Assign number to already registerd runner.
        //Save

        // all athletes in the model and registered athletes in the handicap season model.
        // Read them both in and only add all athletes if not in the season model.

        private ObservableCollection<Types.Athletes.AthleteSimpleViewModel> m_unregisteredAthletes = new ObservableCollection<Types.Athletes.AthleteSimpleViewModel>();
        private ObservableCollection<AthleteSeasonConfig> m_registeredAthletes = new ObservableCollection<AthleteSeasonConfig>();
        private string newRaceNumber = "";

        private int m_registeredSelectedIndex = -1;
        private int m_unregisteredSelectedIndex = -1;

        private IModel model;

        public AthleteRegisterToSeasonViewModel(
            IModel model)
        {
            LoadAthleteInformation(model.Athletes.AthleteDetails);
            LoadRegisteredInformation(model.CurrentSeason.Athletes);

            SaveCommand = new SeasonRegSaveCmd(this);
            RegisterNewCommand = new SeasonRegRegisterNewCmd(this);
            UpdateNumbersCommand = new SeasonRegUpdateNumbersCmd(this);
            NextRaceNumberCommand = new SeasonRegNextRaceNoCmd(this);
            PreviousRaceNumberCommand = new SeasonRegPreviousRaceNoCmd(this);
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>AthleteCollection</name>
        /// <date>02/03/15</date>
        /// <summary>
        /// Gets and sets the athlete collection
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public ObservableCollection<Types.Athletes.AthleteSimpleViewModel> UnregisteredAthletes
        {
            get { return m_unregisteredAthletes; }
            set
            {
                m_unregisteredAthletes = value;
                RaisePropertyChangedEvent("UnregisteredAthletes");
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>AthleteCollection</name>
        /// <date>02/03/15</date>
        /// <summary>
        /// Gets and sets the athlete collection
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public ObservableCollection<AthleteSeasonConfig> RegisteredAthletes
        {
            get { return m_registeredAthletes; }
            set
            {
                m_registeredAthletes = value;
                RaisePropertyChangedEvent("RegisteredAthletes");
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>AthleteCollection</name>
        /// <date>02/03/15</date>
        /// <summary>
        /// Gets and sets the athlete collection
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string NewRaceNumber
        {
            get { return this.newRaceNumber; }
            set
            {
                this.newRaceNumber = value;
                RaisePropertyChangedEvent("NewRaceNumber");
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>RegisteredSelectedAthleteIndex</name>
        /// <date>04/05/15</date>
        /// <summary>
        /// Gets and sets the index of the registered athlete
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public int RegisteredSelectedAthleteIndex
        {
            get { return m_registeredSelectedIndex; }
            set
            {
                m_registeredSelectedIndex = value;
                RaisePropertyChangedEvent("RegisteredSelectedAthleteIndex");
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>UnregisteredSelectedAthleteIndex</name>
        /// <date>04/05/15</date>
        /// <summary>
        /// Gets and sets the index of the unregistered athlete
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public int UnregisteredSelectedAthleteIndex
        {
            get { return m_unregisteredSelectedIndex; }
            set
            {
                m_unregisteredSelectedIndex = value;
                RaisePropertyChangedEvent("UnregisteredSelectedAthleteIndex");
            }
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public ICommand RegisterNewCommand
        {
            get;
            private set;
        }

        public ICommand UpdateNumbersCommand
        {
            get;
            private set;
        }

        public ICommand NextRaceNumberCommand
        {
            get;
            private set;
        }

        public ICommand PreviousRaceNumberCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// To be made obsolete.
        /// </returns>
        public bool IsPreviousNumberAvailable()
        {
            //int searchNumber = NewRaceNumber - 1;
            //while (RaceNumberExists(searchNumber) && searchNumber > 0)
            //{
            //  --searchNumber;
            //}

            //return searchNumber > 0;
            return true;
        }

        public bool CanRegisterAthlete()
        {
            return UnregisteredSelectedAthleteIndex >= 0;
        }

        public bool CanAddRaceNumber()
        {
            return RegisteredSelectedAthleteIndex >= 0;
        }

        /// <summary>
        /// To be made obsolete
        /// </summary>
        public void NextRaceNumber()
        {
            this.NewRaceNumber = "N/A";
            //int searchNumber = NewRaceNumber + 1;
            //while (RaceNumberExists(searchNumber))
            //{
            //  ++searchNumber;
            //}

            //NewRaceNumber = searchNumber;
        }

        /// <summary>
        /// To be made obsolete.
        /// </summary>
        public void PreviousRaceNumber()
        {
            this.NewRaceNumber = "N/A";
            //int searchNumber = NewRaceNumber - 1;
            //while (RaceNumberExists(searchNumber))
            //{
            //  --searchNumber;
            //}

            //NewRaceNumber = searchNumber;
        }

        /// <summary>
        /// Adds a new athlete to the registered list.
        /// </summary>
        /// <remarks>
        /// Creates a new athlete by taking data from the selected unregistered athlete. First timer is worked out 
        /// by checking the number of runs.
        /// The new athlete is added to the list of registered list of athletes.
        /// The selected unregister athlete is removed as it is now registered.
        /// The unregistered list has all athletes deselected.
        /// Thenew available race number is chosen.
        /// </remarks>
        public void RegisterAthleteForSeason()
        {
            AthleteSeasonConfig newAthlete =
              new AthleteSeasonConfig(
                UnregisteredAthletes[UnregisteredSelectedAthleteIndex].Key,
                UnregisteredAthletes[UnregisteredSelectedAthleteIndex].Name)
              {
                  Handicap = UnregisteredAthletes[UnregisteredSelectedAthleteIndex].RoundedHandicap,
                  IsNew = true
              };

            if (UnregisteredAthletes[UnregisteredSelectedAthleteIndex].NumberOfRuns == 0)
            {
                newAthlete.FirstTimer = true;
            }

            RegisteredAthletes.Add(newAthlete);
            UnregisteredAthletes.RemoveAt(UnregisteredSelectedAthleteIndex);
            UnregisteredSelectedAthleteIndex = -1;
            NextRaceNumber();
        }

        /// <summary>
        /// Check that an unregistered athlete is selected and the race doesn't already exist.
        /// </summary>
        /// <returns>flag indicating valid number</returns>
        public bool CanRegisterAthleteForSeason()
        {
            return UnregisteredSelectedAthleteIndex >= 0;
            //&&
            //!RaceNumberExists(NewRaceNumber);
        }

        ///// <summary>
        ///// Adds the new Race number to the selected registered athlete.
        ///// </summary>
        ///// <remarks>
        ///// If the athlete is not new then it can be marked as updated.
        ///// The new race number is chosen.
        ///// </remarks>
        //public void AddNewNumberToRegisteredAthlete()
        //{
        //  RegisteredAthletes[RegisteredSelectedAthleteIndex].RunningNumbers.Add(NewRaceNumber);
        //  if (!RegisteredAthletes[RegisteredSelectedAthleteIndex].IsNew)
        //  {
        //    RegisteredAthletes[RegisteredSelectedAthleteIndex].IsUpdated = true;
        //  }

        //  NextRaceNumber();
        //}

        /// <summary>
        /// Check that a registered athlete is selected and the race doesn't already exist.
        /// </summary>
        /// <returns>flag indicating valid number</returns>
        public bool CanAddNewNumberToRegisteredAthlete()
        {
            return RegisteredSelectedAthleteIndex >= 0;
            //&& !RaceNumberExists(NewRaceNumber);
        }

        public void Save()
        {
            foreach (AthleteSeasonConfig athlete in RegisteredAthletes)
            {
                if (athlete.IsNew)
                {
                    this.model.CurrentSeason.AddNewAthlete(
                      athlete.Key,
                      athlete.Name,
                      athlete.Handicap,
                      athlete.FirstTimer);
                }
            }

            Messenger.Default.Send(
                new HandicapProgressMessage(
                    "Registered Athletes Saved"));
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>LoadAthleteInformation</name>
        /// <date>14/03/15</date>
        /// <summary>
        /// Loads the athlete information via the business library and adds it to the athlete collection.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        private void LoadAthleteInformation(List<AthleteDetails> athletes)
        {
            List<AthleteDetails> orderedList = athletes.OrderBy(athlete => athlete.Forename).ToList();
            orderedList = orderedList.OrderBy(athlete => athlete.Surname).ToList();

            this.UnregisteredAthletes = new ObservableCollection<Types.Athletes.AthleteSimpleViewModel>(); // don't need all this ?
            foreach (AthleteDetails athlete in orderedList)
            {
                this.UnregisteredAthletes.Add(
                  new Types.Athletes.AthleteSimpleViewModel(
                    athlete.Key,
                    athlete.Name,
                    athlete.Club,
                    athlete.Sex.ToString(),
                    athlete.RoundedHandicap.ToString(),
                    athlete.PB.ToString(),
                    athlete.LastAppearance.ToString(),
                    athlete.Appearances,
                    athlete.SignedConsent,
                    athlete.Active));
            }
        }

        /// <summary>
        /// Loads all the registered athletes
        /// </summary>
        /// <param name="athletes">registered athletes list</param>
        private void LoadRegisteredInformation(List<AthleteSeasonDetails> athletes)
        {
            RegisteredAthletes = new ObservableCollection<AthleteSeasonConfig>();

            foreach (AthleteSeasonDetails athlete in athletes)
            {
                RegisteredAthletes.Add(
                  new AthleteSeasonConfig(
                    athlete.Key,
                    athlete.Name));
                foreach (AthleteSimpleViewModel summary in this.UnregisteredAthletes)
                {
                    if (summary.Key == athlete.Key)
                    {
                        this.UnregisteredAthletes.Remove(summary);
                        break;
                    }
                }
            }

            CalculateDefaultRaceNumber();
        }

        /// <summary>
        /// To be made obsolete
        /// </summary>
        private void CalculateDefaultRaceNumber()
        {
            this.NewRaceNumber = "N/A";
            //foreach (AthleteSeasonBase athlete in RegisteredAthletes)
            //{
            //  foreach (int number in athlete.RunningNumbers)
            //  {
            //    if (number >= NewRaceNumber)
            //    {
            //      NewRaceNumber = number + 1;
            //    }
            //  }
            //}
        }

        /// <summary>
        /// To be made obsolete.
        /// </summary>
        /// <param name="testNumber"></param>
        /// <returns></returns>
        private bool RaceNumberExists(int testNumber)
        {
            //foreach (AthleteSeasonBase athlete in RegisteredAthletes)
            //{
            //  foreach (int number in athlete.RunningNumbers)
            //  {
            //    if (number == testNumber)
            //    {
            //      return true;
            //    }
            //  }
            //}

            return false;
        }
    }
}
