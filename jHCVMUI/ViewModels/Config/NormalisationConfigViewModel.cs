namespace jHCVMUI.ViewModels.Config
{
    using System;
    using System.Windows.Input;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Types;
    using CommonLib.Enumerations;
    using HandicapModel.Admin.Manage;
    using jHCVMUI.ViewModels.Commands.Configuration;
    using jHCVMUI.ViewModels.ViewModels;

    public class NormalisationConfigViewModel : ViewModelBase
    {
        /// <summary>
        /// The normalisation configuration manager
        /// </summary>
        private readonly INormalisationConfigMngr normalisationConfigManager;

        /// <summary>
        /// Application logger
        /// </summary>
        private readonly IJHcLogger logger;

        private bool useHandicap = true;
        private bool useHandicapOrig = true;
        private string handicapTime = string.Empty;
        private string handicapTimeOrig = string.Empty;
        private string minimumHandicap = string.Empty;
        private string minimumHandicapOrig = string.Empty;
        private string handicapInterval = string.Empty;
        private string handicapIntervalOrig = string.Empty;

        /// <summary>
        /// Initialises a new instance of the <see cref="NormalisationConfigViewModel"/> class.
        /// </summary>
        /// <param name="normalisationConfigManager">normalisation config manager</param>
        /// <param name="logger">application logger</param>
        public NormalisationConfigViewModel(
            INormalisationConfigMngr normalisationConfigManager,
            IJHcLogger logger)
        {
            this.normalisationConfigManager = normalisationConfigManager;
            this.logger = logger;

            NormalisationConfigType config = 
                this.normalisationConfigManager .ReadNormalisationConfiguration();

            useHandicap = config.UseCalculatedHandicap;
            handicapTime = config.HandicapTime.ToString();
            minimumHandicap = config.MinimumHandicap.ToString();
            this.handicapInterval = config.HandicapInterval.ToString();

            useHandicapOrig = UseHandicap;
            handicapTimeOrig = HandicapTime;
            minimumHandicapOrig = MinimumHandicap;
            this.handicapIntervalOrig = this.HandicapInterval;

            SaveCommand = new NormalisationConfigSaveCmd(this);
        }

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value which indicates if the handicap should be calculated and used.
        /// </summary>
        public bool UseHandicap
        {
            get { return useHandicap; }
            set
            {
                useHandicap = value;
                RaisePropertyChangedEvent(nameof(this.UseHandicap));
                RaisePropertyChangedEvent("UseHandicapChanged");
                RaisePropertyChangedEvent("HandicapTimeChanged");
                RaisePropertyChangedEvent("MinimumHandicapChanged");
                RaisePropertyChangedEvent(nameof(this.HandicapIntervalChanged));
            }
        }

        /// <summary>
        /// Gets a value which indicates if the use handicap value has been updated.
        /// </summary>
        public FieldUpdatedType UseHandicapChanged
        {
            get
            {
                if (useHandicap != useHandicapOrig)
                {
                    return FieldUpdatedType.Changed;
                }
                else
                {
                    return FieldUpdatedType.Unchanged;
                }
            }
        }

        /// <summary>
        /// Gets or sets the time the handicap is measured against.
        /// </summary>
        public string HandicapTime
        {
            get { return handicapTime; }
            set
            {
                handicapTime = value;
                RaisePropertyChangedEvent("HandicapTime");
                RaisePropertyChangedEvent("HandicapTimeChanged");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the handicap time value is valid.
        /// </summary>
        public bool HandicapTimeValid
        {
            get
            {
                int testNum;
                return int.TryParse(HandicapTime, out testNum);
            }
        }

        /// <summary>
        /// Gets the state of the handicap time changed property
        /// </summary>
        public FieldUpdatedType HandicapTimeChanged
        {
            get
            {
                if (!UseHandicap)
                {
                    return FieldUpdatedType.Disabled;
                }
                else if (!HandicapTimeValid)
                {
                    return FieldUpdatedType.Invalid;
                }
                else if (string.Compare(HandicapTime, handicapTimeOrig) != 0)
                {
                    return FieldUpdatedType.Changed;
                }
                else
                {
                    return FieldUpdatedType.Unchanged;
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum valid handicap.
        /// </summary>
        public string MinimumHandicap
        {
            get { return minimumHandicap; }
            set
            {
                minimumHandicap = value;
                RaisePropertyChangedEvent("MinimumHandicap");
                RaisePropertyChangedEvent("MinimumHandicapChanged");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the minimum handicap time value is valid.
        /// </summary>
        public bool MinimumHandicapValid
        {
            get
            {
                int testNum;
                return int.TryParse(MinimumHandicap, out testNum);
            }
        }

        /// <summary>
        /// Gets the state of the minimum handicap changed property
        /// </summary>
        public FieldUpdatedType MinimumHandicapChanged
        {
            get
            {
                if (!UseHandicap)
                {
                    return FieldUpdatedType.Disabled;
                }
                else if (!MinimumHandicapValid)
                {
                    return FieldUpdatedType.Invalid;
                }
                else if (string.Compare(MinimumHandicap, minimumHandicapOrig) != 0)
                {
                    return FieldUpdatedType.Changed;
                }
                else
                {
                    return FieldUpdatedType.Unchanged;
                }
            }
        }

        /// <summary>
        /// Gets or sets the time handicap interval.
        /// </summary>
        public string HandicapInterval
        {
            get { return handicapInterval; }
            set
            {
                handicapInterval = value;
                RaisePropertyChangedEvent(nameof(this.HandicapInterval));
                RaisePropertyChangedEvent(nameof(this.HandicapIntervalChanged));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the handicap interval value is valid.
        /// </summary>
        public bool HandicapIntervalValid
        {
            get
            {
                int testNum;
                return int.TryParse(HandicapInterval, out testNum);
            }
        }

        /// <summary>
        /// Gets the state of the handicap interval changed property
        /// </summary>
        public FieldUpdatedType HandicapIntervalChanged
        {
            get
            {
                if (!UseHandicap)
                {
                    return FieldUpdatedType.Disabled;
                }
                else if (!HandicapIntervalValid)
                {
                    return FieldUpdatedType.Invalid;
                }
                else if (string.Compare(HandicapInterval, handicapIntervalOrig) != 0)
                {
                    return FieldUpdatedType.Changed;
                }
                else
                {
                    return FieldUpdatedType.Unchanged;
                }
            }
        }

        /// <summary>
        /// Indicates if all the entered values are valid.
        /// </summary>
        /// <returns>Entries valid</returns>
        public bool ValidEntries()
        {
            if (!UseHandicap)
            {
                return true;
            }
            else
            {
                return HandicapTimeValid &&
                  MinimumHandicapValid &&
                  HandicapIntervalValid;
            }
        }

        /// <summary>
        /// Save the files.
        /// </summary>
        public void SaveConfig()
        {
            try
            {
                int saveHandicapTime = 0;
                int saveMinimumTime = 0;
                int saveHandicapInterval = 0;

                if (!int.TryParse(HandicapTime, out saveHandicapTime))
                {
                    this.logger.WriteLog("Can't save normalisation config, invalid handicap time");
                    return;
                }

                if (!int.TryParse(MinimumHandicap, out saveMinimumTime))
                {
                    this.logger.WriteLog("Can't save normalisation config, invalid minimum handicap time");
                    return;
                }

                if (!int.TryParse(this.HandicapInterval, out saveHandicapInterval))
                {
                    this.logger.WriteLog("Can't save normalisation config, invalid handicap interval");
                    return;
                }

                this.normalisationConfigManager.SaveNormalisationConfiguration(
                    UseHandicap,
                    saveHandicapTime,
                    saveMinimumTime,
                    saveHandicapInterval);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error saving normalisation config: " + ex.ToString());
            }
        }
    }
}