﻿// Copyright (C) 2018 Jonathan Vasquez <jon@xyinn.org>
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cactus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Configure Dependency Injection Container
            IWindsorContainer container = ConfigureServices();
           // var fs = container.Resolve<IFileSwitcher>();

            var viewModel = container.Resolve<IMainWindowViewModel>();

            // Go
            //fs.Run();
        }

        private IWindsorContainer ConfigureServices()
        {
            var container = new WindsorContainer();
            container.Register(Component.For<IFileSwitcher>().ImplementedBy<FileSwitcher>());
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
