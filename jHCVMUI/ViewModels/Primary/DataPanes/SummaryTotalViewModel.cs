namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.SeasonModel;
    using System;

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
   }
}