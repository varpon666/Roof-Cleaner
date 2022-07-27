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
using TMPro;
using CodeMonkey.Utils;
using CodeMonkey.InventorySystem;
using CodeMonkey.TooltipUICamera;

public class UI_Shop : MonoBehaviour {

    private Transform container;
    private Transform shopItemTemplate;
    private IShopCustomer shopCustomer;

    private void Awake() {
        container = transform.Find("container");
        shopItemTemplate = container.Find("shopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        CreateItemButton(Item.ItemType.Armor_1, Item.GetSprite(Item.ItemType.Armor_1), "Armor 1", Item.GetCost(Item.ItemType.Armor_1), 0);
        CreateItemButton(Item.ItemType.Armor_2, Item.GetSprite(Item.ItemType.Armor_2), "Armor 2", Item.GetCost(Item.ItemType.Armor_2), 1);
        CreateItemButton(Item.ItemType.Helmet, Item.GetSprite(Item.ItemType.Helmet), "Helmet", Item.GetCost(Item.ItemType.Helmet), 2);
        CreateItemButton(Item.ItemType.Sword_2, Item.GetSprite(Item.ItemType.Sword_2), "Sword", Item.GetCost(Item.ItemType.Sword_2), 3);
        CreateItemButton(Item.ItemType.HealthPotion, Item.GetSprite(Item.ItemType.HealthPotion), "HealthPotion", Item.GetCost(Item.ItemType.HealthPotion), 4);

        Hide();
    }

    private void CreateItemButton(Item.ItemType itemType, Sprite itemSprite, string itemName, int itemCost, int positionIndex) {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        shopItemTransform.gameObject.SetActive(true);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 90f;
        shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);

        shopItemTransform.Find("nameText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("costText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());

        shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemSprite;

        shopItemTransform.GetComponent<Button_UI>().ClickFunc = () => {
            // Clicked on shop item button
            TryBuyItem(itemType);
        };
    }

    private void TryBuyItem(Item.ItemType itemType) {
        if (shopCustomer.TrySpendGoldAmount(Item.GetCost(itemType))) {
            // Can afford cost
            shopCustomer.BoughtItem(itemType);
        } else {
            Tooltip_Warning.ShowTooltip_Static("Cannot afford " + Item.GetCost(itemType) + "!");
        }
    }

    public void Show(IShopCustomer shopCustomer) {
        this.shopCustomer = shopCustomer;
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
