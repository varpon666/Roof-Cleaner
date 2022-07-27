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
 * Enemy Class References
 * */
namespace TopDownShooter {
    public class EnemyMain : MonoBehaviour {

        public event EventHandler OnDestroySelf;
        public event EventHandler<OnDamagedEventArgs> OnDamaged;
        public class OnDamagedEventArgs {
            public Player attacker;
            public float damageMultiplier;
        }

        public Enemy Enemy { get; private set; }

        public EnemyPathfindingMovement EnemyPathfindingMovement { get; private set; }
        public EnemyTargeting EnemyTargeting { get; private set; }
        public EnemyStats EnemyStats { get; private set; }
        public Rigidbody2D EnemyRigidbody2D { get; private set; }
        public ICharacterAnims CharacterAnims { get; private set; }
        public IAimShootAnims AimShootAnims { get; private set; }

        public HealthSystem HealthSystem { get; private set; }

        private void Awake() {
            Enemy = GetComponent<Enemy>();

            EnemyPathfindingMovement = GetComponent<EnemyPathfindingMovement>();
            EnemyTargeting = GetComponent<EnemyTargeting>();
            EnemyStats = GetComponent<EnemyStats>();
            EnemyRigidbody2D = GetComponent<Rigidbody2D>();
            CharacterAnims = GetComponent<ICharacterAnims>();
            AimShootAnims = GetComponent<IAimShootAnims>();

            HealthSystem = new HealthSystem(100);
        }

        public Vector3 GetPosition() {
            return transform.position;
        }

        public void DestroySelf() {
            OnDestroySelf?.Invoke(this, EventArgs.Empty);
        }

        public void Damage(Player attacker, float damageMultiplier) {
            OnDamaged?.Invoke(this, new OnDamagedEventArgs {
                attacker = attacker,
                damageMultiplier = damageMultiplier,
            });
        }

    }

}