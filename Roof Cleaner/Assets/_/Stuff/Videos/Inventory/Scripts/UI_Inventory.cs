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
using CodeMonkey.Utils;
using TMPro;
using CodeMonkey.CraftingSystem;

namespace CodeMonkey.InventorySystem {

    public class UI_Inventory : MonoBehaviour {

        [SerializeField] private Transform pfUI_Item = null;

        private Inventory inventory;
        private Transform itemSlotContainer;
        private Transform itemSlotTemplate;
        private Player player;

        private void Awake() {
            itemSlotContainer = transform.Find("itemSlotContainer");
            itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
            itemSlotTemplate.gameObject.SetActive(false);
        }

        public void SetPlayer(Player player) {
            this.player = player;
        }

        public void SetInventory(Inventory inventory) {
            this.inventory = inventory;

            inventory.OnItemListChanged += Inventory_OnItemListChanged;

            RefreshInventoryItems();
        }

        private void Inventory_OnItemListChanged(object sender, System.EventArgs e) {
            RefreshInventoryItems();
        }

        private void RefreshInventoryItems() {
            foreach (Transform child in itemSlotContainer) {
                if (child == itemSlotTemplate) continue;
                Destroy(child.gameObject);
            }

            int x = 0;
            int y = 0;
            float itemSlotCellSize = 54f;
            foreach (Inventory.InventorySlot inventorySlot in inventory.GetInventorySlotArray()) {
                Item item = inventorySlot.GetItem();

                RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);

                itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () => {
                    // Use item
                    //inventory.UseItem(item);
                };
                itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () => {
                    // Split item
                    if (item.IsStackable()) {
                        // Is Stackable
                        if (item.amount > 2) {
                            // Can split
                            int splitAmount = Mathf.FloorToInt(item.amount / 2f);
                            item.amount -= splitAmount;
                            Item duplicateItem = new Item { itemScriptableObject = item.itemScriptableObject, amount = splitAmount };
                            inventory.AddItem(duplicateItem);
                        }
                    }

                    // Drop item
                    //Item duplicateItem = new Item { itemType = item.itemType, amount = item.amount };
                    //inventory.RemoveItem(item);
                    //ItemWorld.DropItem(player.GetPosition(), duplicateItem);
                };

                itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize);

                if (!inventorySlot.IsEmpty()) {
                    // Not Empty, has Item
                    Transform uiItemTransform = Instantiate(pfUI_Item, itemSlotContainer);
                    uiItemTransform.GetComponent<RectTransform>().anchoredPosition = itemSlotRectTransform.anchoredPosition;
                    UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
                    uiItem.SetItem(item);
                }

                Inventory.InventorySlot tmpInventorySlot = inventorySlot;

                UI_ItemSlot uiItemSlot = itemSlotRectTransform.GetComponent<UI_ItemSlot>();
                uiItemSlot.SetOnDropAction(() => {
                    // Dropped on this UI Item Slot
                    Item draggedItem = UI_ItemDrag.Instance.GetItem();
                    draggedItem.RemoveFromItemHolder();
                    inventory.AddItem(draggedItem, tmpInventorySlot);
                });

                /*
                TextMeshProUGUI uiText = itemSlotRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
                if (inventorySlot.IsEmpty()) {
                    // Empty
                    uiText.SetText("");
                } else {
                    if (item.amount > 1) {
                        uiText.SetText(item.amount.ToString());
                    } else {
                        uiText.SetText("");
                    }
                }
                */

                x++;
                int itemRowMax = 7;
                if (x >= itemRowMax) {
                    x = 0;
                    y++;
                }
            }
        }


    }

}