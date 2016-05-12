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

    private void Awake() {
        m_Anim = pal.GetComponent<Animator>();
        bedroomScript = bedroomSet.GetComponent<bedroom_controller>();
    }

    // Use this for initialization
    void Start () {
        // check playerprefs for whether or not tutorial has been completed in previous session

        bedroomScene = new LevelScene(camera, pal, m_Anim);
        bedroomScene.movementNodes = new List<Node> {
            new Node("n1", new Vector2(-7.3f, -2.75f)), // dresser
            new Node("n2", new Vector2(-9.3f, -3.7f)), // door 
            new Node("n3", new Vector2(-3.9f, -3.7f)), // left-floor 
            new Node("n4", new Vector2(0f, -3.4f)), // mid-floor
            new Node("n5", new Vector2(0f, -2.5f)), // desk 
            new Node("n6", new Vector2(2.5f, -4.6f)), // footbed 
            new Node("n7", new Vector2(8.6f, -4.6f)), // headbed
            new Node("n8", new Vector2(5.25f, -2.1f)) // clockbedside
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

    // Update is called once per frame
    void Update() {

        // update object layering for scene
        if(pal.transform.position.y > -3) {
            bedroomScript.BedInFront();
        } else {
            bedroomScript.BedBehind();
        }

        // pal has stopped moving: checking for activities to run
        if (!bedroomScene.isPalMoving()) {
            
            // check if there is a recently clicked box not handled
            if(bedroomScene.clickedBox != null) {

                // if the node finished moving on is the same as the node near the object start the activity
                if(bedroomScene.clickedBox.nodeNearRect == bedroomScene.palNode) {
                    bedroomScene.clickedBox.StartActivity();
                }

                // set the clicked box to null last
                bedroomScene.clickedBox = null;
            }
        }

        // update the bedroom
        bedroomScene.sceneUpdate();
    }  
}
