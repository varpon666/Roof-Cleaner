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
using System.Collections.Generic;
using UnityEngine;
using V_AnimationSystem;
using CodeMonkey.Utils;

public class EnemyAim : MonoBehaviour {
    
    public interface IEnemyTargetable {
        Vector3 GetPosition();
        void Damage(EnemyAim attacker);
    }

    public static List<EnemyAim> enemyList = new List<EnemyAim>();

    public static EnemyAim GetClosestEnemy(Vector3 position, float maxRange) {
        EnemyAim closest = null;
        foreach (EnemyAim enemy in enemyList) {
            if (enemy.IsDead()) continue;
            if (Vector3.Distance(position, enemy.GetPosition()) <= maxRange) {
                if (closest == null) {
                    closest = enemy;
                } else {
                    if (Vector3.Distance(position, enemy.GetPosition()) < Vector3.Distance(position, closest.GetPosition())) {
                        closest = enemy;
                    }
                }
            }
        }
        return closest;
    }

    public static EnemyAim Create(Vector3 position) {
        Transform enemyTransform = Instantiate(GameAssets.i.pfEnemy, position, Quaternion.identity);

        EnemyAim enemyHandler = enemyTransform.GetComponent<EnemyAim>();

        return enemyHandler;
    }


    private const float SPEED = 30f;
    private const float FIRE_RATE = .2f;

    private CharacterAim_Base characterBase;
    private HealthSystem healthSystem;
    private float nextShootTime;
    private State state;

    private Vector3 lastMoveDir;
    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    private float pathfindingTimer;

    private Func<IEnemyTargetable> getEnemyTarget;


    private enum State {
        Normal,
        Attacking,
        Busy,
    }

    private void Awake() {
        enemyList.Add(this);
        characterBase = gameObject.GetComponent<CharacterAim_Base>();
        healthSystem = new HealthSystem(1);
        SetStateNormal();

        characterBase.OnShoot += CharacterBase_OnShoot;
    }

    private void CharacterBase_OnShoot(object sender, CharacterAim_Base.OnShootEventArgs e) {
        Shoot_Flash.AddFlash(e.gunEndPointPosition);
        WeaponTracer.Create(e.gunEndPointPosition, e.shootPosition);
        UtilsClass.ShakeCamera(.3f, .05f);

        // Any hit? Player?
        RaycastHit2D raycastHit = Physics2D.Raycast(e.gunEndPointPosition, (e.shootPosition - e.gunEndPointPosition).normalized, Vector3.Distance(e.gunEndPointPosition, e.shootPosition));
        if (raycastHit.collider != null) {
            PlayerAim player = raycastHit.collider.gameObject.GetComponent<PlayerAim>();
            if (player != null) {
                player.Damage(this);
            }
        }
    }

    public void SetGetTarget(Func<IEnemyTargetable> getEnemyTarget) {
        this.getEnemyTarget = getEnemyTarget;
    }

    private void Update() {
        pathfindingTimer -= Time.deltaTime;

        switch (state) {
        case State.Normal:
            HandleMovement();
            FindTarget();
            break;
        case State.Attacking:
            break;
        case State.Busy:
            break;
        }
    }
    
    private void SetStateNormal() {
        state = State.Normal;
    }
    
    private void FindTarget() {
        float targetRange = 160f;
        float attackRange = 50f;
        if (getEnemyTarget != null) {
            Vector3 targetPosition = getEnemyTarget().GetPosition();
            if (Vector3.Distance(getEnemyTarget().GetPosition(), GetPosition()) < attackRange) {
                StopMoving();
                characterBase.SetAimTarget(targetPosition);
                
                if (Time.time >= nextShootTime) {
                    // Can shoot
                    nextShootTime = Time.time + FIRE_RATE;
                    targetPosition += UtilsClass.GetRandomDir() * UnityEngine.Random.Range(0f, 20f);
                    characterBase.ShootTarget(targetPosition);
                }
                /*
                state = State.Attacking;
                Vector3 attackDir = (getEnemyTarget().GetPosition() - GetPosition()).normalized;
                characterBase.ShootTarget(getEnemyTarget().GetPosition(), () => {
                    if (getEnemyTarget() != null) {
                        getEnemyTarget().Damage(this);
                    }
                }, SetStateNormal);
                */
            } else {
                if (Vector3.Distance(getEnemyTarget().GetPosition(), GetPosition()) < targetRange) {
                    if (pathfindingTimer <= 0f) {
                        pathfindingTimer = .3f;
                        SetTargetPosition(getEnemyTarget().GetPosition());
                    }
                    characterBase.SetAimTarget(targetPosition);
                }
            }
        }
    }

    public bool IsDead() {
        return healthSystem.IsDead();
    }
    
    private void SetStateAttacking() {
        state = State.Attacking;
    }

    public void Damage(IEnemyTargetable attacker) {
        Vector3 bloodDir = (GetPosition() - attacker.GetPosition()).normalized;
        Blood_Handler.SpawnBlood(GetPosition(), bloodDir);

        healthSystem.Damage(30);
        if (IsDead()) {
            FlyingBody.Create(GameAssets.i.pfEnemyFlyingBody, GetPosition(), bloodDir);
            characterBase.DestroySelf();
            Destroy(gameObject);
        } else {
            // Knockback
            transform.position += bloodDir * 5f;
            /*
            if (hitUnitAnim != null) {
                state = State.Busy;
                enemyBase.PlayHitAnimation(bloodDir * (Vector2.one * -1f), SetStateNormal);
            }
            */
        }
    }

    private void HandleMovement() {
        if (pathVectorList != null) {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 1f) {
                Vector3 moveDir = (targetPosition - transform.position).normalized;

                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                characterBase.PlayMoveAnim(moveDir);
                transform.position = transform.position + moveDir * SPEED * Time.deltaTime;
            } else {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count) {
                    StopMoving();
                    characterBase.PlayIdleAnim();
                }
            }
        } else {
            characterBase.PlayIdleAnim();
        }
    }

    private void StopMoving() {
        pathVectorList = null;
    }

    public void SetTargetPosition(Vector3 targetPosition) {
        currentPathIndex = 0;
        //pathVectorList = GridPathfinding.instance.GetPathRouteWithShortcuts(GetPosition(), targetPosition).pathVectorList;
        pathVectorList = new List<Vector3> { targetPosition };
        if (pathVectorList != null && pathVectorList.Count > 1) {
            pathVectorList.RemoveAt(0);
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }
      
}
