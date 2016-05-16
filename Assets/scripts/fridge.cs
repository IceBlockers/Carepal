using UnityEngine;
using System.Collections;

public class fridge : MonoBehaviour {

    public GameObject circleFat;
    public GameObject circleVeggy;
    public GameObject circleMeat;

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
                circleFat.transform.position = new Vector3(
                    hitCollider.transform.position.x,
                    hitCollider.transform.position.y,
                    0);
                Debug.Log("Hit " + hitCollider.transform.name + " x" + hitCollider.transform.position.x + " y " + hitCollider.transform.position.y);
            }
        }
    }
}
