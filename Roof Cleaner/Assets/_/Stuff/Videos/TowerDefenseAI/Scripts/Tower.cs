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
using CodeMonkey;
using CodeMonkey.Utils;

public class Tower : MonoBehaviour {

    private Vector3 projectileShootFromPosition;
    private float range;
    private int damageAmount;
    private float shootTimerMax;
    private float shootTimer;

    private void Awake() {
        projectileShootFromPosition = transform.Find("ProjectileShootFromPosition").position;
        range = 60f;
        damageAmount = 25;
        shootTimerMax = .4f;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            //CMDebug.TextPopupMouse("Click!");
            //ProjectileArrow.Create(projectileShootFromPosition, UtilsClass.GetMouseWorldPosition());
        }

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f) {
            shootTimer = shootTimerMax;

            Enemy enemy = GetClosestEnemy();
            if (enemy != null) {
                // Enemy in range!
                ProjectileArrow.Create(projectileShootFromPosition, enemy, Random.Range(damageAmount - 5, damageAmount + 5));
            }
        }
    }

    private Enemy GetClosestEnemy() {
        return Enemy.GetClosestEnemy(transform.position, range);
    }

    public float GetRange() {
        return range;
    }

    public void UpgradeRange() {
        range += 10f;
    }

    public void UpgradeDamageAmount() {
        damageAmount += 5;
    }

    private void OnMouseEnter() {
        UpgradeOverlay.Show_Static(this);
    }

}
