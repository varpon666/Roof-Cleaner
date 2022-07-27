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
    public class EnemyDropLoot : MonoBehaviour {

        [SerializeField] private Transform lootTransform = null;
        private EnemyMain enemyMain;

        private void Awake() {
            enemyMain = GetComponent<EnemyMain>();
        }

        private void Start() {
            enemyMain.OnDestroySelf += EnemyMain_OnDestroySelf;
        }

        private void EnemyMain_OnDestroySelf(object sender, System.EventArgs e) {
            Instantiate(lootTransform, enemyMain.GetPosition(), Quaternion.identity);
        }

    }

}