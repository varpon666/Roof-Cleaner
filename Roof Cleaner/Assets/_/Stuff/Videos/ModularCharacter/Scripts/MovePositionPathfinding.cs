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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridPathfindingSystem;

public class MovePositionPathfinding : MonoBehaviour, IMovePosition {

    private List<Vector3> pathVectorList;
    private int pathIndex = -1;
    private Action onReachedMovePosition;

    public void SetMovePosition(Vector3 movePosition, Action onReachedMovePosition) {
        this.onReachedMovePosition = onReachedMovePosition;
        pathVectorList = GridPathfinding.instance.GetPathRouteWithShortcuts(transform.position, movePosition).pathVectorList;
        if (pathVectorList.Count > 0) {
            // Remove first position so he doesn't go backwards
            pathVectorList.RemoveAt(0);
        }
        if (pathVectorList.Count > 0) {
            pathIndex = 0;
        } else {
            pathIndex = -1;
            onReachedMovePosition();
        }
    }

    private void Update() {
        if (pathIndex != -1) {
            // Move to next path position
            Vector3 nextPathPosition = pathVectorList[pathIndex];
            Vector3 moveVelocity = (nextPathPosition - transform.position).normalized;
            GetComponent<IMoveVelocity>().SetVelocity(moveVelocity);

            float reachedPathPositionDistance = 1f;
            if (Vector3.Distance(transform.position, nextPathPosition) < reachedPathPositionDistance) {
                pathIndex++;
                if (pathIndex >= pathVectorList.Count) {
                    // End of path
                    pathIndex = -1;
                    onReachedMovePosition();
                }
            }
        } else {
            // Idle
            GetComponent<IMoveVelocity>().SetVelocity(Vector3.zero);
        }
    }

}
