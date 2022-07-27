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

namespace TopDownShooter {
    public class Weapon {

        public event EventHandler OnAmmoChanged;

        public enum WeaponType {
            Pistol,
            Shotgun,
            Rifle,
            Sword,
            Punch
        }

        private WeaponType weaponType;
        private int ammo;

        public Weapon(WeaponType weaponType) {
            this.weaponType = weaponType;
            ammo = GetAmmoMax();
        }

        public WeaponType GetWeaponType() {
            return weaponType;
        }

        public int GetAmmo() {
            return ammo;
        }

        public bool TrySpendAmmo() {
            if (ammo > 0) {
                ammo--;
                OnAmmoChanged?.Invoke(this, EventArgs.Empty);
                return true;
            } else {
                return false;
            }
        }

        public void Reload() {
            ammo = GetAmmoMax();
            OnAmmoChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanReload() {
            return ammo < GetAmmoMax();
        }

        public int GetAmmoMax() {
            switch (weaponType) {
            default:
                return 99;
            case WeaponType.Pistol:
                return 12;
            case WeaponType.Shotgun:
                return 4;
            case WeaponType.Rifle:
                return 25;
            }
        }

        public float GetDamageMultiplier() {
            switch (weaponType) {
            default:
            case WeaponType.Pistol: return 1.2f;
            case WeaponType.Shotgun: return 1.9f;
            case WeaponType.Rifle: return 0.6f;
            }
        }

        public float GetFireRate() {
            switch (weaponType) {
            default:
            case WeaponType.Pistol: return .15f;
            case WeaponType.Shotgun: return .20f;
            case WeaponType.Rifle: return .09f;
            }
        }

        public Sprite GetSprite() {
            switch (weaponType) {
            default:
                return null;
            //case WeaponType.Pistol: return GameAssets.i.s_PistolIcon;
            //case WeaponType.Shotgun: return GameAssets.i.s_ShotgunIcon;
            //case WeaponType.Rifle: return GameAssets.i.s_RifleIcon;
            }
        }

    }

}