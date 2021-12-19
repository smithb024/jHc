namespace jHCVMUI.ViewModels.Config
{
    using System;
    using System.Windows.Input;

    using CommonHandicapLib;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Types;
    using CommonLib.Enumerations;
    using HandicapModel.Admin.Manage;
    using jHCVMUI.ViewModels.Commands.Configuration;
    using jHCVMUI.ViewModels.ViewModels;

    public class SeriesConfigViewModel : ViewModelBase
    {
        private string numberPrefix = string.Empty;
        private string numberPrefixOrig = string.Empty;
        private bool allPositions;
        private bool jointSecondThird;

        /// <summary>
        /// The series configuration manager
        /// </summary>
        private readonly ISeriesConfigMngr seriesConfigManager;

        /// <summary>
        /// Application logger
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="SeriesConfigViewModel"/> class.
        /// </summary>
        /// <param name="seriesConfigurationManager">series configuration manager</param>
        /// <param name="logger">application logger</param>
        public SeriesConfigViewModel(
            ISeriesConfigMngr seriesConfigurationManager,
            IJHcLogger logger)
        {
            this.logger = logger;
            this.seriesConfigManager = seriesConfigurationManager;
            SeriesConfigType config = 
                this.seriesConfigManager.ReadSeriesConfiguration();

            if (config == null)
            {
                this.logger.WriteLog(
                  "Series Config VM not initialised, Config reader has failed.");
                return;
            }

            numberPrefix = config.NumberPrefix;

            numberPrefixOrig = this.NumberPrefix;

            this.SaveCommand = new SeriesConfigSaveCmd(this);

            this.jointSecondThird = !config.AllPositionsShown;
            this.allPositions = config.AllPositionsShown;
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets number prefix.
        /// </summary>
        public string NumberPrefix
        {
            get { return this.numberPrefix; }
            set
            {
                this.numberPrefix = value;
                RaisePropertyChangedEvent("NumberPrefix");
                RaisePropertyChangedEvent("NumberPrefixChanged");
            }
        }

        /// <summary>
        /// Gets or sets all positions flag.
        /// </summary>
        public bool AllPositions
        {
            get { return this.allPositions; }
            set
            {
                this.SetAllPositions(value);
            }
        }

        /// <summary>
        /// Gets or sets all positions flag.
        /// </summary>
        public bool JointSecondThird
        {
            get { return this.jointSecondThird; }
            set
            {
                this.SetJointSecondThird(value);
            }
        }

        /// <summary>
        /// Gets the state of the number prefix changed property
        /// </summary>
        public FieldUpdatedType NumberPrefixChanged
        {
            get
            {
                if (string.Compare(this.NumberPrefix, this.numberPrefixOrig) != 0)
                {
                    return FieldUpdatedType.Changed;
                }
                else
                {
                    return FieldUpdatedType.Unchanged;
                }
            }
        }

        public bool CanSaveConfig()
        {
            return SeriesConfigType.ValidNumber(this.NumberPrefix);
        }

        /// <summary>
        /// Save the files.
        /// </summary>
        public void SaveConfig()
        {
            try
            {
                this.seriesConfigManager.SaveNormalisationConfiguration(
                  this.NumberPrefix,
                  this.AllPositions);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error saving series config: " + ex.ToString());
            }
        }

        /// <summary>
        /// Manage the positions flags. Only one can be set.
        /// </summary>
        /// <param name="value">new all positions</param>
        private void SetAllPositions(bool value)
        {
            this.allPositions = value;
            this.jointSecondThird = !value;

            this.RaisePropertyChangedEvent(nameof(this.AllPositions));
            this.RaisePropertyChangedEvent(nameof(this.JointSecondThird));
        }

        /// <summary>
        /// Manage the positions flags. Only one can be set.
        /// </summary>
        /// <param name="value">new joint second third value</param>
        private void SetJointSecondThird(bool value)
        {
            this.allPositions = !value;
            this.jointSecondThird = value;

            this.RaisePropertyChangedEvent(nameof(this.AllPositions));
            this.RaisePropertyChangedEvent(nameof(this.JointSecondThird));
        }
    }
}