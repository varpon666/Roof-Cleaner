using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractAutomatic : MonoBehaviour {

    [SerializeField] private GameObject doorGameObject = null;
    private IDoor door;

    private void Awake() {
        door = doorGameObject.GetComponent<IDoor>();
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.GetComponent<CharacterController2D>() != null) {
            // Player entered collider!
            door.OpenDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.GetComponent<CharacterController2D>() != null) {
            // Player exited collider!
            door.CloseDoor();
        }
    }
}
