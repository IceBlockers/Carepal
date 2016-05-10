using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

	private Dictionary<string, UnityEvent> dictionary;
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
	
	public static void StopListening(string eventName, UnityAction listener) {
		if (instance.dictionary == null) {
			return;
		}
		UnityEvent thisEvent = null;
		if (instance.dictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.RemoveListener(listener);
		}
	}
	
	public static void TriggerEvent(string eventName) {
		UnityEvent thisEvent = null;
		if (instance.dictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.Invoke();
		}
	}
		
}
