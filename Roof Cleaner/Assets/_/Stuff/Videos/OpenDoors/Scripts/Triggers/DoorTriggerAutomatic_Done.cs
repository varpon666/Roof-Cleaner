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

public class DoorTriggerAutomatic_Done : MonoBehaviour {

    [SerializeField] private DoorAnims door = null;

    private void OnTriggerEnter2D(Collider2D collision) {
        door.OpenDoor();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        door.CloseDoor();
    }

}
