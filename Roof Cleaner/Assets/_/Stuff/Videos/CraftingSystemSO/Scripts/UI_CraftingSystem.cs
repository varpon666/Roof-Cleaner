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
using CodeMonkey.InventorySystem;

namespace CodeMonkey.CraftingSystem {

    public class UI_CraftingSystem : MonoBehaviour {

        [SerializeField] private Transform pfUI_Item = null;

        private Transform[,] slotTransformArray;
        private Transform outputSlotTransform;
        private Transform itemContainer;
        private CraftingSystem craftingSystem;

        private void Awake() {
            Transform gridContainer = transform.Find("gridContainer");
            itemContainer = transform.Find("itemContainer");

            slotTransformArray = new Transform[CraftingSystem.GRID_SIZE, CraftingSystem.GRID_SIZE];

            for (int x = 0; x < CraftingSystem.GRID_SIZE; x++) {
                for (int y = 0; y < CraftingSystem.GRID_SIZE; y++) {
                    slotTransformArray[x, y] = gridContainer.Find("grid_" + x + "_" + y);
                    UI_CraftingItemSlot craftingItemSlot = slotTransformArray[x, y].GetComponent<UI_CraftingItemSlot>();
                    craftingItemSlot.SetXY(x, y);
                    craftingItemSlot.OnItemDropped += UI_CraftingSystem_OnItemDropped;
                }
            }

            outputSlotTransform = transform.Find("outputSlot");

            //CreateItem(0, 0, new Item { itemType = Item.ItemType.Diamond });
            //CreateItem(1, 2, new Item { itemType = Item.ItemType.Wood });
            //CreateItemOutput(new Item { itemType = Item.ItemType.Sword_Wood });
        }

        public void SetCraftingSystem(CraftingSystem craftingSystem) {
            this.craftingSystem = craftingSystem;
            craftingSystem.OnGridChanged += CraftingSystem_OnGridChanged;

            UpdateVisual();
        }

        private void CraftingSystem_OnGridChanged(object sender, System.EventArgs e) {
            UpdateVisual();
        }

        private void UI_CraftingSystem_OnItemDropped(object sender, UI_CraftingItemSlot.OnItemDroppedEventArgs e) {
            craftingSystem.TryAddItem(e.item, e.x, e.y);
        }

        private void UpdateVisual() {
            // Clear old items
            foreach (Transform child in itemContainer) {
                Destroy(child.gameObject);
            }

            // Cycle through grid and spawn items
            for (int x = 0; x < CraftingSystem.GRID_SIZE; x++) {
                for (int y = 0; y < CraftingSystem.GRID_SIZE; y++) {
                    if (!craftingSystem.IsEmpty(x, y)) {
                        CreateItem(x, y, craftingSystem.GetItem(x, y));
                    }
                }
            }

            if (craftingSystem.GetOutputItem() != null) {
                CreateItemOutput(craftingSystem.GetOutputItem());
            }
        }

        private void CreateItem(int x, int y, Item item) {
            Transform itemTransform = Instantiate(pfUI_Item, itemContainer);
            RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
            itemRectTransform.anchoredPosition = slotTransformArray[x, y].GetComponent<RectTransform>().anchoredPosition;
            itemTransform.GetComponent<UI_Item>().SetItem(item);
        }

        private void CreateItemOutput(Item item) {
            Transform itemTransform = Instantiate(pfUI_Item, itemContainer);
            RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
            itemRectTransform.anchoredPosition = outputSlotTransform.GetComponent<RectTransform>().anchoredPosition;
            itemTransform.localScale = Vector3.one * 1.5f;
            itemTransform.GetComponent<UI_Item>().SetItem(item);
        }

    }

}