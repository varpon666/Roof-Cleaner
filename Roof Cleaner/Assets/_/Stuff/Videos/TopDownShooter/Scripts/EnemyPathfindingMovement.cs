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
using GridPathfindingSystem;

/*
 * Responsible for all Enemy Movement Pathfinding
 * */
namespace TopDownShooter {
    public class EnemyPathfindingMovement : MonoBehaviour {

        private const float SPEED = 30f;

        private EnemyMain enemyMain;
        private List<Vector3> pathVectorList;
        private int currentPathIndex;
        private float pathfindingTimer;
        private Vector3 moveDir;
        private Vector3 lastMoveDir;

        private void Awake() {
            enemyMain = GetComponent<EnemyMain>();
        }

        private void Update() {
            pathfindingTimer -= Time.deltaTime;

            HandleMovement();
        }

        private void FixedUpdate() {
            enemyMain.EnemyRigidbody2D.velocity = moveDir * SPEED;
        }

        private void HandleMovement() {
            PrintPathfindingPath();
            if (pathVectorList != null) {
                Vector3 targetPosition = pathVectorList[currentPathIndex];
                float reachedTargetDistance = 5f;
                if (Vector3.Distance(GetPosition(), targetPosition) > reachedTargetDistance) {
                    moveDir = (targetPosition - GetPosition()).normalized;
                    //Debug.Log(moveDir + " " + targetPosition + " " + Vector3.Distance(GetPosition(), targetPosition));
                    lastMoveDir = moveDir;
                    enemyMain.CharacterAnims.PlayMoveAnim(moveDir);
                } else {
                    currentPathIndex++;
                    if (currentPathIndex >= pathVectorList.Count) {
                        StopMoving();
                        enemyMain.CharacterAnims.PlayIdleAnim();
                    }
                }
            } else {
                enemyMain.CharacterAnims.PlayIdleAnim();
            }
        }

        public void StopMoving() {
            pathVectorList = null;
            moveDir = Vector3.zero;
        }

        public List<Vector3> GetPathVectorList() {
            return pathVectorList;
        }

        private void PrintPathfindingPath() {
            if (pathVectorList != null) {
                for (int i = 0; i < pathVectorList.Count - 1; i++) {
                    Debug.DrawLine(pathVectorList[i], pathVectorList[i + 1]);
                }
            }
        }

        public void MoveToTimer(Vector3 targetPosition) {
            if (pathfindingTimer <= 0f) {
                SetTargetPosition(targetPosition);
            }
        }

        public void SetTargetPosition(Vector3 targetPosition) {
            currentPathIndex = 0;

            pathVectorList = GridPathfinding.instance.GetPathRouteWithShortcuts(GetPosition(), targetPosition).pathVectorList;
            pathfindingTimer = .1f;
            //pathVectorList = new List<Vector3> { targetPosition };

            if (pathVectorList != null && pathVectorList.Count > 1) {
                pathVectorList.RemoveAt(0);
            }
        }

        public Vector3 GetPosition() {
            return transform.position;
        }

        public Vector3 GetLastMoveDir() {
            return lastMoveDir;
        }

        public void Enable() {
            enabled = true;
        }

        public void Disable() {
            enabled = false;
            enemyMain.EnemyRigidbody2D.velocity = Vector3.zero;
        }

    }

}