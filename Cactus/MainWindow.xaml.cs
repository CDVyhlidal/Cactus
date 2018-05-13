// Copyright (C) 2018 Jonathan Vasquez <jon@xyinn.org>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Cactus.Interfaces;
using Cactus.ViewModels;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System.Windows;

namespace Cactus
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Configure Dependency Injection Container
            IWindsorContainer container = ConfigureServices();

            var viewModel = container.Resolve<IMainWindowViewModel>();
            DataContext = viewModel;
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
    }
}
