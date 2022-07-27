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

public class MoveVelocity : MonoBehaviour, IMoveVelocity {

    [SerializeField] private float moveSpeed = 50f;

    private Vector3 velocityVector;
    private new Rigidbody2D rigidbody2D;
    private Character_Base characterBase;

    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        characterBase = GetComponent<Character_Base>();
    }

    public void SetVelocity(Vector3 velocityVector) {
        this.velocityVector = velocityVector;
    }

    private void FixedUpdate() {
        rigidbody2D.velocity = velocityVector * moveSpeed;

        characterBase.PlayMoveAnim(velocityVector);
    }

    public void Disable() {
        this.enabled = false;
        rigidbody2D.velocity = Vector3.zero;
    }

    public void Enable() {
        this.enabled = true;
    }

}
