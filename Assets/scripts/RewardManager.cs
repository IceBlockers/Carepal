using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class RewardManager : MonoBehaviour {
		
		
	private static RewardManager rm; // singleton object
	// public Profile profile;
	/*
	EXAMPLE OF INSTANTIATING A PREFAB
	*/
	public GameObject dialog;

	// Load profile data from source (XML/txt file)
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddReward(string Reward) {
		// TODO: add a reward to a profile
		Debug.Log(Reward);
		if (String.Compare(Reward, "yreward") == 0) {
			GameObject dialogClone = (GameObject) Instantiate(dialog, new Vector3(0,0,0), Quaternion.identity);
			dialogClone.SetActive(true);
		}
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
