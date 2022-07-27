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

public class TowerDefenseAI : MonoBehaviour {

    private Grid<GridNode> grid;

    private void Awake() {
        grid = new Grid<GridNode>(20, 10, 25f, Vector3.zero, (Grid<GridNode> g, int x, int y) => new GridNode(g, x, y));
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.B)) {
            //SpawnEnemyWave_1();
        }
        if (Input.GetKeyDown(KeyCode.N)) {
            //SpawnEnemyWave_2();
        }
        if (Input.GetKeyDown(KeyCode.M)) {
            //SpawnEnemyWave_3();
        }

        if (Input.GetMouseButtonDown(1)) {
            //SpawnTower();
        }
    }
    /*
    private void SpawnTower() {
        Vector3 spawnPosition = UtilsClass.GetMouseWorldPosition();
        spawnPosition = ValidateWorldGridPosition(spawnPosition);
        spawnPosition += new Vector3(1, 1, 0) * grid.GetCellSize() * .5f;

        Instantiate(GameAssets.i.pfTower, spawnPosition, Quaternion.identity);
    }


    private Vector3 ValidateWorldGridPosition(Vector3 position) {
        grid.GetXY(position, out int x, out int y);
        return grid.GetWorldPosition(x, y);
    }

    private void SpawnEnemyWave_1() {
        float spawnTime = 0f;
        float timePerSpawn = .6f;

        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Yellow), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Yellow), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Orange), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Yellow), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Yellow), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Orange), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Yellow), spawnTime); spawnTime += timePerSpawn;
    }

    private void SpawnEnemyWave_2() {
        float spawnTime = 0f;
        float timePerSpawn = .5f;

        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Yellow), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Orange), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Orange), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Yellow), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Red), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Yellow), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Red), spawnTime); spawnTime += timePerSpawn;
    }

    private void SpawnEnemyWave_3() {
        float spawnTime = 0f;
        float timePerSpawn = .4f;

        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Yellow), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Orange), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Red), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Orange), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Orange), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Red), spawnTime); spawnTime += timePerSpawn;
        FunctionTimer.Create(() => SpawnEnemy(Enemy.EnemyType.Red), spawnTime); spawnTime += timePerSpawn;
    }

    private void SpawnEnemy(Enemy.EnemyType enemyType) {
        Vector3 spawnPosition = new Vector3(375, 138);
        List<Vector3> waypointPositionList = new List<Vector3> { 
            new Vector3(218, 138),
            new Vector3(210, 122.5f),
            new Vector3(210, 61.8f),
            new Vector3(-15f, 62.1f),
        };

        Enemy enemy = Enemy.Create(spawnPosition, enemyType);
        enemy.SetPathVectorList(waypointPositionList);

        //FunctionTimer.Create(() => enemy.Damage(15), .2f);
    }
    */

    public class GridNode {

        private Grid<GridNode> grid;
        private int x;
        private int y;

        public GridNode(Grid<GridNode> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;

            Vector3 worldPos00 = grid.GetWorldPosition(x, y);
            Vector3 worldPos10 = grid.GetWorldPosition(x + 1, y);
            Vector3 worldPos01 = grid.GetWorldPosition(x, y + 1);
            Vector3 worldPos11 = grid.GetWorldPosition(x + 1, y + 1);

            Debug.DrawLine(worldPos00, worldPos01, Color.white, 999f);
            Debug.DrawLine(worldPos00, worldPos10, Color.white, 999f);
            Debug.DrawLine(worldPos01, worldPos11, Color.white, 999f);
            Debug.DrawLine(worldPos10, worldPos11, Color.white, 999f);
        }
        
    }

}
