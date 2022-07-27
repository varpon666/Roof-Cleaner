using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelateAnimated : MonoBehaviour {

    [SerializeField] private Material material = null;

    private float pixelateAmount;
    private float pixelateAmountTarget;

    private void Start() {
        pixelateAmountTarget = 0f;
        pixelateAmount = pixelateAmountTarget;
    }

    private void Update() {
        float pixelateSpeed = 20f;
        pixelateAmount = Mathf.Lerp(pixelateAmount, pixelateAmountTarget, Time.deltaTime * pixelateSpeed);
        if (pixelateAmount <= .01f) pixelateAmount = 0f;
        
        if (Input.GetKeyDown(KeyCode.T)) {
            pixelateAmountTarget = .7f;
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            pixelateAmountTarget = 0f;
        }

        material.SetFloat("_PixelateAmount", pixelateAmount);
    }

}
