namespace jHandicap
{
    using System.Windows;
    using CommunityToolkit.Mvvm.DependencyInjection;
    using jHandicap.ViewModel;

    /// <summary>
    /// Interaction logic for PrimaryDisplay.xaml
    /// </summary>
    public partial class PrimaryDisplay : Window
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="PrimaryDisplay"/> class.
        /// </summary>
        public PrimaryDisplay()
        {
            this.InitializeComponent();
            this.DataContext = Ioc.Default.GetService<IMainViewModel>();
        }
    }
}
