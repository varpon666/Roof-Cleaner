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
public class PlayerSword : MonoBehaviour {
    
    public static PlayerSword instance;

    public event EventHandler OnEnemyKilled;

    private const float SPEED = 50f;

    private PlayerMain playerMain;
    private Character_Base characterBase;
    private State state;
    private Material material;
    private Color materialTintColor;

    private enum State {
        Normal,
        Attacking,
    }

    private void Awake() {
        instance = this;
        playerMain = GetComponent<PlayerMain>();
        characterBase = gameObject.GetComponent<Character_Base>();
        material = transform.Find("Body").GetComponent<MeshRenderer>().material;
        materialTintColor = new Color(1, 0, 0, 0);
    }

    private void Start() {
        SetStateNormal();
    }

    private void Update() {
        switch (state) {
        case State.Normal:
            //HandleMovement();
            HandleAttack();
            break;
        case State.Attacking:
            HandleAttack();
            break;
        }

        if (materialTintColor.a > 0) {
            float tintFadeSpeed = 6f;
            materialTintColor.a -= tintFadeSpeed * Time.deltaTime;
            material.SetColor("_Tint", materialTintColor);
        }
    }
    
    private void SetStateNormal() {
        state = State.Normal;
        playerMain.PlayerMovementHandler.Enable();
    }

    private void SetStateAttacking() {
        state = State.Attacking;
        playerMain.PlayerMovementHandler.Disable();
    }

    /*
    private void HandleMovement() {
        float moveX = 0f;
        float moveY = 0f;
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            moveY = +1f;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            moveX = +1f;
        }

        Vector3 moveDir = new Vector3(moveX, moveY).normalized;
        bool isIdle = moveX == 0 && moveY == 0;
        if (isIdle) {
            characterBase.PlayIdleAnim();
        } else {
            characterBase.PlayMoveAnim(moveDir);
            transform.position += moveDir * SPEED * Time.deltaTime;
        }
    }
    */
    private void HandleAttack() {
        if (Input.GetMouseButtonDown(0)) {
            // Attack
            SetStateAttacking();
            
            Vector3 attackDir = (UtilsClass.GetMouseWorldPosition() - GetPosition()).normalized;

            EnemyHandler enemyHandler = EnemyHandler.GetClosestEnemy(GetPosition() + attackDir * 4f, 20f);
            if (enemyHandler != null) {
                //enemyHandler.Damage(this);
                if (enemyHandler.IsDead()) {
                    OnEnemyKilled?.Invoke(this, EventArgs.Empty);
                }
                attackDir = (enemyHandler.GetPosition() - GetPosition()).normalized;
                transform.position = enemyHandler.GetPosition() + attackDir * -12f;
            } else {
                transform.position = transform.position + attackDir * 4f;
            }

            Transform swordSlashTransform = Instantiate(GameAssets.i.pfSwordSlash, GetPosition() + attackDir * 13f, Quaternion.Euler(0, 0, UtilsClass.GetAngleFromVector(attackDir)));
            swordSlashTransform.GetComponent<SpriteAnimator>().onLoop = () => Destroy(swordSlashTransform.gameObject);

            UnitAnimType activeAnimType = characterBase.GetUnitAnimation().GetActiveAnimType();
            if (activeAnimType == GameAssets.UnitAnimTypeEnum.dSwordTwoHandedBack_Sword) {
                swordSlashTransform.localScale = new Vector3(swordSlashTransform.localScale.x, swordSlashTransform.localScale.y * -1, swordSlashTransform.localScale.z);
                characterBase.GetUnitAnimation().PlayAnimForced(GameAssets.UnitAnimTypeEnum.dSwordTwoHandedBack_Sword2, attackDir, 1f, (UnitAnim unitAnim) => SetStateNormal(), null, null);
            } else {
                characterBase.GetUnitAnimation().PlayAnimForced(GameAssets.UnitAnimTypeEnum.dSwordTwoHandedBack_Sword, attackDir, 1f, (UnitAnim unitAnim) => SetStateNormal(), null, null);
            }
        }
    }
    
    private void DamageFlash() {
        materialTintColor = new Color(1, 0, 0, 1f);
        material.SetColor("_Tint", materialTintColor);
    }

    public void DamageKnockback(Vector3 knockbackDir, float knockbackDistance) {
        transform.position += knockbackDir * knockbackDistance;
        DamageFlash();
    }

    public Vector3 GetPosition() {
        return transform.position;
    }
        
}
