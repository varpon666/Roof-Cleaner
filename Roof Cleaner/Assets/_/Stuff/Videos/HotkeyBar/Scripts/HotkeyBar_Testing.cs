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

public class HotkeyBar_Testing : MonoBehaviour {

    public static HotkeyBar_Testing Instance { get; private set; }

    //[SerializeField] private PlayerSwapWeapons player;
    [SerializeField] private UI_HotkeyBar uiHotkeyBar = null;

    public Sprite pistolSprite;
    public Sprite shotgunSprite;
    public Sprite swordSprite;
    public Sprite punchSprite;
    public Sprite healthPotionSprite;
    public Sprite manaPotionSprite;

    private HotkeyAbilitySystem hotkeyAbilitySystem;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        hotkeyAbilitySystem = new HotkeyAbilitySystem();
        uiHotkeyBar.SetHotkeyAbilitySystem(hotkeyAbilitySystem);
    }

    private void Update() {
        hotkeyAbilitySystem.Update();
    }

}
