using Common.Stylet;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Threading;

namespace DepthInCSharpDemo
{
    public class Bootstrapper: Bootstrapper<MainViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            builder.Bind<IPage>().ToAllImplementations(true, Array.Empty<Assembly>()).InSingletonScope();
            builder.Bind<IRouter>().And<Router>().To<Router>().InSingletonScope();
            base.ConfigureIoC(builder);
        }
        protected override void Configure()
        {
            base.Configure();
            IoC.GetInstance = new Func<Type, string, object>(base.Container.Get);
            IoC.GetAllInstances = new Func<Type, string, IEnumerable<object>>(base.Container.GetAll);
            IoC.BuildUp = new Action<object>(base.Container.BuildUp);
        }

        protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e)
        {
            // Called on Application.DispatcherUnhandledException
            base.OnUnhandledException(e);
        }
    }
}
