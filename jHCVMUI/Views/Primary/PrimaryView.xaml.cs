namespace jHCVMUI.Views.Primary
{
    using CommunityToolkit.Mvvm.DependencyInjection;
    using jHCVMUI.ViewModels.ViewModels;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for PrimaryView.xaml
    /// </summary>
    public partial class PrimaryView : UserControl
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="PrimaryView"/> class.
        /// </summary>
        public PrimaryView()
        {
            this.InitializeComponent();
            this.DataContext = Ioc.Default.GetService<PrimaryDisplayViewModel>();
        }
    }
}