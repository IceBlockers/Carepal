using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Node {
    public Vector2 position { private set; get; }
    public List<Node> adjNodes { private set; get; }
    public bool visited = false;
    public Node nextInPath;
    public string name;

    public Node(string name, Vector2 position) {
        this.position = position;
        this.name = name;

        adjNodes = new List<Node>();
    }

    public void addAdjNode(Node node) {
        adjNodes.Add(node);
    }
}

public class movement : MonoBehaviour {

    public Text displayPos;
    new public Camera camera;
    public GameObject pal;
    public Vector3 clickPos;
    public Vector2 clickVec;
    public const double delta = 0.1;
    public const double clickThreshold = 3;
    Stack<Node> moveStack = new Stack<Node>();
    public List<Node> movementNodes = new List<Node> {
        new Node("n1", new Vector2((float)-7.3, (float)-2.75)), //dresser
        new Node("n2", new Vector2((float)-9.3, (float)-3.7)), //door 
        new Node("n3", new Vector2((float)-3.9, (float)-3.7)), //left-floor 
        new Node("n4", new Vector2((float)0, (float)-3.4)), //mid-floor
        new Node("n5", new Vector2((float)0, (float)-2.5)), //desk 
        new Node("n6", new Vector2((float)2.5, (float)-4.6)), //footbed 
        new Node("n7", new Vector2((float)8.6, (float)-4.6)) //headbed
    };
    public Node palNode;

	// Use this for initialization
	void Start () {
        clickPos = new Vector3(0, 0, 0);
        createNodeMap();
    }

    void createNodeMap() {
        // define adjacency

        // n1
        movementNodes[0].addAdjNode(movementNodes[2]);
        movementNodes[0].addAdjNode(movementNodes[1]);

        // n2
        movementNodes[1].addAdjNode(movementNodes[2]);
        movementNodes[1].addAdjNode(movementNodes[0]);
        // n3
        movementNodes[2].addAdjNode(movementNodes[0]);
        movementNodes[2].addAdjNode(movementNodes[1]);
        movementNodes[2].addAdjNode(movementNodes[3]);

        // n4
        movementNodes[3].addAdjNode(movementNodes[2]);
        movementNodes[3].addAdjNode(movementNodes[4]);
        movementNodes[3].addAdjNode(movementNodes[5]);

        // n5
        movementNodes[4].addAdjNode(movementNodes[3]);

        // n6
        movementNodes[5].addAdjNode(movementNodes[3]);
        movementNodes[5].addAdjNode(movementNodes[6]);

        // n7
        movementNodes[6].addAdjNode(movementNodes[5]);
    }

    // Update is called once per frame
    void Update() {
        // determine node closest to pal first!
        palNode = findClosestNodeToPal(pal.transform.position);

        // detect left mouse click
        if (Input.GetMouseButtonDown(0) == true) {
            // transform screen coords to world coords
            clickPos = camera.ScreenToWorldPoint(Input.mousePosition);

            // find node closest to click
            var nodeClosest = findClosestNodeToClick(clickPos);
            Debug.Log(nodeClosest.name + " was closest node");

            // check if click is close enough to node to warrant movement

            if (!isClickThreshold(nodeClosest, clickPos, clickThreshold)) {
                // traverse node adjacency to find pal position

                resetNodes();
                // do a breadth first search from the node closest to pal to node closest to click
                bfs(palNode, nodeClosest);

                // add BFS results to stack
                Node cur = nodeClosest;
                moveStack.Push(cur);
                while ((cur = cur.nextInPath) != null) {
                    moveStack.Push(cur);
                    Debug.Log(cur.name + " ");
                }

                // set normal towards new next node ONCLICK
                clickVec.x = moveStack.Peek().position.x - pal.transform.position.x;
                clickVec.y = moveStack.Peek().position.y - pal.transform.position.y;
                clickVec.Normalize();
            }
        }

        // if pal is close enough to a node: pop stack and calculate new movement vector
        if(moveStack.Count > 0) {
            if (palCloseToNode(palNode, moveStack.Peek())) {
                moveStack.Pop();
                if (moveStack.Count > 0) {
                    // calculate new normalized movement vector
                    clickVec.x = moveStack.Peek().position.x - pal.transform.position.x;
                    clickVec.y = moveStack.Peek().position.y - pal.transform.position.y;
                    clickVec.Normalize();
                }
            }
            
            // move the pal
            pal.transform.Translate(Time.deltaTime * clickVec.x * 10, Time.deltaTime * clickVec.y * 10, 0);
        }
           
        // display the pal's position vector
        displayPos.text = pal.transform.position.x + ", " + pal.transform.position.y + ", " + pal.transform.position.z;
    }

    // check if the pal is close enough to a node
    bool palCloseToNode(Node pal, Node node) {
        var x = Mathf.Abs(node.position.y - pal.position.y);
        var y = Mathf.Abs(node.position.x - pal.position.x);
        if ((Mathf.Sqrt(x*x + y*y) < delta)) {
            return true;
        } else {
            return false;
        }
    }

    // find the node closest to the click
    Node findClosestNodeToClick(Vector3 click) {

        Node closestNode = new Node("new", new Vector2(0, 0));
        double min = int.MaxValue;
        foreach(Node n in movementNodes) {

            double x = click.x - n.position.x;
            double y = click.y - n.position.y;
            double distance = Mathf.Sqrt((float)(x*x + y*y));
            if (distance < min) {
                min = distance;
                closestNode = n;
            }
        }
        return closestNode;
    }

    // find the node closest to the click
    Node findClosestNodeToPal(Vector3 palPosition) {

        Node closestNode = new Node("new", new Vector2(0, 0));
        double min = int.MaxValue;

        foreach (Node n in movementNodes) {
            double x = palPosition.x - n.position.x;
            double y = palPosition.y - n.position.y;
            double distance = Mathf.Sqrt((float)(x * x + y * y));
            if (distance < min) {
                min = distance;
                closestNode = n;
            }
        }
        return closestNode;
    }

    // find if the click is close enough to a node
    bool isClickThreshold(Node closest, Vector3 click, double clickThreshold) {

        double x = click.x - closest.position.x;
        double y = click.y - closest.position.y;
        double distance = Mathf.Sqrt((float)(x * x + y * y));
        Debug.Log(distance  + " was the distance");
        if (distance > clickThreshold) {
            return true;
        } else {
            return false;
        }
    }

    // function to set all nodes false
    public void resetNodes() {

        moveStack.Clear();

        foreach (Node n in movementNodes) {
            n.visited = false;
            n.nextInPath = null;
        }
    }

    // breadth first search
    public void bfs(Node palNode, Node clickNode) {
        
        palNode.visited = true;
        Queue<Node> q = new Queue<Node>();
        q.Enqueue(palNode);
        Node vertex;
        
        while (q.Count > 0) {          
            vertex = q.Dequeue();
            
            foreach (Node adj in vertex.adjNodes) {
                if (!adj.visited) {
                    adj.visited = true;            
                    adj.nextInPath = vertex;          
                    if (adj.name == clickNode.name) {
                        return;
                    }
                    q.Enqueue(adj);     
                }
            }
        }
    }
}
