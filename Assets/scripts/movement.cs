using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class movement : MonoBehaviour {

    public Text displayPos;
    new public Camera camera;
    public GameObject pal;
    public Vector3 clickPos;
    public Vector2 clickVec;
    public double delta = 0.1;
    private Animator m_Anim;            // Reference to the player's animator component.

    private void Awake() {
        m_Anim = pal.GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
        clickPos = new Vector3(0, 0, 0);
    }

    void facingLeft(bool left) {
        if (left) {
            pal.transform.localScale = new Vector3(
                -System.Math.Abs(pal.transform.localScale.x), 
                pal.transform.localScale.y, 
                pal.transform.localScale.z);
        } else {
            pal.transform.localScale = new Vector3(
                System.Math.Abs(pal.transform.localScale.x), 
                pal.transform.localScale.y, 
                pal.transform.localScale.z);
        }
    }


	// Update is called once per frame
	void Update () {
        // detect left mouse click
        if(Input.GetMouseButtonDown(0) == true) {
            // transform screen coords to world coords
            clickPos = camera.ScreenToWorldPoint(Input.mousePosition);

            // get vector of direction of click from pal position
            clickVec.x = clickPos.x - pal.transform.position.x;
            clickVec.y = clickPos.y - pal.transform.position.y;
            clickVec.Normalize();
        }

        // if the click position is more than delta away from the pal
        if ((Mathf.Abs(clickPos.y - pal.transform.position.y) > delta) || (Mathf.Abs(clickPos.x - pal.transform.position.x) > delta)) {
            if (clickVec.x < 0) {
                facingLeft(true);
            } else {
                facingLeft(false);
            }
            // set pal position to click position
            pal.transform.Translate( Time.deltaTime * clickVec.x * 10, Time.deltaTime * clickVec.y * 10, 0);
            m_Anim.SetFloat("vSpeed", clickVec.magnitude);
        } else {
            m_Anim.SetFloat("vSpeed", 0);
        }
           
        // display the pal's position vector
        displayPos.text = pal.transform.position.x + ", " + pal.transform.position.y + ", " + pal.transform.position.z;
    }
}
