using UnityEngine;
using System.Collections;
using System;

public class OliveController : MonoBehaviour {
    public float startY = 8;
    public float oliveSpeed;        // Initial speed when "thrown"
    public float oliveYdiff = 2f;   // Units olive starts above final position

    private bool active = false;
    public UnityEngine.Object olivePrefab;
    private GameObject olive;       // The instance
    private float yCutoff = -5f;
    private SandwichController sammy;
    private Vector3 olivePosition;
    private Rigidbody2D rOlive;
    private int oliveRot = 0;
    private Vector3 oliveScale = new Vector3();
    private Vector3 oliveScaleFinal;

	// Use this for initializationz
	void Start () {
        sammy = GameObject.Find("/Sandwich").GetComponent<SandwichController>();
        olivePosition = new Vector3(0, startY + oliveYdiff, transform.position.z);
        olive = (GameObject)Instantiate(olivePrefab, olivePosition, Quaternion.identity);
        rOlive = olive.GetComponent<Rigidbody2D>();
        oliveScaleFinal = olive.transform.localScale;
        olive.transform.localScale = new Vector3();

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButton("Fire1") && !active && sammy.openFaced() && Math.Abs(olive.transform.localScale.magnitude - oliveScaleFinal.magnitude) < 0.01f)
        {
            active = true;
            Vector3 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 oliveHeading = clickPoint - new Vector3(olivePosition.x, olivePosition.y - oliveYdiff, olivePosition.z);

            oliveHeading.Normalize();
            oliveHeading *= oliveSpeed;

            rOlive.isKinematic = false;
            rOlive.AddForce(oliveHeading, ForceMode2D.Impulse);
        }

        if (olive.transform.position.y < yCutoff)
        {
            active = false;
            // Destroy(gameObject);
            // olive = null;
            // Instantiate(olivePrefab, olivePosition, Quaternion.identity);
            rOlive.isKinematic = true;
            olive.transform.position = olivePosition;
            rOlive.velocity = new Vector2();
            rOlive.angularVelocity = 0;
            rOlive.MoveRotation(0);
            olive.transform.localScale = new Vector3();

            sammy.replenishFood();
        }

        if (active == false)
        {
            if (olive.transform.localScale.magnitude < oliveScaleFinal.magnitude)
            {
                olive.transform.localScale += oliveScaleFinal * Time.deltaTime;
                olive.transform.position = new Vector3(0, olive.transform.position.y - oliveYdiff * Time.deltaTime);
            } else {
                olive.transform.localScale = oliveScaleFinal;
            }
            rOlive.MoveRotation(++oliveRot);
        }
    }

}
