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
using CodeMonkey.Utils;

public class QuestPointer_GameHandler : MonoBehaviour {

    [SerializeField] private Window_QuestPointer windowQuestPointer = null;
    [SerializeField] private Sprite customArrowSprite = null;
    [SerializeField] private Sprite customCrossSprite = null;
    [SerializeField] private Sprite exclamationPointSprite = null;

    private void Start() {
        Window_QuestPointer.QuestPointer questPointer_1 = windowQuestPointer.CreatePointer(new Vector3(200, 45), UtilsClass.GetColorFromString("FF0000"), UtilsClass.GetColorFromString("FFFFFF"));
        FunctionUpdater.Create(() => {
            if (Vector3.Distance(Camera.main.transform.position, new Vector3(200, 45)) < 40f) {
                windowQuestPointer.DestroyPointer(questPointer_1);
                return true;
            } else {
                return false;
            }
        });
        
        Window_QuestPointer.QuestPointer questPointer_2 = windowQuestPointer.CreatePointer(new Vector3(190, -32), UtilsClass.GetColorFromString("00FF00"), UtilsClass.GetColorFromString("00FF00"), customArrowSprite, customCrossSprite);
        FunctionUpdater.Create(() => {
            if (Vector3.Distance(Camera.main.transform.position, new Vector3(190, -32)) < 40f) {
                windowQuestPointer.DestroyPointer(questPointer_2);
                return true;
            } else {
                return false;
            }
        });
        
        Window_QuestPointer.QuestPointer questPointer_3 = windowQuestPointer.CreatePointer(new Vector3(-70, 200), UtilsClass.GetColorFromString("FFFFFF"), UtilsClass.GetColorFromString("FFFF00"), customArrowSprite, exclamationPointSprite);
        FunctionUpdater.Create(() => {
            if (Vector3.Distance(Camera.main.transform.position, new Vector3(-70, 200)) < 40f) {
                windowQuestPointer.DestroyPointer(questPointer_3);
                return true;
            } else {
                return false;
            }
        });
    }
}
