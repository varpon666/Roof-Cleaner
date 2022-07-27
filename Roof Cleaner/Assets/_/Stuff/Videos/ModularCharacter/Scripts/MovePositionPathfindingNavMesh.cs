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
using UnityEngine.AI;
using GridPathfindingSystem;

public class MovePositionPathfindingNavMesh : MonoBehaviour, IMovePosition {

    private int pathIndex = -1;
    private NavMeshPath path;
    private Vector3[] pathVectorArray;
    private Vector3 lastMovePosition;
    private float lastMovePositionTime;
    private int failCount;
    private Action onReachedMovePosition;

    public void SetMovePosition(Vector3 movePosition, Action onReachedMovePosition) {
        this.onReachedMovePosition = onReachedMovePosition;
        float sameDistancePosition = 1f;
        if (Vector3.Distance(movePosition, lastMovePosition) < sameDistancePosition) {
            // Position is too similar to last position
            // Has it been long enough?
            float timeToRepeatSamePath = .5f;
            if (Time.realtimeSinceStartup - lastMovePositionTime < timeToRepeatSamePath) {
                // Not long enough
                return;
            }
        }

        //Debug.Log("SetMovePosition " + Time.realtimeSinceStartup);
        path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, movePosition, NavMesh.AllAreas, path)) {
            failCount = 0;
            lastMovePositionTime = Time.realtimeSinceStartup;
            lastMovePosition = movePosition;
            if (path.corners.Length > 1) {
                pathIndex = 1;
                pathVectorArray = path.corners;
            } else {
                pathIndex = -1;
            }
        } else {
            //Debug.Log("Path fail " + failCount + " " + movePosition);
            pathIndex = -1;
            failCount++;
            if (failCount >= 100) {
                pathVectorArray = new Vector3[] { movePosition };
                pathIndex = 0;
            }
        }
    }

    private void Update() {
        if (pathIndex != -1) {
            // Move to next path position
            Vector3 nextPathPosition = pathVectorArray[pathIndex];
            Vector3 moveVelocity = (nextPathPosition - transform.position).normalized;
            GetComponent<IMoveVelocity>().SetVelocity(moveVelocity);

            float reachedPathPositionDistance = .1f;
            if (Vector3.Distance(transform.position, nextPathPosition) < reachedPathPositionDistance) {
                pathIndex++;
                if (pathIndex >= pathVectorArray.Length) {
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

    public void Disable() {
        GetComponent<IMoveVelocity>().SetVelocity(Vector3.zero);
        this.enabled = false;
    }

    public void Enable() {
        this.enabled = true;
    }

}
