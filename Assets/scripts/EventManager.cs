using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/*
EventManager
Uses a string-based dictionary to tie callbacks to an event.
From anywhere in the program, one can call
	EventManager.StartListening("name_of_event", CallbackFunction);
This registers the function to that name.
The point of this is to decouple WHEN an event is called from WHICH functions the event is tied to.
To actually call an event, use
	EventManager.TriggerEvent("name_of_event");
	
An example use case:
Whenever the character enters a new room (say a kitchen), call the KitchenPrompt event.
This event will direct the player to an item of interest.
This item could change in response to different actions.
For example, the first time they enter the room they are told to go to the fridge.
After finishing the fridge activity, the next time they enter the kitchen they will be prompted to go to the oven.
The event hasn't changed (KitchenPrompt), but the function tied to the event has.
The part of code responsible for triggering KitchenPrompt is completely agnostic of what function is actually being triggered.

Note that a single event can call multiple functions. Some queue handling should be implemented.
*/

public sealed class EventManager : MonoBehaviour {

	private Dictionary<string, UnityEvent> dictionary;
	
	// implement the singleton pattern
	private static EventManager em;
	
	public static EventManager instance {
		get {
			if (!em) {
				em = FindObjectOfType(typeof(EventManager)) as EventManager;
				if (!em) {
					Debug.Log("Could not find an EventManager object");
				}
				else {
					em.Init();
				}
			}
			return em;
		}
	}
	
	private void Init() {
		if (dictionary == null) {
			dictionary = new Dictionary<string, UnityEvent>();
		}
	}
	
	// register an event
	public static void StartListening(string eventName, UnityAction listener) {
		UnityEvent thisEvent = null;
		
		if (instance.dictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.AddListener(listener);
		} else {
			thisEvent = new UnityEvent();
			thisEvent.AddListener(listener);
			instance.dictionary.Add(eventName, thisEvent);
		}
	}
	
	// remove a function from an event
	public static void StopListening(string eventName, UnityAction listener) {
		if (instance.dictionary == null) {
			return;
		}
		UnityEvent thisEvent = null;
		if (instance.dictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.RemoveListener(listener);
		}
	}
	
	// call an event
	public static void TriggerEvent(string eventName) {
		UnityEvent thisEvent = null;
		if (instance.dictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.Invoke();
		}
	}
		
}
