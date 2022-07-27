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
using TopDownShooter;

namespace CodeMonkey.BossBattleVideo {

    public class BossBattle : MonoBehaviour {

        public event EventHandler OnBossBattleStarted;
        public event EventHandler OnBossBattleOver;

        public enum Stage {
            WaitingToStart,
            Stage_1,
            Stage_2,
            Stage_3,
        }

        [SerializeField] private CaptureOnTriggerEnter2D colliderTrigger = null;
        [SerializeField] private Transform pfHealthPickup = null;
        [SerializeField] private EnemySpawn pfEnemyShooterSpawn = null;
        [SerializeField] private EnemySpawn pfEnemyArcherSpawn = null;
        [SerializeField] private EnemySpawn pfEnemyChargerSpawn = null;
        [SerializeField] private EnemyTurretLogic enemyTurret = null;
        [SerializeField] private GameObject shield_1 = null;
        [SerializeField] private GameObject shield_2 = null;

        private List<EnemySpawn> enemySpawnList;
        private List<Vector3> spawnPositionList;
        private Stage stage;

        private void Awake() {
            enemySpawnList = new List<EnemySpawn>();
            spawnPositionList = new List<Vector3>();

            foreach (Transform spawnPosition in transform.Find("spawnPositions")) {
                spawnPositionList.Add(spawnPosition.position);
            }

            stage = Stage.WaitingToStart;
        }

        private void Start() {
            colliderTrigger.OnPlayerTriggerEnter2D += ColliderTrigger_OnPlayerEnterTrigger;

            enemyTurret.GetHealthSystem().OnDamaged += BossBattle_OnDamaged;
            enemyTurret.GetHealthSystem().OnDead += BossBattle_OnDead;

            shield_1.SetActive(false);
            shield_2.SetActive(false);
        }

        private void BossBattle_OnDead(object sender, System.EventArgs e) {
            // Turret dead! Boss battle over!
            Debug.Log("Boss Battle Over!");

            FunctionPeriodic.StopAllFunc("spawnEnemy");
            FunctionPeriodic.StopAllFunc("spawnEnemy_2");
            FunctionPeriodic.StopAllFunc("spawnHealthPickup");

            DestroyAllEnemies();

            OnBossBattleOver?.Invoke(this, EventArgs.Empty);
        }

        private void BossBattle_OnDamaged(object sender, System.EventArgs e) {
            // Turret took damage
            switch (stage) {
                case Stage.Stage_1:
                    if (enemyTurret.GetHealthSystem().GetHealthNormalized() <= .7f) {
                        // Turret under 70% health
                        StartNextStage();
                    }
                    break;
                case Stage.Stage_2:
                    if (enemyTurret.GetHealthSystem().GetHealthNormalized() <= .3f) {
                        // Turret under 30% health
                        StartNextStage();
                    }
                    break;
            }
        }

        private void ColliderTrigger_OnPlayerEnterTrigger(object sender, System.EventArgs e) {
            StartBattle();
            colliderTrigger.OnPlayerTriggerEnter2D -= ColliderTrigger_OnPlayerEnterTrigger;
        }

        private void StartBattle() {
            Debug.Log("StartBattle");
            StartNextStage();

            OnBossBattleStarted?.Invoke(this, EventArgs.Empty);
        }

        private void StartNextStage() {
            switch (stage) {
                case Stage.WaitingToStart:
                    stage = Stage.Stage_1;

                    SpawnEnemy();
                    SpawnEnemy();

                    FunctionPeriodic.Create(SpawnEnemy, 4f, "spawnEnemy");
                    FunctionPeriodic.Create(SpawnHealthPickup, 3f, "spawnHealthPickup");
                    break;
                case Stage.Stage_1:
                    stage = Stage.Stage_2;
                    shield_1.SetActive(true);
                    SpawnHealthPickup();
                    SpawnEnemy();
                    SpawnEnemy();
                    FunctionTimer.Create(SpawnEnemy, .5f);
                    FunctionTimer.Create(SpawnEnemy, 1.5f);
                    FunctionTimer.Create(SpawnEnemy, 4.5f);
                    FunctionTimer.Create(SpawnEnemy, 6.5f);
                    break;
                case Stage.Stage_2:
                    stage = Stage.Stage_3;
                    shield_2.SetActive(true);
                    SpawnHealthPickup();
                    SpawnHealthPickup();
                    SpawnEnemy();
                    SpawnEnemy();
                    SpawnEnemy();
                    FunctionTimer.Create(SpawnEnemy, .5f);
                    FunctionTimer.Create(SpawnEnemy, 1.5f);
                    FunctionTimer.Create(SpawnEnemy, 4.5f);
                    FunctionTimer.Create(SpawnEnemy, 6.5f);
                    FunctionTimer.Create(SpawnEnemy, 7.5f);
                    FunctionTimer.Create(SpawnEnemy, 8.5f);
                    FunctionPeriodic.Create(SpawnEnemy, 3f, "spawnEnemy_2");
                    break;
            }
            Debug.Log("Starting next stage: " + stage);
        }

        private void SpawnEnemy() {
            int aliveCount = 0;
            foreach (EnemySpawn enemySpawned in enemySpawnList) {
                if (enemySpawned.IsAlive()) {
                    // Enemy alive
                    aliveCount++;
                    if (aliveCount >= 7) {
                        // Don't spawn more enemies
                        return;
                    }
                }
            }

            Vector3 spawnPosition = spawnPositionList[UnityEngine.Random.Range(0, spawnPositionList.Count)];

            EnemySpawn pfEnemySpawn;
            int rnd = UnityEngine.Random.Range(0, 100);

            pfEnemySpawn = pfEnemyArcherSpawn;
            if (rnd < 65) pfEnemySpawn = pfEnemyChargerSpawn;
            if (rnd < 15) pfEnemySpawn = pfEnemyShooterSpawn;

            EnemySpawn enemySpawn = Instantiate(pfEnemySpawn, spawnPosition, Quaternion.identity);
            enemySpawn.Spawn();

            enemySpawnList.Add(enemySpawn);
        }

        private void SpawnHealthPickup() {
            Vector3 spawnPosition = spawnPositionList[UnityEngine.Random.Range(0, spawnPositionList.Count)];
            Transform healthPickupTransform = Instantiate(pfHealthPickup, spawnPosition, Quaternion.identity);
        }

        private void DestroyAllEnemies() {
            foreach (EnemySpawn enemySpawn in enemySpawnList) {
                if (enemySpawn.IsAlive()) {
                    enemySpawn.KillEnemy();
                }
            }
        }

    }

}