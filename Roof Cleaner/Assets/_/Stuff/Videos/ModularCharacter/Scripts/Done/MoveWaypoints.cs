/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MoveWaypoints : MonoBehaviour {
    
    [SerializeField] private Vector3[] waypointList = null;
    private int waypointIndex;

    private void Update() {
        SetMovePosition(GetWaypointPosition());

        float arrivedAtPositionDistance = 1f;
        if (Vector3.Distance(transform.position, GetWaypointPosition()) < arrivedAtPositionDistance) {
            // Reached position
            waypointIndex = (waypointIndex + 1) % waypointList.Length;
        }
    }

    private Vector3 GetWaypointPosition() {
        return waypointList[waypointIndex];
    }

    private void SetMovePosition(Vector3 movePosition) {
        GetComponent<IMovePosition>().SetMovePosition(movePosition, () => { });
    }

}
