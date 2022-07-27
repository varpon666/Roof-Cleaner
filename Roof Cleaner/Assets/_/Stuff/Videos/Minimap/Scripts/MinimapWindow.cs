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

namespace Minimap {

    public class MinimapWindow : MonoBehaviour {
        
        public static event EventHandler OnWindowShow;
        public static event EventHandler OnWindowHide;

        private static MinimapWindow instance;

        private void Awake() {
            instance = this;
        }

        public static void Show() {
            instance.gameObject.SetActive(true);
            if (OnWindowShow != null) OnWindowShow(instance, EventArgs.Empty);
        }

        public static void Hide() {
            instance.gameObject.SetActive(false);
            if (OnWindowHide != null) OnWindowHide(instance, EventArgs.Empty);
        }
    }

}