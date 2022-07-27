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

public class DoorTriggerPressurePlate_Done : MonoBehaviour {

    [SerializeField] private DoorAnims door = null;

    private float timer;

    private void Update() {
        if (timer > 0f) {
            timer -= Time.deltaTime;
            if (timer <= 0f) {
                door.CloseDoor();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        float timeToStayOpen = 2f;
        timer = timeToStayOpen;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        door.OpenDoor();
    }

}
