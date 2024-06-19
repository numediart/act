using System;
using System.Collections.Concurrent;


public class EventManagerService
{
    private readonly ConcurrentDictionary<string, ConcurrentBag<Action<string, string>>> _eventListeners = new();
    private static EventManagerService _eventManager;
    public static EventManagerService Instance
    {
        get => _eventManager;
    }

    public void On(string eventName, Action<string, string> callback)
    {
        _eventListeners.GetOrAdd(eventName, _ => new ConcurrentBag<Action<string, string>>()).Add(callback);// add callback to the list of callbacks for the event thread safe
    }
    public void Once(string eventName, Action<string, string> callback)
    {
        Action<string, string> callbackOnce = null;
        callbackOnce = (data, clientId) =>
        {
            callback(data, clientId);
            _eventListeners[eventName].TryTake(out callbackOnce);
        };
        On(eventName, callbackOnce);
    }

    public void Emit(string eventName, Object? data, string clientId)
    {
        if(data ==null) data = "";
        if (_eventListeners.TryGetValue(eventName, out var listeners))
            foreach (var listener in listeners)
                listener.Invoke(data.ToString()!, clientId);
    }
}