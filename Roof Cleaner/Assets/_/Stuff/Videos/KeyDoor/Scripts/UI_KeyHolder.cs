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

public class UI_KeyHolder : MonoBehaviour {

    [SerializeField] private KeyHolder keyHolder = null;

    private Transform container;
    private Transform keyTemplate;

    private void Awake() {
        container = transform.Find("container");
        keyTemplate = container.Find("keyTemplate");
        keyTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        keyHolder.OnKeysChanged += KeyHolder_OnKeysChanged;
    }

    private void KeyHolder_OnKeysChanged(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        // Clean up old keys
        foreach (Transform child in container) {
            if (child == keyTemplate) continue;
            Destroy(child.gameObject);
        }

        // Instantiate current key list
        List<Key.KeyType> keyList = keyHolder.GetKeyList();
        container.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(keyList.Count - 1) * 80 / 2f, -234);
        for (int i = 0; i < keyList.Count; i++) {
            Key.KeyType keyType = keyList[i];
            Transform keyTransform = Instantiate(keyTemplate, container);
            keyTransform.gameObject.SetActive(true);
            keyTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(80 * i, 0);
            Image keyImage = keyTransform.Find("image").GetComponent<Image>();
            switch (keyType) {
            default:
            case Key.KeyType.Red:   keyImage.color = Color.red;     break;
            case Key.KeyType.Green: keyImage.color = Color.green;   break;
            case Key.KeyType.Blue:  keyImage.color = Color.blue;    break;
            }
        }
    }

}
