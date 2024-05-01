﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManagers2 : MonoBehaviour
{

    private Dictionary<string, UnityEvent> eventDictionary;
    private List<UnityEvent> eventStack = new List<UnityEvent>();

    private static EventManagers2 eventManager;

    public static EventManagers2 instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManagers2)) as EventManagers2;

                if (!eventManager)
                {
                    //Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        if (instance == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance)
        {
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }
    }
    public static void AddEventNextFrame(UnityAction listener)
    {
        UnityEvent thisEvent = new UnityEvent();
        thisEvent.AddListener(listener);
        instance.eventStack.Add(thisEvent);
    }
    private void Update()
    {
        while (instance.eventStack.Count > 0)
        {
            UnityEvent thisEvent = instance.eventStack[0];
            thisEvent.Invoke();
            instance.eventStack.RemoveAt(0);
        }
    }
}
