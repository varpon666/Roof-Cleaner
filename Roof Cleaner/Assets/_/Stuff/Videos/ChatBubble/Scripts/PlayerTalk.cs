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

public class PlayerTalk : MonoBehaviour {

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            UI_InputWindow.Show_Static(
                "Say what?", 
                "", 
                "abcdefghijklmnopqrstuvxywz! ABCDEFGHIJKLMNOPQRSTUVXYWZ,.?", 
                50, 
                () => { 
                }, 
                (string inputText) => {
                    ChatBubble.Create(transform, new Vector3(3, 8), ChatBubble.IconType.Happy, inputText);
                }
            );
        }
    }
}
