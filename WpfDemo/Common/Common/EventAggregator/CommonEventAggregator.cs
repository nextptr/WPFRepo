using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.EventAggregator
{
    public interface IEventArgs { }

    public interface IEventHandler<in TEvent> where TEvent : class, IEventArgs
    {
        void Handler(TEvent @event);

    }

    public interface IEventAggregator
    {
        void Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : class, IEventArgs;
        void Publish<TEvent>(TEvent @event) where TEvent : class, IEventArgs;
    }

    public class CommonEventAggregator : IEventAggregator
    {
        private static readonly Dictionary<Type, List<object>> _eventHandlers = new Dictionary<Type, List<object>>();
        public void Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : class, IEventArgs
        {
            var eventType = typeof(TEvent);
            if (_eventHandlers.ContainsKey(eventType))
            {
                var handlers = _eventHandlers[eventType];
                if (handlers != null)
                {
                    handlers.Add(handler);
                }
                else
                {
                    handlers = new List<object> { handler };
                }
            }
            else
            {
                _eventHandlers.Add(eventType, new List<object> { handler });
            }
        }
        public void Publish<TEvent>(TEvent @event) where TEvent : class, IEventArgs
        {
            var eventType = @event.GetType();
            if (_eventHandlers.ContainsKey(eventType)
                && _eventHandlers[eventType] != null
                && _eventHandlers[eventType].Count > 0)
            {
                var handlers = _eventHandlers[eventType];
                foreach (var handler in handlers)
                {
                    var eventHandler = handler as IEventHandler<TEvent>;
                    eventHandler?.Handler(@event);
                }
            }
        }



        private CommonEventAggregator() { }

        //线程安全的单例类 1
        /*
        private static readonly object lockObj = new object();
        private static CMEventAggregator _instance = null;
        public static CMEventAggregator Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance=new CMEventAggregator();
                        }
                    }
                }
                return _instance;
            }
        }
        */
        //完全懒惰的单例类 1
        public static CommonEventAggregator Instance
        {
            get
            {
                return Nested._instance;
            }
        }
        private class Nested
        {
            static Nested()
            { }

            internal static readonly CommonEventAggregator _instance = new CommonEventAggregator();

        }
    }
}
