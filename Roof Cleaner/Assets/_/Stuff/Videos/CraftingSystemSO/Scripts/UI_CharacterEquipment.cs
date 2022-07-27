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

    public class UI_CharacterEquipment : MonoBehaviour {

        [SerializeField] private Transform pfUI_Item = null;

        private Transform itemContainer;
        private UI_CharacterEquipmentSlot weaponSlot;
        private UI_CharacterEquipmentSlot helmetSlot;
        private UI_CharacterEquipmentSlot armorSlot;
        private CharacterEquipment characterEquipment;

        private void Awake() {
            itemContainer = transform.Find("itemContainer");
            weaponSlot = transform.Find("weaponSlot").GetComponent<UI_CharacterEquipmentSlot>();
            helmetSlot = transform.Find("helmetSlot").GetComponent<UI_CharacterEquipmentSlot>();
            armorSlot = transform.Find("armorSlot").GetComponent<UI_CharacterEquipmentSlot>();

            weaponSlot.OnItemDropped += WeaponSlot_OnItemDropped;
            helmetSlot.OnItemDropped += HelmetSlot_OnItemDropped;
            armorSlot.OnItemDropped += ArmorSlot_OnItemDropped;
        }

        private void ArmorSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e) {
            // Item dropped in Armor slot
            CharacterEquipment.EquipSlot equipSlot = CharacterEquipment.EquipSlot.Armor;
            if (characterEquipment.IsEquipSlotEmpty(equipSlot) && characterEquipment.CanEquipItem(equipSlot, e.item)) {
                e.item.RemoveFromItemHolder();
                characterEquipment.EquipItem(e.item);
            }
            //characterEquipment.TryEquipItem(CharacterEquipment.EquipSlot.Armor, e.item);
        }

        private void HelmetSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e) {
            // Item dropped in Helmet slot
            CharacterEquipment.EquipSlot equipSlot = CharacterEquipment.EquipSlot.Helmet;
            if (characterEquipment.IsEquipSlotEmpty(equipSlot) && characterEquipment.CanEquipItem(equipSlot, e.item)) {
                e.item.RemoveFromItemHolder();
                characterEquipment.EquipItem(e.item);
            }
            //characterEquipment.TryEquipItem(CharacterEquipment.EquipSlot.Helmet, e.item);
        }

        private void WeaponSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e) {
            // Item dropped in weapon slot
            CharacterEquipment.EquipSlot equipSlot = CharacterEquipment.EquipSlot.Weapon;
            if (characterEquipment.IsEquipSlotEmpty(equipSlot) && characterEquipment.CanEquipItem(equipSlot, e.item)) {
                e.item.RemoveFromItemHolder();
                characterEquipment.EquipItem(e.item);
            }
            //characterEquipment.TryEquipItem(CharacterEquipment.EquipSlot.Weapon, e.item);
        }

        public void SetCharacterEquipment(CharacterEquipment characterEquipment) {
            this.characterEquipment = characterEquipment;
            UpdateVisual();

            characterEquipment.OnEquipmentChanged += CharacterEquipment_OnEquipmentChanged;
        }

        private void CharacterEquipment_OnEquipmentChanged(object sender, System.EventArgs e) {
            UpdateVisual();
        }

        private void UpdateVisual() {
            foreach (Transform child in itemContainer) {
                Destroy(child.gameObject);
            }

            Item weaponItem = characterEquipment.GetWeaponItem();
            if (weaponItem != null) {
                Transform uiItemTransform = Instantiate(pfUI_Item, itemContainer);
                uiItemTransform.GetComponent<RectTransform>().anchoredPosition = weaponSlot.GetComponent<RectTransform>().anchoredPosition;
                uiItemTransform.localScale = Vector3.one * 1.5f;
                //uiItemTransform.GetComponent<CanvasGroup>().blocksRaycasts = false;
                UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
                uiItem.SetItem(weaponItem);
                weaponSlot.transform.Find("emptyImage").gameObject.SetActive(false);
            } else {
                weaponSlot.transform.Find("emptyImage").gameObject.SetActive(true);
            }

            Item armorItem = characterEquipment.GetArmorItem();
            if (armorItem != null) {
                Transform uiItemTransform = Instantiate(pfUI_Item, itemContainer);
                uiItemTransform.GetComponent<RectTransform>().anchoredPosition = armorSlot.GetComponent<RectTransform>().anchoredPosition;
                uiItemTransform.localScale = Vector3.one * 1.5f;
                //uiItemTransform.GetComponent<CanvasGroup>().blocksRaycasts = false;
                UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
                uiItem.SetItem(armorItem);
                armorSlot.transform.Find("emptyImage").gameObject.SetActive(false);
            } else {
                armorSlot.transform.Find("emptyImage").gameObject.SetActive(true);
            }

            Item helmetItem = characterEquipment.GetHelmetItem();
            if (helmetItem != null) {
                Transform uiItemTransform = Instantiate(pfUI_Item, itemContainer);
                uiItemTransform.GetComponent<RectTransform>().anchoredPosition = helmetSlot.GetComponent<RectTransform>().anchoredPosition;
                uiItemTransform.localScale = Vector3.one * 1.5f;
                //uiItemTransform.GetComponent<CanvasGroup>().blocksRaycasts = false;
                UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
                uiItem.SetItem(helmetItem);
                helmetSlot.transform.Find("emptyImage").gameObject.SetActive(false);
            } else {
                helmetSlot.transform.Find("emptyImage").gameObject.SetActive(true);
            }
        }

    }

}