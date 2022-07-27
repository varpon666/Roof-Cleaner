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
 * Spawn this Enemy with a nice Dissolve Effect
 * */
namespace TopDownShooter {
    public class EnemySpawn : MonoBehaviour {

        public event EventHandler OnDead;

        private EnemyMain enemyMain;

        private void Awake() {
            gameObject.SetActive(false);
            enemyMain = GetComponent<EnemyMain>();
        }

        private void Start() {
            enemyMain.HealthSystem.OnDead += HealthSystem_OnDead;
        }

        private void HealthSystem_OnDead(object sender, System.EventArgs e) {
            OnDead?.Invoke(this, EventArgs.Empty);
        }

        public void Spawn() {
            gameObject.SetActive(true);
            transform.SetParent(null); // Go to root

            EnemyPathfindingMovement enemyPathfindingMovement = GetComponent<EnemyPathfindingMovement>();
            EnemyTargeting enemyTargeting = GetComponent<EnemyTargeting>();

            if (enemyPathfindingMovement != null) enemyPathfindingMovement.enabled = false;
            if (enemyTargeting != null) enemyTargeting.enabled = false;

            FunctionTimer.Create(() => {
                if (enemyPathfindingMovement != null) enemyPathfindingMovement.enabled = true;
                if (enemyTargeting != null) enemyTargeting.enabled = true;
            }, 1.5f);

            DissolveAnimate dissolveAnimate = GetComponent<DissolveAnimate>();
            if (dissolveAnimate != null) {
                float dissolveTime = 2f;
                dissolveAnimate.StartDissolve(1f, -1f / dissolveTime);
            }
        }

        public bool IsAlive() {
            return !enemyMain.HealthSystem.IsDead();
        }

        public void KillEnemy() {
            enemyMain.Damage(Player.Instance, 1000f);
        }

    }
}