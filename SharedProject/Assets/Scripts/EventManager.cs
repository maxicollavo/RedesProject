using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance = new EventManager();

    Dictionary<GameEventTypes, EventHandler> events = new Dictionary<GameEventTypes, EventHandler>();

    private EventManager()
    {

    }

    public void Register(GameEventTypes eventName, EventHandler handler)
    {
        if (!events.ContainsKey(eventName))
        {
            events.Add(eventName, null);
        }
        events[eventName] += handler;
    }

    public void Unregister(GameEventTypes eventName, EventHandler handler)
    {
        events[eventName] -= handler;
    }

    public void Dispatch(GameEventTypes eventName, object sender, EventArgs args)
    {
        events[eventName].Invoke(sender, args);
    }
}