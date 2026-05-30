namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using CommonHandicapLib.Messages;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using System;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// View model for the event summary view.
    /// </summary>
    public class SummaryEventViewModel : SummaryViewModel
    {
        /// <summary>
        /// The event model object.
        /// </summary>
        private IHandicapEvent eventModel;

        /// <summary>
        /// Initialises a new instance of the <see cref="SummaryEventViewModel"/> class.
        /// </summary>
        /// <param name="model">handicap model</param>
        public SummaryEventViewModel(
            IModel model)
            : base (model.CurrentEvent.Summary)
        {
            this.eventModel = model.CurrentEvent;
            this.eventModel.SummaryChangedEvent += this.ModelUpdated;

            CommonMessenger.Default.Register<RefreshDataPaneMessage>(
                this,
                this.Refresh);
        }

        /// <summary>
        /// The whole summary model object has been replaced, update the view models. 
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        private void ModelUpdated(
            object sender,
            EventArgs e)
        {
            this.UpdateModel(eventModel.Summary);
        }

        /// <summary>
        /// Refresh this view model.
        /// </summary>
        /// <param name="message">refresh view model message</param>
        private void Refresh(
            RefreshDataPaneMessage message)
        {
        }
    }
}