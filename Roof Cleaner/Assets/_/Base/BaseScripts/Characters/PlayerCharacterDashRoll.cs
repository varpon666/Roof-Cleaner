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
using CodeMonkey;

public class PlayerCharacterDashRoll : MonoBehaviour {

    [SerializeField] private Transform pfDashEffect = null;
    private PlayerCharacterDashRoll_Base playerCharacterBase;
    private Vector3 lastMoveDir;
    private Vector3 slideDir;
    private float slideSpeed;

    private State state;
    private enum State {
        Normal,
        DodgeRollSliding,
    }

    private void Awake() {
        playerCharacterBase = gameObject.GetComponent<PlayerCharacterDashRoll_Base>();
        state = State.Normal;
    }

    private void Update() {
        switch (state) {
        case State.Normal:
            HandleMovement();
            HandleDash();
            HandleDodgeRoll();
            break;
        case State.DodgeRollSliding:
            HandleDodgeRollSliding();
            break;
        }
    }

    private void HandleMovement() {
        float speed = 50f;
        float moveX = 0f;
        float moveY = 0f;
        
        if (Input.GetKey(KeyCode.W)) {
            moveY = +1f;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveX = +1f;
        }

        bool isIdle = moveX == 0 && moveY == 0;
        if (isIdle) {
            playerCharacterBase.PlayIdleAnimation(lastMoveDir);
        } else {
            Vector3 moveDir = new Vector3(moveX, moveY).normalized;

            if (TryMove(moveDir, speed * Time.deltaTime)) {
                playerCharacterBase.PlayWalkingAnimation(lastMoveDir);
            } else {
                playerCharacterBase.PlayIdleAnimation(lastMoveDir);
            }
        }
    }

    private bool CanMove(Vector3 dir, float distance) {
        return Physics2D.Raycast(transform.position, dir, distance).collider == null;
    }

    private bool TryMove(Vector3 baseMoveDir, float distance) {
        Vector3 moveDir = baseMoveDir;
        bool canMove = CanMove(moveDir, distance);
        if (!canMove) {
            // Cannot move diagonally
            moveDir = new Vector3(baseMoveDir.x, 0f).normalized;
            canMove = moveDir.x != 0f && CanMove(moveDir, distance);
            if (!canMove) {
                // Cannot move horizontally
                moveDir = new Vector3(0f, baseMoveDir.y).normalized;
                canMove = moveDir.y != 0f && CanMove(moveDir, distance);
            }
        }

        if (canMove) {
            lastMoveDir = moveDir;
            transform.position += moveDir * distance;
            return true;
        } else {
            return false;
        }
    }

    private void HandleDash() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            float dashDistance = 30f;
            Vector3 beforeDashPosition = transform.position;
            if (TryMove(lastMoveDir, dashDistance)) {
                Transform dashEffectTransform = Instantiate(pfDashEffect, beforeDashPosition, Quaternion.identity);
                dashEffectTransform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(lastMoveDir));
                float dashEffectWidth = 30f;
                dashEffectTransform.localScale = new Vector3(dashDistance / dashEffectWidth, 1f, 1f);
            }
        }
    }

    private void HandleDodgeRoll() {
        if (Input.GetMouseButtonDown(1)) {
            state = State.DodgeRollSliding;
            slideDir = (UtilsClass.GetMouseWorldPosition() - transform.position).normalized;
            slideSpeed = 500f;
            playerCharacterBase.PlayDodgeAnimation(slideDir);
        }
    }

    private void HandleDodgeRollSliding() {
        TryMove(slideDir, slideSpeed * Time.deltaTime);
        //transform.position += slideDir * slideSpeed * Time.deltaTime;

        slideSpeed -= slideSpeed * 10f * Time.deltaTime;
        if (slideSpeed < 10f) {
            state = State.Normal;
        }
    }

}
