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

public class MoveAnimsTransformLastMoveDir : MonoBehaviour {

    private Character_Base characterBase;
    private Vector3 lastPosition;

    private void Awake() {
        characterBase = GetComponent<Character_Base>();
        lastPosition = transform.position;
    }

    private void Update() {
        Vector3 lastMoveDir = (transform.position - lastPosition).normalized;

        Debug.Log(lastMoveDir + " " + transform.position);
        characterBase.PlayMoveAnim(lastMoveDir);

        lastPosition = transform.position;
    }

}
