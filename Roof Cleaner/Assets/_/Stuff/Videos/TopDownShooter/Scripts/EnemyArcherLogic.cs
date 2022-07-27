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
using System.Collections.Generic;
using UnityEngine;
using V_AnimationSystem;
using CodeMonkey.Utils;
using CodeMonkey;

/*
 * Enemy Archer, throw Shuriken
 * */
namespace TopDownShooter {
    public class EnemyArcherLogic : MonoBehaviour {

        private enum State {
            Normal,
            Attacking,
            Busy,
        }

        private EnemyMain enemyMain;
        private Character_Base characterBase;
        private State state;
        private Enemy.IEnemyTargetable enemyTarget;
        private Transform aimTransform;

        private void Awake() {
            enemyMain = GetComponent<EnemyMain>();
            characterBase = GetComponent<Character_Base>();

            aimTransform = transform.Find("Aim");

            SetStateNormal();
            HideAim();
        }

        private void Start() {
            enemyMain.HealthSystem.SetHealthMax(50, true);
        }

        private void Update() {
            switch (state) {
            case State.Normal:
                enemyTarget = enemyMain.EnemyTargeting.GetActiveTarget();
                if (enemyTarget != null) {
                    Vector3 targetPosition = enemyTarget.GetPosition();
                    enemyMain.EnemyPathfindingMovement.MoveToTimer(targetPosition);
                    float attackDistance = 80f;
                    float targetDistance = Vector3.Distance(GetPosition(), targetPosition);
                    if (targetDistance < attackDistance) {
                        // Target within attack distance
                        //int layerMask = ~(1 << GameAssets.i.enemyLayer | 1 << GameAssets.i.ignoreRaycastLayer | 1 << GameAssets.i.shieldLayer);
                        int layerMask = ~0;
                        RaycastHit2D raycastHit2D = Physics2D.Raycast(GetPosition(), (targetPosition - GetPosition()).normalized, targetDistance, layerMask);
                        if (raycastHit2D.collider != null && raycastHit2D.collider.GetComponent<Player>()) {
                            // Player in line of sight
                            enemyMain.EnemyPathfindingMovement.Disable();
                            SetStateAttacking();
                            Vector3 targetDir = (targetPosition - GetPosition()).normalized;
                            characterBase.PlayPunchAnimation(targetDir, (Vector3 hitPosition) => {
                                // Throw rock
                                enemyTarget = enemyMain.EnemyTargeting.GetActiveTarget();
                                if (enemyTarget != null) {
                                    Vector3 throwDir = (enemyTarget.GetPosition() - hitPosition).normalized;
                                    EnemyShuriken enemyShuriken = EnemyShuriken.Create(enemyMain.Enemy, hitPosition, throwDir);
                                }
                                //CMDebug.TextPopup("#", hitPosition);
                            }, () => {
                                // Punch complete
                                enemyMain.EnemyPathfindingMovement.Enable();
                                SetStateNormal();
                            });
                        }
                    }
                }
                break;
            case State.Attacking:
                break;
            case State.Busy:
                break;
            }
        }

        public void SetAimTarget(Vector3 targetPosition) {
            Vector3 aimDir = (targetPosition - transform.position).normalized;
            aimTransform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(aimDir));
        }

        private void ShowAim() {
            aimTransform.gameObject.SetActive(true);
        }

        private void HideAim() {
            aimTransform.gameObject.SetActive(false);
        }

        private void SetStateNormal() {
            state = State.Normal;
        }

        private void SetStateAttacking() {
            state = State.Attacking;
        }

        public Vector3 GetPosition() {
            return transform.position;
        }

    }
}