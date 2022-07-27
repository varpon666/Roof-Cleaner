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
using TopDownShooter;

namespace CodeMonkey.BattleSystemVideo {

    public class BattleSystem : MonoBehaviour {

        public event EventHandler OnBattleStarted;
        public event EventHandler OnBattleOver;

        private enum State {
            Idle,
            Active,
            BattleOver,
        }

        [SerializeField] private CaptureOnTriggerEnter2D colliderTrigger = null;
        [SerializeField] private Wave[] waveArray = null;

        private State state;

        private void Awake() {
            state = State.Idle;
        }

        private void Start() {
            colliderTrigger.OnPlayerTriggerEnter2D += ColliderTrigger_OnPlayerEnterTrigger;
        }

        private void ColliderTrigger_OnPlayerEnterTrigger(object sender, System.EventArgs e) {
            if (state == State.Idle) {
                StartBattle();
                colliderTrigger.OnPlayerTriggerEnter2D -= ColliderTrigger_OnPlayerEnterTrigger;
            }
        }

        private void StartBattle() {
            Debug.Log("StartBattle");
            state = State.Active;
            OnBattleStarted?.Invoke(this, EventArgs.Empty);
        }

        private void Update() {
            switch (state) {
                case State.Active:
                    foreach (Wave wave in waveArray) {
                        wave.Update();
                    }

                    TestBattleOver();
                    break;
            }
        }

        private void TestBattleOver() {
            if (state == State.Active) {
                if (AreWavesOver()) {
                    // Battle is over!
                    state = State.BattleOver;
                    Debug.Log("Battle Over!");
                    OnBattleOver?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private bool AreWavesOver() {
            foreach (Wave wave in waveArray) {
                if (wave.IsWaveOver()) {
                    // Wave is over
                } else {
                    // Wave not over
                    return false;
                }
            }

            return true;
        }


        /*
         * Represents a single Enemy Spawn Wave
         * */
        [System.Serializable]
        private class Wave {

            [SerializeField] private EnemySpawn[] enemySpawnArray = null;
            [SerializeField] private float timer = 1f;

            public void Update() {
                if (timer >= 0) {
                    timer -= Time.deltaTime;
                    if (timer < 0) {
                        SpawnEnemies();
                    }
                }
            }

            private void SpawnEnemies() {
                foreach (EnemySpawn enemySpawn in enemySpawnArray) {
                    enemySpawn.Spawn();
                }
            }

            public bool IsWaveOver() {
                if (timer < 0) {
                    // Wave spawned
                    foreach (EnemySpawn enemySpawn in enemySpawnArray) {
                        if (enemySpawn.IsAlive()) {
                            return false;
                        }
                    }
                    return true;
                } else {
                    // Enemies haven't spawned yet
                    return false;
                }
            }
        }

    }

}