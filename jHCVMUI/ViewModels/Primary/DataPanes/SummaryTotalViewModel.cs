namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using CommonHandicapLib.Messages;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.SeasonModel;
    using System;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// View model for the total season summary view.
    /// </summary>
    public class SummaryTotalViewModel : SummaryViewModel
    {
        /// <summary>
        /// The season model object.
        /// </summary>
        private ISeason seasonModel;

        /// <summary>
        /// Initialises a new instance of the <see cref="SummaryTotalViewModel"/> class.
        /// </summary>
        /// <param name="model">Junior handicap model</param>
        public SummaryTotalViewModel(
            IModel model)
            : base(model.CurrentSeason.Summary)
        {
            this.seasonModel = model.CurrentSeason;
            this.seasonModel.SummaryChangedEvent += this.ModelUpdated;

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
            this.UpdateModel(seasonModel.Summary);
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