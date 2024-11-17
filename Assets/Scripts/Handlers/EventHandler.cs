using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class EventHandler
{
    private static readonly Dictionary<string, Delegate> eventCache = new Dictionary<string, Delegate>();
    public static event Action PlayerAttackEvent;

    private static Delegate GetEventDelegate(string eventName)
    {
        // Check if the event is already in the cache
        if (eventCache.TryGetValue(eventName, out var cachedDelegate))
        {
            return cachedDelegate;
        }

        var eventField = typeof(EventHandler).GetField(eventName,
            BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

        if (eventField != null)
        {
            var eventDelegate = eventField.GetValue(null) as Delegate;
            if (eventDelegate != null)
            {
                eventCache[eventName] = eventDelegate;
                return eventDelegate;
            }
        }

        Debug.LogError($"Event {eventName} not found or could not be retrieved.");
        return null;
    }

    public static void CallEvent<T>(string eventName, T arg)
    {
        var eventDelegate = GetEventDelegate(eventName);

        if (eventDelegate is Action<T> action)
        {
            action.Invoke(arg);
        }
        else
        {
            Debug.LogError($"Event {eventName} does not match the expected type Action<{typeof(T).Name}>.");
        }
    }

    public static void CallEvent(string eventName)
    {
        var eventDelegate = GetEventDelegate(eventName);

        if (eventDelegate is Action action)
        {
            action.Invoke();
        }
        else
        {
            Debug.LogError($"Event {eventName} does not match the expected type Action.");
        }
    }
}
