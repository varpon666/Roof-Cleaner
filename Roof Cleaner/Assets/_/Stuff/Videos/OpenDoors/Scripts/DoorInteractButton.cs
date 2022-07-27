using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractButton : MonoBehaviour {

    [SerializeField] private Transform playerTransform = null;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            float interactRadius = 40f;
            Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(playerTransform.position, interactRadius);
            foreach (Collider2D collider2D in collider2DArray) {
                IDoor door = collider2D.GetComponent<IDoor>();
                if (door != null) {
                    // There's a Door in range!
                    door.ToggleDoor();
                }
            }
        }
    }
}
