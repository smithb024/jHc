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
    }
}