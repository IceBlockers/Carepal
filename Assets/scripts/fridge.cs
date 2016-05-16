using UnityEngine;
using System.Collections;

public class fridge : MonoBehaviour {

    public GameObject circleFat;
    public GameObject circleVeggy;
    public GameObject circleMeat;
    public GameObject btnAdvance;

    private bool bChoseFat;
    private bool bChoseVeggy;
    private bool bChoseMeat;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mousePosition = new Vector2(worldPos.x, worldPos.y);
            var hitCollider = Physics2D.OverlapPoint(mousePosition);
            
            if (hitCollider) {
                if (hitCollider.tag.Contains("fat")) {
                    placeCircle(hitCollider, circleFat);
                    bChoseFat = true;
                } else if (hitCollider.tag.Contains("meat")) {
                    placeCircle(hitCollider, circleMeat);
                    bChoseMeat = true;
                } else if (hitCollider.tag.Contains("veggy")) {
                    placeCircle(hitCollider, circleVeggy);
                    bChoseVeggy = true;
                } else {
                    //hit the advance button.
                    Debug.Log("Moving on to the next scene!");
                }
                updateButton();
            }
        }
    }

    void placeCircle(Collider2D hitCollider, GameObject circle) {
        circle.transform.position = new Vector3(
            hitCollider.transform.position.x,
            hitCollider.transform.position.y,
            0);
    }

    void updateButton() {
        if (bChoseFat && bChoseMeat && bChoseVeggy) {
            Vector3 pos = btnAdvance.transform.position;
            pos.x = 6.621002f;
            pos.y = -3.56601f;
            btnAdvance.transform.position = pos;
        } else {
            Vector3 pos = btnAdvance.transform.position;
            pos.y = -1000;
            btnAdvance.transform.position = pos;
        }
    }
}
