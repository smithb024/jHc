namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using System;

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
    }
}