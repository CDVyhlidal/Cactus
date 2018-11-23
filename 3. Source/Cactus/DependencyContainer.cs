// Copyright (C) 2018 Jonathan Vasquez <jon@xyinn.org>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see<https://www.gnu.org/licenses/>.

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
            container.Register(Component.For<IEntryManager>().ImplementedBy<EntryManager>());
            container.Register(Component.For<IProcessManager>().ImplementedBy<ProcessManager>());
            container.Register(Component.For<ILogger>().ImplementedBy<Logger>());
            container.Register(Component.For<IRegistryService>().ImplementedBy<RegistryService>());
            container.Register(Component.For<IMainWindowViewModel>().ImplementedBy<MainWindowViewModel>());
            container.Register(Component.For<IEditWindowViewModel>().ImplementedBy<EditWindowViewModel>());
            container.Register(Component.For<IAddWindowViewModel>().ImplementedBy<AddWindowViewModel>());
            container.Register(Component.For<IFileGenerator>().ImplementedBy<FileGenerator>());
            container.Register(Component.For<IPathBuilder>().ImplementedBy<PathBuilder>());
            container.Register(Component.For<IJsonManager>().ImplementedBy<JsonManager>());
            return container;
        }

        public IMainWindowViewModel MainWindow
        {
            get
            {
                return _container.Resolve<IMainWindowViewModel>();
            }
        }

        public IEditWindowViewModel EditWindow
        {
            get
            {
                return _container.Resolve<IEditWindowViewModel>();
            }
        }

        public IAddWindowViewModel AddWindow
        {
            get
            {
                return _container.Resolve<IAddWindowViewModel>();
            }
        }
    }
}