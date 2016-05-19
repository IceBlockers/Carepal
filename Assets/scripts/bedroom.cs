using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
using UnityEngine.SceneManagement;

public class bedroom : MonoBehaviour {

    new public Camera camera;
    public GameObject pal;
    private Animator m_Anim;

    LevelScene bedroomScene;
    public GameObject bedroomSet;
    private bedroom_controller bedroomScript;
    public GameObject clockObject;
    public GameObject myCanvas;
    public GameObject greyOverlay;
    public GameObject sandwich_icon;
    public Text bubbleText;
    public GameObject onStartTest;

    public bool doIntro = false;
    public bool displaySandwich = false;
    private static bool created = false;
    

    private void Awake() {
        //PlayerPrefs.SetInt("Tutorial", 1);
        Debug.Log(PlayerPrefs.GetInt("Tutorial"));

        // game was just launched
        if (!created) {
            // set sandwichMade to 0 on game start
            PlayerPrefs.SetInt("SandwichMade", 0);

            //This will break on Android if onStartTest is null
            //DontDestroyOnLoad(onStartTest.gameObject);
            created = true;
            if (PlayerPrefs.GetInt("Tutorial") == 1) {
                doIntro = true;
            } else {
                doIntro = false;
            }
        } else {
            //This will break on Android if onStartTest is null
            //Destroy(onStartTest.gameObject);
            doIntro = false;
        }

        m_Anim = pal.GetComponent<Animator>();
        bedroomScript = bedroomSet.GetComponent<bedroom_controller>();


        // our initializations previously in start
        bedroomScene = new LevelScene(camera, pal, m_Anim);
        bedroomScene.movementNodes = new List<Node> {
            new Node(1, new Vector2(-7.3f, -2.75f)), // dresser
            new Node(2, new Vector2(-9.3f, -3.7f)), // door 
            new Node(3, new Vector2(-3.9f, -3.7f)), // left-floor 
            new Node(4, new Vector2(0f, -3.4f)), // mid-floor
            new Node(5, new Vector2(0f, -2.5f)), // desk 
            new Node(6, new Vector2(2.5f, -4.6f)), // footbed 
            new Node(7, new Vector2(8.6f, -4.6f)), // headbed
            new Node(8, new Vector2(5.25f, -2.1f)) // clockbedside
        };

        bedroomScene.clickPos = new Vector3(0, 0, 0);
        createNodeMap();

        // define clockBox clickBox and it's animation function delegate
        // position is center of object - half of width/height
        Clickable clockBox = new Clickable(new Vector2((6.481f - .5f), (0.253f - .5f)), 1, 1, bedroomScene.movementNodes[7]);
        var clockScript = clockObject.GetComponent<clock_controller>();
        clockBox.StartActivity = clockScript.StartRing;

        Clickable doorBox = new Clickable(new Vector2((-10.14934f - 2), (-0.310485f - 2)), 4, 6, bedroomScene.movementNodes[1]);
        doorBox.StartActivity = () => SceneManager.LoadScene("LevelKitchen");

        Clickable wardrobeBox = new Clickable(new Vector2((-7.7f), (-2.31f)), 3.73f, 5.6f, bedroomScene.movementNodes[0]);
        wardrobeBox.StartActivity = bedroomScript.ToggleWardrobe;

        Clickable bedBox = new Clickable(new Vector2((3.1f), (-2.0f)), 8.0f, 1.0f, bedroomScene.movementNodes[6]);
        bedBox.StartActivity = bedroomScript.ToggleSleeping;

        // populate the clickboxlist
        bedroomScene.clickBoxList = new List<Clickable> {
            clockBox,
            doorBox,
            wardrobeBox,
            bedBox
        };
    }

