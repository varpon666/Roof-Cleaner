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

    public static class Minimap {
        
        public static event EventHandler OnWindowShow;
        public static event EventHandler OnWindowHide;
        public static event EventHandler OnZoomChanged;

        public static void Init() {
            MinimapCamera.OnZoomChanged += MinimapCamera_OnZoomChanged;
            MinimapWindow.OnWindowShow += MinimapWindow_OnWindowShow;
            MinimapWindow.OnWindowHide += MinimapWindow_OnWindowHide;
        }

        private static void MinimapWindow_OnWindowHide(object sender, EventArgs e) {
            if (OnWindowHide != null) OnWindowHide(sender, e);
        }

        private static void MinimapWindow_OnWindowShow(object sender, EventArgs e) {
            if (OnWindowShow != null) OnWindowShow(sender, e);
        }

        private static void MinimapCamera_OnZoomChanged(object sender, EventArgs e) {
            if (OnZoomChanged != null) OnZoomChanged(sender, e);
        }

        public static void ShowWindow() {
            MinimapWindow.Show();
        }

        public static void HideWindow() {
            MinimapWindow.Hide();
        }

        public static void SetZoom(float orthographicSize) {
            MinimapCamera.SetZoom(orthographicSize);
        }

        public static float GetZoom() {
            return MinimapCamera.GetZoom();
        }

        public static void ZoomIn() {
            MinimapCamera.ZoomIn();
        }

        public static void ZoomOut() {
            MinimapCamera.ZoomOut();
        }


    }

}