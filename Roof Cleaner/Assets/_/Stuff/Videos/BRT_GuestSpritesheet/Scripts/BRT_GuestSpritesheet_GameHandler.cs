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
using CodeMonkey;
using CodeMonkey.Utils;

public class BRT_GuestSpritesheet_GameHandler : MonoBehaviour {

    [SerializeField] private Texture2D baseTexture = null;
    //[SerializeField] private Texture2D headTexture = null;
    //[SerializeField] private Texture2D bodyTexture = null;
    [SerializeField] private Texture2D bodyTextureWhite = null;
    [SerializeField] private Texture2D bodyTextureMask = null;
    [SerializeField] private Texture2D hairTexture = null;
    [SerializeField] private Texture2D beardTexture = null;
    [SerializeField] private Texture2D baseHeadTexture = null;
    [SerializeField] private Texture2D baseHeadMaskTexture = null;
    [SerializeField] private Texture2D handTexture = null;
    [SerializeField] private Material guestMaterial = null;

    private Color primaryColor = Color.red;
    private Color secondaryColor = Color.yellow;
    private GuestSpritesheetData guestSpritesheetData;

    private void Awake() {
        guestSpritesheetData = GuestSpritesheetData.GenerateRandom();
        SetGuestSpritesheetData(guestSpritesheetData);

        CMDebug.ButtonUI(new Vector2(100, -170), "Randomize", () => { 
            guestSpritesheetData = GuestSpritesheetData.GenerateRandom();
            SetGuestSpritesheetData(guestSpritesheetData);
        });
        CMDebug.ButtonUI(new Vector2(250, -170), "Save", () => {
            guestSpritesheetData.Save();
            CMDebug.TextPopupMouse("Saved!");
        });
        CMDebug.ButtonUI(new Vector2(400, -170), "Load", () => {
            guestSpritesheetData = GuestSpritesheetData.Load_Static();
            SetGuestSpritesheetData(guestSpritesheetData);
            CMDebug.TextPopupMouse("Loaded!");
        });
    }

    private void SetGuestSpritesheetData(GuestSpritesheetData guestSpritesheetData) {
        Texture2D texture = GetTexture(guestSpritesheetData);
        guestMaterial.mainTexture = texture;
    }

    private Texture2D GetTexture(GuestSpritesheetData guestSpritesheetData) {
        Texture2D texture = new Texture2D(512, 512, TextureFormat.RGBA32, true);

        Color[] spritesheetBasePixels = baseTexture.GetPixels(0, 0, 512, 512);
        texture.SetPixels(0, 0, 512, 512, spritesheetBasePixels);

        Color skinColor = guestSpritesheetData.skinColor;

        Color[] headPixels = baseHeadTexture.GetPixels(0, 0, 128, 128);
        Color[] headSkinMaskPixels = baseHeadMaskTexture.GetPixels(0, 0, 128, 128);
        TintColorArraysInsideMask(headPixels, skinColor, headSkinMaskPixels);
        
        Color[] handPixels = handTexture.GetPixels(0, 0, 64, 64);
        TintColorArray(handPixels, skinColor);
        texture.SetPixels(384, 448, 64, 64, handPixels);

        Color hairColor = guestSpritesheetData.hairColor;

        bool hasHair = guestSpritesheetData.hairIndex != -1;
        if (hasHair) {
            int hairIndex = guestSpritesheetData.hairIndex;
            Color[] hairPixels = hairTexture.GetPixels(128 * hairIndex, 0, 128, 128);
            TintColorArray(hairPixels, hairColor);
            MergeColorArrays(headPixels, hairPixels);
        }

        bool hasBeard = guestSpritesheetData.beardIndex != -1;
        if (hasBeard) {
            int beardIndex = guestSpritesheetData.beardIndex;
            Color[] beardPixels = beardTexture.GetPixels(128 * beardIndex, 0, 128, 128);
            TintColorArray(beardPixels, hairColor);
            MergeColorArrays(headPixels, beardPixels);
        }

        texture.SetPixels(0, 384, 128, 128, headPixels);

        int bodyIndex = guestSpritesheetData.bodyIndex;
        Color[] bodyPixels = bodyTextureWhite.GetPixels(128 * bodyIndex, 0, 128, 128);
        Color[] bodyMaskPixels = bodyTextureMask.GetPixels(128 * bodyIndex, 0, 128, 128);
        Color primaryColor = guestSpritesheetData.bodyPrimaryColor;
        Color primaryMaskColor = new Color(0, 1, 0);
        TintColorArraysInsideMask(bodyPixels, primaryColor, bodyMaskPixels, primaryMaskColor);
        Color secondaryColor = guestSpritesheetData.bodySecondaryColor;
        Color secondaryMaskColor = new Color(0, 0, 1);
        TintColorArraysInsideMask(bodyPixels, secondaryColor, bodyMaskPixels, secondaryMaskColor);
        texture.SetPixels(0, 256, 128, 128, bodyPixels);

        texture.Apply();

        return texture;
    }

    private void MergeColorArrays(Color[] baseArray, Color[] overlay) {
        for (int i = 0; i < baseArray.Length; i++) {
            if (overlay[i].a > 0) {
                // Overlay has color
                if (overlay[i].a >= 1) {
                    // Fully replace
                    baseArray[i] = overlay[i];
                } else {
                    // Interpolate colors
                    float alpha = overlay[i].a;
                    baseArray[i].r += (overlay[i].r - baseArray[i].r) * alpha;
                    baseArray[i].g += (overlay[i].g - baseArray[i].g) * alpha;
                    baseArray[i].b += (overlay[i].b - baseArray[i].b) * alpha;
                    baseArray[i].a += overlay[i].a;
                }
            }
        }
    }

    private void TintColorArray(Color[] baseArray, Color tint) {
        for (int i = 0; i < baseArray.Length; i++) {
            // Apply tint
            baseArray[i].r = baseArray[i].r * tint.r;
            baseArray[i].g = baseArray[i].g * tint.g;
            baseArray[i].b = baseArray[i].b * tint.b;
        }
    }

    private void TintColorArraysInsideMask(Color[] baseArray, Color tint, Color[] mask) {
        for (int i = 0; i < baseArray.Length; i++) {
            if (mask[i].a > 0) {
                // Apply tint
                Color baseColor = baseArray[i];
                Color fullyTintedColor = tint * baseColor;
                float interpolateAmount = mask[i].a;
                baseArray[i].r += (fullyTintedColor.r - baseColor.r) * interpolateAmount;
                baseArray[i].g += (fullyTintedColor.g - baseColor.g) * interpolateAmount;
                baseArray[i].b += (fullyTintedColor.b - baseColor.b) * interpolateAmount;
            }
        }

    }

    private void TintColorArraysInsideMask(Color[] baseArray, Color tint, Color[] mask, Color maskColor) {
        for (int i = 0; i < baseArray.Length; i++) {
            if (mask[i].a > 0 && mask[i] == maskColor) {
                // Apply tint
                Color baseColor = baseArray[i];
                Color fullyTintedColor = tint * baseColor;
                float interpolateAmount = mask[i].a;
                baseArray[i].r += (fullyTintedColor.r - baseColor.r) * interpolateAmount;
                baseArray[i].g += (fullyTintedColor.g - baseColor.g) * interpolateAmount;
                baseArray[i].b += (fullyTintedColor.b - baseColor.b) * interpolateAmount;
            }
        }

    }



}
