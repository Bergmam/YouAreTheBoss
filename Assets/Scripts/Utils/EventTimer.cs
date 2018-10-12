using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTimer : MonoBehaviour
{
    float timer = 0.0f;
    bool timerOn = false;

    EventTrigger trigger;
    void Awake () {
        trigger = gameObject.GetComponent<EventTrigger>();
    }
    public void AddTimedTrigger(Action action) {
        trigger.triggers.Clear();
        AddDownEvent(trigger);
        AddUpEvent(action, trigger);
    }

    void AddDownEvent(EventTrigger trigger) {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => {StartTimer();});
        trigger.triggers.Add(entry);
    }

    void AddUpEvent(Action action, EventTrigger trigger) {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => {EndTimer(action);});
        trigger.triggers.Add(entry);
    }

    void StartTimer() {
        timer = 0.0f;
        timerOn = true;
    }

    void EndTimer(Action action) {
        timerOn = false;
        if (timer <= 0.25f) {
            action();
        }  
    }

    void Update() {
        if (timerOn) {
            timer += Time.deltaTime;
        }
    }
}