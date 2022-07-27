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
using CodeMonkey.CraftingSystem;

namespace CodeMonkey.InventorySystem {

    [Serializable]
    public class Item {

        public enum ItemType {
            None,
            Sword,
            HealthPotion,
            ManaPotion,
            Coin,
            Medkit,
            ArmorNone,
            Armor_1,
            Armor_2,
            HelmetNone,
            Helmet,
            SwordNone,
            Sword_1,
            Sword_2,

            Diamond,
            Wood,
            Planks,
            Stick,
            Sword_Wood,
            Sword_Diamond,
        }


        public InventoryItemScriptableObject itemScriptableObject;
        //public ItemType itemType;
        public int amount = 1;
        private IItemHolder itemHolder;


        public void SetItemHolder(IItemHolder itemHolder) {
            this.itemHolder = itemHolder;
        }

        public IItemHolder GetItemHolder() {
            return itemHolder;
        }

        public void RemoveFromItemHolder() {
            if (itemHolder != null) {
                // Remove from current Item Holder
                itemHolder.RemoveItem(this);
            }
        }

        public void MoveToAnotherItemHolder(IItemHolder newItemHolder) {
            RemoveFromItemHolder();
            // Add to new Item Holder
            newItemHolder.AddItem(this);
        }



        public Sprite GetSprite() {
            return itemScriptableObject.itemSprite;
        }

        public static Sprite GetSprite(ItemType itemType) {
            switch (itemType) {
                default:
                case ItemType.Sword: return ItemAssets.Instance.s_Sword;
                case ItemType.HealthPotion: return ItemAssets.Instance.s_HealthPotion;
                case ItemType.ManaPotion: return ItemAssets.Instance.s_ManaPotion;
                case ItemType.Coin: return ItemAssets.Instance.s_Coin;
                case ItemType.Medkit: return ItemAssets.Instance.s_Medkit;

                case ItemType.ArmorNone: return ItemAssets.Instance.s_ArmorNone;
                case ItemType.Armor_1: return ItemAssets.Instance.s_Armor_1;
                case ItemType.Armor_2: return ItemAssets.Instance.s_Armor_2;
                case ItemType.HelmetNone: return ItemAssets.Instance.s_HelmetNone;
                case ItemType.Helmet: return ItemAssets.Instance.s_Helmet;
                case ItemType.Sword_1: return ItemAssets.Instance.s_Sword_1;
                case ItemType.Sword_2: return ItemAssets.Instance.s_Sword_2;

                case ItemType.Wood: return ItemAssets.Instance.s_Wood;
                case ItemType.Planks: return ItemAssets.Instance.s_Planks;
                case ItemType.Stick: return ItemAssets.Instance.s_Stick;
                case ItemType.Diamond: return ItemAssets.Instance.s_Diamond;
                case ItemType.Sword_Wood: return ItemAssets.Instance.s_Sword_Wood;
                case ItemType.Sword_Diamond: return ItemAssets.Instance.s_Sword_Diamond;
            }
        }

        public Color GetColor() {
            return Color.white;// GetColor(itemType);
        }

        public static Color GetColor(ItemType itemType) {
            switch (itemType) {
                default:
                case ItemType.Sword: return new Color(1, 1, 1);
                case ItemType.HealthPotion: return new Color(1, 0, 0);
                case ItemType.ManaPotion: return new Color(0, 0, 1);
                case ItemType.Coin: return new Color(1, 1, 0);
                case ItemType.Medkit: return new Color(1, 0, 1);
            }
        }

        public bool IsStackable() {
            return true;// IsStackable(itemType);
        }


        public static bool IsStackable(ItemType itemType) {
            switch (itemType) {
                default:
                case ItemType.Coin:
                case ItemType.HealthPotion:
                case ItemType.ManaPotion:
                    return true;
                case ItemType.Sword:
                case ItemType.SwordNone:
                case ItemType.Medkit:
                case ItemType.Sword_1:
                case ItemType.Sword_2:
                case ItemType.HelmetNone:
                case ItemType.Helmet:
                case ItemType.ArmorNone:
                case ItemType.Armor_1:
                case ItemType.Armor_2:
                    return false;

                case ItemType.Wood:
                case ItemType.Planks:
                case ItemType.Stick:
                case ItemType.Diamond:
                    return true;
                case ItemType.Sword_Diamond:
                case ItemType.Sword_Wood:
                    return false;
            }
        }

        public int GetCost() {
            return 0;// GetCost(itemType);
        }

        public static int GetCost(ItemType itemType) {
            switch (itemType) {
                default:
                case ItemType.ArmorNone: return 0;
                case ItemType.Armor_1: return 30;
                case ItemType.Armor_2: return 100;
                case ItemType.HelmetNone: return 0;
                case ItemType.Helmet: return 90;
                case ItemType.HealthPotion: return 30;
                case ItemType.Sword_1: return 0;
                case ItemType.Sword_2: return 150;
            }
        }

        public override string ToString() {
            return itemScriptableObject.itemName;
        }

        public CharacterEquipment.EquipSlot GetEquipSlot() {
            return itemScriptableObject.equipSlot;
            /*
            switch (itemType) {
            default:
                return CharacterEquipment.EquipSlot.None;
            case ItemType.ArmorNone:
            case ItemType.Armor_1:
            case ItemType.Armor_2:
                return CharacterEquipment.EquipSlot.Armor;
            case ItemType.HelmetNone:
            case ItemType.Helmet:
                return CharacterEquipment.EquipSlot.Helmet;
            case ItemType.SwordNone:
            case ItemType.Sword:
            case ItemType.Sword_1:
            case ItemType.Sword_2:
            case ItemType.Sword_Wood:
            case ItemType.Sword_Diamond:
                return CharacterEquipment.EquipSlot.Weapon;
            }
            */
        }

    }

}