using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D_Simple : MonoBehaviour {

    private const float MOVE_SPEED = 60f;

    [SerializeField] private LayerMask dashLayerMask = new LayerMask();

    private Character_Base characterBase;
    private new Rigidbody2D rigidbody2D;
    private Vector3 moveDir;
    private bool isDashButtonDown;

    private void Awake() {
        characterBase = GetComponent<Character_Base>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update() {
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

        moveDir = new Vector3(moveX, moveY).normalized;
        characterBase.PlayMoveAnim(moveDir);

        if (Input.GetKeyDown(KeyCode.Space)) {
            isDashButtonDown = true;
        }
    }

    private void FixedUpdate() {
        rigidbody2D.velocity = moveDir * MOVE_SPEED;

        if (isDashButtonDown) {
            float dashAmount = 50f;
            Vector3 dashPosition = transform.position + moveDir * dashAmount;

            RaycastHit2D raycastHit2d = Physics2D.Raycast(transform.position, moveDir, dashAmount, dashLayerMask);
            if (raycastHit2d.collider != null) {
                dashPosition = raycastHit2d.point;
            }

            // Spawn visual effect
            DashEffect.CreateDashEffect(transform.position, moveDir, Vector3.Distance(transform.position, dashPosition));

            rigidbody2D.MovePosition(dashPosition);
            isDashButtonDown = false;
        }
    }

}
