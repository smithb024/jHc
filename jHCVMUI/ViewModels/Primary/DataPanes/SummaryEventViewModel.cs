namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using HandicapModel.Interfaces.Common;

    /// <summary>
    /// View model for the event summary view.
    /// </summary>
    /// <remarks>
    /// It contains no information, the type is just used to determine which view model is linked
    /// as the data context.
    /// </remarks>
    public class SummaryEventViewModel : SummaryViewModel
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SummaryEventViewModel"/> class.
        /// </summary>
        /// <param name="model">Junior handicap model</param>
        public SummaryEventViewModel(
            ISummary model)
            : base (model)
        {
        }
    }
}