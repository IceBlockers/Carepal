using UnityEngine;

class HungerBar : MonoBehaviour {
	 
	public float barDisplay;
	public Vector2 pos;
	public Vector2 size;
	public Texture2D progressBarEmpty;
	public Texture2D progressBarFull;
	 
	private const float hungerTimerThreshold = 5.0f; // how frequently the hunger bar should decrease in seconds
	private const float maxHunger = 5.0f; 	// maximum value for hunger
	private float curHunger = 5.0f;
	private float hungerTimer = 0.0f;
	 
	 private void Start() {
		 // preserve bar between scenes
		 DontDestroyOnLoad(this);
		 
		 // prevent duplicates when reloading scene where bar is created
		 if (FindObjectsOfType(GetType()).Length > 1) {
			 Destroy(gameObject);
		 }
	 }
	 
	 private void OnGUI()
	 {	 
		 // draw the background:
		 GUI.BeginGroup (new Rect (pos.x, pos.y, size.x, size.y));
			 GUI.Box (new Rect (0,0, size.x, size.y),progressBarEmpty);
	 
			 // draw the filled-in part:
			 GUI.BeginGroup (new Rect (0, 0, size.x * barDisplay, size.y));
				 GUI.Box (new Rect (0,0, size.x, size.y),progressBarFull);
			 GUI.EndGroup ();
	 
		 GUI.EndGroup ();
	 
	 } 
	 
	 private void Update()
	 {
		 // handle logic for decreaing hunger
		if (curHunger > 0 && (hungerTimer += Time.deltaTime) >= hungerTimerThreshold) {
			curHunger = (curHunger == 0 ? 0 : curHunger -= 1.0f);
			barDisplay = curHunger / maxHunger;
			hungerTimer = 0.0f;
		}
	 }
 }