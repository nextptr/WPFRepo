﻿using CVbox.Common;
using CVbox.Common.Attach;
using CVbox.Panel.Home;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using CVWrapper;

namespace CVbox
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
            IoC.GetInstance = new Func<Type, string, object>(base.Container.Get);
            IoC.GetAllInstances = new Func<Type, string, IEnumerable<object>>(base.Container.GetAll);
            IoC.BuildUp = new Action<object>(base.Container.BuildUp);
            CVWrapper.CVproxy.CreateInstance();
            //CVAlgorithm.CVProxy.CreateInstance();
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
