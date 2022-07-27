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
 * Handles a Battle, Start, test for End, optional Door
 * */
namespace TopDownShooter {
    public class BattleSystem_Done : MonoBehaviour {

        public event EventHandler OnBattleStarted;
        public event EventHandler OnBattleEnded;

        private enum State {
            WaitingToSpawn,
            Active,
            BattleOver,
        }

        [Serializable]
        public class Wave {
            public Transform enemySpawnContainer;
            public EnemySpawn[] enemySpawnArray;
            public float time;
            public bool alreadySpawned;
        }

        [SerializeField] private Wave[] waveArray = null;

        [SerializeField] private CaptureOnTriggerEnter2D startBattleTrigger = null;
        [SerializeField] private DoorAnims doorAnims = null;

        private State state;
        private List<EnemySpawn> enemySpawnList;

        private void Awake() {
            state = State.WaitingToSpawn;
            enemySpawnList = new List<EnemySpawn>();
        }

        private void Start() {
            startBattleTrigger.OnPlayerTriggerEnter2D += StartBattleTrigger_OnPlayerTriggerEnter2D;
        }

        private void StartBattleTrigger_OnPlayerTriggerEnter2D(object sender, System.EventArgs e) {
            StartBattle();
            startBattleTrigger.OnPlayerTriggerEnter2D -= StartBattleTrigger_OnPlayerTriggerEnter2D;
        }

        private void Update() {
            switch (state) {
            case State.Active:
                foreach (Wave wave in waveArray) {
                    if (wave.alreadySpawned) continue; // Wave already spawned
                    wave.time -= Time.deltaTime;
                    if (wave.time <= 0f) {
                        wave.alreadySpawned = true;
                        SpawnWave(wave);
                    }
                }
                break;
            }
        }

        private void SpawnWave(Wave wave) {
            List<EnemySpawn> waveSpawnEnemyList = new List<EnemySpawn>();
            if (wave.enemySpawnContainer != null) {
                foreach (Transform transform in wave.enemySpawnContainer) {
                    EnemySpawn enemySpawn = transform.GetComponent<EnemySpawn>();
                    if (enemySpawn != null) {
                        waveSpawnEnemyList.Add(enemySpawn);
                    }
                }
            }

            if (wave.enemySpawnArray != null) {
                waveSpawnEnemyList.AddRange(wave.enemySpawnArray);
            }

            foreach (EnemySpawn enemySpawn in waveSpawnEnemyList) {
                enemySpawn.Spawn();
                enemySpawn.OnDead += EnemySpawn_OnDead;
                enemySpawnList.Add(enemySpawn);
            }
        }

        private void StartBattle() {
            state = State.Active;

            if (doorAnims != null) {
                doorAnims.SetColor(DoorAnims.ColorName.Red);
                doorAnims.CloseDoor();
            }

            OnBattleStarted?.Invoke(this, EventArgs.Empty);
        }

        private void EndBattle() {
            if (doorAnims != null) {
                doorAnims.SetColor(DoorAnims.ColorName.Green);
                FunctionTimer.Create(doorAnims.OpenDoor, 1.5f);
            }

            OnBattleEnded?.Invoke(this, EventArgs.Empty);
        }

        private void EnemySpawn_OnDead(object sender, System.EventArgs e) {
            TestBattleOver();
        }

        private void TestBattleOver() {
            foreach (EnemySpawn enemySpawn in enemySpawnList) {
                if (enemySpawn.IsAlive()) {
                    // Still alive
                    return;
                }
            }

            // All dead!
            EndBattle();
        }

    }
}