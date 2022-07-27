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

namespace TopDownShooter {
    public class EnemyShuriken : MonoBehaviour {

        private const float MOVE_SPEED = 90f;

        public static EnemyShuriken Create(Enemy enemySpawner, Vector3 spawnPosition, Vector3 moveDir) {
            Transform enemyShurikenTransform = null;// Instantiate(GameAssets.i.pfEnemyShuriken, spawnPosition, Quaternion.identity);

            EnemyShuriken enemyShuriken = enemyShurikenTransform.GetComponent<EnemyShuriken>();
            enemyShuriken.Setup(enemySpawner, moveDir);

            return enemyShuriken;
        }

        private Enemy enemySpawner;
        private Vector3 moveDir;
        private Rigidbody2D shurikenRigidbody2D;
        private Transform particleTransform;

        private void Awake() {
            shurikenRigidbody2D = GetComponent<Rigidbody2D>();
            shurikenRigidbody2D.AddTorque(30f, ForceMode2D.Impulse);
            particleTransform = transform.Find("ParticleSystem");
        }

        private void Setup(Enemy enemySpawner, Vector3 moveDir) {
            this.enemySpawner = enemySpawner;
            this.moveDir = moveDir;
        }

        private void FixedUpdate() {
            shurikenRigidbody2D.velocity = moveDir * MOVE_SPEED;
        }

        private void OnTriggerEnter2D(Collider2D collider) {
            //int hitLayerMask = ~(1 << GameAssets.i.enemyLayer | 1 << GameAssets.i.ignoreRaycastLayer);
            int hitLayerMask = ~0;
            bool colliderInLayerMask = ((1 << collider.gameObject.layer) & hitLayerMask) != 0;
            if (colliderInLayerMask) {
                // Touching a target layer
                Player player = collider.GetComponent<Player>();
                if (player != null) {
                    player.Damage(enemySpawner, 1f);
                } else {
                    // Hit something else
                }
                particleTransform.SetParent(null);
                particleTransform.GetComponent<ParticleSystem>().Stop();
                Destroy(particleTransform.gameObject, 1f);
                Destroy(gameObject);
                /*
                GetComponent<Collider2D>().enabled = false;
                shurikenRigidbody2D.velocity = Vector2.zero;
                this.enabled = false;
                transform.Find("Sprite").gameObject.SetActive(false);
                */
            }
        }

    }

}