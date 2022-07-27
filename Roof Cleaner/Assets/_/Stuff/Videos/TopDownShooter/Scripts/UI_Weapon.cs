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

namespace TopDownShooter {
    public class UI_Weapon : MonoBehaviour {

        private Image weaponImage;
        private TextMeshProUGUI ammoText;
        private Weapon weapon;

        private void Awake() {
            weaponImage = transform.Find("weaponImage").GetComponent<Image>();
            ammoText = transform.Find("ammoText").GetComponent<TextMeshProUGUI>();
        }

        public void SetWeapon(Weapon weapon) {
            this.weapon = weapon;

            weaponImage.sprite = weapon.GetSprite();
            weapon.OnAmmoChanged += Weapon_OnAmmoChanged;
            UpdateAmmoText();
        }

        private void Weapon_OnAmmoChanged(object sender, System.EventArgs e) {
            UpdateAmmoText();
        }

        private void UpdateAmmoText() {
            ammoText.text = weapon.GetAmmo() + "/" + weapon.GetAmmoMax();
            if (weapon.GetAmmo() == 0) {
                ammoText.color = Color.red;
            } else {
                ammoText.color = Color.white;
            }
        }

    }
}