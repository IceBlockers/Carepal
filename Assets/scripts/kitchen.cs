using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
using UnityEngine.SceneManagement;

public class kitchen : MonoBehaviour {

    new public Camera camera;
    public GameObject pal;
    public GameObject kitchenSet;
    private kitchen_controller kitchenScript;
    private Animator m_Anim;

    public GameObject myCanvas;
    public Text bubbleText;
    public GameObject sandwich_icon;
    public bool displaySandwich = false;
    public GameObject sandwichToEat;
    public GameObject EnzymePill;
    public GameObject starsPrefab;
    public float bubbleLifeCount = 0f;

    LevelScene kitchenScene;

    private void Awake() {
        m_Anim = pal.GetComponent<Animator>();
        kitchenScript = kitchenSet.GetComponent<kitchen_controller>();
                
        // our initializations previously in start
        kitchenScene = new LevelScene(camera, pal, m_Anim);
        kitchenScene.movementNodes = new List<Node> {
            new Node(1, new Vector2(3.59f, -1.78f)), // doorway
            new Node(2, new Vector2(1.87f, -2.68f)), // middle of open kitchen area
            new Node(3, new Vector2(6.548397f, -3.559552f)), // fridge
            new Node(4, new Vector2(-0.9f, -2.68f)), // near orange counterspace
            new Node(5, new Vector2(-4.34f, -2.62f)), // dishes
            new Node(6, new Vector2(-8.67f, -2.72f)), // far blue counterspace
            new Node(7, new Vector2(6.0f,  -0.5f)), // hallway
        };

        kitchenScene.clickPos = new Vector3(0, 0, 0);
        createNodeMap();

        // define clockBox clickBox and it's animation function delegate
        // position is center of object - half of width/height
        Clickable stairsBox = new Clickable(new Vector2((3.644229f - 2), (1.755371f - 2)), 4, 4, kitchenScene.movementNodes[6]);
        stairsBox.StartActivity = () => SceneManager.LoadScene("LevelBedroom");

        Clickable fridgeBox = new Clickable(new Vector2(8.537492f - 2f, 0.03424625f - 3.5f), 4, 7, kitchenScene.movementNodes[2]);
        fridgeBox.StartActivity = () => SceneManager.LoadScene("LevelFridge");

        // populate the clickboxlist
        kitchenScene.clickBoxList = new List<Clickable> {
            stairsBox,
            fridgeBox
        };

        if (PlayerPrefs.GetInt("SandwichMade") == 1) {
            Clickable pillBox = new Clickable(new Vector2(-2.32f - 1f, 0.92f - 1f), 2, 2, kitchenScene.movementNodes[3]);
            pillBox.StartActivity = () => clickEnzymeToEat();

            Clickable sandwichBox = new Clickable(new Vector2(-0.81f - 1f, 0.69f - 1f), 2, 2, kitchenScene.movementNodes[3]);
            sandwichBox.StartActivity = () => clickSandwichToEat();

            kitchenScene.clickBoxList.Add(sandwichBox);
            kitchenScene.clickBoxList.Add(pillBox);
            kitchenScene.clickBoxList.Reverse();

            sandwichToEat.SetActive(true);
            EnzymePill.SetActive(true);
        }
    }

    void clickEnzymeToEat() {
        // disable visibility of pill and remove clickbox from list
        EnzymePill.SetActive(false);
        PlayerPrefs.SetInt("AteEnzyme", 1);
        kitchenScene.clickBoxList.RemoveAt(0);

        myCanvas.SetActive(true);
        bubbleText.text = "Sandwich time!";
    }

    void clickSandwichToEat() {
       if(PlayerPrefs.GetInt("AteEnzyme") == 1) {
            // disable visibility of sandwich and remove clickbox from list
            sandwichToEat.SetActive(false);
            Vector3 starPos = pal.transform.position;
            starPos.y += 2; // +2 to center on character, rather than char's feet.
            Instantiate(starsPrefab, starPos, Quaternion.identity);
            kitchenScene.clickBoxList.RemoveAt(0);

            // update hunger to reflect eating sandwich!
            var newhunger = PlayerPrefs.GetFloat("Hunger") + 4;
            if (newhunger > 5) newhunger = 5;
            PlayerPrefs.SetFloat("Hunger", newhunger);

            myCanvas.SetActive(true);
            bubbleText.text = "Yum!";
        } else {
            myCanvas.SetActive(true);
            bubbleText.text = "I need the\n enzyme pill first!";
        }
    }

    // Use this for initialization
    void Start () {
        myCanvas.transform.position = new Vector2(pal.transform.position.x - 1.2f, pal.transform.position.y + 4.9f);
        myCanvas.SetActive(true);

        if (PlayerPrefs.GetInt("SandwichMade") == 1) {
            bubbleText.text = "Looks tasty!";
        }
    }

    // define node adjacency
    void createNodeMap() {

        // n1 adj to n2
        Node.addAdj(kitchenScene.movementNodes[0], kitchenScene.movementNodes[1]);

        // n1 adj to n3
        Node.addAdj(kitchenScene.movementNodes[0], kitchenScene.movementNodes[2]);

        // n2 adj to n3
        Node.addAdj(kitchenScene.movementNodes[1], kitchenScene.movementNodes[2]);

        // n2 adj to n4
        Node.addAdj(kitchenScene.movementNodes[1], kitchenScene.movementNodes[3]);

        // n4 adj to n5
        Node.addAdj(kitchenScene.movementNodes[3], kitchenScene.movementNodes[4]);

        // n5 adj to n6
        Node.addAdj(kitchenScene.movementNodes[4], kitchenScene.movementNodes[5]);

        // n7 adj to n1
        Node.addAdj(kitchenScene.movementNodes[6], kitchenScene.movementNodes[0]);
    }

    void hungerBubble() {       
        // if they are hungry or not, display the need text
        if (PlayerPrefs.GetFloat("Hunger") <= 4) {
            //myCanvas.SetActive(true);
            if(PlayerPrefs.GetInt("SandwichMade") == 0) {
                bubbleText.text = "I'm hungry!\n Let's make a \n sandwich!";
            }
        }
    }

    void bubbleLifetime() {
        if(myCanvas.activeSelf == true) {
            if (bubbleLifeCount > 4) {
                myCanvas.SetActive(false);
                bubbleLifeCount = 0f;
            } else {
                bubbleLifeCount += Time.deltaTime;
            }
        }
    }

    // Update is called once per frame
    void Update () {

        // update object layering for scene
        if (pal.transform.position.y > -1.8) {
            kitchenScript.InHallway();
        } else {
            kitchenScript.InKitchen();
        }

        // update the kitchen
        kitchenScene.sceneUpdate();

        myCanvas.transform.position = new Vector2(pal.transform.position.x - 1.2f, pal.transform.position.y + 4.9f);

        var newhunger = PlayerPrefs.GetFloat("Hunger") - (Time.deltaTime * 0.5f);
        PlayerPrefs.SetFloat("Hunger", newhunger);

        if (PlayerPrefs.GetFloat("Hunger") <= 4) {
            if (!displaySandwich) {
                Instantiate(sandwich_icon, new Vector2(7.551609f, -0.1312826f), Quaternion.identity);
                displaySandwich = true;
                
            }
        } else {
            if (displaySandwich) {
                Destroy(sandwich_icon);
            }
        }
        // logic of whether or not to display the hunger bubble
        // if sandwich is made
        hungerBubble();

        // check bubble lifetime
        bubbleLifetime();
    }
}
