using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVbox
{
    public static class IoC
    {
        public static T Get<T>(string key = null)
        {
            return (T)((object)IoC.GetInstance(typeof(T), key));
        }

        public static IEnumerable<T> GetAll<T>(string str)
        {
            return IoC.GetAllInstances(typeof(T), str).Cast<T>();
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
