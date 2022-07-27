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

public class DoorTriggerKey_Done : MonoBehaviour {
    
    [SerializeField] private DoorAnims door = null;
    [SerializeField] private Key.KeyType keyType = Key.KeyType.Green;

    private void OnTriggerEnter2D(Collider2D collider) {
        KeyHolder keyHolder = collider.GetComponent<KeyHolder>();
        if (keyHolder != null) {
            if (keyHolder.ContainsKey(keyType)) {
                door.OpenDoor();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        KeyHolder keyHolder = collider.GetComponent<KeyHolder>();
        if (keyHolder != null) {
            door.CloseDoor();
        }
    }

}