    // Use this for initialization
    void Start() {
        // start the bubble over the pal
        myCanvas.transform.position = new Vector2(pal.transform.position.x + 1.2f, pal.transform.position.y + 5.9f);

        // if tutorial is true, set the hunger to this value to trigger hunger soon
        if (PlayerPrefs.GetInt("Tutorial") == 1) {
            PlayerPrefs.SetFloat("Hunger", 4);
            PlayerPrefs.SetInt("AteEnzyme", 0);
            PlayerPrefs.SetInt("SandwichMade", 0);
        }
        // check if tutorial is true. if it is, do the intro
        if (doIntro) {
            myCanvas.SetActive(true);
            greyOverlay.SetActive(true);
            // pal is in the middle
            pal.transform.position = bedroomScene.movementNodes[4].position;
        } else {
            myCanvas.SetActive(false);
            greyOverlay.SetActive(false);
            // pal is near the door
            pal.transform.position = bedroomScene.movementNodes[1].position;
        }
    }
    // define node adjacency
    void createNodeMap() {

        // n1 adj to n2 and n3
        Node.addAdj(bedroomScene.movementNodes[0], bedroomScene.movementNodes[1]);
        Node.addAdj(bedroomScene.movementNodes[0], bedroomScene.movementNodes[2]);

        // n2 adj to n3
        Node.addAdj(bedroomScene.movementNodes[1], bedroomScene.movementNodes[2]);

        // n3 adj to n4
        Node.addAdj(bedroomScene.movementNodes[2], bedroomScene.movementNodes[3]);

        // n4 adj to n5 and n6
        Node.addAdj(bedroomScene.movementNodes[3], bedroomScene.movementNodes[4]);
        Node.addAdj(bedroomScene.movementNodes[3], bedroomScene.movementNodes[5]);

        // n5 adj to n8
        Node.addAdj(bedroomScene.movementNodes[4], bedroomScene.movementNodes[7]);

        // n6 adj to n7
        Node.addAdj(bedroomScene.movementNodes[5], bedroomScene.movementNodes[6]);

        // n7 adj to n6
        Node.addAdj(bedroomScene.movementNodes[6], bedroomScene.movementNodes[5]);
    }

    void hungerBubble() {
        bubbleText.text = "I'm hungry!\n Let's make a \n sandwich!";
        myCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        // check playerprefs for whether or not tutorial has been completed in previous session
        if (doIntro) {
            // if we click, the game is playable
            if (Input.GetMouseButtonDown(0) == true) {
                myCanvas.SetActive(false);
                greyOverlay.SetActive(false);
                doIntro = false;  
            }
        } else {
            // update object layering for scene
            if (pal.transform.position.y > -3) {
                bedroomScript.BedInFront();
            } else {
                bedroomScript.BedBehind();
            }

            //If the player is in bed, any click will cause them to leave the bed.
            if (Input.GetMouseButtonDown(0) == true && bedroomScript.isInBed()) {
                bedroomScript.ToggleSleeping();
            } else {
                // update the bedroom
                bedroomScene.sceneUpdate();
            }
        }

        // move the speech bubble over the pal on each frame
        myCanvas.transform.position = new Vector2(pal.transform.position.x + 1.2f, pal.transform.position.y + 5.9f);

        // update the pal's hunger on each frame
        if(!doIntro) {
            var newhunger = PlayerPrefs.GetFloat("Hunger") - ((Time.deltaTime * 0.5f)/1.5f);
            if (newhunger >= 0) {
                PlayerPrefs.SetFloat("Hunger", newhunger);
            }
        }        
        // start sandwich game quest: character says they are hungry, speech bubble follows them.
        // sandwich floating and wiggling on door and fridge
        if (PlayerPrefs.GetFloat("Hunger") <= 2) {
            if(!displaySandwich) {
                Instantiate(sandwich_icon);
                displaySandwich = true;
                hungerBubble();
            }  
        } else {
            if(displaySandwich) {
                DestroyImmediate(sandwich_icon, true);
                displaySandwich = false;
            } 
        }
    }  
}
