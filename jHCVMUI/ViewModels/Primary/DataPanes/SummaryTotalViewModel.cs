namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using HandicapModel.Interfaces.Common;

    /// <summary>
    /// View model for the total season summary view.
    /// </summary>
    /// It contains no information, the type is just used to determine which view model is linked
    /// as the data context.
    /// </remarks>
    public class SummaryTotalViewModel : SummaryViewModel
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SummaryTotalViewModel"/> class.
        /// </summary>
        /// <param name="model">Junior handicap model</param>
        public SummaryTotalViewModel(
            ISummary model)
            : base(model)
        {
        }
   }
}
