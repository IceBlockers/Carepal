using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodController : MonoBehaviour {
    public float speed = 5;
    
    private Vector3 target;
    private Vector3 origin;
    private Vector3 heading;
    private float xScale;
    private float yScale;

    // ref to sandwich-central
    private SandwichController sammy;

    // Use this for initialization
    void Start () {
        sammy = GameObject.Find("/Sandwich").GetComponent<SandwichController>();

        target = sammy.getTop(transform);
        origin = transform.position;
        origin.z = target.z;
        GetComponent<SpriteRenderer>().sortingOrder = (int)target.z;
        
        // Final scale values (set in Unity)
        xScale = transform.localScale.x;
        yScale = transform.localScale.y;

        if (!CompareTag("bread"))
        {
            transform.localScale = new Vector3(0, 0, 1);
        }
    }
	
	// Update is called once per frame
	void Update () {
        heading = target - transform.position;
        heading.z = 0;

        // Progression from origin to target as a float 0.0 -> 1.0
        float progress = 1 - heading.magnitude / (target - origin).magnitude;
        // Bread uses this same script, but doesn't scale as it moves
        if (!CompareTag("bread"))
        {
        transform.localScale = new Vector3(progress * xScale, progress * yScale, 1);
        }
        if (heading.magnitude > 0.05)
        {
            heading.Normalize();

            Vector3 speedVector = heading * speed + transform.position;
            transform.position = Vector3.Lerp(transform.position, speedVector, Time.deltaTime);
        }
        else
        {
            Destroy(GetComponent<FoodController>());
        }
    }
}
