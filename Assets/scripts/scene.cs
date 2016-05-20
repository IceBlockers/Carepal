﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.scripts {
    class LevelScene {
        public Camera camera;
        public GameObject pal;
        private Animator m_Anim;

        public Node palNode;
        public List<Node> movementNodes;
        public List<Clickable> clickBoxList;
        public Stack<Node> moveStack = new Stack<Node>();
        public Vector3 clickPos;
        public Vector2 clickVec;
        public Clickable clickedBox;

        bool palStillMoving = false;
        
        public const double delta = 0.1;
        
        public const double clickThreshold = 3;

        public LevelScene(Camera camera, GameObject pal, Animator m_Anim) {
            this.camera = camera;
            this.pal = pal;
            this.m_Anim = m_Anim;
        }

        // function to handle scene update logic
        public void sceneUpdate() {
            // pal has stopped moving: checking for activities to run
            if (!isPalMoving()) {

                // check if there is a recently clicked box not handled
                if (clickedBox != null) {

                    // if the node finished moving on is the same as the node near the object start the activity
                    if (clickedBox.nodeNearRect == palNode) {
                        clickedBox.StartActivity();
                    }

                    // set the clicked box to null last
                    clickedBox = null;
                }
            }

            // determine node closest to pal first!
            palNode = findClosestNodeToPal(pal.transform.position);

            if (Input.GetMouseButtonDown(0) == true) {
                handleMouseDown(Input.mousePosition);
            }
            movePal();
        }

        // function to handle mousedown events
        public void handleMouseDown(Vector3 mousePos) {

            // transform screen coords to world coords
            clickPos = camera.ScreenToWorldPoint(mousePos);

            // find node closest to click
            var nodeClosest = findClosestNodeToClick(clickPos);

            if(clickBoxList != null) {
                // check list of clickable boxes
                foreach (Clickable clickBox in clickBoxList) {
                    if (clickBox.clickRect.Contains(clickPos)) {
                        // set path to that node
                        nodeClosest = clickBox.nodeNearRect;

                        // set click pos to be that node
                        clickPos = clickBox.nodeNearRect.position;

                        // set the clicked box variable
                        clickedBox = clickBox;
                        Debug.Log("clicked on Thing " + clickedBox.ToString());
                    }
                }
            } else {
                Debug.Log("clickboxlist is null");
            }

            if (findClosestNodeToClick(clickPos) != findClosestNodeToPal(pal.transform.position)) {

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
                        Debug.Log("node " + cur.nodeNumber + ", ");
                    }

                    // set normal towards new next node ONCLICK
                    clickVec.x = moveStack.Peek().position.x - pal.transform.position.x;
                    clickVec.y = moveStack.Peek().position.y - pal.transform.position.y;
                    clickVec.Normalize();
                }
            }
        }

        // function to move the pal in each scene update
        public void movePal() {
            
            // if pal is close enough to a node: pop stack and calculate new movement vector
            if (moveStack.Count > 0) {
                if (palCloseToNode(moveStack.Peek())) {
                    moveStack.Pop();
                }

                if (moveStack.Count > 0) {
                    // calculate new normalized movement vector
                    clickVec.x = moveStack.Peek().position.x - pal.transform.position.x;
                    clickVec.y = moveStack.Peek().position.y - pal.transform.position.y;
                    clickVec.Normalize();
                }

                if (clickVec.x < 0) {

                    facingLeft(true);
                } else {
                    facingLeft(false);
                }

                // move the pal
                pal.transform.Translate(Time.deltaTime * clickVec.x * 10, Time.deltaTime * clickVec.y * 10, 0);

                m_Anim.SetFloat("vSpeed", clickVec.magnitude);
                palStillMoving = true;
            } else {
                m_Anim.SetFloat("vSpeed", 0);
                palStillMoving = false;

            }
        }

        // function to determine the direction the pal is facing
        void facingLeft(bool left) {
            if (left) {
                pal.transform.localScale = new Vector3(
                    -System.Math.Abs(pal.transform.localScale.x),
                    pal.transform.localScale.y,
                    pal.transform.localScale.z);
            } else {
                pal.transform.localScale = new Vector3(
                    System.Math.Abs(pal.transform.localScale.x),
                    pal.transform.localScale.y,
                    pal.transform.localScale.z);
            }
        }

        // check if the pal is close enough to a node
        bool palCloseToNode(Node node) {
            var x = Mathf.Abs(node.position.y - pal.transform.position.y);
            var y = Mathf.Abs(node.position.x - pal.transform.position.x);
            if ((Mathf.Sqrt(x * x + y * y) < delta)) {
                return true;
            } else {
                return false;
            }
        }

        // find the node closest to the click
        Node findClosestNodeToClick(Vector3 click) {

            Node closestNode = new Node(0, new Vector2(0, 0));
            double min = int.MaxValue;
            foreach (Node n in movementNodes) {

                double x = click.x - n.position.x;
                double y = click.y - n.position.y;
                double distance = Mathf.Sqrt((float)(x * x + y * y));
                if (distance < min) {
                    min = distance;
                    closestNode = n;
                }
            }
            return closestNode;
        }

        // find the node closest to the click
        public Node findClosestNodeToPal(Vector3 palPosition) {

            Node closestNode = new Node(0, new Vector2(0, 0));
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
            Debug.Log(distance + " was the distance");
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
                        if (adj.nodeNumber == clickNode.nodeNumber) {
                            return;
                        }
                        q.Enqueue(adj);
                    }
                }
            }
        }

        public bool isPalMoving() {
            return palStillMoving;
        }
    }

    // node class
    public class Node {
        public Vector2 position { private set; get; }
        public List<Node> adjNodes { private set; get; }
        public bool visited = false;
        public Node nextInPath;
        public int nodeNumber;

        public Node(int nodeNumber, Vector2 position) {
            this.position = position;
            this.nodeNumber = nodeNumber;

            adjNodes = new List<Node>();
        }

        public void addAdjNode(Node node) {
            adjNodes.Add(node);
        }

        public static void addAdj(Node node1, Node node2) {
            node1.adjNodes.Add(node2);
            node2.adjNodes.Add(node1);
        }
    }

    public class Clickable {
        public Rect clickRect { private set; get; }
        public Node nodeNearRect;
        public delegate void Activity();
        public Activity StartActivity;

        public Clickable(Vector2 topLeftPos, int width, int height) {
            clickRect = new Rect(topLeftPos.x, topLeftPos.y, width, height);
            nodeNearRect = null;
        }

        public Clickable(Vector2 topLeftPos, float width, float height, Node nearestNode) {
            clickRect = new Rect(topLeftPos.x, topLeftPos.y, width, height);
            nodeNearRect = nearestNode;
        }

    }
}
