namespace jHCVMUI.Views.Primary
{
    using CommunityToolkit.Mvvm.DependencyInjection;
    using jHCVMUI.ViewModels.Primary;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SeasonRibbonControls.xaml
    /// </summary>
    public partial class SeasonRibbonPane : UserControl
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SeasonRibbonPane"/> class.
        /// </summary>
        public SeasonRibbonPane()
        {
            this.InitializeComponent();
            this.DataContext = Ioc.Default.GetService<SeasonPaneViewModel>();
        }
    }
}
