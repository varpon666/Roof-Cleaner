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

public class MoveTransformVelocity : MonoBehaviour, IMoveVelocity {

    [SerializeField] private float moveSpeed = 10f;

    private Vector3 velocityVector;
    private Character_Base characterBase;

    private void Awake() {
        characterBase = GetComponent<Character_Base>();
    }

    public void SetVelocity(Vector3 velocityVector) {
        this.velocityVector = velocityVector;
    }

    private void Update() {
        transform.position += velocityVector * moveSpeed * Time.deltaTime;
        characterBase.PlayMoveAnim(velocityVector);
    }

    public void Disable() {
        this.enabled = false;
    }

    public void Enable() {
        this.enabled = true;
    }

}
