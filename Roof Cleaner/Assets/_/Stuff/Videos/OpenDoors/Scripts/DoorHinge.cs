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

public class DoorHinge : MonoBehaviour, IDoor {

    private HingeJoint2D hingeJoint2D;
    private JointAngleLimits2D openDoorLimits;
    private JointAngleLimits2D closeDoorLimits;
    private bool isOpen = false;

    private void Awake() {
        hingeJoint2D = transform.Find("Hinge").GetComponent<HingeJoint2D>();
        openDoorLimits = hingeJoint2D.limits;
        closeDoorLimits = new JointAngleLimits2D { min = 0f, max = 0f };
        CloseDoor();
    }

    public void OpenDoor() {
        isOpen = true;
        hingeJoint2D.limits = openDoorLimits;
    }

    public void CloseDoor() {
        isOpen = false;
        hingeJoint2D.limits = closeDoorLimits;
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
