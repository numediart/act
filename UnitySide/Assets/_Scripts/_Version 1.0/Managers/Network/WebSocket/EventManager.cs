using System;
using System.Collections.Concurrent;

namespace _Scripts._Version_1._0.Managers.Network.WebSocket
{
    public class EventManager
    {
        private readonly ConcurrentDictionary<string, ConcurrentBag<Action<string>>> _eventListeners = new();
        private static EventManager _eventManager;
        public static EventManager Instance
        {
            get
            {
                if(_eventManager==null) _eventManager=new EventManager();
                return _eventManager;
            }
        }

        public void On(string eventName, Action<string> callback)
        {
            _eventListeners.GetOrAdd(eventName, _ => new ConcurrentBag<Action<string>>()).Add(callback);// add callback to the list of callbacks for the event thread safe
        }

        public void Emit(string eventName, Object? data)
        {
            if(data ==null) data = "";
            if (_eventListeners.TryGetValue(eventName, out var listeners))
                foreach (var listener in listeners)
                    listener.Invoke(data.ToString()!);
        }
        public void Off(string eventName, Action<string> callback)
        {
            if (_eventListeners.TryGetValue(eventName, out var listeners))
                listeners.TryTake(out callback);
        }
    }
}