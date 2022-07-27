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
using UnityEngine.EventSystems;
using CodeMonkey.InventorySystem;

namespace CodeMonkey.CraftingSystem {

    public class UI_CharacterEquipmentSlot : MonoBehaviour, IDropHandler {

        public event EventHandler<OnItemDroppedEventArgs> OnItemDropped;
        public class OnItemDroppedEventArgs : EventArgs {
            public Item item;
        }

        public void OnDrop(PointerEventData eventData) {
            UI_ItemDrag.Instance.Hide();
            Item item = UI_ItemDrag.Instance.GetItem();
            OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs { item = item });
        }

    }

}