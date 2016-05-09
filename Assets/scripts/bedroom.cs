using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;

public class bedroom : MonoBehaviour {

    public Text displayPos;
    new public Camera camera;
    public GameObject pal;
    private Animator m_Anim;
    Scene bedroomScene;

    private void Awake() {
        m_Anim = pal.GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
        bedroomScene = new Scene(camera, pal, m_Anim);
        bedroomScene.movementNodes = new List<Node> {
            new Node("n1", new Vector2((float)-7.3, (float)-2.75)), //dresser
            new Node("n2", new Vector2((float)-9.3, (float)-3.7)), //door 
            new Node("n3", new Vector2((float)-3.9, (float)-3.7)), //left-floor 
            new Node("n4", new Vector2((float)0, (float)-3.4)), //mid-floor
            new Node("n5", new Vector2((float)0, (float)-2.5)), //desk 
            new Node("n6", new Vector2((float)2.5, (float)-4.6)), //footbed 
            new Node("n7", new Vector2((float)8.6, (float)-4.6)) //headbed
        };

        bedroomScene.clickPos = new Vector3(0, 0, 0);
        createNodeMap();
    }

    // define node adjacency
    void createNodeMap() {

        // n1
        bedroomScene.movementNodes[0].addAdjNode(bedroomScene.movementNodes[2]);
        bedroomScene.movementNodes[0].addAdjNode(bedroomScene.movementNodes[1]);

        // n2
        bedroomScene.movementNodes[1].addAdjNode(bedroomScene.movementNodes[2]);
        bedroomScene.movementNodes[1].addAdjNode(bedroomScene.movementNodes[0]);
        // n3
        bedroomScene.movementNodes[2].addAdjNode(bedroomScene.movementNodes[0]);
        bedroomScene.movementNodes[2].addAdjNode(bedroomScene.movementNodes[1]);
        bedroomScene.movementNodes[2].addAdjNode(bedroomScene.movementNodes[3]);

        // n4
        bedroomScene.movementNodes[3].addAdjNode(bedroomScene.movementNodes[2]);
        bedroomScene.movementNodes[3].addAdjNode(bedroomScene.movementNodes[4]);
        bedroomScene.movementNodes[3].addAdjNode(bedroomScene.movementNodes[5]);

        // n5
        bedroomScene.movementNodes[4].addAdjNode(bedroomScene.movementNodes[3]);

        // n6
        bedroomScene.movementNodes[5].addAdjNode(bedroomScene.movementNodes[3]);
        bedroomScene.movementNodes[5].addAdjNode(bedroomScene.movementNodes[6]);

        // n7
        bedroomScene.movementNodes[6].addAdjNode(bedroomScene.movementNodes[5]);
    }

    // Update is called once per frame
    void Update() {

        // update the bedroom
        bedroomScene.sceneUpdate();
    }  
}
