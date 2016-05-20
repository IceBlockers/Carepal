using UnityEngine;
using UnityEngine.Events;

class HungerBar : MonoBehaviour {
	 
	public float barDisplay;
	public Vector2 pos;
	public Vector2 size;
	public Texture2D progressBarEmpty;
	public Texture2D progressBarFull;
	public Texture2D[] hungerPic;
	 
	private const float hungerTimerThreshold = 5.0f; // how frequently the hunger bar should decrease in seconds
	private const float maxHunger = 8.0f; 	// maximum value for hunger
	private float curHunger = 8.0f;
	private float hungerTimer = 0.0f;
    public UnityEvent HungryEvent;
	 
	 private void Start() {
		 // preserve bar between scenes
		 DontDestroyOnLoad(this);
		 
		 // prevent duplicates when reloading scene where bar is created
		 if (FindObjectsOfType(GetType()).Length > 1) {
			 Destroy(gameObject);
		 }
	 }
	 
	 // draw the hunger meter
	 private void OnGUI()
	 {	 
		int hunger = (int) PlayerPrefs.GetFloat("Hunger");
		Rect sandwichPos = new Rect(0, 0, size.x, size.y);
		GUI.BeginGroup(sandwichPos);
			GUI.Box(sandwichPos, hungerPic[hunger]);
		GUI.EndGroup();	 
	 } 
	 
	 private void Update()
	 {
        //read hunger/fullness level from PlayerPrefs
        barDisplay = PlayerPrefs.GetFloat("Hunger") / maxHunger;
	 }
 }