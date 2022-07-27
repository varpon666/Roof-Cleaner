using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurAnimated : MonoBehaviour {

    [SerializeField] private Material material = null;

    private float blurAmount;
    private bool blurActive;

    private void Start() {
        blurAmount = 0;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            blurActive = !blurActive;
        }

        float blurSpeed = 15f;
        if (blurActive) {
            blurAmount += blurSpeed * Time.deltaTime;
        } else {
            blurAmount -= blurSpeed * Time.deltaTime;
        }

        blurAmount = Mathf.Clamp(blurAmount, 0f, 4f);
        material.SetFloat("_BlurAmount", blurAmount);
    }

}
