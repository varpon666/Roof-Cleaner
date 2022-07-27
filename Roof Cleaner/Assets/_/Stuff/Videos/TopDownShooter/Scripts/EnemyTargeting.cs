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

/*
 * Handles finding Enemy Targets
 * */
namespace TopDownShooter {
    public class EnemyTargeting : MonoBehaviour {

        private EnemyMain enemyMain;
        private Func<Enemy.IEnemyTargetable> getEnemyTarget;
        private Enemy.IEnemyTargetable activeEnemyTarget;

        private void Awake() {
            enemyMain = GetComponent<EnemyMain>();
        }

        private void Start() {
            if (getEnemyTarget == null) {
                SetGetTarget(() => Player.Instance);
            }
        }

        private void Update() {
            FindTarget();
        }

        public void SetGetTarget(Func<Enemy.IEnemyTargetable> getEnemyTarget) {
            this.getEnemyTarget = getEnemyTarget;
        }

        private void FindTarget() {
            float targetRange = enemyMain.EnemyStats.targetRange;
            activeEnemyTarget = null;
            if (getEnemyTarget != null) {
                if (Vector3.Distance(getEnemyTarget().GetPosition(), enemyMain.GetPosition()) < targetRange) {
                    // Target within range
                    activeEnemyTarget = getEnemyTarget();
                }
            }
        }

        public Enemy.IEnemyTargetable GetActiveTarget() {
            return activeEnemyTarget;
        }

    }
}