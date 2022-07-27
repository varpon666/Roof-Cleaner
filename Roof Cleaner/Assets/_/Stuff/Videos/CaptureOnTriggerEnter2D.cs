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

public class CaptureOnTriggerEnter2D : MonoBehaviour {

    public event EventHandler OnCapturedTriggerEnter2D;
    public event EventHandler OnPlayerTriggerEnter2D;
    public event EventHandler OnCapturedTriggerExit2D;
    public event EventHandler OnPlayerTriggerExit2D;

    private void OnTriggerEnter2D(Collider2D collider) {
        OnCapturedTriggerEnter2D?.Invoke(collider, EventArgs.Empty);

        Player player = collider.GetComponent<Player>();
        if (player != null) {
            OnPlayerTriggerEnter2D?.Invoke(player, EventArgs.Empty);
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        OnCapturedTriggerExit2D?.Invoke(collider, EventArgs.Empty);

        Player player = collider.GetComponent<Player>();
        if (player != null) {
            OnPlayerTriggerExit2D?.Invoke(player, EventArgs.Empty);
        }
    }

}
