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

namespace CodeMonkey.BattleSystemVideo {

    public class Testing_BattleSystem : MonoBehaviour {

        [SerializeField] private DoorAnims entryDoor = null;
        [SerializeField] private DoorAnims exitDoor = null;
        [SerializeField] private BattleSystem battleSystem = null;

        private void Start() {
            battleSystem.OnBattleStarted += BattleSystem_OnBattleStarted;
            battleSystem.OnBattleOver += BattleSystem_OnBattleOver;
        }

        private void BattleSystem_OnBattleOver(object sender, System.EventArgs e) {
            exitDoor.OpenDoor();
            exitDoor.SetColor(DoorAnims.ColorName.Green);
        }

        private void BattleSystem_OnBattleStarted(object sender, System.EventArgs e) {
            entryDoor.CloseDoor();
            entryDoor.SetColor(DoorAnims.ColorName.Red);
        }
    }

}