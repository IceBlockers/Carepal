using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Textdbg : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButton("Fire1")) {
            Text text = gameObject.GetComponent<Text>();
            Vector3 mouse = Input.mousePosition;
            Vector2 pos = new Vector2(mouse.x, mouse.y);
            Vector3 wmouse = Camera.main.ScreenToWorldPoint(mouse);
            Vector2 wpos = new Vector2(wmouse.x, wmouse.y);

            PlayerPrefs.SetFloat("Timer", Time.deltaTime);

            text.text = "Fire! " + pos;
            text.text += "\nWorld: " + wpos;
            text.text += "\nTutorial: " + PlayerPrefs.GetInt("Tutorial");
            text.text += "\nHunger: " + PlayerPrefs.GetInt("Hunger");
            text.text += "\nTime: " + PlayerPrefs.GetFloat("Timer");
        }
	}
}
