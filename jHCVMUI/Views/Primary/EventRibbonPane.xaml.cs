namespace jHCVMUI.Views.Primary
{
    using CommunityToolkit.Mvvm.DependencyInjection;
    using jHCVMUI.ViewModels.Primary;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for EventRibbonControls.xaml
    /// </summary>
    public partial class EventRibbonPane : UserControl
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="EventRibbonPane"/> class.
        /// </summary>
        public EventRibbonPane()
        {
            this.InitializeComponent();
            this.DataContext = Ioc.Default.GetService<EventPaneViewModel>();
        }
    }
}