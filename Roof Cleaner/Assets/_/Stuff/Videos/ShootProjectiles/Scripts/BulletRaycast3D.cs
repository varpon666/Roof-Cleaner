using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletRaycast3D {

    public static void Shoot(Vector3 shootPosition, Vector3 shootDirection) {
        if (Physics.Raycast(shootPosition, shootDirection, out RaycastHit raycastHit)) {
            // Hit!
            Target target = raycastHit.collider.GetComponent<Target>();
            if (target != null) {
                target.Damage();
            }
        }
    }

}
