/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System;
using UnityEngine;
using System.Collections;

namespace GridPathfindingSystem {

    public class PathNode {

        public event EventHandler OnWalkableChanged;

        public int xPos;
        public int yPos;
        public PathNode parent;
        public PathNode north;
        public PathNode south;
        public PathNode west;
        public PathNode east;
        public bool moveNorth;
        public bool moveSouth;
        public bool moveWest;
        public bool moveEast;

        public bool isOnOpenList = false;
        public bool isOnClosedList = false;

        public int weight = 0;
        public int gValue = 0;
        public int hValue;
        public int fValue;

        //public Transform trans;
        //public int layerMask = 1 << 9;

        public PathNode(int _xPos, int _yPos) {
            xPos = _xPos;
            yPos = _yPos;

            moveNorth = true;
            moveSouth = true;
            moveWest = true;
            moveEast = true;

            //trans = ((GameObject) Object.Instantiate(Resources.Load("pfPathNode"), new Vector3(xPos*10, 0, zPos*10), Quaternion.identity)).transform;
            TestHitbox();
        }
        public void ResetRestrictions() {
            moveNorth = true;
            moveSouth = true;
            moveWest = true;
            moveEast = true;
        }
        public override string ToString() {
            return "x: " + xPos + ", y:" + yPos;
        }
        public void SetWalkable(bool walkable) {
            weight = walkable ? 0 : GridPathfinding.WALL_WEIGHT;
            if (OnWalkableChanged != null) OnWalkableChanged(this, EventArgs.Empty);
        }
        public void SetWeight(int weight) {
            this.weight = weight;
        }
        public bool IsWalkable() {
            return weight < GridPathfinding.WALL_WEIGHT;
        }
        public void TestHitbox() {
            weight = 0;
        }
        public MapPos GetMapPos() {
            return new MapPos(xPos, yPos);
        }
        public void CalculateFValue() {
            fValue = gValue + hValue;
        }
        public Vector3 GetWorldVector(Vector3 worldOrigin, float nodeSize) {
            return worldOrigin + new Vector3(xPos * nodeSize, yPos * nodeSize);
        }
    }

}