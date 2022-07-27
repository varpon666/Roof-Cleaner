using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class CameraTarget : MonoBehaviour {

    public enum Axis {
        XZ,
        XY,
    }

    [SerializeField] private Axis axis = Axis.XZ;
    [SerializeField] private float moveSpeed = 50f;



    private void Update() {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W)) {
            moveY = +1f;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveX = +1f;
        }

        Vector3 moveDir;

        switch (axis) {
            default:
            case Axis.XZ:
                moveDir = new Vector3(moveX, 0, moveY).normalized;
                break;
            case Axis.XY:
                moveDir = new Vector3(moveX, moveY).normalized;
                break;
        }
        
        if (moveX != 0 || moveY != 0) {
            // Not idle
        }

        if (axis == Axis.XZ) {
            moveDir = UtilsClass.ApplyRotationToVectorXZ(moveDir, 30f);
        }

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

}
