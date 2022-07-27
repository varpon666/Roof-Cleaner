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
using V_ObjectSystem;

/*
 * Character Aim Base Class
 * */
public class CharacterAim_Base : MonoBehaviour {

    #region BaseSetup
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs {
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;
        public GameObject hitObject;
    }
    
    public enum WeaponType {
        Pistol,
        Rifle,
        Shotgun,
    }

    public enum Axis {
        XY,
        XZ
    }

    [SerializeField] private Axis axis = Axis.XY;
    private V_Object vObject;
    private V_UnitSkeleton unitSkeleton;
    private V_UnitAnimation unitAnimation;
    private V_UnitSkeleton_Composite_Weapon unitSkeletonCompositeWeapon;
    private V_UnitSkeleton_Composite_Walker unitSkeletonCompositeWalker_BodyHead;
    private V_UnitSkeleton_Composite_Walker unitSkeletonCompositeWalker_Feet;
    private V_IObjectTransform objectTransform;
    private bool canShoot;
    private Vector3 aimDir;
    private Vector3 lastMoveDir;

    private void Awake() {
        vObject = CreateBasicUnit(transform, GetPosition(), 30f, null);
        unitAnimation = vObject.GetLogic<V_UnitAnimation>();
        unitSkeleton = vObject.GetLogic<V_UnitSkeleton>();
        objectTransform = vObject.GetLogic<V_IObjectTransform>();
        
        UnitAnimType idleUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_Idle");
        UnitAnimType walkUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_Walk");
        UnitAnimType hitUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_Hit");
        UnitAnimType attackUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_PunchQuickAttack");

        canShoot = true;

        //unitSkeletonCompositeWeapon = new V_UnitSkeleton_Composite_Weapon(vObject, unitSkeleton, UnitAnim.GetUnitAnim("Aim_PistolRight"), UnitAnim.GetUnitAnim("Aim_PistolLeft"), UnitAnim.GetUnitAnim("Aim_PistolShootRight"), UnitAnim.GetUnitAnim("Aim_PistolShootLeft"));
        unitSkeletonCompositeWeapon = new V_UnitSkeleton_Composite_Weapon(vObject, unitSkeleton, UnitAnim.GetUnitAnim("dMarine_AimWeaponRight"), UnitAnim.GetUnitAnim("dMarine_AimWeaponLeft"), UnitAnim.GetUnitAnim("dMarine_ShootWeaponRight"), UnitAnim.GetUnitAnim("dMarine_ShootWeaponLeft"));
        //unitSkeletonCompositeWeapon = new V_UnitSkeleton_Composite_Weapon(vObject, unitSkeleton, UnitAnim.GetUnitAnim("Aim_ShotgunRight"), UnitAnim.GetUnitAnim("Aim_ShotgunLeft"), UnitAnim.GetUnitAnim("Aim_ShotgunShootRight"), UnitAnim.GetUnitAnim("Aim_ShotgunShootLeft"));
        vObject.AddRelatedObject(unitSkeletonCompositeWeapon);
        unitSkeletonCompositeWeapon.SetActive();
            
        unitSkeletonCompositeWalker_BodyHead = new V_UnitSkeleton_Composite_Walker(vObject, unitSkeleton, GameAssets.UnitAnimTypeEnum.dMarine_Walk, GameAssets.UnitAnimTypeEnum.dMarine_Idle, new[] { "Body", "Head" });
        vObject.AddRelatedObject(unitSkeletonCompositeWalker_BodyHead);
            
        unitSkeletonCompositeWalker_Feet = new V_UnitSkeleton_Composite_Walker(vObject, unitSkeleton, GameAssets.UnitAnimTypeEnum.dMarine_Walk, GameAssets.UnitAnimTypeEnum.dMarine_Idle, new[] { "FootL", "FootR" });
        vObject.AddRelatedObject(unitSkeletonCompositeWalker_Feet);
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

    public void SetWeaponType(WeaponType weaponType) {
        switch (weaponType) {
        default:
        case WeaponType.Rifle:
            unitSkeletonCompositeWeapon.SetAnims(UnitAnim.GetUnitAnim("dMarine_AimWeaponRight"), UnitAnim.GetUnitAnim("dMarine_AimWeaponLeft"), UnitAnim.GetUnitAnim("dMarine_ShootWeaponRight"), UnitAnim.GetUnitAnim("dMarine_ShootWeaponLeft"));
            break;
        case WeaponType.Pistol:
            unitSkeletonCompositeWeapon.SetAnims(UnitAnim.GetUnitAnim("Aim_PistolRight"), UnitAnim.GetUnitAnim("Aim_PistolLeft"), UnitAnim.GetUnitAnim("Aim_PistolShootRight"), UnitAnim.GetUnitAnim("Aim_PistolShootLeft"));
            break;
        case WeaponType.Shotgun:
            unitSkeletonCompositeWeapon.SetAnims(UnitAnim.GetUnitAnim("Aim_ShotgunRight"), UnitAnim.GetUnitAnim("Aim_ShotgunLeft"), UnitAnim.GetUnitAnim("Aim_ShotgunShootRight"), UnitAnim.GetUnitAnim("Aim_ShotgunShootLeft"));
            break;
        }
    }

    public void SetVObjectEnabled(bool isEnabled) {
        vObject.SetIsDisabled(!isEnabled);
    }

    public void RefreshBodySkeletonMesh() {
        Transform bodyTransform = transform.Find("Body");
        bodyTransform.GetComponent<MeshFilter>().mesh = unitSkeleton.GetMesh();
    }

    public void DestroySelf() {
        vObject.DestroySelf();
    }

    public V_UnitAnimation GetUnitAnimation() {
        return unitAnimation;
    }
    public Vector3 GetPosition() {
        return transform.position;
    }
    #endregion


    public void SetAimTarget(Vector3 targetPosition) {
        aimDir = (targetPosition - vObject.GetPosition()).normalized;

        if (axis == Axis.XZ) {
            unitSkeletonCompositeWeapon.SetAimTarget(new Vector3(aimDir.x, aimDir.z));
        } else {
            unitSkeletonCompositeWeapon.SetAimTarget(aimDir);
        }
    }

    public void ShootTarget(Vector3 targetPosition) {
        ShootTarget(targetPosition, null);
    }

    public void ShootTarget(Vector3 targetPosition, Action onShootComplete) {
        aimDir = (targetPosition - vObject.GetPosition()).normalized;
        SetAimTarget(targetPosition);
            
        // Check for hits
        Vector3 gunEndPointPosition = vObject.GetLogic<V_UnitSkeleton>().GetBodyPartPosition("MuzzleFlash");
        GameObject hitObject = null;
        RaycastHit2D raycastHit = Physics2D.Raycast(gunEndPointPosition, (targetPosition - gunEndPointPosition).normalized, Vector3.Distance(gunEndPointPosition, targetPosition));
        if (raycastHit.collider != null) {
            // Hit something
            targetPosition = (Vector3)raycastHit.point + (targetPosition - gunEndPointPosition).normalized;
            hitObject = raycastHit.collider.gameObject;
        }

        canShoot = false;

        // Replace Body and Head with Attack
        if (axis == Axis.XZ) {
            unitSkeleton.ReplaceBodyPartSkeletonAnim(GameAssets.UnitAnimTypeEnum.dMarine_Attack.GetUnitAnim(new Vector3(aimDir.x, aimDir.z)), "Body", "Head");
        } else {
            unitSkeleton.ReplaceBodyPartSkeletonAnim(GameAssets.UnitAnimTypeEnum.dMarine_Attack.GetUnitAnim(aimDir), "Body", "Head");
        }

        // Shoot Composite Skeleton
        Vector3 shootAimDir;
        if (axis == Axis.XZ) {
            shootAimDir = new Vector3(aimDir.x, aimDir.z);
        } else {
            shootAimDir = aimDir;
        }

        unitSkeletonCompositeWeapon.Shoot(shootAimDir, () => {
            canShoot = true;
            onShootComplete?.Invoke();
        });

        // Add Effects
        Vector3 shootFlashPosition = vObject.GetLogic<V_UnitSkeleton>().GetBodyPartPosition("MuzzleFlash");
        if (OnShoot != null) OnShoot(this, new OnShootEventArgs { gunEndPointPosition = shootFlashPosition, shootPosition = targetPosition, hitObject = hitObject });
    }

    public void PlayMoveAnim(Vector3 moveDir) {
        if (axis == Axis.XZ && moveDir.y == 0f) moveDir = new Vector3(moveDir.x, moveDir.z);
        lastMoveDir = moveDir;
        bool isMoving = true;

        // Update Feet
        unitSkeletonCompositeWalker_Feet.UpdateBodyParts(isMoving, moveDir);

        if (canShoot) {
            // Update Head and Body parts only when Not Shooting
            if (axis == Axis.XZ) {
                unitSkeletonCompositeWalker_BodyHead.UpdateBodyParts(isMoving, new Vector3(aimDir.x, aimDir.z));
            } else {
                unitSkeletonCompositeWalker_BodyHead.UpdateBodyParts(isMoving, aimDir);
            }
        }
    }

    public void PlayIdleAnim() {
        bool isMoving = false;

        // Update Feet
        unitSkeletonCompositeWalker_Feet.UpdateBodyParts(isMoving, lastMoveDir);

        if (canShoot) {
            // Update Head and Body parts only when Not Shooting
            if (axis == Axis.XZ) {
                unitSkeletonCompositeWalker_BodyHead.UpdateBodyParts(isMoving, new Vector3(aimDir.x, aimDir.z));
            } else {
                unitSkeletonCompositeWalker_BodyHead.UpdateBodyParts(isMoving, aimDir);
            }
        }
    }
    
}
