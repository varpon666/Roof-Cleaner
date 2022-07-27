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
using UnityEngine.EventSystems;
using TMPro;
using CodeMonkey.InventorySystem;

namespace CodeMonkey.CraftingSystem {

    public class UI_ItemDrag : MonoBehaviour {

        public static UI_ItemDrag Instance { get; private set; }

        private Canvas canvas;
        private RectTransform rectTransform;
        private RectTransform parentRectTransform;
        private CanvasGroup canvasGroup;
        private Image image;
        private Item item;
        private TextMeshProUGUI amountText;

        private void Awake() {
            Instance = this;

            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            canvas = GetComponentInParent<Canvas>();
            image = transform.Find("image").GetComponent<Image>();
            amountText = transform.Find("amountText").GetComponent<TextMeshProUGUI>();
            parentRectTransform = transform.parent.GetComponent<RectTransform>();

            Hide();
        }

        private void Update() {
            UpdatePosition();
        }

        private void UpdatePosition() {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, Input.mousePosition, null, out Vector2 localPoint);
            transform.localPosition = localPoint;
        }

        public Item GetItem() {
            return item;
        }

        public void SetItem(Item item) {
            this.item = item;
        }

        public void SetSprite(Sprite sprite) {
            image.sprite = sprite;
        }

        public void SetAmountText(int amount) {
            if (amount <= 1) {
                amountText.text = "";
            } else {
                // More than 1
                amountText.text = amount.ToString();
            }
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        public void Show(Item item) {
            gameObject.SetActive(true);

            SetItem(item);
            SetSprite(item.GetSprite());
            SetAmountText(item.amount);
            UpdatePosition();
        }

    }

}