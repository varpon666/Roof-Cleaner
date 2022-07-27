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


/*
 * Enable Lights when Player enters trigger
 * */
public class EnableLights : MonoBehaviour {
    
    [SerializeField] private CaptureOnTriggerEnter2D enableLightsTrigger = null;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D[] lightArray = null;
    [SerializeField] private float targetLightIntensity = 1f;
    [SerializeField] private float lightIntensitySpeed = 1f;

    private float lightIntensity;

    private void Start() {
        if (enableLightsTrigger != null) {
            enableLightsTrigger.OnPlayerTriggerEnter2D += EnableLightsTrigger_OnPlayerTriggerEnter2D;
        }
        
        foreach (UnityEngine.Rendering.Universal.Light2D light in lightArray) {
            light.intensity = 0f;
        }

        enabled = false;
    }

    private void EnableLightsTrigger_OnPlayerTriggerEnter2D(object sender, System.EventArgs e) {
        TurnLightsOn();
        enableLightsTrigger.OnPlayerTriggerEnter2D -= EnableLightsTrigger_OnPlayerTriggerEnter2D;
    }

    private void Update() {
        lightIntensity += lightIntensitySpeed * Time.deltaTime;
        lightIntensity = Mathf.Clamp(lightIntensity, 0f, targetLightIntensity);

        foreach (UnityEngine.Rendering.Universal.Light2D light in lightArray) {
            light.intensity = lightIntensity;
        }

        if (lightIntensity >= targetLightIntensity) {
            enabled = false;
        }
    }

    public void TurnLightsOn() {
        enabled = true;
    }

}
