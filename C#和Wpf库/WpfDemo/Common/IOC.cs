using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public static class IOC
    {
        public static T Get<T>(string key = null)
        {
            return (T)((object)IOC.GetInstance(typeof(T), key));
        }

        public static IEnumerable<T> GetAll<T>(string str)
        {
            return IOC.GetAllInstances(typeof(T), str).Cast<T>();
        }

        public static Func<Type, string, object> GetInstance = delegate (Type service, string key)
        {
            throw new InvalidOperationException("IoC is not initialized");
        };

        public static Func<Type, string, IEnumerable<object>> GetAllInstances = delegate (Type service, string str)
        {
            throw new InvalidOperationException("IoC is not initialized");
        };

        public static Action<object> BuildUp = delegate (object instance)
        {
            throw new InvalidOperationException("IoC is not initialized");
        };
    }
}
