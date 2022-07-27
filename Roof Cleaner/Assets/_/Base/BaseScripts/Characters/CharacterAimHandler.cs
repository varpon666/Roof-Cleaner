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
using V_ObjectSystem;
using V_AnimationSystem;
using CodeMonkey;
using CodeMonkey.Utils;

public class CharacterAimHandler : MonoBehaviour, EnemyHandler.IEnemyTargetable {

    public class OnShootEventArgs : EventArgs {
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;
    }
    public event EventHandler<OnShootEventArgs> OnShoot;

    private bool isMoving;
    private Vector3 moveDir;

	// Use this for initialization
	private void Start () {
        V_Object vObject = CreateBasicUnit(transform, new Vector3(500, 680), 30f, GameAssets.i.m_MarineSpriteSheet);
        V_UnitAnimation unitAnimation = vObject.GetLogic<V_UnitAnimation>();
        V_UnitSkeleton unitSkeleton = vObject.GetLogic<V_UnitSkeleton>();
        V_IObjectTransform objectTransform = vObject.GetLogic<V_IObjectTransform>();

        bool canShoot = true;

        V_UnitSkeleton_Composite_WeaponInvert unitSkeletonCompositeWeapon = new V_UnitSkeleton_Composite_WeaponInvert(vObject, unitSkeleton, GameAssets.UnitAnimEnum.dMarine_AimWeaponRight, GameAssets.UnitAnimEnum.dMarine_AimWeaponRightInvertV, GameAssets.UnitAnimEnum.dMarine_ShootWeaponRight, GameAssets.UnitAnimEnum.dMarine_ShootWeaponRightInvertV);
        vObject.AddRelatedObject(unitSkeletonCompositeWeapon);
        unitSkeletonCompositeWeapon.SetActive();
            
        V_UnitSkeleton_Composite_Walker unitSkeletonCompositeWalker_BodyHead = new V_UnitSkeleton_Composite_Walker(vObject, unitSkeleton, GameAssets.UnitAnimTypeEnum.dMarine_Walk, GameAssets.UnitAnimTypeEnum.dMarine_Idle, new[] { "Body", "Head" });
        vObject.AddRelatedObject(unitSkeletonCompositeWalker_BodyHead);
            
        V_UnitSkeleton_Composite_Walker unitSkeletonCompositeWalker_Feet = new V_UnitSkeleton_Composite_Walker(vObject, unitSkeleton, GameAssets.UnitAnimTypeEnum.dMarine_Walk, GameAssets.UnitAnimTypeEnum.dMarine_Idle, new[] { "FootL", "FootR" });
        vObject.AddRelatedObject(unitSkeletonCompositeWalker_Feet);

        FunctionUpdater.Create(() => {
            Vector3 targetPosition = UtilsClass.GetMouseWorldPosition();
            Vector3 aimDir = (targetPosition - vObject.GetPosition()).normalized;
            
            // Check for hits
            Vector3 gunEndPointPosition = vObject.GetLogic<V_UnitSkeleton>().GetBodyPartPosition("MuzzleFlash");
            RaycastHit2D raycastHit = Physics2D.Raycast(gunEndPointPosition, (targetPosition - gunEndPointPosition).normalized, Vector3.Distance(gunEndPointPosition, targetPosition));
            if (raycastHit.collider != null) {
                // Hit something
                targetPosition = raycastHit.point;
            }

            unitSkeletonCompositeWeapon.SetAimTarget(targetPosition);

            if (canShoot && Input.GetMouseButton(0)) {
                // Shoot
                canShoot = false;
                // Replace Body and Head with Attack
                unitSkeleton.ReplaceBodyPartSkeletonAnim(GameAssets.UnitAnimTypeEnum.dMarine_Attack.GetUnitAnim(aimDir), "Body", "Head");
                // Shoot Composite Skeleton
                unitSkeletonCompositeWeapon.Shoot(targetPosition, () => {
                    canShoot = true;
                });

                // Add Effects
                Vector3 shootFlashPosition = vObject.GetLogic<V_UnitSkeleton>().GetBodyPartPosition("MuzzleFlash");
                if (OnShoot != null) OnShoot(this, new OnShootEventArgs { gunEndPointPosition = shootFlashPosition, shootPosition = targetPosition });

                //Shoot_Flash.AddFlash(shootFlashPosition);
                //WeaponTracer.Create(shootFlashPosition, targetPosition);
            }


            // Manual Movement
            isMoving = false;
            moveDir = new Vector3(0, 0);
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                moveDir.y = +1; isMoving = true;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                moveDir.y = -1; isMoving = true;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                moveDir.x = -1; isMoving = true;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                moveDir.x = +1; isMoving = true;
            }
            moveDir.Normalize();

            float moveSpeed = 50f;
            Vector3 targetMoveToPosition = objectTransform.GetPosition() + moveDir * moveSpeed * Time.deltaTime;
            // Test if can move there
            raycastHit = Physics2D.Raycast(GetPosition() + moveDir * .1f, moveDir, Vector3.Distance(GetPosition(), targetMoveToPosition));
            if (raycastHit.collider != null) {
                // Hit something
            } else {
                // Can move, no wall
                objectTransform.SetPosition(targetMoveToPosition);
            }

            if (isMoving) {
                Dirt_Handler.SpawnInterval(GetPosition(), moveDir * -1f);
            }
            

            // Update Feet
            unitSkeletonCompositeWalker_Feet.UpdateBodyParts(isMoving, moveDir);

            if (canShoot) {
                // Update Head and Body parts only when Not Shooting
                unitSkeletonCompositeWalker_BodyHead.UpdateBodyParts(isMoving, aimDir);
            }
        });	
	}

    public Vector3 GetPosition() {
        return transform.position;
    }

    public bool IsMoving() {
        return isMoving;
    }

    public Vector3 GetMoveDir() {
        return moveDir;
    }

    public void Damage(EnemyHandler enemyHandler) {
        Damage(enemyHandler.GetPosition());
    }

    public void Damage(Vector3 attackerPosition) {
        Vector3 bloodDir = (GetPosition() - attackerPosition).normalized;
        Blood_Handler.SpawnBlood(GetPosition(), bloodDir);
        // Knockback
        transform.position += bloodDir * 3f;
    }

    
    private static V_Object CreateBasicUnit(Transform unitTransform, Vector3 spawnPosition, float walkerSpeed, Material materialSpriteSheet) {
        V_Object unitObject = V_Object.CreateObject(V_Main.UpdateType.Unit);
                    
        Transform instantiatedTransform = unitTransform;

        V_IObjectTransform transform = new V_ObjectTransform_LateUpdate(instantiatedTransform, () => instantiatedTransform.transform.position, V_Main.RegisterOnLateUpdate, V_Main.DeregisterOnLateUpdate);
        unitObject.AddRelatedObject(transform);
        V_IObjectTransformBody transformBody = new V_ObjectTransformBody(instantiatedTransform.Find("Body"), materialSpriteSheet);
        unitObject.AddRelatedObject(transformBody);

        V_UnitSkeleton unitSkeleton = new V_UnitSkeleton(1f, transformBody.ConvertBodyLocalPositionToWorldPosition, transformBody.SetBodyMesh);
        unitObject.AddRelatedObject(unitSkeleton);
        unitObject.AddActiveLogic(new V_ObjectLogic_SkeletonUpdater(unitSkeleton));
        V_UnitAnimation unitAnimation = new V_UnitAnimation(unitSkeleton);
        unitObject.AddRelatedObject(unitAnimation);

        unitObject.AddRelatedObject(new V_IHasWorldPosition_Class(transform.GetPosition));

        return unitObject;
    }
}
