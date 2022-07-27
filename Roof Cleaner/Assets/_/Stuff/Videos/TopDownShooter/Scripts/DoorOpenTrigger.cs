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

public class DoorOpenTrigger : MonoBehaviour {

    [SerializeField] private CaptureOnTriggerEnter2D captureOnTriggerEnter2D = null;
    [SerializeField] private DoorAnims doorAnims = null;

    private void Awake() {
        captureOnTriggerEnter2D.OnPlayerTriggerEnter2D += DoorOpenTrigger_OnPlayerTriggerEnter2D;
    }

    private void DoorOpenTrigger_OnPlayerTriggerEnter2D(object sender, System.EventArgs e) {
        doorAnims.SetColor(DoorAnims.ColorName.Green);
        doorAnims.OpenDoor();
        captureOnTriggerEnter2D.OnPlayerTriggerEnter2D -= DoorOpenTrigger_OnPlayerTriggerEnter2D;
    }

}
