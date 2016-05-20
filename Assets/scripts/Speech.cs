using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Speech : MonoBehaviour {
	
	public string[] Lines;
	public Text Line;
	private int cur;
	public GameObject tracking;
    public float camSpeed = 3.0f;

	// Use this for initialization
	void Start () {
		cur = 0;
		Line.text = Lines[cur];
	}
	
	// Update is called once per frame
	void Update () {
		if (tracking) {
			 Vector3 pos = gameObject.transform.position;
			//The x position of the character, but clamped 
			//  so that we don't see past the end of the background sprite.
			float dest = tracking.transform.position.x;
			float desty = tracking.transform.position.y + 4;


			//Distance to travel from current location to the position we want to see.
			float diff = dest - gameObject.transform.position.x;
			float diffy = desty - gameObject.transform.position.y;

			//A calculate a small time-based fraction of the distance to travel, 
			//  to slow down the motion and make it smoother.
			float factor = Time.deltaTime * camSpeed;
			factor = Mathf.Clamp(factor, 0.000001f, 1.0f); //prevent explosions
			float partialMovement = diff * factor;
			float partialMovementy = diffy * factor;

			//the calculated position to put the camera
			pos.x += partialMovement;
			pos.y += partialMovementy;
			gameObject.transform.position = pos;	
		}
	}
	
	public void NextLine () {
		if (cur < Lines.Length-1) {
			Line.text = Lines[++cur];
		} else {
			// close panel
			this.transform.gameObject.SetActive(false);
		}
	}
	
	public void PrevLine() {
		if (cur >= 0) {
			Line.text = Lines[--cur];
		}
	}
}
