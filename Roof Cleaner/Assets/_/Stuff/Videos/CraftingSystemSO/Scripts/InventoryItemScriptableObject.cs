using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.InventorySystem;

namespace CodeMonkey.CraftingSystem {

    [CreateAssetMenu(menuName = "ScriptableObjects/ItemScriptableObject")]
    public class InventoryItemScriptableObject : ScriptableObject {

        public Item.ItemType itemType;
        public string itemName;
        public Sprite itemSprite;

        public CharacterEquipment.EquipSlot equipSlot;

    }
}