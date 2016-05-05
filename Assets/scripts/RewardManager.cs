using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class RewardManager : MonoBehaviour {
		
		
	private static RewardManager rm; // singleton object
	// public Profile profile;

	// Load profile data from source (XML/txt file)
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddReward(string Reward) {
		// TODO: add a reward to a profile
		Debug.Log(Reward);
	}
	
	public static RewardManager Instance() {
		if (!rm) {
			rm = FindObjectOfType(typeof (RewardManager)) as RewardManager;
			if (!rm) {
				Debug.Log("Reward Manager not found");
			}
		}
		return rm;
	}
}
