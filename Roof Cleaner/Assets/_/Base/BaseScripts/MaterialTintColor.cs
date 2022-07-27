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

public class MaterialTintColor : MonoBehaviour {
    
    private Material material;
    private Color materialTintColor;
    private float tintFadeSpeed;

    private void Awake() {
        materialTintColor = new Color(1, 0, 0, 0);
        SetMaterial(transform.Find("Body").GetComponent<MeshRenderer>().material);
        tintFadeSpeed = 6f;
    }

    private void Update() {
        if (materialTintColor.a > 0) {
            materialTintColor.a = Mathf.Clamp01(materialTintColor.a - tintFadeSpeed * Time.deltaTime);
            material.SetColor("_Tint", materialTintColor);
        }
    }

    public void SetMaterial(Material material) {
        this.material = material;
    }

    public void SetTintColor(Color color) {
        materialTintColor = color;
        material.SetColor("_Tint", materialTintColor);
    }

    public void SetTintFadeSpeed(float tintFadeSpeed) {
        this.tintFadeSpeed = tintFadeSpeed;
    }

}
