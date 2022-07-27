using System;
using UnityEngine;
using V_AnimationSystem;
using CodeMonkey.Utils;

public class GuestHandler : MonoBehaviour {

    private V_UnitSkeleton unitSkeleton;
    private V_UnitAnimation unitAnimation;

    private UnitAnimType idleUnitAnimType;


    private void Start() {
        Transform bodyTransform = transform.Find("Body");
        unitSkeleton = new V_UnitSkeleton(1f, bodyTransform.TransformPoint, (Mesh mesh) => bodyTransform.GetComponent<MeshFilter>().mesh = mesh);
        unitAnimation = new V_UnitAnimation(unitSkeleton);
        
        idleUnitAnimType = UnitAnimType.GetUnitAnimType("dBareHands_Idle");

        unitAnimation.PlayAnim(idleUnitAnimType, new Vector3(0, -1), 1f, null, null, null);
    }

    private void Update() {
        unitSkeleton.Update(Time.deltaTime);
    }

        
}
