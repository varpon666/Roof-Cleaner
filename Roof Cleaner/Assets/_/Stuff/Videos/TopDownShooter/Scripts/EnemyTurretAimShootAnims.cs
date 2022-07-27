/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

/*
 * Handles Aim and Shoot visuals
 * */
namespace TopDownShooter {
    public class EnemyTurretAimShootAnims : MonoBehaviour, IAimShootAnims {

        public event EventHandler<CharacterAim_Base.OnShootEventArgs> OnShoot;

        private Transform turretAimTransform;
        private Transform turretAimGunEndPointTransform;
        private Animator turretAnimator;

        private void Awake() {
            turretAimTransform = transform.Find("TurretAim");
            turretAimGunEndPointTransform = turretAimTransform.Find("GunEndPointTransform");
            turretAnimator = GetComponent<Animator>();
        }

        public void SetAimTarget(Vector3 targetPosition) {
            Vector3 aimDir = (targetPosition - transform.position).normalized;
            turretAimTransform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(aimDir));
        }

        public void ShootTarget(Vector3 targetPosition, Action onShootComplete) {
            SetAimTarget(targetPosition);

            // Check for hits
            Vector3 gunEndPointPosition = turretAimGunEndPointTransform.position;
            //int layerMask = ~(1 << GameAssets.i.enemyLayer | 1 << GameAssets.i.ignoreRaycastLayer | 1 << GameAssets.i.shieldLayer);
            int layerMask = ~0;
            RaycastHit2D raycastHit = Physics2D.Raycast(gunEndPointPosition, (targetPosition - gunEndPointPosition).normalized, Vector3.Distance(gunEndPointPosition, targetPosition), layerMask);
            GameObject hitObject = null;
            if (raycastHit.collider != null) {
                // Hit something
                targetPosition = (Vector3)raycastHit.point + (targetPosition - gunEndPointPosition).normalized;
                hitObject = raycastHit.collider.gameObject;
            }

            turretAnimator.SetTrigger("Shoot");

            OnShoot?.Invoke(this, new CharacterAim_Base.OnShootEventArgs {
                gunEndPointPosition = turretAimGunEndPointTransform.position,
                hitObject = hitObject,
                shootPosition = targetPosition,
            });
            onShootComplete();
        }

    }

}