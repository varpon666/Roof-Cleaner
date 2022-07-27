/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GridPathfindingSystem {

    public class PathRoute {

        public List<PathNode> pathNodeList;
        public List<Vector3> pathVectorList;
        public MapPos finalPos;

        public PathRoute(List<PathNode> pathNodeList, List<Vector3> pathVectorList, MapPos finalPos) {
            this.pathNodeList = pathNodeList;
            this.pathVectorList = pathVectorList;
            this.finalPos = finalPos;
        }

        public PathRoute(List<PathNode> pathNodeList, Vector3 worldOrigin, float nodeSize, MapPos finalPos) {
            this.pathNodeList = pathNodeList;
            pathVectorList = new List<Vector3>();
            foreach (PathNode pathNode in pathNodeList) {
                pathVectorList.Add(pathNode.GetWorldVector(worldOrigin, nodeSize));
            }
            this.finalPos = finalPos;
        }

        public void AddVector(Vector3 vector) {
            pathVectorList.Add(vector);
        }

    }

}