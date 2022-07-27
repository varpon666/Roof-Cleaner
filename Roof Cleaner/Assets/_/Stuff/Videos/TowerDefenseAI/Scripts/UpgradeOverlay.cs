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
using CodeMonkey.Utils;

public class UpgradeOverlay : MonoBehaviour {

    private static UpgradeOverlay Instance { get; set; }

    private Tower tower;

    private void Awake() {
        Instance = this;

        transform.Find("RangeBtn").GetComponent<Button_Sprite>().ClickFunc = UpgradeRange;
        transform.Find("DamageBtn").GetComponent<Button_Sprite>().ClickFunc = UpgradeDamage;
        transform.Find("CloseBtn").GetComponent<Button_Sprite>().ClickFunc = Hide;

        Hide();
    }

    public static void Show_Static(Tower tower) {
        Instance.Show(tower);
    }

    private void Show(Tower tower) {
        this.tower = tower;
        gameObject.SetActive(true);
        transform.position = tower.transform.position;
        RefreshRangeVisual();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void UpgradeRange() {
        tower.UpgradeRange();
        RefreshRangeVisual();
    }

    private void UpgradeDamage() {
        tower.UpgradeDamageAmount();
    }

    private void RefreshRangeVisual() {
        transform.Find("Range").localScale = Vector3.one * tower.GetRange() * 2f;
    }

}
