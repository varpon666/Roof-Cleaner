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
using UnityEngine;
using V_AnimationSystem;
using CodeMonkey.Utils;

/*
 * Player movement with Arrow keys
 * Attack with Space
 * */
public class CharacterSwordHandler : MonoBehaviour, EnemyHandler.IEnemyTargetable {

    private const float speed = 50f;
        
    private V_UnitSkeleton unitSkeleton;
    private V_UnitAnimation unitAnimation;
    private AnimatedWalker animatedWalker;
    private State state;

    private UnitAnimType idleUnitAnim;
    private UnitAnimType walkUnitAnim;
    private UnitAnimType hitUnitAnim;
    private UnitAnimType attackUnitAnim;

    private enum State {
        Normal,
        Busy,
    }

    private void Start() {
        Transform bodyTransform = transform.Find("Body");
        unitSkeleton = new V_UnitSkeleton(1f, bodyTransform.TransformPoint, (Mesh mesh) => bodyTransform.GetComponent<MeshFilter>().mesh = mesh);
        unitAnimation = new V_UnitAnimation(unitSkeleton);
        
        idleUnitAnim = GameAssets.UnitAnimTypeEnum.dSwordTwoHandedBack_Idle;
        walkUnitAnim = GameAssets.UnitAnimTypeEnum.dSwordTwoHandedBack_Walk;
        hitUnitAnim = null;
        attackUnitAnim = null;
        
        idleUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_Idle");
        walkUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_Walk");
        hitUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_Hit");
        attackUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_PunchQuickAttack");

        animatedWalker = new AnimatedWalker(unitAnimation, idleUnitAnim, walkUnitAnim, 1f, .75f);

        state = State.Normal;
    }

    private void Update() {
        switch (state) {
        case State.Normal:
            HandleMovement();
            HandleAttack();
            break;
        case State.Busy:
            HandleAttack();
            break;
        }
        unitSkeleton.Update(Time.deltaTime);
    }

    private void HandleMovement() {
        float moveX = 0;
        float moveY = 0;

        if (Input.GetKey(KeyCode.UpArrow)) {
            moveY = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            moveY = -1;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            moveX = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            moveX = 1;
        }

        Vector3 moveDir = new Vector3(moveX, moveY).normalized;
        bool isIdle = moveX == 0 && moveY == 0;
        if (!isIdle) {
            Dirt_Handler.SpawnInterval(GetPosition() + new Vector3(0, -4), moveDir * -1);
        }
        animatedWalker.SetMoveVector(moveDir);
        transform.position = transform.position + moveDir * speed * Time.deltaTime;
    }

    private void HandleAttack() {
        Vector3 attackDir = animatedWalker.GetLastMoveVector();

        if (Input.GetMouseButtonDown(0)) {
            attackDir = (UtilsClass.GetMouseWorldPosition() - GetPosition()).normalized;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            // Attack
            SetStateBusy();

            EnemyHandler enemyHandler = EnemyHandler.GetClosestEnemy(GetPosition() + attackDir * 4f, 20f);
            if (enemyHandler != null) {
                enemyHandler.Damage(this);
                attackDir = (enemyHandler.GetPosition() - GetPosition()).normalized;
                transform.position = enemyHandler.GetPosition() + attackDir * -12f;
            } else {
                transform.position = transform.position + attackDir * 4f;
            }

            /*
            unitAnimation.PlayAnimForced(UnitAnimType.GetUnitAnimType("dBareHands_PunchStartup"), attackDir, 2f, (UnitAnim unitAnim) => {
                unitAnimation.PlayAnimForced(UnitAnimType.GetUnitAnimType("dBareHands_PunchQuickAttack"), attackDir, 1f, (UnitAnim unitAnim2) => SetStateNormal(), null, null);
            }, null, null);
            */

            UnitAnimType activeAnimType = unitAnimation.GetActiveAnimType();
            switch (activeAnimType.GetName()) {
            default:
                unitAnimation.PlayAnimForced(UnitAnimType.GetUnitAnimType("dBareHands_PunchQuick"), attackDir, 1f, (UnitAnim unitAnim2) => SetStateNormal(), null, null);
                break;
            case "dBareHands_PunchQuick":
                unitAnimation.PlayAnimForced(UnitAnimType.GetUnitAnimType("dBareHands_KickQuick"), attackDir, 1f, (UnitAnim unitAnim2) => SetStateNormal(), null, null);
                break;
            }

            //unitAnimation.PlayAnimForced(UnitAnimType.GetUnitAnimType("dBareHands_PunchQuickAttack"), attackDir, 1f, (UnitAnim unitAnim2) => SetStateNormal(), null, null);

            /*
            Transform swordSlashTransform = Instantiate(GameAssets.i.pfSwordSlash, GetPosition() + attackDir * 13f, Quaternion.Euler(0, 0, UtilsClass.GetAngleFromVector(attackDir)));
            swordSlashTransform.GetComponent<SpriteAnimator>().onLoop = () => Destroy(swordSlashTransform.gameObject);

            UnitAnimType activeAnimType = unitAnimation.GetActiveAnimType();
            if (activeAnimType == GameAssets.UnitAnimTypeEnum.dSwordTwoHandedBack_Sword) {
                swordSlashTransform.localScale = new Vector3(swordSlashTransform.localScale.x, swordSlashTransform.localScale.y * -1, swordSlashTransform.localScale.z);
                unitAnimation.PlayAnimForced(GameAssets.UnitAnimTypeEnum.dSwordTwoHandedBack_Sword2, attackDir, 1f, (UnitAnim unitAnim) => SetStateNormal(), null, null);
            } else {
                unitAnimation.PlayAnimForced(GameAssets.UnitAnimTypeEnum.dSwordTwoHandedBack_Sword, attackDir, 1f, (UnitAnim unitAnim) => SetStateNormal(), null, null);
            }
            */
        }
    }

    public void Damage(EnemyHandler enemyHandler) {
    }

    private void SetStateBusy() {
        state = State.Busy;
    }

    private void SetStateNormal() {
        state = State.Normal;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

        
}
