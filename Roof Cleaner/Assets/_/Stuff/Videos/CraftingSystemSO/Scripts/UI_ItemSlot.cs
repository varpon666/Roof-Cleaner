using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CodeMonkey.CraftingSystem {

    public class UI_ItemSlot : MonoBehaviour, IDropHandler {

        private Action onDropAction;

        public void SetOnDropAction(Action onDropAction) {
            this.onDropAction = onDropAction;
        }

        public void OnDrop(PointerEventData eventData) {
            UI_ItemDrag.Instance.Hide();
            onDropAction();
        }
    }

}