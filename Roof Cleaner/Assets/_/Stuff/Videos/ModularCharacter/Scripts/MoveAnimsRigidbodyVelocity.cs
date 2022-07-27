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

public class MoveAnimsRigidbodyVelocity : MonoBehaviour {

    private new Rigidbody2D rigidbody2D;
    private Character_Base characterBase;

    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        characterBase = GetComponent<Character_Base>();
    }

    private void FixedUpdate() {
        characterBase.PlayMoveAnim(rigidbody2D.velocity);
    }

}
