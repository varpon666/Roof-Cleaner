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

namespace TopDownShooter {
    public interface ICharacterAnims {

        void PlayIdleAnim();
        void PlayMoveAnim(Vector3 animDir);
        V_UnitAnimation GetUnitAnimation();

    }
}