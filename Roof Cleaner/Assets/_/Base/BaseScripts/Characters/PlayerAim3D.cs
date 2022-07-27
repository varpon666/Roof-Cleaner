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

public class PlayerAim3D : MonoBehaviour, Enemy.IEnemyTargetable, EnemyHandler.IEnemyTargetable, EnemyAim.IEnemyTargetable {
    
    public static PlayerAim3D instance;

    private const float SPEED = 10f;
    private const float FIRE_RATE = .15f;

    private CharacterAim_Base playerBase;
    private float nextShootTime;
    private State state;
    private HealthSystem healthSystem;
    private float offsetY;

    private enum State {
        Normal,
    }

    private void Awake() {
        instance = this;
        playerBase = gameObject.GetComponent<CharacterAim_Base>();
        healthSystem = new HealthSystem(100);
        offsetY = transform.Find("Body").position.y + 2f;
        SetStateNormal();

        playerBase.OnShoot += PlayerBase_OnShoot;
    }

    private void PlayerBase_OnShoot(object sender, CharacterAim_Base.OnShootEventArgs e) {
        Vector3 shootDir = (e.shootPosition - e.gunEndPointPosition).normalized;
        BulletRaycast3D.Shoot(e.gunEndPointPosition, shootDir);

        Shoot_Flash.AddFlash(e.gunEndPointPosition, new Vector3(3, 2.7f));
        Vector3 mouseWorldPosition = Mouse3D.GetMouseWorldPosition();
        mouseWorldPosition.y = e.gunEndPointPosition.y;
        WeaponTracer.Create3D(e.gunEndPointPosition, mouseWorldPosition);
    }

    private void Update() {
        switch (state) {
        case State.Normal:
            HandleAimShooting();
            HandleMovement();
            break;
        }
    }
    
    private void SetStateNormal() {
        state = State.Normal;
    }

    private void HandleAimShooting() {
        Vector3 targetPosition = Mouse3D.GetMouseWorldPosition();
        targetPosition.y = offsetY;
        playerBase.SetAimTarget(targetPosition);

        if (Input.GetMouseButton(0) && Time.time >= nextShootTime) {
            nextShootTime = Time.time + FIRE_RATE;
            playerBase.ShootTarget(targetPosition);
        }
    }

    private void HandleMovement() {
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.W)) {
            moveZ = +1f;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveZ = -1f;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveX = +1f;
        }

        Vector3 moveDir = new Vector3(moveX, 0, moveZ).normalized;
        bool isIdle = moveX == 0 && moveZ == 0;
        if (isIdle) {
            playerBase.PlayIdleAnim();
        } else {
            playerBase.PlayMoveAnim(moveDir);
            transform.position += moveDir * SPEED * Time.deltaTime;
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public bool IsDead() {
        return healthSystem.IsDead();
    }

    public void Damage(Enemy enemy) { }

    public void Damage(EnemyHandler enemy) { }

    public void Damage(EnemyAim attacker) { 
        Vector3 bloodDir = (GetPosition() - attacker.GetPosition()).normalized;
        Blood_Handler.SpawnBlood(GetPosition(), bloodDir);

        healthSystem.Damage(1);
        if (IsDead()) {
            FlyingBody.Create(GameAssets.i.pfEnemyFlyingBody, GetPosition(), bloodDir);
            playerBase.DestroySelf();
            Destroy(gameObject);
        } else {
            // Knockback
            transform.position += bloodDir * 2.5f;
        }
    }
}
