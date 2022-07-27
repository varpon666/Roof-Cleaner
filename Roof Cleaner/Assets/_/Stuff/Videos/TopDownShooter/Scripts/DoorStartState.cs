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

public class DoorStartState : MonoBehaviour {

    [SerializeField] private bool startOpen = false;
    [SerializeField] private DoorAnims.ColorName doorColor = DoorAnims.ColorName.Green;

    private void Start() {
        DoorAnims doorAnims = GetComponent<DoorAnims>();
        if (startOpen) {
            doorAnims.OpenDoor();
        } else {
            doorAnims.CloseDoor();
        }

        doorAnims.SetColor(doorColor);
    }

}
