namespace jHCVMUI.ViewModels.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using CommonHandicapLib;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using CommonLib.Enumerations;
    using CommonLib.Helpers;
    using CommonLib.Types;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel;
    using HandicapModel.Admin.Manage;
    using HandicapModel.AthletesModel;
    using HandicapModel.Interfaces;
    using Helpers;
    using jHCVMUI.ViewModels.Commands.Configuration;
    using jHCVMUI.ViewModels.ViewModels.Types;
    using NynaeveLib.Commands;

    public class AthleteConfigurationViewModel : ViewModelBase
    {
        /// <summary>
        /// Junior handicap model.
        /// </summary>
        private IModel model;

        private int m_selectedIndex = -1;
        private string m_changeClub = string.Empty;

        /// <summary>
        /// Used to indicate whether the current athlete has a signed consent form.
        /// </summary>
        private bool changeSignedConsent = false;
        private bool changeActive = true;
        private string changePredeclaredHandicap;
        private string addRaceNumber = string.Empty;
        private string m_newName = string.Empty;
        private string m_newClub = string.Empty;
        private string newRaceNumber = string.Empty;
        private SexType m_newSex = SexType.Male;
        private string m_newInitialHandicap = string.Empty;
        private string m_newAge = string.Empty;
        private string birthDay = string.Empty;
        private string birthMonth = string.Empty;
        private string birthYear = string.Empty;

        private string searchString;
        private List<string> surnameLetterSelectorCollection;
        private int surnameSelectorIndex;

        private string numberPrefix;

        /// <summary>
        /// A collection of all <see cref="AthleteType"/>s managed by this view model.
        /// </summary>
        private ObservableCollection<AthleteType> athleteCollection;
        private ObservableCollection<ClubType> m_clubCollection = new ObservableCollection<ClubType>();

        /// <summary>
        /// Initialises a new instance of the <see cref="AthleteConfigurationViewModel"/> class.
        /// </summary>
        /// <param name="model">handicap model</param>
        /// <param name="seriesConfigManager">series configutation manager</param>
        public AthleteConfigurationViewModel(
            IModel model,
            ISeriesConfigMngr seriesConfigManager)
        {
            this.model = model;
            this.athleteCollection = new ObservableCollection<AthleteType>();
            this.changePredeclaredHandicap = string.Empty;

            this.numberPrefix = seriesConfigManager.ReadNumberPrefix();
            LoadAthleteInformation();

            ClubCollection.Add(new ClubType() { Club = string.Empty });

            // Order clubs alphabetically.
            List<string> clubs = model.GetClubList().OrderBy(club => club).ToList();

            foreach (string club in clubs)
            {
                ClubCollection.Add(new ClubType() { Club = club });
            }

            this.newRaceNumber = this.NextRunningNumber;
            this.addRaceNumber = this.NextRunningNumber;

            // Search parameters
            this.searchString = string.Empty;
            this.surnameLetterSelectorCollection = new List<string>() { "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "X", "Y", "Z" };
            this.surnameSelectorIndex = 0;

            AthleteAddNumberCommand = new AthleteConfigAddNumberCmd(this);
            //AthleteChangeCommand    = new AthleteConfigChangeCmd(this);
            this.AthleteChangeCommand =
              new SimpleCommand(
                this.ChangeAthlete,
                this.CanChange);
            //AthleteDeleteCommand    = new AthleteConfigDeleteCmd(this);
            this.AthleteDeleteCommand =
              new SimpleCommand(
                this.DeleteAthlete,
                this.CanDelete);
            this.AthleteNewCommand =
              new SimpleCommand(
                this.AddNewAthlete,
                this.CanAdd);
            //AthleteNewCommand       = new AthleteConfigNewCmd(this);
            //AthleteSaveCommand      = new AthleteConfigSaveCmd(this);
            this.AthleteSaveCommand =
              new SimpleCommand(
                this.SaveAthletes,
                this.CanSave);
        }

        /// <summary>
        /// Gets all numbers allocated to athletes in alphabetical order.
        /// </summary>
        private List<string> AllocatedNumbers
        {
            get
            {
                List<string> allocatedNumbers = new List<string>();

                foreach (AthleteType athlete in this.athleteCollection)
                {
                    foreach (string number in athlete.RunningNumbers)
                    {
                        allocatedNumbers.Add(number);
                    }
                }

                allocatedNumbers.Sort();
                return allocatedNumbers;
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>AthleteCollection</name>
        /// <date>02/03/15</date>
        /// <summary>
        /// Gets and sets the athlete collection
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public ObservableCollection<AthleteType> AthleteCollection
        {
            get
            {
                return AthleteCollectionFilter.FilterSurname(
                  this.athleteCollection,
                  this.SelectedLetter());
            }
        }

        /// <summary>
        /// Gets the collection of athletes, filtered by the search string.
        /// </summary>
        public ObservableCollection<AthleteType> FilteredAthleteCollection
        {
            get
            {
                return AthleteCollectionFilter.SearchName(
                  this.AthleteCollection,
                  this.SearchString);
            }
        }

        /// <summary>
        /// Gets or sets the search string.
        /// </summary>
        public string SearchString
        {
            get
            {
                return this.searchString;
            }
            set
            {
                this.searchString = value;
                this.RaisePropertyChangedEvent(nameof(this.SearchString));
                this.AthletesRaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets and sets index of the surname first letter collection
        /// </summary>
        public int SurnameSelectorIndex
        {
            get
            {
                return this.surnameSelectorIndex;
            }
            set
            {
                this.surnameSelectorIndex = value;
                this.AthletesRaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets the surname selector collection.
        /// </summary>
        public List<string> SurnameSelectorCollection => this.surnameLetterSelectorCollection;


        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>ClubCollection</name>
        /// <date>09/03/15</date>
        /// <summary>
        /// Gets and sets the club collection
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public ObservableCollection<ClubType> ClubCollection
        {
            get { return m_clubCollection; }
            set { m_clubCollection = value; }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>SelectedClubIndex</name>
        /// <date>24/02/15</date>
        /// <summary>
        /// Gets and sets the index of the club list
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public int SelectedAthleteIndex
        {
            get { return m_selectedIndex; }
            set
            {
                m_selectedIndex = value;

                if (m_selectedIndex >= 0 && m_selectedIndex < AthleteCollection.Count)
                {
                    this.ChangeClub = AthleteCollection[this.m_selectedIndex].Club;
                    this.ChangeSignedConsent = AthleteCollection[this.m_selectedIndex].SignedConsent;
                    this.ChangeActive = AthleteCollection[this.m_selectedIndex].Active;
                    this.ChangePredeclaredHandicap = AthleteCollection[this.m_selectedIndex].PredeclaredHandicap;
                }
                else
                {
                    ChangeClub = string.Empty;
                }

                RaisePropertyChangedEvent(nameof(this.SelectedAthleteIndex));
                RaisePropertyChangedEvent(nameof(this.CanBeUpdated));
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>NewName</name>
        /// <date>09/03/15</date>
        /// <summary>
        /// Gets and sets the new athlete's name
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string NewName
        {
            get { return m_newName; }
            set
            {
                m_newName = value;
                RaisePropertyChangedEvent("NewName");
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>NewClub</name>
        /// <date>09/03/15</date>
        /// <summary>
        /// Gets and sets the new athlete's club.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string NewClub
        {
            get { return m_newClub; }
            set
            {
                m_newClub = value;
                RaisePropertyChangedEvent("NewClub");
            }
        }

        /// <summary>
        /// Gets and sets the new number for the race number.
        /// </summary>
        public string AddRaceNumber
        {
            get { return addRaceNumber; }
            set
            {
                addRaceNumber = value;
                RaisePropertyChangedEvent("AddRaceNumber");
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>NewClub</name>
        /// <date>14/03/15</date>
        /// <summary>
        /// Gets and sets the athlete's new club.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string ChangeClub
        {
            get { return m_changeClub; }
            set
            {
                m_changeClub = value;
                RaisePropertyChangedEvent("ChangeClub");
            }
        }

        /// <summary>
        /// Gets a value indicating whether a signed consent form has been provided.
        /// </summary>
        public bool ChangeSignedConsent
        {
            get { return this.changeSignedConsent; }
            set
            {
                this.changeSignedConsent = value;
                RaisePropertyChangedEvent(nameof(this.ChangeSignedConsent));
            }
        }

        /// <summary>
        /// Gets a value indicating if the updated value is an active account or not.
        /// </summary>
        public bool ChangeActive
        {
            get { return this.changeActive; }
            set
            {
                this.changeActive = value;
                RaisePropertyChangedEvent(nameof(this.ChangeActive));
            }
        }

        /// <summary>
        /// Gets the new inital handicap value. This is used when no handicap is present or the system
        /// is not set up to use handicaps.
        /// </summary>
        public string ChangePredeclaredHandicap
        {
            get { return this.changePredeclaredHandicap; }
            set
            {
                this.changePredeclaredHandicap = value;
                RaisePropertyChangedEvent(nameof(this.ChangePredeclaredHandicap));
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>NewSex</name>
        /// <date>09/03/15</date>
        /// <summary>
        /// Gets and sets the new athlete's sex.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public SexType NewSex
        {
            get { return m_newSex; }
            set
            {
                m_newSex = value;
                RaisePropertyChangedEvent("NewSex");
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>NewSex</name>
        /// <date>09/03/15</date>
        /// <summary>
        /// Gets and sets the new athlete's sex.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public Array SexSource
        {
            get { return Enum.GetValues(typeof(SexType)); }
        }

        /// <summary>
        /// Gets and sets the new athlete's race number.
        /// </summary>
        public string NewRaceNumber
        {
            get { return newRaceNumber; }
            set
            {
                newRaceNumber = value;
                RaisePropertyChangedEvent("NewRaceNumber");
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>NewInitialHandicap</name>
        /// <date>09/03/15</date>
        /// <summary>
        /// Gets and sets the new athlete's initial handicap.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string NewInitialHandicap
        {
            get { return m_newInitialHandicap; }
            set
            {
                m_newInitialHandicap = value;
                RaisePropertyChangedEvent("NewInitialHandicap");
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>NewInitialHandicap</name>
        /// <date>09/03/15</date>
        /// <summary>
        /// Gets and sets the new athlete's initial handicap.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string NewAge
        {
            get { return m_newAge; }
            set
            {
                m_newAge = value;
                NewInitialHandicap = HandicapConverter.ConvertAgeToHandicap(m_newAge);
                RaisePropertyChangedEvent("NewAge");
            }
        }

        /// <summary>
        /// Gets a value indicating if the athlete can be updated.
        /// </summary>
        public bool CanBeUpdated
        {
            get
            {
                return this.CanChange();
            }
        }

        /// <summary>
        /// Gets the next available default running number.
        /// </summary>
        public string NextRunningNumber
        {
            get
            {
                int nextNumber = 0;

                foreach (string number in this.AllocatedNumbers)
                {
                    if (numberPrefix.Length <= number.Length &&
                      number.Substring(0, numberPrefix.Length).Equals(numberPrefix))
                    {
                        int convertedNumber;

                        if (int.TryParse(number.Substring(numberPrefix.Length), out convertedNumber))
                        {
                            // Assign largest number to next number
                            nextNumber = convertedNumber > nextNumber ? convertedNumber : nextNumber;
                        }
                    }
                }

                return $"{numberPrefix}{(nextNumber + 1).ToString("000000")}";
            }
        }

        public string NewBirthDay
        {
            get
            {
                return this.birthDay;
            }

            set
            {
                birthDay = value;
                RaisePropertyChangedEvent(nameof(this.NewBirthDay));
            }
        }

        public string NewBirthMonth
        {
            get
            {
                return this.birthMonth;
            }

            set
            {
                birthMonth = value;
                RaisePropertyChangedEvent(nameof(this.NewBirthMonth));
            }
        }

        public string NewBirthYear
        {
            get
            {
                return this.birthYear;
            }

            set
            {
                birthYear = value;
                RaisePropertyChangedEvent(nameof(this.birthYear));
            }
        }

        public ICommand AthleteSaveCommand
        {
            get;
            private set;
        }

        public ICommand AthleteNewCommand
        {
            get;
            private set;
        }

        public ICommand AthleteDeleteCommand
        {
            get;
            private set;
        }

        public ICommand AthleteChangeCommand
        {
            get;
            private set;
        }

        public ICommand AthleteAddNumberCommand
        {
            get;
            private set;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>SaveAthletes</name>
        /// <date>14/03/15</date>
        /// <summary>
        /// Saves the current list to a file.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public void SaveAthletes()
        {
            foreach (AthleteType athlete in AthleteCollection)
            {
                switch (athlete.Status)
                {
                    case StatusType.Added:
                        if (athlete.PredeclaredHandicap.Contains(":"))
                        {
                            int initialHandicapMinutes = 0;
                            int initialHandicapSeconds = 0;

                            if (int.TryParse(athlete.PredeclaredHandicap.Substring(0, athlete.PredeclaredHandicap.IndexOf(":")), out initialHandicapMinutes))
                            {
                                if (int.TryParse(athlete.PredeclaredHandicap.Substring(athlete.PredeclaredHandicap.IndexOf(":") + 1), out initialHandicapSeconds))
                                {
                                    this.model.CreateNewAthlete(
                                      athlete.Name,
                                      athlete.Club,
                                      initialHandicapMinutes,
                                      initialHandicapSeconds,
                                      athlete.Sex,
                                      ListOCConverter.ToList(athlete.RunningNumbers),
                                      athlete.BirthYear,
                                      athlete.BirthMonth,
                                      athlete.BirthDay,
                                      athlete.SignedConsent,
                                      athlete.Active);
                                }
                            }
                        }
                        else
                        {
                            int initialHandicap = 0;
                            if (int.TryParse(athlete.PredeclaredHandicap, out initialHandicap))
                            {
                                this.model.CreateNewAthlete(
                                  athlete.Name,
                                  athlete.Club,
                                  initialHandicap,
                                  athlete.Sex,
                                  ListOCConverter.ToList(athlete.RunningNumbers),
                                  athlete.BirthYear,
                                  athlete.BirthMonth,
                                  athlete.BirthDay,
                                  athlete.SignedConsent,
                                  athlete.Active);
                            }
                        }

                        break;
                    case StatusType.Deleted:
                        this.model.DeleteAthlete(athlete.Key);
                        break;
                    case StatusType.Updated:
                        this.model.UpdateAthlete(
                          athlete.Key,
                          athlete.Club,
                          ListOCConverter.ToList(athlete.RunningNumbers),
                          athlete.SignedConsent,
                          athlete.Active,
                          new TimeType(athlete.PredeclaredHandicap));
                        break;
                    default:
                        break;
                }
            }

            this.model.SaveAthleteList();
            ResetSelectedIndex();
            LoadAthleteInformation();

            Messenger.Default.Send(
                new HandicapProgressMessage(
                    "Athletes Saved"));
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>CanSave</name>
        /// <date>02/03/15</date>
        /// <summary>
        /// Returns true the data is not currently saved.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public bool CanSave()
        {
            return true;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>AddNewAthlete</name>
        /// <date>14/03/15</date>
        /// <summary>
        /// Adds a new club and resets the NewClub property.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public void AddNewAthlete()
        {
            ObservableCollection<string> newNumbers =
              new ObservableCollection<string>()
              {
                  this.NewRaceNumber
              };

            // TODO, we're not recording DOB, can this be removed, or default numbers added.
            AthleteType newAthlete =
              new AthleteType(
                0,
                NewName,
                NewClub,
                NewSex,
                newNumbers,
                this.NewBirthYear,
                this.NewBirthMonth,
                this.NewBirthDay,
                false,
                true,
                this.NewInitialHandicap)
              {
                  Status = StatusType.Added
              };

            this.athleteCollection.Add(newAthlete);

            NewName = string.Empty;
            NewClub = string.Empty;
            NewInitialHandicap = string.Empty;
            NewSex = SexType.Male;

            this.ResetSelectedIndex();
            this.UpdateNextRunningNumber();

            this.RaisePropertyChangedEvent(nameof(this.AthleteCollection));
            this.RaisePropertyChangedEvent(nameof(this.FilteredAthleteCollection));
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>CanAdd</name>
        /// <date>02/03/15</date>
        /// <summary>
        /// Returns true if the new club name field is not empty.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public bool CanAdd()
        {
            if (!this.RunningNumberValid(this.NewRaceNumber))
            {
                return false;
            }

            if (!this.BirthDateValid(
              this.NewBirthDay,
              this.NewBirthMonth,
              this.NewBirthYear))
            {
                return false;
            }

            if (NewName != string.Empty && NewSex != SexType.Default)
            {
                return this.TimeValid(this.NewInitialHandicap);
                //int initialHandicap = 0;
                //if (int.TryParse(NewInitialHandicap, out initialHandicap))
                //{
                //  return true;
                //}
                //else
                //{
                //  if (NewInitialHandicap.Contains(":"))
                //  {
                //    int initialHandicapMinutes = 0;
                //    int initialHandicapSeconds = 0;

                //    if (int.TryParse(NewInitialHandicap.Substring(0, NewInitialHandicap.IndexOf(":")), out initialHandicapMinutes))
                //    {
                //      if (int.TryParse(NewInitialHandicap.Substring(NewInitialHandicap.IndexOf(":") + 1), out initialHandicapSeconds))
                //      {
                //        return true;
                //      }
                //    }
                //  }

                //  return false;
                //}
            }
            else
            {
                return false;
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>DeleteAthlete</name>
        /// <date>14/03/15</date>
        /// <summary>
        /// Marks the athlete for removal and resets the index. If the athlete is new, then it removes it completely
        /// from the list.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public void DeleteAthlete()
        {
            if (AthleteCollection[SelectedAthleteIndex].Status == StatusType.Added)
            {
                AthleteCollection.RemoveAt(SelectedAthleteIndex);
            }
            else
            {
                AthleteCollection[SelectedAthleteIndex].Status = StatusType.Deleted;
            }

            ResetSelectedIndex();
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>CanDelete</name>
        /// <date>14/03/15</date>
        /// <summary>
        /// Returns true if the selected index is valid.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public bool CanDelete()
        {
            if (SelectedAthleteIndex >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the selected club and resets the index.
        /// </summary>
        public void AddNewNumber()
        {
            AthleteCollection[SelectedAthleteIndex].AddNewRunningNumber(this.AddRaceNumber);

            if (AthleteCollection[SelectedAthleteIndex].Status == StatusType.Ok)
            {
                AthleteCollection[SelectedAthleteIndex].Status = StatusType.Updated;
            }

            this.ResetSelectedIndex();
            this.UpdateNextRunningNumber();
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>DeleteClub</name>
        /// <date>02/03/15</date>
        /// <summary>
        /// Removes the selected club and resets the index.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public void ChangeAthlete()
        {
            this.AthleteCollection[this.SelectedAthleteIndex].Club = this.ChangeClub;
            this.AthleteCollection[this.SelectedAthleteIndex].SignedConsent = this.ChangeSignedConsent;
            this.AthleteCollection[this.SelectedAthleteIndex].Active = this.ChangeActive;
            this.AthleteCollection[this.SelectedAthleteIndex].PredeclaredHandicap = this.ChangePredeclaredHandicap;
            if (this.AthleteCollection[this.SelectedAthleteIndex].Status == StatusType.Ok)
            {
                this.AthleteCollection[this.SelectedAthleteIndex].Status = StatusType.Updated;
            }

            this.ResetSelectedIndex();
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>CanChange</name>
        /// <date>02/03/15</date>
        /// <summary>
        /// Returns true if the selected index is valid.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public bool CanChange()
        {
            if (SelectedAthleteIndex >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool NewNumberValid()
        {
            return this.RunningNumberValid(this.AddRaceNumber);
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>LoadAthleteInformation</name>
        /// <date>14/03/15</date>
        /// <summary>
        /// Loads the athlete information via the business library and adds it to the athlete collection.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        private void LoadAthleteInformation()
        {
            List<AthleteDetails> orderedList = this.model.Athletes.AthleteDetails.OrderBy(athlete => athlete.Forename).ToList();
            orderedList = orderedList.OrderBy(athlete => athlete.Surname).ToList();

            this.athleteCollection = new ObservableCollection<AthleteType>();
            foreach (AthleteDetails athlete in orderedList)
            {
                this.athleteCollection.Add(
                  new AthleteType(
                    athlete.Key,
                    athlete.Name,
                    athlete.Club,
                    athlete.Sex,
                    ListOCConverter.ToObservableCollection(athlete.RunningNumbers),
                    athlete.BirthDate.BirthYear,
                    athlete.BirthDate.BirthMonth,
                    athlete.BirthDate.BirthDay,
                    athlete.SignedConsent,
                    athlete.Active,
                    athlete.PredeclaredHandicap.ToString()));
            }

            this.AthletesRaisePropertyChanged();
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>ResetSelectedIndex/</name>
        /// <date>14/03/15</date>
        /// <summary>
        /// Provides a method for clearing the selected index.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        private void ResetSelectedIndex()
        {
            SelectedAthleteIndex = -1;
        }

        /// <summary>
        /// Updat the two race number properties to have the same value as the next available running
        /// number.
        /// </summary>
        private void UpdateNextRunningNumber()
        {
            this.RaisePropertyChangedEvent("NextRunningNumber");
            this.NewRaceNumber = this.NextRunningNumber;
            this.AddRaceNumber = this.NextRunningNumber;
        }

        /// <summary>
        /// Determine whether the input number is unique, and doesn't start with the banned string.
        /// </summary>
        /// <param name="testNumber">number to test</param>
        /// <returns>valid flag</returns>
        private bool RunningNumberValid(string testNumber)
        {
            if (string.IsNullOrEmpty(testNumber))
            {
                return false;
            }

            return !this.AllocatedNumbers.Exists(s => s.Equals(testNumber)) &&
              SeriesConfigType.ValidNumber(testNumber);
        }

        private bool BirthDateValid(
          string birthDay,
          string birthMonth,
          string birthYear)
        {
            if (birthDay == string.Empty &&
              birthMonth == string.Empty &&
              birthYear == string.Empty)
            {
                return true;
            }

            int birthDayInt = 0;
            int birthMonthInt = 0;
            int birthYearInt = 0;

            if (!int.TryParse(birthDay, out birthDayInt) ||
              !int.TryParse(birthMonth, out birthMonthInt) ||
              !int.TryParse(birthYear, out birthYearInt))
            {
                return false;
            }

            try
            {
                DateTime date = new DateTime(birthYearInt, birthMonthInt, birthDayInt);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Looks at a string to see if it is a valid time.
        /// </summary>
        /// <param name="testTime"></param>
        /// <returns></returns>
        private bool TimeValid(string testTime)
        {
            int simpleInteger = 0;

            if (int.TryParse(testTime, out simpleInteger))
            {
                return true;
            }

            // Not a whole minute.
            if (testTime.Contains(":"))
            {
                int minutes = 0;
                int seconds = 0;

                if (int.TryParse(testTime.Substring(0, testTime.IndexOf(":")), out minutes))
                {
                    if (int.TryParse(testTime.Substring(testTime.IndexOf(":") + 1), out seconds))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Return the currently selected surname letter.
        /// </summary>
        /// <returns>surname letter</returns>
        private string SelectedLetter()
        {
            if (this.SurnameSelectorIndex >= 0 &&
              this.SurnameSelectorCollection != null &&
              this.SurnameSelectorIndex < this.SurnameSelectorCollection.Count)
            {
                return this.SurnameSelectorCollection[this.SurnameSelectorIndex];
            }

            return string.Empty;
        }

        /// <summary>
        /// Raise the property changed events associated with a change to the athlete collections.
        /// </summary>
        private void AthletesRaisePropertyChanged()
        {
            this.RaisePropertyChangedEvent(nameof(this.SurnameSelectorIndex));
            this.RaisePropertyChangedEvent(nameof(this.AthleteCollection));
            this.RaisePropertyChangedEvent(nameof(this.FilteredAthleteCollection));
        }
    }
}