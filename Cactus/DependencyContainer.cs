using Cactus.Interfaces;
using Cactus.ViewModels;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Cactus
{
    /// <summary>
    /// This class is responsible for providing all of the dependencies for the application.
    /// </summary>
    public class DependencyContainer
    {
        private IWindsorContainer _container;

        public DependencyContainer()
        {
            _container = ConfigureServices();
        }

        private IWindsorContainer ConfigureServices()
        {
            var container = new WindsorContainer();
            container.Register(Component.For<IFileSwitcher>().ImplementedBy<FileSwitcher>());
            container.Register(Component.For<IEntryLoader>().ImplementedBy<EntryLoader>());
            container.Register(Component.For<IEntryManager>().ImplementedBy<EntryManager>());
            container.Register(Component.For<IProcessManager>().ImplementedBy<ProcessManager>());
            container.Register(Component.For<IVersionManager>().ImplementedBy<VersionManager>());
            container.Register(Component.For<ILogger>().ImplementedBy<Logger>());
            container.Register(Component.For<IRegistryService>().ImplementedBy<RegistryService>());
            container.Register(Component.For<IMainWindowViewModel>().ImplementedBy<MainWindowViewModel>());
            container.Register(Component.For<IPatchFileGenerator>().ImplementedBy<PatchFileGenerator>());
            return container;
        }

        public IMainWindowViewModel MainWindow
        {
            get
            {
                return _container.Resolve<IMainWindowViewModel>();
            }
        }
    }
}