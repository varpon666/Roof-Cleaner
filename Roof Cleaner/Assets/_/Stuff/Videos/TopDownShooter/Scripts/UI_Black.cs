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
using UnityEngine.UI;

namespace TopDownShooter {
    public class UI_Black : MonoBehaviour {

        private Image blackImage;
        private Color color;

        private void Awake() {
            GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            GetComponent<RectTransform>().sizeDelta = Vector3.zero;

            blackImage = transform.Find("blackImage").GetComponent<Image>();
            color = new Color(0, 0, 0, 1f);
            blackImage.color = color;
        }

        private void Update() {
            float fadeSpeed = 2f;
            color.a -= fadeSpeed * Time.deltaTime;

            blackImage.color = color;

            if (color.a <= 0f) {
                gameObject.SetActive(false);
            }
        }

    }
}