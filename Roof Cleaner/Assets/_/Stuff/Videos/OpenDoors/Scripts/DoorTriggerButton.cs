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

public class DoorTriggerButton : MonoBehaviour {

    [SerializeField] private GameObject doorGameObjectA = null;
    [SerializeField] private GameObject doorGameObjectB = null;
    [SerializeField] private GameObject doorGameObjectC = null;

    private IDoor doorA;
    private IDoor doorB;
    private IDoor doorC;

    private void Awake() {
        doorA = doorGameObjectA.GetComponent<IDoor>();
        doorB = doorGameObjectB.GetComponent<IDoor>();
        doorC = doorGameObjectC.GetComponent<IDoor>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            doorA.OpenDoor();
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            doorA.CloseDoor();
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            doorB.OpenDoor();
        }
        if (Input.GetKeyDown(KeyCode.G)) {
            doorB.CloseDoor();
        }
        if (Input.GetKeyDown(KeyCode.V)) {
            doorC.OpenDoor();
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            doorC.CloseDoor();
        }
    }

}
