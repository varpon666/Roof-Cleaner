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

namespace TopDownShooter {
    public class PlayerMovementHandler : MonoBehaviour {

        private const float SPEED = 50f;

        private PlayerMain playerMain;

        private Vector3 moveDir;
        private Vector3 lastMoveDir;

        private void Awake() {
            playerMain = GetComponent<PlayerMain>();
        }

        private void Update() {
            HandleMovement();
        }

        private void HandleMovement() {
            float moveX = 0f;
            float moveY = 0f;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                moveY = +1f;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                moveY = -1f;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                moveX = -1f;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                moveX = +1f;
            }

            moveDir = new Vector3(moveX, moveY).normalized;

            bool isIdle = moveX == 0 && moveY == 0;
            if (isIdle) {
                playerMain.PlayerSwapAimNormal.PlayIdleAnim();
            } else {
                lastMoveDir = moveDir;
                playerMain.PlayerSwapAimNormal.PlayMoveAnim(moveDir);
            }
        }

        private void FixedUpdate() {
            playerMain.PlayerRigidbody2D.velocity = moveDir * SPEED;
        }

        public void Enable() {
            enabled = true;
        }

        public void Disable() {
            enabled = false;
            playerMain.PlayerRigidbody2D.velocity = Vector3.zero;
        }

        public Vector3 GetLastMoveDir() {
            return lastMoveDir;
        }

    }
}