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

namespace CodeMonkey.BossBattleVideo {
    public class BossBattleInteraction : MonoBehaviour {

        [SerializeField] private DoorAnims entryDoor = null;
        [SerializeField] private GameObject keyGameObject = null;
        [SerializeField] private BossBattle bossBattle = null;

        private void Start() {
            bossBattle.OnBossBattleStarted += BossBattle_OnBossBattleStarted;
            bossBattle.OnBossBattleOver += BossBattle_OnBossBattleOver;
            keyGameObject.SetActive(false);
        }

        private void BossBattle_OnBossBattleOver(object sender, System.EventArgs e) {
            keyGameObject.SetActive(true);
        }

        private void BossBattle_OnBossBattleStarted(object sender, System.EventArgs e) {
            entryDoor.CloseDoor();
            entryDoor.SetColor(DoorAnims.ColorName.Red);
        }

    }

}