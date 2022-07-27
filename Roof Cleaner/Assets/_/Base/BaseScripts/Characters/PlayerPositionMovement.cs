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
using V_AnimationSystem;
using CodeMonkey.Utils;

/*
 * Player movement with TargetPosition
 * */
public class PlayerPositionMovement : MonoBehaviour {
    
    private const float SPEED = 50f;
    
    private Player_Base playerBase;
    private Vector3 targetPosition;

    private void Awake() {
        playerBase = gameObject.GetComponent<Player_Base>();
    }

    private void Update() {
        HandleMovement();

        if (Input.GetMouseButtonDown(0)) {
            SetTargetPosition(UtilsClass.GetMouseWorldPosition());
        }
    }

    private void HandleMovement() {
        if (Vector3.Distance(transform.position, targetPosition) > 1f) {
            Vector3 moveDir = (targetPosition - transform.position).normalized;

            playerBase.PlayMoveAnim(moveDir);
            transform.position += moveDir * SPEED * Time.deltaTime;
        } else {
            playerBase.PlayIdleAnim();
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }
        
    public void SetTargetPosition(Vector3 targetPosition) {
        targetPosition.z = 0f;
        this.targetPosition = targetPosition;
    }

}
