using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;

public class SolarPanel : MonoBehaviour {

    [SerializeField] private Texture2D dirtMaskTextureBase;
    [SerializeField] private Texture2D dirtBrush;
    [SerializeField] private Material material;
    [SerializeField] private TextMeshProUGUI uiText;

    private Texture2D dirtMaskTexture;
    private bool isFlipped;
    private Animation solarAnimation;
    private float dirtAmountTotal;
    private float dirtAmount;
    private Vector2Int lastPaintPixelPosition;

    private void Awake() {
        dirtMaskTexture = new Texture2D(dirtMaskTextureBase.width, dirtMaskTextureBase.height);
        dirtMaskTexture.SetPixels(dirtMaskTextureBase.GetPixels());
        dirtMaskTexture.Apply();
        material.SetTexture("_DirtMask", dirtMaskTexture);

        solarAnimation = GetComponent<Animation>();

        dirtAmountTotal = 0f;
        for (int x = 0; x < dirtMaskTextureBase.width; x++) {
            for (int y = 0; y < dirtMaskTextureBase.height; y++) {
                dirtAmountTotal += dirtMaskTextureBase.GetPixel(x, y).g;
            }
        }
        dirtAmount = dirtAmountTotal;

        FunctionPeriodic.Create(() => {
            uiText.text = Mathf.RoundToInt(GetDirtAmount() * 100f) + "%";
        }, .03f);
    }

    private void Update() {
        if (Input.GetMouseButton(0)) {

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit)) {
                Vector2 textureCoord = raycastHit.textureCoord;

                int pixelX = (int)(textureCoord.x * dirtMaskTexture.width);
                int pixelY = (int)(textureCoord.y * dirtMaskTexture.height);

                Vector2Int paintPixelPosition = new Vector2Int(pixelX, pixelY);
                //Debug.Log("UV: " + textureCoord + "; Pixels: " + paintPixelPosition);

                int paintPixelDistance = Mathf.Abs(paintPixelPosition.x - lastPaintPixelPosition.x) + Mathf.Abs(paintPixelPosition.y - lastPaintPixelPosition.y);
                int maxPaintDistance = 7;
                if (paintPixelDistance < maxPaintDistance) {
                    // Painting too close to last position
                    return;
                }
                lastPaintPixelPosition = paintPixelPosition;

                /*
                // Paint Square in Dirt Mask
                int squareSize = 32;
                int pixelXOffset = pixelX - (dirtBrush.width / 2);
                int pixelYOffset = pixelY - (dirtBrush.height / 2);

                for (int x = 0; x < squareSize; x++) {
                    for (int y = 0; y < squareSize; y++) {
                        dirtMaskTexture.SetPixel(
                            pixelXOffset + x,
                            pixelYOffset + y,
                            Color.black
                        );
                    }
                }
                //*/


                //* 
                int pixelXOffset = pixelX - (dirtBrush.width / 2);
                int pixelYOffset = pixelY - (dirtBrush.height / 2);

                for (int x = 0; x < dirtBrush.width; x++) {
                    for (int y = 0; y < dirtBrush.height; y++) {
                        Color pixelDirt = dirtBrush.GetPixel(x, y);
                        Color pixelDirtMask = dirtMaskTexture.GetPixel(pixelXOffset + x, pixelYOffset + y);

                        float removedAmount = pixelDirtMask.g - (pixelDirtMask.g * pixelDirt.g);
                        dirtAmount -= removedAmount;

                        dirtMaskTexture.SetPixel(
                            pixelXOffset + x, 
                            pixelYOffset + y, 
                            new Color(0, pixelDirtMask.g * pixelDirt.g, 0)
                        );
                    }
                }
                //*/

                dirtMaskTexture.Apply();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            isFlipped = !isFlipped;
            if (isFlipped) {
                solarAnimation.Play("SolarPanelFlip");
            } else {
                solarAnimation.Play("SolarPanelFlipBack");
            }
        }
    }

    private float GetDirtAmount() {
        return this.dirtAmount / dirtAmountTotal;
    }

}
