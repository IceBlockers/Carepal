using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class movement : MonoBehaviour {

    public Text displayPos;
    new public Camera camera;
    public GameObject pal;
    public Vector3 clickPos;
    public Vector2 clickVec;
    public double delta = 0.1;
    Queue<Vector3> nodeQueue = new Queue<Vector3>();

    // Use this for initialization
    void Start () {
        clickPos = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        // detect left mouse click
        if(Input.GetMouseButtonDown(0) == true) {
            // transform screen coords to world coords
            clickPos = camera.ScreenToWorldPoint(Input.mousePosition);

            // prevent pal from clipping edge of camera
            //if (clickPos.x > 8) clickPos.x = 8;
            //if (clickPos.x < -8) clickPos.x = -8;
            //if (clickPos.y > 3) clickPos.y = 3;
            //if (clickPos.y < -3) clickPos.y = -3;

            // get vector of direction of click from pal position
            clickVec.x = clickPos.x - pal.transform.position.x;
            clickVec.y = clickPos.y - pal.transform.position.y;
            clickVec.Normalize();

            // when we have nodes, add the click position to the queue
            nodeQueue.Enqueue(clickPos);
        }

        // if the click position is more than delta away from the pal
        if ((Mathf.Abs(clickPos.y - pal.transform.position.y) > delta) || (Mathf.Abs(clickPos.x - pal.transform.position.x) > delta)) {
            // set pal position to click position
            pal.transform.Translate( Time.deltaTime * clickVec.x * 5, Time.deltaTime * clickVec.y * 5, 0);
        }
           
        // display the pal's position vector
        displayPos.text = pal.transform.position.x + ", " + pal.transform.position.y + ", " + pal.transform.position.z;
    }
}
