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
 * Enemy Base Class
 * */
public class Enemy_Base : MonoBehaviour {
    
    #region BaseSetup
    private V_UnitSkeleton unitSkeleton;
    private V_UnitAnimation unitAnimation;
    private AnimatedWalker animatedWalker;

    private void Start() {
        Transform bodyTransform = transform.Find("Body");
        unitSkeleton = new V_UnitSkeleton(1f, bodyTransform.TransformPoint, (Mesh mesh) => bodyTransform.GetComponent<MeshFilter>().mesh = mesh);
        unitAnimation = new V_UnitAnimation(unitSkeleton);
        
        UnitAnimType idleUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_Idle");
        UnitAnimType walkUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_Walk");
        UnitAnimType hitUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_Hit");

        animatedWalker = new AnimatedWalker(unitAnimation, idleUnitAnim, walkUnitAnim, 1f, 1f);
    }

    private void Update() {
        unitSkeleton.Update(Time.deltaTime);
    }

    public V_UnitAnimation GetUnitAnimation() {
        return unitAnimation;
    }
    #endregion


    public void PlayMoveAnim(Vector3 moveDir) {
        animatedWalker.SetMoveVector(moveDir);
    }

    public void PlayIdleAnim() {
        animatedWalker.SetMoveVector(Vector3.zero);
    }
    
    public bool IsPlayingPunchAnimation() {
        return unitAnimation.GetActiveAnimType().GetName() == "dBareHands_PunchQuick";
    }
    
    public void PlayPunchAnimation(Vector3 dir, Action<Vector3> onHit, Action onAnimComplete) {
        unitAnimation.PlayAnimForced(UnitAnimType.GetUnitAnimType("dBareHands_PunchQuick"), dir, 1f, (UnitAnim unitAnim2) => {
            if (onAnimComplete != null) onAnimComplete();
        }, (string trigger) => {
            // HIT = HandR
            // HIT2 = HandL
            string hitBodyPartName = trigger == "HIT" ? "HandR" : "HandL";
            Vector3 impactPosition = unitSkeleton.GetBodyPartPosition(hitBodyPartName);
            if (onHit != null) {
                onHit(impactPosition);
            }
        }, null);
    }
    
    public void PlayAttackAnimation(Vector3 dir, Action onHit, Action onAnimComplete) {
        unitAnimation.PlayAnimForced(UnitAnimType.GetUnitAnimType("dBareHands_Punch"), dir, 1f, (UnitAnim unitAnim2) => {
            if (onAnimComplete != null) onAnimComplete();
        }, (string trigger) => {
            if (onHit != null) {
                onHit();
            }
        }, null);
    }
    
    public void PlayHitAnimation(Vector3 dir, Action onAnimComplete) {
        unitAnimation.PlayAnimForced(UnitAnimType.GetUnitAnimType("dBareHands_Hit"), dir, 1f, (UnitAnim unitAnim2) => {
            if (onAnimComplete != null) onAnimComplete();
        }, (string trigger) => {
        }, null);
    }
    
    public void PlayDodgeAnimation(Vector3 dir) {
        unitAnimation.PlayAnimForced(UnitAnimType.GetUnitAnimType("dSwordShield_Roll"), dir, 1f, null, null, null);
    }

    public Vector3 GetHandLPosition() {
        return unitSkeleton.GetBodyPartPosition("HandL");
    }

}
