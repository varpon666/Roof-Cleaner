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
using CodeMonkey.Utils;

namespace TopDownShooter {
    public class EnemyTurretLogic : MonoBehaviour {

        private const float FIRE_RATE = .06f;
        private const int BULLETS_TO_FIRE = 10;

        private enum State {
            Normal,
            AimingAtTarget,
            Attacking,
            Busy,
        }

        private EnemyMain enemyMain;
        private State state;
        private Enemy.IEnemyTargetable enemyTarget;
        private IAimShootAnims aimAnims;
        private AimingShoot aimingShoot;
        private int bulletsFired;
        private float fireRate;

        private void Awake() {
            enemyMain = GetComponent<EnemyMain>();
            aimAnims = GetComponent<IAimShootAnims>();
            aimingShoot = GetComponent<AimingShoot>();

            SetStateNormal();
        }

        private void Start() {
            enemyMain.OnDamaged += EnemyMain_OnDamaged;
            Player.Instance.OnDodged += Player_OnDodged;
            enemyMain.HealthSystem.SetHealthMax(500, true);
        }

        private void Player_OnDodged(object sender, System.EventArgs e) {
            if (state == State.AimingAtTarget && aimingShoot.IsInsideDodgeAngle()) {
                SetStateNormal();
            }
        }

        public HealthSystem GetHealthSystem() {
            return enemyMain.HealthSystem;
        }

        private void EnemyMain_OnDamaged(object sender, EnemyMain.OnDamagedEventArgs e) {
            Vector3 bloodDir = (GetPosition() - e.attacker.GetPosition()).normalized;
            Blood_Handler.SpawnBlood(5, GetPosition(), bloodDir);

            enemyMain.HealthSystem.Damage(Mathf.RoundToInt(30 * e.damageMultiplier));

            if (enemyMain.HealthSystem.IsDead()) {
                //FlyingBody.Create(GameAssets.i.pfEnemyFlyingBody, GetPosition(), bloodDir);
                enemyMain.DestroySelf();
            }
        }

        private void Update() {
            switch (state) {
            case State.Normal:
                enemyTarget = enemyMain.EnemyTargeting.GetActiveTarget();
                if (enemyTarget != null) {
                    state = State.AimingAtTarget;
                    bulletsFired = 0;
                    aimingShoot.StartAimingAtTarget(enemyTarget.GetPosition(), 20f, 0f, 10f, ShootTarget);
                }
                break;
            case State.AimingAtTarget:
                enemyTarget = enemyMain.EnemyTargeting.GetActiveTarget();
                if (enemyTarget != null) {
                    Vector3 targetPosition = enemyTarget.GetPosition();
                    aimAnims.SetAimTarget(targetPosition);
                    aimingShoot.UpdateAimShootAtTarget(targetPosition);
                }
                break;
            case State.Attacking:
                fireRate -= Time.deltaTime;
                if (fireRate <= 0f) {
                    TestContinueShooting();
                }
                break;
            case State.Busy:
                break;
            }
        }

        private void ShootTarget() {
            enemyTarget = enemyMain.EnemyTargeting.GetActiveTarget();
            if (enemyTarget != null) {
                // Shoot!
                Sound_Manager.PlaySound(Sound_Manager.Sound.Rifle_Fire, GetPosition());
                Vector3 targetPosition = enemyTarget.GetPosition();

                SetStateAttacking();
                fireRate = FIRE_RATE;
                targetPosition += UtilsClass.GetRandomDir() * UnityEngine.Random.Range(-5f, 15f);
                aimAnims.ShootTarget(targetPosition, () => { });
            }
        }

        private void TestContinueShooting() {
            enemyTarget = enemyMain.EnemyTargeting.GetActiveTarget();
            if (bulletsFired < BULLETS_TO_FIRE && enemyTarget != null) {
                bulletsFired++;
                ShootTarget();
            } else {
                SetStateNormal();
            }
        }


        private void SetStateNormal() {
            state = State.Normal;
        }

        private void SetStateAttacking() {
            state = State.Attacking;
        }

        public Vector3 GetPosition() => enemyMain.GetPosition();

    }

}