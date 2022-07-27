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

public class PlayerShop : MonoBehaviour, Enemy.IEnemyTargetable, IShopCustomer {

    public static PlayerShop Instance { get; private set; }

    public event EventHandler OnGoldAmountChanged;
    public event EventHandler OnHealthPotionAmountChanged;
    
    private int goldAmount;
    private int healthPotionAmount;


    #region Private
    [SerializeField] private Texture2D baseSpritesheetTexture = null;
    [SerializeField] private Texture2D swordTexture = null;
    [SerializeField] private Texture2D swordDarkTexture = null;
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

    private void Awake() {
        Instance = this;
        playerBase = GetComponent<Player_Base>();
        playerPunch = GetComponent<PlayerPunch>();
        playerSword = GetComponent<PlayerSword>();

        playerBase.enabled = true;
        playerPunch.enabled = false;
        playerSword.enabled = false;

        healthPotionAmount = 1;
    }

    private void Start() {
        material = transform.Find("Body").GetComponent<MeshRenderer>().material;
        baseSpritesheetTexture = material.mainTexture as Texture2D;

        EquipWeapon_Sword();
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

        if (sword == 0) {
            texture.SetPixels(0, 128, 128, 128, swordTexture.GetPixels(0, 0, 128, 128));
        }
        if (sword == 1) {
            texture.SetPixels(0, 128, 128, 128, swordDarkTexture.GetPixels(0, 0, 128, 128));
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
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            TryConsumeHealthPotion();
        }
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

    private void AddHealthPotion() {
        healthPotionAmount++;
        OnHealthPotionAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public void AddGoldAmount(int addGoldAmount) {
        goldAmount += addGoldAmount;
        OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetGoldAmount() {
        return goldAmount;
    }

    public void BoughtItem(Item.ItemType itemType) {
        Debug.Log("Bought item: " + itemType);
        switch (itemType) {
        case Item.ItemType.ArmorNone:   EquipArmorNone();   break;
        case Item.ItemType.Armor_1:     EquipArmor_1();     break;
        case Item.ItemType.Armor_2:     EquipArmor_2();     break;
        case Item.ItemType.HelmetNone:  EquipHelmetNone();  break;
        case Item.ItemType.Helmet:      EquipHelmet();      break;
        case Item.ItemType.HealthPotion:AddHealthPotion();  break;
        case Item.ItemType.Sword_2:     EquipWeapon_Sword2();  break;
        }
    }

    public bool TrySpendGoldAmount(int spendGoldAmount) {
        if (GetGoldAmount() >= spendGoldAmount) {
            goldAmount -= spendGoldAmount;
            OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
            return true;
        } else {
            return false;
        }
    }

}