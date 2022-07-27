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
using CodeMonkey.Utils;
using CodeMonkey.InventorySystem;
using CodeMonkey.CraftingSystem;

public class Player : MonoBehaviour, Enemy.IEnemyTargetable {

    public static Player Instance { get; private set; }

    public event EventHandler OnGoldAmountChanged;
    public event EventHandler OnHealthPotionAmountChanged;
    public event EventHandler OnEquipChanged;
    
    private Inventory inventory;

    #region Private
    [SerializeField] private Texture2D baseSpritesheetTexture = null;
    [SerializeField] private Texture2D swordTexture = null;
    [SerializeField] private Texture2D swordDarkTexture = null;
    [SerializeField] private Texture2D swordWoodTexture = null;
    [SerializeField] private Texture2D swordDiamondTexture = null;
    [SerializeField] private Texture2D helmetTexture = null;
    [SerializeField] private Texture2D chest1Texture = null;
    [SerializeField] private Texture2D chest2Texture = null;
    [SerializeField] private Texture2D chest3Texture = null;

    private Player_Base playerBase;
    private PlayerPunch playerPunch;
    private PlayerSword playerSword;
    private Material material;

    private bool helmet;
    private int chest;
    private int sword;

    private int goldAmount;
    private int healthPotionAmount;

    private void Awake() {
        Instance = this;
        playerBase = GetComponent<Player_Base>();
        playerPunch = GetComponent<PlayerPunch>();
        playerSword = GetComponent<PlayerSword>();

        playerBase.enabled = true;
        playerPunch.enabled = false;
        playerSword.enabled = false;

        healthPotionAmount = 1;
        inventory = new Inventory(UseItem, 14);
    }

    private void Start() {
        material = transform.Find("Body").GetComponent<MeshRenderer>().material;
        baseSpritesheetTexture = material.mainTexture as Texture2D;

        SetEquipment(Item.ItemType.SwordNone);
        SetEquipment(Item.ItemType.ArmorNone);
    }

    private void FlashColor(Color color) {
        GetComponent<MaterialTintColor>().SetTintColor(color);
    }

    private void UpdateTexture() {
        UpdateTexture(helmet, chest, sword);
    }

    private void UpdateTexture(bool helmet, int chest, int sword) {
        Texture2D texture = new Texture2D(512, 512, TextureFormat.RGBA32, true);

        Color[] spritesheetBasePixels = baseSpritesheetTexture.GetPixels(0, 0, 512, 512);
        texture.SetPixels(0, 0, 512, 512, spritesheetBasePixels);
        
        
        if (helmet) {
            texture.SetPixels(0, 384, 384, 128, helmetTexture.GetPixels(0, 0, 384, 128));
        }

        switch (chest) {
        default:
        case 0:
            texture.SetPixels(0, 256, 384, 128, chest1Texture.GetPixels(0, 0, 384, 128));
            break;
        case 1:
            texture.SetPixels(0, 256, 384, 128, chest2Texture.GetPixels(0, 0, 384, 128));
            break;
        case 2:
            texture.SetPixels(0, 256, 384, 128, chest3Texture.GetPixels(0, 0, 384, 128));
            break;
        }

        switch (sword) {
        default:
        case 0:
            texture.SetPixels(0, 128, 128, 128, swordTexture.GetPixels(0, 0, 128, 128));
            break;
        case 1:
            texture.SetPixels(0, 128, 128, 128, swordDarkTexture.GetPixels(0, 0, 128, 128));
            break;
        case 2:
            texture.SetPixels(0, 128, 128, 128, swordWoodTexture.GetPixels(0, 0, 128, 128));
            break;
        case 3:
            texture.SetPixels(0, 128, 128, 128, swordDiamondTexture.GetPixels(0, 0, 128, 128));
            break;
        }

        texture.Apply();

        material.mainTexture = texture;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void Damage(Enemy attacker) {
        Vector3 bloodDir = (GetPosition() - attacker.GetPosition()).normalized;
        Blood_Handler.SpawnBlood(GetPosition(), bloodDir);
    }

    public void ConsumeManaPotion() {
        FlashColor(Color.blue);
    }
    #endregion

    #region Equip
    private void Update() {
        /*
        if (Input.GetKeyDown(KeyCode.E)) {
            TryConsumeHealthPotion();
        }
        
        if (Input.GetKeyDown(KeyCode.T)) {
            EquipArmor_1();
        }
        */
    }

    public void EquipWeapon_Punch() {
        playerPunch.enabled = true;
        playerSword.enabled = false;
    }

    public void EquipWeapon_Sword() {
        playerPunch.enabled = false;
        playerSword.enabled = true;

        sword = 0;
        UpdateTexture();
    }

    public void EquipWeapon_Sword2() {
        playerPunch.enabled = false;
        playerSword.enabled = true;
        
        sword = 1;
        UpdateTexture();
    }

    public void EquipWeapon_SwordWooden() {
        playerPunch.enabled = false;
        playerSword.enabled = true;
        
        sword = 2;
        UpdateTexture();
    }

    public void EquipWeapon_SwordDiamond() {
        playerPunch.enabled = false;
        playerSword.enabled = true;
        
        sword = 3;
        UpdateTexture();
    }

    public void EquipArmorNone() {
        chest = 0;
        UpdateTexture();
    }

    public void EquipArmor_1() {
        chest = 1;
        UpdateTexture();
    }

    public void EquipArmor_2() {
        chest = 2;
        UpdateTexture();
    }

    public void EquipHelmet() {
        helmet = true;
        UpdateTexture();
    }

    public void EquipHelmetNone() {
        helmet = false;
        UpdateTexture();
    }
    
    public void TryConsumeHealthPotion() {
        if (healthPotionAmount > 0) {
            healthPotionAmount--;
            OnHealthPotionAmountChanged?.Invoke(this, EventArgs.Empty);
            FlashColor(Color.green);
        }
    }

    public int GetHealthPotionAmount() {
        return healthPotionAmount;
    }

    public void AddGoldAmount(int addGoldAmount) {
        goldAmount += addGoldAmount;
        OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetGoldAmount() {
        return goldAmount;
    }
    #endregion

    public void UseItem(Item inventoryItem) {
        Debug.Log("Use Item: " + inventoryItem);
    }

    public Inventory GetInventory() {
        return inventory;
    }

    public void SetEquipment(InventoryItemScriptableObject item) {
        Debug.LogError("Item no longer has itemType!");
        //SetEquipment(item.itemType);
    }

    public void SetEquipment(Item item) {
        Debug.LogError("Item no longer has itemType!");
        //SetEquipment(item.itemType);
    }

    public void SetEquipment(Item.ItemType itemType) {
        // Equip Item
        switch (itemType) {
        case Item.ItemType.ArmorNone:   EquipArmorNone();       break;
        case Item.ItemType.Armor_1:     EquipArmor_1();         break;
        case Item.ItemType.Armor_2:     EquipArmor_2();         break;

        case Item.ItemType.HelmetNone:  EquipHelmetNone();      break;
        case Item.ItemType.Helmet:      EquipHelmet();          break;

        case Item.ItemType.SwordNone:   EquipWeapon_Punch();    break;
        case Item.ItemType.Sword_1:     EquipWeapon_Sword();    break;
        case Item.ItemType.Sword_2:     EquipWeapon_Sword2();   break;
        case Item.ItemType.Sword_Wood:      EquipWeapon_SwordWooden();   break;
        case Item.ItemType.Sword_Diamond:   EquipWeapon_SwordDiamond();   break;
        }
        OnEquipChanged?.Invoke(this, EventArgs.Empty);
    }

}