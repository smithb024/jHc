namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using System;
    using HandicapModel.Interfaces.SeasonModel;

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
            ISeason model)
            : base(model.Summary)
        {
            this.seasonModel = model;
            model.SummaryChangedEvent += this.ModelUpdated;
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