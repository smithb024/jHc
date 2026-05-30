namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using CommonHandicapLib.Messages;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// View model for the event summary view.
    /// </summary>
    public class SummaryEventViewModel : SummaryViewModel
    {
        /// <summary>
        /// The event model object.
        /// </summary>
        private readonly IHandicapEvent eventModel;

        /// <summary>
        /// Initialises a new instance of the <see cref="SummaryEventViewModel"/> class.
        /// </summary>
        /// <param name="model">handicap model</param>
        public SummaryEventViewModel(
            IModel model)
            : base (model.CurrentEvent.Summary)
        {
            this.eventModel = model.CurrentEvent;

            CommonMessenger.Default.Register<RefreshDataPaneMessage>(
                this,
                this.Refresh);
        }

        /// <summary>
        /// Refresh this view model.
        /// </summary>
        /// <param name="message">refresh view model message</param>
        private void Refresh(
            RefreshDataPaneMessage message)
        {
            if (message.RefreshSummaryEvent)
            {
                this.UpdateModel(this.eventModel.Summary);
            }
        }
    }
}