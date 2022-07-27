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

public class DoorSetActive : MonoBehaviour, IDoor {

    private bool isOpen = false;

    public void OpenDoor() {
        isOpen = true;
        gameObject.SetActive(false);
    }

    public void CloseDoor() {
        isOpen = false;
        gameObject.SetActive(true);
    }

    public void ToggleDoor() {
        isOpen = !isOpen;
        if (isOpen) {
            OpenDoor();
        } else {
            CloseDoor();
        }
    }

}
