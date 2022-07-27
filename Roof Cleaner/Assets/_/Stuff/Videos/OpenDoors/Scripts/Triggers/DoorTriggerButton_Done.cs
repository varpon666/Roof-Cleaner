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

public class DoorTriggerButton_Done : MonoBehaviour {

    //[SerializeField] private DoorAnims door = null;

    [SerializeField] private Transform playerTransform = null;

    //private bool isOpen;

    private void Awake() {
        //isOpen = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(playerTransform.position, 20f);
            Debug.Log(collider2DArray.Length);
            foreach (Collider2D collider2D in collider2DArray) {
                DoorAnims doorAnims = collider2D.GetComponent<DoorAnims>();
                if (doorAnims != null) {
                    doorAnims.OpenDoor();
                    //isOpen = true;
                }
            }

            /*
            isOpen = !isOpen;
            if (isOpen) {
                door.OpenDoor();
            } else {
                door.CloseDoor();
            }
            */
        }
    }

}
