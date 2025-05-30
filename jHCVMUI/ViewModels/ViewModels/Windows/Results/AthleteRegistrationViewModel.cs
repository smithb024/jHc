namespace jHCVMUI.ViewModels.ViewModels.Windows.Results
{
    using jHCVMUI.ViewModels.ViewModels.Types.Athletes;
    using System.Collections.ObjectModel;

    /// <summary>
    /// View model which supports the unregistered athletes on the raw results dialog.
    /// </summary>
    public class AthleteRegistrationViewModel : AthleteSeasonBase
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="AthleteRegistrationViewModel"/> class.
        /// </summary>
        /// <param name="key">The athlete's unique key</param>
        /// <param name="name">The full name of the athlete.</param>
        /// <param name="runningNumbers">
        /// A collection of all running numbers associated with the athlete.
        /// </param>
        /// <param name="isActive">Indicates whether this athlete is active</param>
        public AthleteRegistrationViewModel(
            int key,
            string name,
            ObservableCollection<string> runningNumbers,
            bool isActive) 
            : base(key, name, runningNumbers)
        {
            this.IsActive = isActive;
        }

        /// <summary>
        /// Gets a value indicating whether this athlete has been registered.
        /// </summary>
        public bool IsRegisteredForCurrentEvent { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this athlete is currently active.
        /// </summary>
        public bool IsActive { get; }

        /// <summary>
        /// Attempt to set the athlete as registered. It will set if the correct key is provided
        /// and the athlete is not already registered.
        /// </summary>
        /// <param name="key">The key of the athlete to register</param>
        /// <returns>
        /// Returns positive if this is the correct athlete and the registration is unsuccessful.
        /// </returns>
        public bool SetRegistered(int key)
        {
            if (this.Key == key &&
                !this.IsRegisteredForCurrentEvent)
            {
                this.IsRegisteredForCurrentEvent = true;
                return true;
            }

            return false;
        }
    }
}
