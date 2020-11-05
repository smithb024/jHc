namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using System;
    using CommonLib.Types;
    using HandicapModel.Interfaces.Common;
    using jHCVMUI.ViewModels.ViewModels;

    /// <summary>
    /// View Model used by the Data Pane to display summary information.
    /// </summary>
    public class SummaryViewModel : ViewModelBase
    {
        /// <summary>
        /// The model associated with this view model.
        /// </summary>
        ISummary model;

        /// <summary>
        /// Initialises a new instance of <see cref="ViewModels.SummaryViewModel"/> class.
        /// </summary>
        public SummaryViewModel(
            ISummary model)
        {
            this.model = model;
            this.MaleRunners = this.model.MaleRunners;
            this.FemaleRunners = this.model.FemaleRunners;
            this.SBs = this.model.SBs;
            this.PBs = this.model.PBs;
            this.FirstTimers = this.model.FirstTimers;
            this.FastestBoy = this.model.FastestBoy;
            this.FastestBoyTime = this.model.FastestBoyTime;
            this.FastestGirl = this.model.FastestGirl;
            this.FastestGirlTime = this.model.FastestGirlTime;

            this.model.SummaryDataChangedEvent += this.PopulateSummaryFromModel;
            this.model.FastestDataChangedEvent += this.FastestAthletesFromModel;
        }

        /// <summary>
        /// Gets the number of total runners.
        /// </summary>
        public int TotalRunners => MaleRunners + FemaleRunners;

        /// <summary>
        /// Gets the number of male runners.
        /// </summary>
        public int MaleRunners { get; private set; }

        /// <summary>
        /// Gets the number of female runners.
        /// </summary>
        public int FemaleRunners { get; private set; }

        /// <summary>
        /// Gets the number of seasons bests.
        /// </summary>
        public int SBs { get; private set; }

        /// <summary>
        /// Gets the number of personal bests.
        /// </summary>
        public int PBs { get; private set; }

        /// <summary>
        /// Gets the number of first timers
        /// </summary>
        public int FirstTimers { get; private set; }

        /// <summary>
        /// Gets the name of the fastest boy
        /// </summary>
        public string FastestBoy { get; private set; }

        /// <summary>
        /// Gets the fastest boy time
        /// </summary>
        public TimeType FastestBoyTime { get; private set; }

        /// <summary>
        /// Gets the name of the fastest girl.
        /// </summary>
        public string FastestGirl { get; private set; }

        /// <summary>
        /// Gets the fastest girls time
        /// </summary>
        public TimeType FastestGirlTime { get; private set; }

        /// <summary>
        /// Populate the season summary with the latest information.
        /// </summary>
        /// <param name="summary">summary information</param>
        public void PopulateSummaryFromModel(
            object sender,
            EventArgs e)
        {
            this.MaleRunners = this.model.MaleRunners;
            this.FemaleRunners = this.model.FemaleRunners;
            this.SBs = this.model.SBs;
            this.PBs = this.model.PBs;
            this.FirstTimers = this.model.FirstTimers;

            this.RaisePropertyChangedEvent(nameof(this.MaleRunners));
            this.RaisePropertyChangedEvent(nameof(this.FemaleRunners));
            this.RaisePropertyChangedEvent(nameof(this.SBs));
            this.RaisePropertyChangedEvent(nameof(this.PBs));
            this.RaisePropertyChangedEvent(nameof(this.FirstTimers));
            this.RaisePropertyChangedEvent(nameof(this.TotalRunners));
        }

        /// <summary>
        /// Populate the season summary with the latest information.
        /// </summary>
        /// <param name="summary">summary information</param>
        public void FastestAthletesFromModel(
            object sender,
            EventArgs e)
        {
            this.FastestBoy = this.model.FastestBoy;
            this.FastestBoyTime = this.model.FastestBoyTime;
            this.FastestGirl = this.model.FastestGirl;
            this.FastestGirlTime = this.model.FastestGirlTime;

            this.RaisePropertyChangedEvent(nameof(this.FastestBoy));
            this.RaisePropertyChangedEvent(nameof(this.FastestGirl));
            this.RaisePropertyChangedEvent(nameof(this.FastestBoyTime));
            this.RaisePropertyChangedEvent(nameof(this.FastestGirlTime));
        }
    }
}