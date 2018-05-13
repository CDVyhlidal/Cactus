using Cactus.Interfaces;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Cactus.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private IWindsorContainer _container;

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
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
            return container;
        }

        public IMainWindowViewModel MainWindow
        {
            get
            {
                return _container.Resolve<IMainWindowViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}