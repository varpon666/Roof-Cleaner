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

/*
 * Enemy Charger
 * */
namespace TopDownShooter {
    public class EnemyChargerLogic : MonoBehaviour {

        private enum State {
            Normal,
            Charging,
            Attacking,
            Busy,
        }

        private EnemyMain enemyMain;
        private Character_Base characterBase;
        private State state;
        private Enemy.IEnemyTargetable enemyTarget;
        private Vector3 chargeDir;
        private float chargeSpeed;
        private float chargeDelay;
        private Transform aimTransform;

        private void Awake() {
            enemyMain = GetComponent<EnemyMain>();
            characterBase = GetComponent<Character_Base>();

            aimTransform = transform.Find("Aim");

            SetStateNormal();
            HideAim();
        }

        private void Start() {
            enemyMain.HealthSystem.SetHealthMax(120, true);
        }

        private void Update() {
            switch (state) {
            case State.Normal:
                chargeDelay -= Time.deltaTime;
                enemyTarget = enemyMain.EnemyTargeting.GetActiveTarget();
                if (enemyTarget != null) {
                    Vector3 targetPosition = enemyTarget.GetPosition();
                    if (chargeDelay > 0) {
                        // Too soon to charge, move to it
                        enemyMain.EnemyPathfindingMovement.MoveToTimer(targetPosition);
                        //Debug.Log(enemyMain.EnemyPathfindingMovement);
                    } else {
                        if (CanChargeToTarget(targetPosition, enemyTarget.GetGameObject())) {
                            chargeDir = (targetPosition - GetPosition()).normalized;
                            SetAimTarget(targetPosition);
                            ShowAim();
                            chargeSpeed = 200f;
                            enemyMain.EnemyPathfindingMovement.Disable();
                            characterBase.PlayDodgeAnimation(chargeDir);
                            state = State.Charging;
                        } else {
                            // Cannot see target, move to it
                            enemyMain.EnemyPathfindingMovement.MoveToTimer(targetPosition);
                        }
                    }
                }
                break;
            case State.Charging:
                float chargeSpeedDropMultiplier = 1f;
                chargeSpeed -= chargeSpeed * chargeSpeedDropMultiplier * Time.deltaTime;

                float hitDistance = 3f;
                int hitLayerMask = ~0;
                RaycastHit2D raycastHit2D = Physics2D.Raycast(GetPosition(), chargeDir, hitDistance, hitLayerMask);// 1 << GameAssets.i.playerLayer);
                if (raycastHit2D.collider != null) {
                    Player player = raycastHit2D.collider.GetComponent<Player>();
                    if (player != null) {
                        player.Damage(enemyMain.Enemy, .6f);
                        player.Knockback(chargeDir, 5f);
                        chargeSpeed = 60f;
                        chargeDir *= -1f;
                    }
                }

                float chargeSpeedMinimum = 70f;
                if (chargeSpeed < chargeSpeedMinimum) {
                    state = State.Normal;
                    enemyMain.EnemyPathfindingMovement.Enable();
                    chargeDelay = 1.5f;
                    SetStateNormal();
                    HideAim();
                }
                break;
            case State.Attacking:
                break;
            case State.Busy:
                break;
            }
        }

        private void FixedUpdate() {
            switch (state) {
            case State.Charging:
                enemyMain.EnemyRigidbody2D.velocity = chargeDir * chargeSpeed;
                break;
            }
        }

        private bool CanChargeToTarget(Vector3 targetPosition, GameObject targetGameObject) {
            float targetDistance = Vector3.Distance(GetPosition(), targetPosition);

            float maxChargeDistance = 70f;
            if (targetDistance > maxChargeDistance) {
                return false;
            }

            Vector3 dirToTarget = (targetPosition - GetPosition()).normalized;
            int hitLayerMask = ~0;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(GetPosition(), dirToTarget, targetDistance, hitLayerMask);// ~(1 << GameAssets.i.enemyLayer | 1 << GameAssets.i.ignoreRaycastLayer));
            return raycastHit2D.collider == null || raycastHit2D.collider.gameObject == targetGameObject;
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