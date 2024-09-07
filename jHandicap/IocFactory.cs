namespace jHandicap
{
    using CommunityToolkit.Mvvm.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Factory class, used to set up dependency injection
    /// </summary>
    public class IocFactory
    {
        /// <summary>
        /// Setup IOC.
        /// </summary>
        public static void Setup()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                .AddSingleton<IMainWindowViewModel, MainWindowViewModel>()
                .BuildServiceProvider());
        }
    }
}
