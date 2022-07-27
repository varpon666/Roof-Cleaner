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

namespace Minimap {

    public class MinimapIcon : MonoBehaviour {

        private Vector3 baseScale;

        private void Start() {
            baseScale = transform.localScale;
            Minimap.OnZoomChanged += Minimap_OnZoomChanged;
        }

        private void Minimap_OnZoomChanged(object sender, System.EventArgs e) {
            transform.localScale = baseScale * Minimap.GetZoom() / 180f;
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        private void OnDestroy() {
            Minimap.OnZoomChanged -= Minimap_OnZoomChanged;
        }
    }

}