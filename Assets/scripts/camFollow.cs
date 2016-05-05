using UnityEngine;
using System.Collections;

public class camFollow : MonoBehaviour {

    public GameObject tracking;
    public float minX = -0.6f;
    public float maxX =  2.8f;
    public float camSpeed = 3.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = gameObject.transform.position;
        //The x position of the character, but clamped 
        //  so that we don't see past the end of the background sprite.
        float dest = Mathf.Clamp(tracking.transform.position.x, minX, maxX);

        //Distance to travel from current location to the position we want to see.
        float diff = dest - gameObject.transform.position.x;

        //A calculate a small time-based fraction of the distance to travel, 
        //  to slow down the motion and make it smoother.
        float factor = Time.deltaTime * camSpeed;
        factor = Mathf.Clamp(factor, 0.000001f, 1.0f); //prevent explosions
        float partialMovement = diff * factor;

        //the calculated position to put the camera
        pos.x = gameObject.transform.position.x + partialMovement;
        gameObject.transform.position = pos;
	}
}
