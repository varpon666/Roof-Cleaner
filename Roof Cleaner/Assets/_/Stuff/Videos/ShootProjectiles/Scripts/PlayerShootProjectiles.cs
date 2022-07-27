using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class PlayerShootProjectiles : MonoBehaviour {

    [SerializeField] private Transform pfBullet = null;
    [SerializeField] private Transform pfBulletPhysics = null;

    private delegate void ShootActionDelegate(Vector3 gunEndPointPosition, Vector3 shootPosition);
    private ShootActionDelegate shootAction;

    private void Awake() {
        shootAction = ShootPhysics;
        GetComponent<CharacterAim_Base>().OnShoot += PlayerShootProjectiles_OnShoot;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) shootAction = ShootTransform;
        if (Input.GetKeyDown(KeyCode.Y)) shootAction = ShootPhysics;
        if (Input.GetKeyDown(KeyCode.U)) shootAction = ShootRaycast;
    }

    private void PlayerShootProjectiles_OnShoot(object sender, CharacterAim_Base.OnShootEventArgs e) {
        // Shoot
        shootAction(e.gunEndPointPosition, e.shootPosition);
    }

    private void ShootRaycast(Vector3 gunEndPointPosition, Vector3 shootPosition) {
        Vector3 shootDir = (shootPosition - gunEndPointPosition).normalized;
        BulletRaycast.Shoot(gunEndPointPosition, shootDir);

        WeaponTracer.Create(gunEndPointPosition, UtilsClass.GetMouseWorldPosition());
    }

    private void ShootPhysics(Vector3 gunEndPointPosition, Vector3 shootPosition) {
        Transform bulletTransform = Instantiate(pfBulletPhysics, gunEndPointPosition, Quaternion.identity);

        Vector3 shootDir = (shootPosition - gunEndPointPosition).normalized;
        bulletTransform.GetComponent<BulletPhysics>().Setup(shootDir);
    }

    private void ShootTransform(Vector3 gunEndPointPosition, Vector3 shootPosition) {
        Transform bulletTransform = Instantiate(pfBullet, gunEndPointPosition, Quaternion.identity);

        Vector3 shootDir = (shootPosition - gunEndPointPosition).normalized;
        bulletTransform.GetComponent<Bullet>().Setup(shootDir);
    }

}
