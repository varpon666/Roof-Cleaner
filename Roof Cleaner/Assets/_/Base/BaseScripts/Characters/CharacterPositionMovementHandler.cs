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
using CodeMonkey.Utils;

public class CharacterPositionMovementHandler : MonoBehaviour {

    private const float speed = 40f;

    private V_UnitSkeleton unitSkeleton;
    private V_UnitAnimation unitAnimation;
    private AnimatedWalker animatedWalker;
    private Vector3 targetPosition;


    private void Start() {
        Transform bodyTransform = transform.Find("Body");
        unitSkeleton = new V_UnitSkeleton(1f, bodyTransform.TransformPoint, (Mesh mesh) => bodyTransform.GetComponent<MeshFilter>().mesh = mesh);
        unitAnimation = new V_UnitAnimation(unitSkeleton);
        animatedWalker = new AnimatedWalker(unitAnimation, UnitAnimType.GetUnitAnimType("dMarine_Idle"), UnitAnimType.GetUnitAnimType("dMarine_Walk"), 1f, 1f);
    }

    private void Update() {
        HandleMovement();
        unitSkeleton.Update(Time.deltaTime);
            
        if (Input.GetMouseButtonDown(0)) {
            SetTargetPosition(UtilsClass.GetMouseWorldPosition());
        }
    }

    private void HandleMovement() {
        if (Vector3.Distance(transform.position, targetPosition) > 1f) {
            Vector3 moveDir = (targetPosition - transform.position).normalized;

            float distanceBefore = Vector3.Distance(transform.position, targetPosition);
            animatedWalker.SetMoveVector(moveDir);
            transform.position = transform.position + moveDir * speed * Time.deltaTime;
        } else {
            animatedWalker.SetMoveVector(Vector3.zero);
        }
    }


    public void SetTargetPosition(Vector3 targetPosition) {
        targetPosition.z = 0f;
        this.targetPosition = targetPosition;
    }

}