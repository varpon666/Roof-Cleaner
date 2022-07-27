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
using V_AnimationSystem;

/*
 * Character moves between waypoints
 * */
public class CharacterWaypointsHandler : MonoBehaviour {
        
    private const float speed = 30f;

    [SerializeField] private List<Vector3> waypointList = null;
    [SerializeField] private List<float> waitTimeList = null;
    private int waypointIndex;

    [SerializeField] private string idleAnimation = "dZombie_Idle";
    [SerializeField] private string walkAnimation = "dZombie_Walk";

    [SerializeField] private float idleFrameRate = 1f;
    [SerializeField] private float walkFrameRate = 1f;
    [SerializeField] private Vector3 defaultAimDirection = Vector3.zero;
    
    [SerializeField] private PlayerMovement player = null;

    private V_UnitSkeleton unitSkeleton;
    private V_UnitAnimation unitAnimation;
    private AnimatedWalker animatedWalker;

    private enum State {
        Waiting,
        Moving,
        AttackingPlayer,
        Busy,
    }

    private State state;
    private float waitTimer;
    private UnitAnimType attackUnitAnim;
    private Vector3 lastMoveDir;

    private void Start() {
        Transform bodyTransform = transform.Find("Body");
        unitSkeleton = new V_UnitSkeleton(1f, bodyTransform.TransformPoint, (Mesh mesh) => bodyTransform.GetComponent<MeshFilter>().mesh = mesh);
        unitAnimation = new V_UnitAnimation(unitSkeleton);
        animatedWalker = new AnimatedWalker(unitAnimation, UnitAnimType.GetUnitAnimType(idleAnimation), UnitAnimType.GetUnitAnimType(walkAnimation), idleFrameRate, walkFrameRate);
        state = State.Waiting;
        waitTimer = waitTimeList[0];
        lastMoveDir = defaultAimDirection;
        attackUnitAnim = UnitAnimType.GetUnitAnimType("dMarine_Attack");
    }

    private void Update() {
        switch (state) {
        default:
        case State.Waiting:
        case State.Moving:
            HandleMovement();
            FindTargetPlayer();
            break;
        case State.AttackingPlayer:
            AttackPlayer();
            break;
        case State.Busy:
            break;
        }
        unitSkeleton.Update(Time.deltaTime);
    }
    
    private void FindTargetPlayer() {
        float viewDistance = 50f;
        float fov = 180f;
        if (Vector3.Distance(GetPosition(), player.GetPosition()) < viewDistance) {
            // Player inside viewDistance
            Vector3 dirToPlayer = (player.GetPosition() - GetPosition()).normalized;
            if (Vector3.Angle(GetAimDir(), dirToPlayer) < fov / 2f) {
                // Player inside Field of View
                RaycastHit2D raycastHit2D = Physics2D.Raycast(GetPosition(), dirToPlayer, viewDistance);
                if (raycastHit2D.collider != null) {
                    // Hit something
                    if (raycastHit2D.collider.gameObject.GetComponent<PlayerMovement>() != null) {
                        // Hit Player
                        StartAttackingPlayer();
                    } else {
                        // Hit something else
                    }
                }
            }
        }
    }

    public void StartAttackingPlayer() {
        AttackPlayer();
    }

    private void AttackPlayer() {
        state = State.Busy;

        Vector3 targetPosition = player.GetPosition();
        Vector3 dirToTarget = (targetPosition - GetPosition()).normalized;
        lastMoveDir = dirToTarget;

        unitAnimation.PlayAnimForced(attackUnitAnim, dirToTarget, 2f, (UnitAnim unitAnim) => {
            // Attack complete
            if (player.IsDead()) {
                state = State.Moving;
            } else {
                state = State.AttackingPlayer;
            }
        }, (string trigger) => {
            // Damage Player
            player.Damage(this);
        }, null);
        
        Vector3 gunEndPointPosition = unitSkeleton.GetBodyPartPosition("MuzzleFlash");
        Shoot_Flash.AddFlash(gunEndPointPosition);
        WeaponTracer.Create(gunEndPointPosition, player.GetPosition());
    }

    private void HandleMovement() {
        switch (state) {
        case State.Waiting:
            waitTimer -= Time.deltaTime;
            animatedWalker.SetMoveVector(Vector3.zero);
            if (waitTimer <= 0f) {
                state = State.Moving;
            }
            break;
        case State.Moving:
            Vector3 waypoint = waypointList[waypointIndex];

            Vector3 waypointDir = (waypoint - transform.position).normalized;

            float distanceBefore = Vector3.Distance(transform.position, waypoint);
            animatedWalker.SetMoveVector(waypointDir);
            transform.position = transform.position + waypointDir * speed * Time.deltaTime;
            float distanceAfter = Vector3.Distance(transform.position, waypoint);
            
            float arriveDistance = .1f;
            if (distanceAfter < arriveDistance || distanceBefore <= distanceAfter) {
                // Go to next waypoint
                waitTimer = waitTimeList[waypointIndex];
                waypointIndex = (waypointIndex + 1) % waypointList.Count;
                state = State.Waiting;
            }
            break;
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public Vector3 GetAimDir() {
        return lastMoveDir;
    }

}
