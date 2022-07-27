using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class SpawnEnemyOnKeyDown : MonoBehaviour {

    [SerializeField] private Transform pfEnemyTransform = null;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.M)) {
            int spawnCount = 5;
            Vector3 spawnPosition = Vector3.zero;
            float radiusMin = 50f;
            float radiusMax = 150f;
            for (int i = 0; i < spawnCount; i++) {
                Transform enemyTransform = Instantiate(pfEnemyTransform, spawnPosition + UtilsClass.GetRandomDir() * Random.Range(radiusMin, radiusMax), Quaternion.identity);
            }
        }
    }

}
