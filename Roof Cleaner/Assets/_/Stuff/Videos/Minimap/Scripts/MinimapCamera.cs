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

    public class MinimapCamera : MonoBehaviour {

        public static event EventHandler OnZoomChanged;

        private static MinimapCamera instance;
        
        private const float ZOOM_CHANGE_AMOUNT = 30f;
        private const float ZOOM_MIN = 30f;
        private const float ZOOM_MAX = 300f;

        private Camera minimapCamera;
        private float zoom;

        private void Awake() {
            instance = this;
            minimapCamera = transform.GetComponent<Camera>();
            zoom = minimapCamera.orthographicSize;
        }

        public static void SetZoom(float orthographicSize) {
            instance.minimapCamera.orthographicSize = orthographicSize;
            if (OnZoomChanged != null) OnZoomChanged(instance, EventArgs.Empty);
        }

        public static float GetZoom() {
            return instance.minimapCamera.orthographicSize;
        }

        public static void ZoomIn() {
            instance.zoom -= ZOOM_CHANGE_AMOUNT;
            if (instance.zoom < ZOOM_MIN) {
                instance.zoom = ZOOM_MIN;
            }
            SetZoom(instance.zoom);
        }

        public static void ZoomOut() {
            instance.zoom += ZOOM_CHANGE_AMOUNT;
            if (instance.zoom > ZOOM_MAX) {
                instance.zoom = ZOOM_MAX;
            }
            SetZoom(instance.zoom);
        }


    }

}