using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletRaycast {

    public static void Shoot(Vector3 shootPosition, Vector3 shootDirection) {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(shootPosition, shootDirection);

        if (raycastHit2D.collider != null) {
            // Hit!
            Target target = raycastHit2D.collider.GetComponent<Target>();
            if (target != null) {
                target.Damage();
            }
        }
    }

}
