using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SandwichController : MonoBehaviour {
    public List<Object> foodIcons;
    public float spacing;               // Spacing between ingredients
    public float breadSpacing;          // Spacing between bottom bread & first ingredient
    public float topBreadSpacing;       // Spacing between top bread and top ingredient
    public int maxSandwichSize;
    public int sandwichThreshold;   // No of ingredients before bread appears and sandwich can be completed
    public GameObject topBread;
    public GameObject breadIcon;
    public int spawnMax;          // Max # of ingreds to simultaneously display
    public float topBreadDelay;

    public List<Transform> ingredients;
    private float breadDelay = 0.5f;
    private Vector3 breadPos;
    private int zIndex = 0;
    private GameObject[] spawnPoints;
    private Object[] spawnIndices;
    private int spawnCount = 0;
    private bool bigEnough = false;     // Does sandwich have enough ingredients to be completed?
    private bool breadSpawned = false;  // Has bread icon been spawned? (only happens once)
    private bool sandwichComplete = false;      // Nothing more can be added to sandwich once complete
    private GameObject salt;
    // private object topLock = new Object();
    private int[] passedIngreds = new int[3];

    // Use this for initialization
    void Start () {
        ingredients.Add(GameObject.Find("/bread").transform);
        breadPos = ingredients[0].position;
        breadPos.y = 6;

        passedIngreds[0] = PlayerPrefs.GetInt("food1");
        passedIngreds[1] = PlayerPrefs.GetInt("food2");
        passedIngreds[2] = PlayerPrefs.GetInt("food3");

        spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        Debug.Log("Spawn Points: " + spawnPoints.Length);
        spawnIndices = new Object[spawnPoints.Length];

        salt = GameObject.Find("salt-pivot");

        // Load food icon prefabs
        /*
        Resources.Load("Sprites/cheese2");
        Resources.Load("Sprites/lettuce1(squished)");
        Resources.Load("Sprites/deli-meat");
        
        foodIcons = new List<Object>();
        foodIcons.Add(Resources.Load("cheese2", typeof(GameObject)));
        Debug.Log("foodIcon 0: " + (foodIcons[0] == null ? "NULL" : "NOT-NULL"));
        foodIcons.Add(Resources.Load("/Sprites/lettuce1(squished)"));
        foodIcons.Add(Resources.Load("/Sprites/deli-meat"));

        // foodIcons = GameObject.FindGameObjectsWithTag("foodIcon").ToList<GameObject>();       // Doesn't work on pre-fabs!
        // foodIcons = Resources.FindObjectsOfTypeAll(typeof(GameObject)).Cast<GameObject>().Where(g => g.tag == "foodIcon").ToList();
        Debug.Log("foodIcons length: " + foodIcons.Count);
        */

        for (int i = 0; i < spawnMax; ++i)
        {
            spawnFood();
        }

    }

    void spawnFood ()
    {
        while (true)
        {
            int spawn = Random.Range(0, spawnPoints.Length);
            if (spawnIndices[spawn] == null)
            {
                // Debug.Log("spawnFood() attempted!");
                ++spawnCount;
                if (bigEnough && !breadSpawned)     // Bread icon needs to be spawned before anything else
                {
                    spawnIndices[spawn] = Instantiate(breadIcon, spawnPoints[spawn].transform.position, Quaternion.identity);
                    breadSpawned = true;
                }
                else
                {
                int foodNo = Random.Range(0, 3);
                spawnIndices[spawn] = Instantiate(foodIcons[passedIngreds[foodNo]], spawnPoints[spawn].transform.position, Quaternion.identity);
                }
            break;              
            }
        }
    }

    public void unSpawnFood(Object foodIcon)
    {
        for (int i = 0; i < spawnIndices.Length; ++i)
        {
            if (foodIcon == spawnIndices[i])
            {
                spawnIndices[i] = null;
                spawnCount--;

                break;
            }

        }
    }

    public void replenishFood()
    {
        // Debug.Log("ReplenishFood() called!");
        if (openFaced()) {
            // Debug.Log("ReplenishFood() - openFaced TRUE");
            while (spawnCount < spawnMax) {
                // Debug.Log("ReplenishFood() - spawnFood() called");
                spawnFood();
            }
        }
    }

    public bool openFaced()
    {
        // return ingredients.Count - 1 < maxSandwichSize;
        return !sandwichComplete;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    // Returns "open" position at top of sandwich, adds new ingredient to list
    public Vector3 getTop(Transform newIngredient)
    {
        // lock(topLock) {

        Vector3 result = ingredients[0].position;
        result.y += (ingredients.Count - 1) * spacing + breadSpacing;
        result.z = zIndex++;

        // If new ingredient is the final piece of bread, add extra space for fine-tuning
        if (newIngredient.CompareTag("bread"))
        {
            result.y += topBreadSpacing;
            Debug.Log("Ingredient " + (ingredients.Count - 1) + " is bread!");
        }

        ingredients.Add(newIngredient);

        if (ingredients.Count - 1 == sandwichThreshold)     // Is sandwich big enough to complete?
        {
            bigEnough = true;
        }

        // Drop bread when max ingredients met (regardless if bread is hit)
            if (ingredients.Count - 1 == maxSandwichSize && 
            !newIngredient.CompareTag("bread") && !newIngredient.CompareTag("salt"))
        {
            // Debug.Log("Ingredient " + (ingredients.Count - 1) + " is bread? " + newIngredient.CompareTag("bread"));
            Debug.Log("Ingredient " + (ingredients.Count - 1) + " tag: " + newIngredient.tag);

            Invoke("dropBread", breadDelay);
            // bigEnough = true;
        }


        return result;
        // }
    }

    public void dropBread()
    {
        salt.GetComponent<Animator>().SetTrigger("SaltyTime");
        sandwichComplete = true;
        Invoke("actuallyDropBread", topBreadDelay);
    }

    private void actuallyDropBread()
    {
        Instantiate(topBread, breadPos, Quaternion.identity);

        
        Invoke("exitRoom", 5);
    }

    private void exitRoom() {
        PlayerPrefs.SetInt("SandwichMade", 1);
        PlayerPrefs.SetInt("Tutorial", 0);
        SceneManager.LoadScene("LevelKitchen");
    }
}
