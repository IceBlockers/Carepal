using UnityEngine;
using System.Collections;

public class TestEvent : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
		EventManager.StartListening("Timer", TimeFunction);
	}
	
	void OnDisable() {
		EventManager.StopListening("Timer", TimeFunction);
	}
	
	// Update is called once per frame
	private float dt = 0;
	void Update () {
		dt += Time.deltaTime;
		if (dt > 5.0f) {
			EventManager.TriggerEvent("Timer");
			dt = 0.0f;
		}
	}
	
	void TimeFunction() {
		TestModalPanel tmp = FindObjectOfType(typeof (TestModalPanel)) as TestModalPanel;
		tmp.testPanel();
	}
}
