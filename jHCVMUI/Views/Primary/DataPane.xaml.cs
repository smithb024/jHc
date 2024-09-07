namespace jHCVMUI.Views.Primary
{
    using CommunityToolkit.Mvvm.DependencyInjection;
    using jHCVMUI.ViewModels.Primary;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for DataPane.xaml
    /// </summary>
    public partial class DataPane : UserControl
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="DataPane"/> class.
        /// </summary>
        public DataPane()
        {
            this.InitializeComponent();
            this.DataContext = Ioc.Default.GetService<DataPaneViewModel>();
        }
    }
}
