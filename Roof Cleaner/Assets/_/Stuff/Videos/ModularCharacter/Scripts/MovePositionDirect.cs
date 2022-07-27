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

public class MovePositionDirect : MonoBehaviour, IMovePosition {

    private Vector3 movePosition;
    private Action onReachedMovePosition;
    private bool isMovingToPosition;

    public void SetMovePosition(Vector3 movePosition, Action onReachedMovePosition) {
        this.movePosition = movePosition;
        this.onReachedMovePosition = onReachedMovePosition;
        isMovingToPosition = true;
    }

    private void Update() {
        if (isMovingToPosition) {
            Vector3 moveDir = (movePosition - transform.position).normalized;
            float reachedPositionDistance = 1f;
            if (Vector3.Distance(movePosition, transform.position) < reachedPositionDistance) {
                // Reached target position
                moveDir = Vector3.zero;
                isMovingToPosition = false;
                onReachedMovePosition();
            }
            GetComponent<IMoveVelocity>().SetVelocity(moveDir);
        } else {
            GetComponent<IMoveVelocity>().SetVelocity(Vector3.zero);
        }
    }

}
