using Common;
using Common.Attach;
using Common.Interface;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using WpfDemo.Home;
using WpfDemo.Logic;

namespace WpfDemo
{
    public class Bootstrapper : Bootstrapper<MainViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            builder.Bind<IPage>().ToAllImplementations(true, Array.Empty<Assembly>()).InSingletonScope();
            builder.Bind<IRouter>().And<Router>().To<Router>().InSingletonScope();
            InitAttribute(builder);
            base.ConfigureIoC(builder);
        }
        protected override void Configure()
        {
            base.Configure();
            IOC.GetInstance = new Func<Type, string, object>(base.Container.Get);
            IOC.GetAllInstances = new Func<Type, string, IEnumerable<object>>(base.Container.GetAll);
            IOC.BuildUp = new Action<object>(base.Container.BuildUp);

            string fullPath = Directory.GetCurrentDirectory();
        }
        public void InitAttribute(IStyletIoCBuilder builder)
        {
            Assembly ab = Assembly.GetExecutingAssembly();
            var tps = ab.DefinedTypes;
            foreach (var item in tps)
            {
                if (item.GetCustomAttribute(typeof(Singleton)) != null)
                {
                    builder.Bind(item).ToSelf().InSingletonScope();
                }
            }
        }
        protected override void OnLaunch()
        {
            base.OnLaunch();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
        protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e)
        {
            // Called on Application.DispatcherUnhandledException
            base.OnUnhandledException(e);
        }
    }
}
