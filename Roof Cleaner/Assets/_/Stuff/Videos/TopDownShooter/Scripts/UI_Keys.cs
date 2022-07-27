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
    public class UI_Keys : MonoBehaviour {

        [SerializeField] private KeyHolder keyHolder = null;

        private Image keyImage;

        private void Awake() {
            keyImage = transform.Find("key").Find("image").GetComponent<Image>();
            keyImage.gameObject.SetActive(false);
        }

        private void Start() {
            keyHolder.OnKeysChanged += KeyHolder_OnKeysChanged;
        }

        private void KeyHolder_OnKeysChanged(object sender, System.EventArgs e) {
            List<Key.KeyType> keyList = keyHolder.GetKeyList();
            if (keyList.Count >= 1) {
                keyImage.gameObject.SetActive(true);
                keyImage.color = Key.GetColor(keyList[0]);
            } else {
                keyImage.gameObject.SetActive(false);
            }
        }

    }
}