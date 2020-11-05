namespace HandicapModel.Admin.Manage.Event
{
    using HandicapModel.Interfaces;

    /// <summary>
    /// Results manager base class.
    /// </summary>
    public class EventResultsMngr
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="EventResultsMngr"/> class.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        protected EventResultsMngr(IModel model)
        {
            this.Model = model;
        }

        /// <summary>
        /// Gets the junior handicap model
        /// </summary>
        protected IModel Model { get; }

        /// <summary>
        /// Save all tables.
        /// </summary>
        protected void SaveAll()
        {
            this.Model.CurrentEvent.SaveResultsTable();

            this.Model.CurrentEvent.SaveEventSummary();
            this.Model.CurrentSeason.SaveCurrentSeason();
            this.Model.SaveGlobalSummary();
            this.Model.SaveClubList();
            this.Model.SaveAthleteList();
        }
    }
}