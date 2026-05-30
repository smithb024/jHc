namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using CommonHandicapLib.Messages;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.SeasonModel;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// View model for the total season summary view.
    /// </summary>
    public class SummaryTotalViewModel : SummaryViewModel
    {
        /// <summary>
        /// The season model object.
        /// </summary>
        private readonly ISeason seasonModel;

        /// <summary>
        /// Initialises a new instance of the <see cref="SummaryTotalViewModel"/> class.
        /// </summary>
        /// <param name="model">Junior handicap model</param>
        public SummaryTotalViewModel(
            IModel model)
            : base(model.CurrentSeason.Summary)
        {
            this.seasonModel = model.CurrentSeason;

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
            if (message.RefreshSummaryTotal)
            {
                this.UpdateModel(this.seasonModel.Summary);
            }
        }
    }
}