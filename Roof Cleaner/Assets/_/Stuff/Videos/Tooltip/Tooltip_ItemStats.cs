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
using UnityEngine.UI;
using CodeMonkey.Utils;

namespace CodeMonkey.TooltipUICamera {

    public class Tooltip_ItemStats : MonoBehaviour {

        private static Tooltip_ItemStats instance;

        [SerializeField]
        private Camera uiCamera = null;
        [SerializeField]
        private RectTransform canvasRectTransform = null;

        private Image image;
        private Text nameText;
        private Text descriptionText;
        private Text levelText;
        private RectTransform backgroundRectTransform;

        private void Awake() {
            instance = this;
            backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
            image = transform.Find("image").GetComponent<Image>();
            nameText = transform.Find("nameText").GetComponent<Text>();
            descriptionText = transform.Find("descriptionText").GetComponent<Text>();
            levelText = transform.Find("levelText").GetComponent<Text>();

            HideTooltip();
        }

        private void Update() {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
            transform.localPosition = localPoint;

            Vector2 anchoredPosition = transform.GetComponent<RectTransform>().anchoredPosition;
            if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width) {
                anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
            }
            if (anchoredPosition.y - backgroundRectTransform.rect.height > canvasRectTransform.rect.height) {
                anchoredPosition.y = canvasRectTransform.rect.height + backgroundRectTransform.rect.height;
            }
            transform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        }

        private void ShowTooltip(Sprite itemSprite, string itemName, string itemDescription, int level) {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            nameText.text = itemName;
            descriptionText.text = itemDescription;
            levelText.text = level.ToString();
            image.sprite = itemSprite;
            Update();
        }

        private void HideTooltip() {
            gameObject.SetActive(false);
        }

        public static void ShowTooltip_Static(Sprite itemSprite, string itemName, string itemDescription, int level) {
            instance.ShowTooltip(itemSprite, itemName, itemDescription, level);
        }

        public static void HideTooltip_Static() {
            instance.HideTooltip();
        }





        public static void AddTooltip(Transform transform, Sprite itemSprite, string itemName, string itemDescription, int level) {
            if (transform.GetComponent<Button_UI>() != null) {
                transform.GetComponent<Button_UI>().MouseOverOnceTooltipFunc = () => Tooltip_ItemStats.ShowTooltip_Static(itemSprite, itemName, itemDescription, level);
                transform.GetComponent<Button_UI>().MouseOutOnceTooltipFunc = () => Tooltip_ItemStats.HideTooltip_Static();
            }
        }

    }

}