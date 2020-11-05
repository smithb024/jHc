namespace jHCVMUI.ViewModels.ViewModels
{
    using System.Collections.ObjectModel;
    using CommonLib.Helpers;
    using HandicapModel;
    using HandicapModel.Interfaces;
    using HandicapModel.SeasonModel;
    using jHCVMUI.ViewModels.ViewModels.Types.Athletes;

    public class AthleteSeasonSummaryViewModel : ViewModelBase
    {
        /// <summary>
        /// Junior handicap model.
        /// </summary>
        private IModel model;

        private ObservableCollection<AthleteSeasonBase> m_registeredAthletes = new ObservableCollection<AthleteSeasonBase>();

        /// <summary>
        /// View model which supports a Season Summary view.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        public AthleteSeasonSummaryViewModel(
            IModel model)
        {
            this.model = model;
            GetCurrentAthleteList();

            // TODO want this updating, so need to register a callback with the model. or raise an event.
            // TODO, the season information may be a little limited (eg no club), so athlete season type could be inadequate here. (inheritance?)
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>AthleteCollection</name>
        /// <date>02/03/15</date>
        /// <summary>
        /// Gets and sets the athlete collection
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public ObservableCollection<AthleteSeasonBase> RegisteredAthletes
        {
            get { return m_registeredAthletes; }
            set
            {
                m_registeredAthletes = value;
                RaisePropertyChangedEvent("RegisteredAthletes");
            }
        }

        /// <summary>
        /// Gets the current athlete list and add registered athletes in numerical order.
        /// </summary>
        private void GetCurrentAthleteList()
        {
            //List<AthleteSeasonDetails> orderedList = Model.Instance.CurrentSeason.Athletes.OrderBy(athlete => athlete.PrimaryNumber).ToList();

            RegisteredAthletes = new ObservableCollection<AthleteSeasonBase>();

            foreach (AthleteSeasonDetails athlete in this.model.CurrentSeason.Athletes)
            {
                RegisteredAthletes.Add(
                  new AthleteSeasonBase(
                    athlete.Key,
                    athlete.Name,
                    ListOCConverter.ToObservableCollection(
                      this.model.Athletes.GetAthleteRunningNumbers(
                        athlete.Key))));
            }
        }
    }
}