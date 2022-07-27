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

public class HotkeyAbilitySystem {

    public event EventHandler OnAbilityListChanged;

    public enum AbilityType {
        Pistol,
        Shotgun,
        Sword,
        Punch,
        HealthPotion,
        ManaPotion,
    }

    //private PlayerSwapWeapons player;
    private List<HotkeyAbility> hotkeyAbilityList;
    private List<HotkeyAbility> extraHotkeyAbilityList;

    public HotkeyAbilitySystem() {
        hotkeyAbilityList = new List<HotkeyAbility>();
        extraHotkeyAbilityList = new List<HotkeyAbility>();

        // Health Potion
        hotkeyAbilityList.Add(new HotkeyAbility {
            abilityType = AbilityType.HealthPotion,
            activateAbilityAction = () => { }// player.ConsumeHealthPotion()
        });

        
        // Mana Potion
        extraHotkeyAbilityList.Add(new HotkeyAbility { 
            abilityType = AbilityType.ManaPotion, 
            activateAbilityAction = () => { }// player.ConsumeManaPotion()
        });
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            hotkeyAbilityList[0].activateAbilityAction();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            hotkeyAbilityList[1].activateAbilityAction();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            hotkeyAbilityList[2].activateAbilityAction();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            hotkeyAbilityList[3].activateAbilityAction();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            hotkeyAbilityList[4].activateAbilityAction();
        }
    }
    
    public List<HotkeyAbility> GetHotkeyAbilityList() {
        return hotkeyAbilityList;
    }
    
    public List<HotkeyAbility> GetExtraHotkeyAbilityList() {
        return extraHotkeyAbilityList;
    }

    public void SwapAbility(int abilityIndexA, int abilityIndexB) {
        HotkeyAbility hotkeyAbility = hotkeyAbilityList[abilityIndexA];
        hotkeyAbilityList[abilityIndexA] = hotkeyAbilityList[abilityIndexB];
        hotkeyAbilityList[abilityIndexB] = hotkeyAbility;
        OnAbilityListChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public void SwapAbility(HotkeyAbility hotkeyAbilityA, HotkeyAbility hotkeyAbilityB) {
        if (extraHotkeyAbilityList.Contains(hotkeyAbilityA)) {
            // A is on Extra List
            int indexB = hotkeyAbilityList.IndexOf(hotkeyAbilityB);
            hotkeyAbilityList[indexB] = hotkeyAbilityA;

            extraHotkeyAbilityList.Remove(hotkeyAbilityA);
            extraHotkeyAbilityList.Add(hotkeyAbilityB);
        } else {
            if (extraHotkeyAbilityList.Contains(hotkeyAbilityB)) {
                // B is on the Extra List
                int indexA = hotkeyAbilityList.IndexOf(hotkeyAbilityA);
                hotkeyAbilityList[indexA] = hotkeyAbilityB;

                extraHotkeyAbilityList.Remove(hotkeyAbilityB);
                extraHotkeyAbilityList.Add(hotkeyAbilityA);
            } else {
                // Neither are on the Extra List
                int indexA = hotkeyAbilityList.IndexOf(hotkeyAbilityA);
                int indexB = hotkeyAbilityList.IndexOf(hotkeyAbilityB);
                HotkeyAbility tmp = hotkeyAbilityList[indexA];
                hotkeyAbilityList[indexA] = hotkeyAbilityList[indexB];
                hotkeyAbilityList[indexB] = tmp;
            }
        }

        OnAbilityListChanged?.Invoke(this, EventArgs.Empty);
    }

    /*
     * Represents a single Hotkey Ability of any Type
     * */
    public class HotkeyAbility {
        public AbilityType abilityType;
        public Action activateAbilityAction;

        public Sprite GetSprite() {
            switch (abilityType) {
            default:
            case AbilityType.HealthPotion:  return null;// Testing.Instance.healthPotionSprite;
            }
        }

        public override string ToString() {
            return abilityType.ToString();
        }
    }

}
