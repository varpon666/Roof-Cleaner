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
using V_AnimationSystem;
using CodeMonkey.Utils;

public class CharacterController3D : MonoBehaviour {

    private const float MOVE_SPEED = 10f;

    private enum State {
        Normal,
        Rolling,
        Busy,
    }

    [SerializeField] private LayerMask dashLayerMask = new LayerMask();

    private Character_Base characterBase;
    private Rigidbody rigidbody3D;
    private Vector3 moveDir;
    private Vector3 rollDir;
    private Vector3 lastMoveDir;
    private float rollSpeed;
    private bool isDashButtonDown;
    private State state;

    private void Awake() {
        characterBase = GetComponent<Character_Base>();
        rigidbody3D = GetComponent<Rigidbody>();
        state = State.Normal;
    }

    private void Update() {
        switch (state) {
        case State.Normal:
            float moveX = 0f;
            float moveZ = 0f;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                moveZ = +1f;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                moveZ = -1f;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                moveX = -1f;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                moveX = +1f;
            }

            moveDir = new Vector3(moveX, 0f, moveZ).normalized;
            if (moveX != 0 || moveZ != 0) {
                // Not idle
                lastMoveDir = moveDir;
            }
            //Vector2 moveDirVector2 = new Vector2(moveDir.x, moveDir.z);
            characterBase.PlayMoveAnim(moveDir);

            if (Input.GetKeyDown(KeyCode.F)) {
                isDashButtonDown = true;
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                rollDir = lastMoveDir;
                rollSpeed = 40f;
                state = State.Rolling;
                characterBase.PlayRollAnimation(rollDir);
            }
            break;
        case State.Rolling:
            float rollSpeedDropMultiplier = 5f;
            rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;

            float rollSpeedMinimum = MOVE_SPEED;
            if (rollSpeed < rollSpeedMinimum) {
                state = State.Normal;
            }
            break;
        case State.Busy:
            break;
        }
    }

    private void FixedUpdate() {
        switch (state) {
        case State.Normal:
            rigidbody3D.velocity = moveDir * MOVE_SPEED;

            if (isDashButtonDown) {
                float dashAmount = 5f;
                Vector3 dashPosition = transform.position + lastMoveDir * dashAmount;

                RaycastHit2D raycastHit2d = Physics2D.Raycast(transform.position, lastMoveDir, dashAmount, dashLayerMask);
                if (raycastHit2d.collider != null) {
                    dashPosition = raycastHit2d.point;
                }

                // Spawn visual effect
                DashEffect.CreateDashEffect(transform.position, lastMoveDir, Vector3.Distance(transform.position, dashPosition));

                rigidbody3D.MovePosition(dashPosition);
                isDashButtonDown = false;
            }
            break;
        case State.Rolling:
            rigidbody3D.velocity = rollDir * rollSpeed;
            break;
        case State.Busy:
            break;
        }
    }

    public void CinematicLookDown(float timer) {
        state = State.Busy;
        rigidbody3D.velocity = Vector3.zero;
        //characterBase.GetUnitAnimation().PlayAnimForced(UnitAnimType.GetUnitAnimType(""), new Vector3(0, -1), 1f, null, null, null);
        characterBase.GetUnitAnimation().PlayAnimForced(UnitAnim.GetUnitAnim("dBareHands_Victory"), 1f, null, null, null);
        FunctionTimer.Create(() => {
            state = State.Normal;
        }, timer);
    }

}
