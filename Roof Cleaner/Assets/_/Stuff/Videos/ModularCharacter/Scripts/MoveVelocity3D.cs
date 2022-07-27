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

public class MoveVelocity3D : MonoBehaviour, IMoveVelocity {

    [SerializeField] private float moveSpeed = 50f;

    private Vector3 velocityVector;
    private new Rigidbody rigidbody;
    private Character_Base characterBase;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        characterBase = GetComponent<Character_Base>();
    }

    public void SetVelocity(Vector3 velocityVector) {
        this.velocityVector = velocityVector;
    }

    private void FixedUpdate() {
        rigidbody.velocity = velocityVector * moveSpeed;

        characterBase.PlayMoveAnim(velocityVector);
    }

    public void Disable() {
        this.enabled = false;
        rigidbody.velocity = Vector3.zero;
    }

    public void Enable() {
        this.enabled = true;
    }

}
