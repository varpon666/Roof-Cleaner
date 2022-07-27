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
using CodeMonkey;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;
using GridPathfindingSystem;

public class GameHandler_Setup : MonoBehaviour {

    public static GridPathfinding gridPathfinding;

    [SerializeField] private CameraFollow cameraFollow = null;
    [SerializeField] private float cameraZoom = 50f;
    [SerializeField] private Transform followTransform = null;
    [SerializeField] private bool cameraPositionWithMouse = true;

    [SerializeField] private CharacterAimHandler characterAimHandler = null;
    [SerializeField] private CharacterAim_Base characterAimBase = null;

    private void Start() {
        //Sound_Manager.Init();
        cameraFollow.Setup(GetCameraPosition, () => cameraZoom == -1 ? 60f : cameraZoom, true, true);

        //FunctionPeriodic.Create(SpawnEnemy, 1.5f);
        //for (int i = 0; i < 1000; i++) SpawnEnemy();
        
        gridPathfinding = new GridPathfinding(new Vector3(-400, -400), new Vector3(400, 400), 5f);
        gridPathfinding.RaycastWalkable();

        //EnemyHandler.Create(new Vector3(20, 0));
        
        if (characterAimHandler != null) {
            characterAimHandler.OnShoot += CharacterAimHandler_OnShoot;
        }
        if (characterAimBase != null) {
            characterAimBase.OnShoot += CharacterAimBase_OnShoot;
        }
    }

    private void CharacterAimBase_OnShoot(object sender, CharacterAim_Base.OnShootEventArgs e) {
        Shoot_Flash.AddFlash(e.gunEndPointPosition);
        WeaponTracer.Create(e.gunEndPointPosition, e.shootPosition);
        UtilsClass.ShakeCamera(.6f, .05f);

        // Any enemy hit?
        RaycastHit2D raycastHit = Physics2D.Raycast(e.gunEndPointPosition, (e.shootPosition - e.gunEndPointPosition).normalized, Vector3.Distance(e.gunEndPointPosition, e.shootPosition));
        if (raycastHit.collider != null) {
            EnemyHandler enemyHandler = raycastHit.collider.gameObject.GetComponent<EnemyHandler>();
            if (enemyHandler != null) {
                Debug.Log("Cannot Damage!");
                //enemyHandler.Damage(characterAimBase);
            }
        }
    }

    private void CharacterAimHandler_OnShoot(object sender, CharacterAimHandler.OnShootEventArgs e) {
        Shoot_Flash.AddFlash(e.gunEndPointPosition);
        WeaponTracer.Create(e.gunEndPointPosition, e.shootPosition);
        UtilsClass.ShakeCamera(.6f, .05f);

        // Any enemy hit?
        RaycastHit2D raycastHit = Physics2D.Raycast(e.gunEndPointPosition, (e.shootPosition - e.gunEndPointPosition).normalized, Vector3.Distance(e.gunEndPointPosition, e.shootPosition));
        if (raycastHit.collider != null) {
            EnemyHandler enemyHandler = raycastHit.collider.gameObject.GetComponent<EnemyHandler>();
            if (enemyHandler != null) {
                enemyHandler.Damage(characterAimHandler);
            }
        }
    }

    private Vector3 GetCameraPosition() {
        if (followTransform == null) {
            return cameraFollow.transform.position;
        }

        if (cameraPositionWithMouse) {
            Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
            Vector3 playerToMouseDirection = mousePosition - followTransform.position;
            return followTransform.position + playerToMouseDirection * .3f;
        } else {
            return followTransform.position;
        }
    }

    private void SpawnEnemy() {
        Vector3 spawnPosition = Vector3.zero + UtilsClass.GetRandomDir() * 40f;
        if (characterAimHandler != null) {
            spawnPosition = characterAimHandler.GetPosition() + UtilsClass.GetRandomDir() * 40f;
        }
        EnemyHandler.Create(spawnPosition);
    }
}
