namespace jHCVMUI.ViewModels.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using CommonHandicapLib.Types;
    using CommonLib.Helpers;

    using HandicapModel.Admin.Manage;
    using HandicapModel.AthletesModel;
    using HandicapModel.Common;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.SeasonModel;

    using jHCVMUI.ViewModels.ViewModels.Types.Misc;

    using Types.Athletes;

    /// <summary>
    /// View model to describe all the althetes in the system.
    /// </summary>
    public class AthleteSummaryViewModel : ViewModelBase
    {
        /// <summary>
        /// Junior handicap model.
        /// </summary>
        private readonly IModel model;

        /// <summary>
        /// Normalisation configuration manager.
        /// </summary>
        private readonly INormalisationConfigMngr normalisationConfigManager;

        /// <summary>
        /// The collection of athletes.
        /// </summary>
        private ObservableCollection<AthleteCompleteViewModel> athleteCollection;

        /// <summary>
        /// The index of the selected <see cref="athleteCollection"/>.
        /// </summary>
        private int athleteCollectionIndex;

        /// <summary>
        /// Initialises a new instance of the <see cref="AthleteSummaryViewModel"/> class.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="normalisationConfigManager">Normalisation configuration manager</param>
        public AthleteSummaryViewModel(
            IModel model,
            INormalisationConfigMngr normalisationConfigManager)
        {
            this.model = model;
            this.normalisationConfigManager = normalisationConfigManager;

            this.LoadAthleteInformation(model.Athletes.AthleteDetails);

            this.athleteCollectionIndex = -1;
        }
        
        /// <summary>
        /// Gets a value which indicates if the index is valid.
        /// </summary>
        private bool IndexValid =>
          this.AthleteCollectionIndex >= 0 && this.AthleteCollectionIndex < this.AthleteCollection?.Count;

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>AthleteCollection</name>
        /// <date>02/03/15</date>
        /// <summary>
        /// Gets and sets the athlete collection
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public ObservableCollection<AthleteCompleteViewModel> AthleteCollection
        {
            get => this.athleteCollection;
            set
            {
                this.athleteCollection = value;
                this.RaisePropertyChangedEvent(nameof(this.AthleteCollection));
            }
        }

        /// <summary>
        /// Gets or sets the athlete collection index.
        /// </summary>
        public int AthleteCollectionIndex
        {
            get => this.athleteCollectionIndex;

            set
            {
                this.athleteCollectionIndex = value;
                this.RaisePropertyChangedEvent(nameof(this.AthleteCollectionIndex));
                this.UpdatePropertyChangedEvents();
            }
        }

        /// <summary>
        /// Gets the key for the currently selected athlete.
        /// </summary>
        public string AthleteSummaryKey =>
            this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].Key.ToString() :
              string.Empty;

        /// <summary>
        /// Gets the name for the currently selected athlete.
        /// </summary>
        public string AthleteSummaryName =>
            this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].Name :
              string.Empty;

        /// <summary>
        /// Gets the club for the currently selected athlete.
        /// </summary>
        public string AthleteSummaryClub =>
            this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].Club :
              string.Empty;

        /// <summary>
        /// Gets the sex for the currently selected athlete.
        /// </summary>
        public string AthleteSummarySex =>
            this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].Sex :
              string.Empty;

        /// <summary>
        /// Gets the rounded handicap for the currently selected athlete.
        /// </summary>
        public string AthleteSummaryHandicap =>
            this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].RoundedHandicap :
              string.Empty;

        /// <summary>
        /// Gets the PB for the currently selected athlete.
        /// </summary>
        public string AthleteSummaryPB =>
            this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].PB :
              string.Empty;

        /// <summary>
        /// Gets the last appearance for the currently selected athlete.
        /// </summary>
        public string AthleteSummaryLastAppearance =>
            this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].LastAppearance :
              string.Empty;

        /// <summary>
        /// Gets the events run count for the currently selected athlete.
        /// </summary>
        public string AthleteSummaryEventsRun =>
            this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].NumberOfRuns.ToString() :
              string.Empty;

        /// <summary>
        /// Gets the active for the currently selected athlete.
        /// </summary>
        public string AthleteSummarySignedConsent =>
            this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].SignedConsent.ToString() :
              string.Empty;

        /// <summary>
        /// Gets the active for the currently selected athlete.
        /// </summary>
        public string AthleteSummaryActive =>
            this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].Active.ToString() :
              string.Empty;

        /// <summary>
        /// Gets the season best for the currently selected athlete.
        /// </summary>
        public string AthleteSummarySB =>
            this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].SB :
              string.Empty;

        /// <summary>
        /// Gets the running numbers for the currently selected athlete.
        /// </summary>
        public ObservableCollection<string> AthleteSummaryRunningNumbers =>
            this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].CurrentSeasonNumbers :
              new ObservableCollection<string>();

        /// <summary>
        /// Gets all times for the currently selected athlete.
        /// </summary>
        public ObservableCollection<AppearancesViewModel> AthleteSummaryAllTimes =>
            this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].AllTimes :
              new ObservableCollection<AppearancesViewModel>();

        /// <summary>
        /// Gets this seasons times for the currently selected athlete.
        /// </summary>
        public ObservableCollection<AppearancesViewModel> AthleteSummarySeasonTimes =>
           this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].CurrentSeasonTimes :
              new ObservableCollection<AppearancesViewModel>();

        /// <summary>
        /// Gets the total points for the currently selected athlete.
        /// </summary>
        public string AthleteSummaryTotalPoints =>
            this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].CurrentSeasonTotalPoints.ToString() :
              string.Empty;

        /// <summary>
        /// Gets the finishing points for the currently selected athlete.
        /// </summary>
        public string AthleteSummaryFinishingPoints =>
            this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].CurrentSeasonFinishingPoints.ToString() :
              string.Empty;

        /// <summary>
        /// Gets the position points for the currently selected athlete.
        /// </summary>
        public string AthleteSummaryPositionPoints =>
            this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].CurrentSeasonPositionPoints.ToString() :
              string.Empty;

        /// <summary>
        /// Gets the best points for the currently selected athlete.
        /// </summary>
        public string AthleteSummaryBestPoints =>
            this.IndexValid ?
              this.AthleteCollection[this.AthleteCollectionIndex].CurrentSeasonBestPoints.ToString() :
              string.Empty;

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>LoadAthleteInformation</name>
        /// <date>14/03/15</date>
        /// <summary>
        /// Loads the athlete information via the business library and adds it to the athlete collection.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        private void LoadAthleteInformation(List<AthleteDetails> athletes)
        {
            NormalisationConfigType hcConfiguration =
              this.normalisationConfigManager.ReadNormalisationConfiguration();

            List<AthleteDetails> orderedList = athletes.OrderBy(athlete => athlete.Forename).ToList();
            orderedList = orderedList.OrderBy(athlete => athlete.Surname).ToList();

            this.AthleteCollection = new ObservableCollection<AthleteCompleteViewModel>();
            foreach (AthleteDetails athlete in orderedList)
            {
                IAthleteSeasonDetails athleteCurrentSeason =
                 this.model.CurrentSeason.Athletes.Find(a => a.Key == athlete.Key);

                if (athleteCurrentSeason == null)
                {
                    athleteCurrentSeason =
                      new AthleteSeasonDetails(
                        athlete.Key,
                        athlete.Name);
                }

                string handicap = athleteCurrentSeason.GetRoundedHandicap(hcConfiguration)?.ToString() ?? athlete.RoundedHandicap.ToString();

                this.AthleteCollection.Add(
                  new AthleteCompleteViewModel(
                    athlete.Key,
                    athlete.Name,
                    athlete.Club,
                    athlete.Sex.ToString(),
                    handicap,
                    athlete.PB.ToString(),
                    athlete.LastAppearance.ToString(),
                    athlete.Appearances,
                    athlete.SignedConsent,
                    athlete.Active,
                    athleteCurrentSeason.SB.ToString(),
                    ListOCConverter.ToObservableCollection(athlete.RunningNumbers),
                    this.ConvertAppearances(athleteCurrentSeason.Times),
                    this.ConvertAppearances(athlete.Times),
                    athleteCurrentSeason.Points.TotalPoints,
                    athleteCurrentSeason.Points.FinishingPoints,
                    athleteCurrentSeason.Points.PositionPoints,
                    athleteCurrentSeason.Points.BestPoints));
            }
        }

        /// <summary>
        /// The athlete data has changed,reload the whole lot.
        /// </summary>
        public void AthleteInfoUpdated()
        {
            this.LoadAthleteInformation(this.model.Athletes.AthleteDetails);
        }

        /// <summary>
        /// Convert a collection of appearances to a view model.
        /// </summary>
        /// <param name="originList">origin collection</param>
        /// <returns>return collection</returns>
        private ObservableCollection<AppearancesViewModel> ConvertAppearances(
          List<Appearances> originList)
        {
            ObservableCollection<AppearancesViewModel> returnCollection = new ObservableCollection<AppearancesViewModel>();

            foreach (Appearances listObject in originList)
            {
                returnCollection.Add(
                  new AppearancesViewModel(
                    listObject.Time.ToString(),
                    listObject.Date.ToString()));
            }

            return returnCollection;
        }

        /// <summary>
        /// Update all the properties.
        /// </summary>
        private void UpdatePropertyChangedEvents()
        {
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummaryKey));
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummaryName));
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummaryClub));
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummarySex));
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummaryHandicap));
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummaryPB));
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummaryLastAppearance));
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummaryEventsRun));
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummarySignedConsent));
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummaryActive));
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummarySB));
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummaryRunningNumbers));
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummaryAllTimes));
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummarySeasonTimes));
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummaryTotalPoints));
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummaryFinishingPoints));
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummaryPositionPoints));
            this.RaisePropertyChangedEvent(nameof(this.AthleteSummaryBestPoints));
        }
    }
}