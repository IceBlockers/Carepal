using UnityEngine;
using System.Collections;

public class OliveController : MonoBehaviour {
    public float startY;
    public float oliveSpeed;        // Initial speed when "thrown"

    private bool active = false;
    public Object olivePrefab;
    private GameObject olive;       // The instance
    private float yCutoff = -5f;
    private SandwichController sammy;

	// Use this for initializationz
	void Start () {
        sammy = GameObject.Find("/Sandwich").GetComponent<SandwichController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1") && !active && sammy.openFaced())
        {
            active = true;
            Vector3 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 olivePosition = new Vector3(0, startY, transform.position.z);
            Vector3 oliveHeading = clickPoint - olivePosition;
            oliveHeading.Normalize();
            oliveHeading *= oliveSpeed;
            // GetComponent<Rigidbody2D>().velocity = new Vector2();
            olive = (GameObject)Instantiate(olivePrefab, olivePosition, Quaternion.identity);
            olive.GetComponent<Rigidbody2D>().AddForce(oliveHeading, ForceMode2D.Impulse);
        }

        if (olive != null && olive.transform.position.y < yCutoff)
        {
            active = false;
            Destroy(olive);
            olive = null;

            sammy.replenishFood();
        }
    }

}
