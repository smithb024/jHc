namespace jHCVMUI.ViewModels.Primary.DataPanes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using jHCVMUI.ViewModels.ViewModels;
    using jHCVMUI.ViewModels.ViewModels.Types.Athletes;

    using NynaeveLib.Commands;

    /// <summary>
    /// View model used by the Data Pane to display the Results Table.
    /// </summary>
    public class ResultsTableViewModel : ViewModelBase
    {
        /// <summary>
        /// Associated handicap event model
        /// </summary>
        private IHandicapEvent model;

        /// <summary>
        /// Indicates the way the results are ordered.
        /// </summary>
        private ResultsOrder resultsOrder;

        /// <summary>
        /// The table of results.
        /// </summary>
        private ObservableCollection<ResultsTableRowViewModel> resultsTable =
            new ObservableCollection<ResultsTableRowViewModel>();

        /// <summary>
        /// Indicates whether table shows a full set of information or a truncated set.
        /// </summary>
        private bool expandedData = false;

        /// <summary>
        /// Initialises a new instance of the <see cref="ResultsTableViewModel"/> class.
        /// </summary>
        /// <param name="model">Junior handicap model</param>
        public ResultsTableViewModel(
            IHandicapEvent model)
        {
            this.model = model;
            this.resultsOrder = ResultsOrder.Default;

            model.ResultsChangedEvent += this.ModelResultsChanged;
            //model.CurrentSeason.CurrentEvent.ResultsCallback = new ResultsDelegate(this.PopulateResultsTable);

            ExpandCommand =
              new SimpleCommand(
                this.UpdateExpandedFlag);

            this.ResultsOrderBySpeedCommand = 
                new SimpleCommand(
                    this.OrderResultsBySpeed,
                    this.CanOrderResultsBySpeed);
            this.ResultsOrderByTimeCommand = 
                new SimpleCommand(
                    this.OrderResultsByTime,
                    this.CanOrderResultsByTime);

            this.PopulateResultsTable();
        }

        /// <summary>
        /// Gets the command which is used to toggle between full and partial results details.
        /// </summary>
        public ICommand ExpandCommand { get; private set; }

        /// <summary>
        /// Gets the command which orders the results by speed.
        /// </summary>
        public ICommand ResultsOrderBySpeedCommand { get; private set; }

        /// <summary>
        /// Gets the command which orders the results by time.
        /// </summary>
        public ICommand ResultsOrderByTimeCommand { get; private set; }

        /// <summary>
        /// Gets and sets the results table.
        /// </summary>
        public ObservableCollection<ResultsTableRowViewModel> ResultsTable
        {
            get
            {
                return this.resultsTable;
            }

            set
            {
                this.resultsTable = value;
                RaisePropertyChangedEvent(nameof(this.ResultsTable));
            }
        }

        /// <summary>
        /// Gets and sets the expanded data flag.
        /// </summary>
        public bool ExpandedData
        {
            get
            {
                return this.expandedData;
            }

            set
            {
                this.expandedData = value;
                RaisePropertyChangedEvent(nameof(this.ExpandedData));
                RaisePropertyChangedEvent(nameof(this.ExpandedLabel));
            }
        }

        /// <summary>
        /// Gets the expanded label value.
        /// </summary>
        public string ExpandedLabel => this.ExpandedData ? "^" : "v";

        /// <summary>
        /// Toggle expanded data flag.
        /// </summary>
        public void UpdateExpandedFlag()
        {
            this.ExpandedData = !this.ExpandedData;

            foreach (ResultsTableRowViewModel results in this.ResultsTable)
            {
                results.ExpandedData = this.ExpandedData;
            }
        }

        /// <summary>
        /// Populate the the results table.
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event arguments</param>
        public void ModelResultsChanged(
            object sender,
            EventArgs e)
        {
            this.PopulateResultsTable();
        }

        /// <summary>
        /// Run the <see cref="ResultsOrderByTimeCommand"/>.
        /// </summary>
        public void OrderResultsByTime()
        {
            this.SetResults(
              this.ResultsTable.OrderBy(
                rs => rs.TotalTime).ToList());

            this.resultsOrder = ResultsOrder.Time;
        }

        /// <summary>
        /// Gets a value which indicates whether the <see cref="ResultsOrderByTimeCommand"/> 
        /// can be run.
        /// </summary>
        /// <returns>Indicates whether the command can be run.</returns>
        public bool CanOrderResultsByTime()
        {
            return this.resultsOrder != ResultsOrder.Time;
        }

        /// <summary>
        /// Run the <see cref="ResultsOrderBySpeedCommand"/>.
        /// </summary>
        public void OrderResultsBySpeed()
        {
            this.SetResults(
              this.ResultsTable.OrderBy(
                rs => rs.RunningOrder).ToList());

            this.resultsOrder = ResultsOrder.Speed;
        }

        /// <summary>
        /// Gets a value which indicates whether the <see cref="ResultsOrderBySpeedCommand"/> 
        /// can be run.
        /// </summary>
        /// <returns>Indicates whether the command can be run.</returns>
        public bool CanOrderResultsBySpeed()
        {
            return this.resultsOrder != ResultsOrder.Speed;
        }

        /// <summary>
        /// Task a list of <see cref="ResultsTableViewModel"/> objects and convert to 
        /// an observable collection and set as results.
        /// </summary>
        /// <param name="listResults"></param>
        private void SetResults(List<ResultsTableRowViewModel> listResults)
        {
            this.ResultsTable = new ObservableCollection<ResultsTableRowViewModel>();

            foreach (ResultsTableRowViewModel entry in listResults)
            {
                this.ResultsTable.Add(entry);
            }

            this.RaisePropertyChangedEvent(nameof(this.ResultsTable));
        }

        /// <summary>
        /// Populate the results table with the latest information from the model
        /// </summary>
        private void PopulateResultsTable()
        {
            this.ResultsTable = new ObservableCollection<ResultsTableRowViewModel>();

            foreach (IResultsTableEntry entry in this.model.ResultsTable.Entries)
            {
                this.ResultsTable.Add(
                  new ResultsTableRowViewModel(
                    entry.Key,
                    entry.Position,
                    entry.Name,
                    entry.Club,
                    entry.Handicap.ToString(),
                    entry.ExtraInfo,
                    entry.Notes,
                    entry.FirstTimer,
                    entry.RunningOrder,
                    entry.PB,
                    entry.Points.TotalPoints,
                    entry.Points.FinishingPoints,
                    entry.Points.PositionPoints,
                    entry.Points.BestPoints,
                    entry.TeamTrophyPoints,
                    entry.RaceNumber,
                    entry.Time.ToString(),
                    entry.RunningTime.ToString(),
                    entry.Sex.ToString(),
                    entry.SB));
            }

            this.resultsOrder = ResultsOrder.Time;
        }
    }
}
