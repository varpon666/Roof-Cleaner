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
using CodeMonkey.Utils;
using V_AnimationSystem;

public class PunchAttack : MonoBehaviour, IAttack {

    private enum State {
        Normal,
        Attacking
    }

    private Character_Base characterBase;
    private State state;

    private void Awake() {
        characterBase = GetComponent<Character_Base>();
        SetStateNormal();

        switch (state) {
            default:
                break;
        }
    }

    private void SetStateAttacking() {
        state = State.Attacking;
        GetComponent<IMoveVelocity>().Disable();
    }

    private void SetStateNormal() {
        state = State.Normal;
        GetComponent<IMoveVelocity>().Enable();
    }

    public void Attack(Vector3 attackDir, Action onAttackComplete) {
        // Attack
        SetStateAttacking();
            
        //Vector3 attackDir = (UtilsClass.GetMouseWorldPosition() - GetPosition()).normalized;

        characterBase.PlayAttackAnimation(attackDir, () => { SetStateNormal(); onAttackComplete(); });
    }

    private Vector3 GetPosition() {
        return transform.position;
    }

}
