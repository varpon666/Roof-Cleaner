using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.CraftingSystem;

namespace CodeMonkey.InventorySystem {

    public class Inventory : IItemHolder {

        public event EventHandler OnItemListChanged;

        private List<Item> itemList;
        private Action<Item> useItemAction;
        private InventorySlot[] inventorySlotArray;

        public Inventory(Action<Item> useItemAction, int inventorySlotCount) {
            this.useItemAction = useItemAction;
            itemList = new List<Item>();

            inventorySlotArray = new InventorySlot[inventorySlotCount];
            for (int i = 0; i < inventorySlotCount; i++) {
                inventorySlotArray[i] = new InventorySlot(i);
            }

            //AddItem(new Item { itemType = Item.ItemType.Wood, amount = 10 });
            //AddItem(new Item { itemType = Item.ItemType.Planks, amount = 10 });
            //AddItem(new Item { itemType = Item.ItemType.Diamond, amount = 10 });
            //AddItem(new Item { itemType = Item.ItemType.Stick, amount = 10 });
            //AddItem(new Item { itemType = Item.ItemType.Sword_Wood });
            //AddItem(new Item { itemType = Item.ItemType.Sword_Diamond });
        }

        public InventorySlot GetEmptyInventorySlot() {
            foreach (InventorySlot inventorySlot in inventorySlotArray) {
                if (inventorySlot.IsEmpty()) {
                    return inventorySlot;
                }
            }
            Debug.LogError("Cannot find an empty InventorySlot!");
            return null;
        }

        public InventorySlot GetInventorySlotWithItem(Item item) {
            foreach (InventorySlot inventorySlot in inventorySlotArray) {
                if (inventorySlot.GetItem() == item) {
                    return inventorySlot;
                }
            }
            Debug.LogError("Cannot find Item " + item + " in a InventorySlot!");
            return null;
        }

        public void AddItem(Item item) {
            itemList.Add(item);
            item.SetItemHolder(this);
            GetEmptyInventorySlot().SetItem(item);
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
        }

        public void AddItemMergeAmount(Item item) {
            // Adds an Item and increases amount if same ItemType already present
            if (item.IsStackable()) {
                bool itemAlreadyInInventory = false;
                foreach (Item inventoryItem in itemList) {
                    if (inventoryItem.itemScriptableObject == item.itemScriptableObject) {
                        inventoryItem.amount += item.amount;
                        itemAlreadyInInventory = true;
                    }
                }
                if (!itemAlreadyInInventory) {
                    itemList.Add(item);
                    item.SetItemHolder(this);
                    GetEmptyInventorySlot().SetItem(item);
                }
            } else {
                itemList.Add(item);
                item.SetItemHolder(this);
                GetEmptyInventorySlot().SetItem(item);
            }
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveItem(Item item) {
            GetInventorySlotWithItem(item).RemoveItem();
            itemList.Remove(item);
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
        }

        /*public void RemoveItemAmount(Item.ItemType itemType, int amount) {
            RemoveItemRemoveAmount(new Item { itemType = itemType, amount = amount });
        }*/

        public void RemoveItemRemoveAmount(Item item) {
            // Removes item but tries to remove amount if possible
            if (item.IsStackable()) {
                Item itemInInventory = null;
                foreach (Item inventoryItem in itemList) {
                    if (inventoryItem.itemScriptableObject == item.itemScriptableObject) {
                        inventoryItem.amount -= item.amount;
                        itemInInventory = inventoryItem;
                    }
                }
                if (itemInInventory != null && itemInInventory.amount <= 0) {
                    GetInventorySlotWithItem(itemInInventory).RemoveItem();
                    itemList.Remove(itemInInventory);
                }
            } else {
                GetInventorySlotWithItem(item).RemoveItem();
                itemList.Remove(item);
            }
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
        }

        public void AddItem(Item item, InventorySlot inventorySlot) {
            // Add Item to a specific Inventory slot
            itemList.Add(item);
            item.SetItemHolder(this);
            inventorySlot.SetItem(item);

            OnItemListChanged?.Invoke(this, EventArgs.Empty);
        }

        public void UseItem(Item item) {
            useItemAction(item);
        }

        public List<Item> GetItemList() {
            return itemList;
        }

        public InventorySlot[] GetInventorySlotArray() {
            return inventorySlotArray;
        }

        public bool CanAddItem() {
            return GetEmptyInventorySlot() != null;
        }


        /*
         * Represents a single Inventory Slot
         * */
        public class InventorySlot {

            private int index;
            private Item item;

            public InventorySlot(int index) {
                this.index = index;
            }

            public Item GetItem() {
                return item;
            }

            public void SetItem(Item item) {
                this.item = item;
            }

            public void RemoveItem() {
                item = null;
            }

            public bool IsEmpty() {
                return item == null;
            }

        }

    }

}