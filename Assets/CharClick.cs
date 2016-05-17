using UnityEngine;
using System.Collections;

public class CharClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown() {
		Debug.Log("OnMouseDown");
		RewardManager.Instance().AddReward("OnMouseDownReward");
	}
}
