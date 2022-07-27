using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeMonkey.TooltipUICamera {

    public class Window_Items : MonoBehaviour {

        private void Start() {
            for (int i = 1; i <= 6; i++) {
                ItemInfo itemInfo = transform.Find("itemBtn_" + i).GetComponent<ItemInfo>();
                //Tooltip_ItemStats.AddTooltip(transform.Find("itemBtn_" + i), itemInfo.sprite, itemInfo.itemName, itemInfo.itemDescription, itemInfo.DEX, itemInfo.CON, itemInfo.STR);
            }
        }

    }

}