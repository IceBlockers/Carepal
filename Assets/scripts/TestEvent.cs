using UnityEngine;
using System.Collections;

// script to test event manager working; not tied to useful functionality
public class TestEvent : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
	//	EventManager.StartListening("Timer", TimeFunction);
	//	TotallyUnrelatedFunction();
	}
	
	void OnDisable() {
	//	EventManager.StopListening("Timer", TimeFunction);
	}
	
	// Example of how a trigger for an event is decoupled from adding the functions to the event
	/*
	private float dt = 0;
	void Update () {
		dt += Time.deltaTime;
		if (dt > 5.0f) {
			EventManager.TriggerEvent("Timer");
			dt = 0.0f;
		}
	}
	*/
	
	void TimeFunction() {
		TestModalPanel tmp = FindObjectOfType(typeof (TestModalPanel)) as TestModalPanel;
		tmp.testPanel();
	}
	
	void TotallyUnrelatedFunction() {
		EventManager.StartListening("Timer", foobar);
	}
	
	void foobar() {
		Debug.Log("hi grayden");
	}
}
