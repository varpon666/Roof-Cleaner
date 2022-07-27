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

public class PlayerMovementMouse : MonoBehaviour {

    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            GetComponent<IMovePosition>().SetMovePosition(UtilsClass.GetMouseWorldPosition(), () => { });
        }
    }

}
