using UnityEngine;
using System.Collections;

public class FoodCollider : MonoBehaviour {
    public GameObject foodObject;
    private SandwichController sammy;

	// Use this for initialization
	void Start () {
        sammy = GameObject.Find("/Sandwich").GetComponent<SandwichController>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        // foodObject.GetComponent<FoodController>().foodLaunch(transform.position);
        if (other.CompareTag("olive"))
        {
            if (sammy.openFaced())
            {
                if (CompareTag("bread"))
                {
                    sammy.dropBread();
                    Destroy(gameObject);

                }
                else
                {
                    Instantiate(foodObject, transform.position, Quaternion.identity);
                    sammy.unSpawnFood(gameObject);
                    Destroy(gameObject);
                }
            }
        }
    }
}
