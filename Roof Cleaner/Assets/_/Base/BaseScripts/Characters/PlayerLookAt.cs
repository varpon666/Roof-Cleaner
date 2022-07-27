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
using System.Collections.Generic;
using UnityEngine;
using V_AnimationSystem;
using CodeMonkey.Utils;

public class PlayerLookAt : MonoBehaviour {

    private const float SPEED = 50f;

    private CharacterLookAt_Base playerBase;
    private Vector3 lookAtPosition;

    private void Awake() {
        playerBase = gameObject.GetComponent<CharacterLookAt_Base>();
    }

    private void Update() {
        //HandleLookAtMouse();
        HandleMovement();

        /*
        if (Input.GetMouseButtonDown(0)) {
            transform.Find("Aim").GetComponent<Animator>().SetTrigger("Shoot");
            ShellParticleSystemHandler.Instance.SpawnShell(GetPosition(), (lookAtPosition - GetPosition()).normalized);
        }
        */
    }
    
    private void HandleLookAtMouse() {
        lookAtPosition = UtilsClass.GetMouseWorldPosition();
    }

    public void SetLookAtPosition(Vector3 lookAtPosition) {
        this.lookAtPosition = lookAtPosition;
    }

    private void HandleMovement() {
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

        Vector3 lookAtDir = (lookAtPosition - GetPosition()).normalized;

        Vector3 moveDir = new Vector3(moveX, moveY).normalized;

        bool isIdle = moveX == 0 && moveY == 0;
        if (isIdle) {
            playerBase.PlayFeetIdleAnim(moveDir);
            playerBase.PlayBodyHeadIdleAnim(lookAtDir);
        } else {
            playerBase.PlayFeetWalkAnim(moveDir);
            playerBase.PlayBodyHeadWalkAnim(lookAtDir);
            transform.position += moveDir * SPEED * Time.deltaTime;
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

}
