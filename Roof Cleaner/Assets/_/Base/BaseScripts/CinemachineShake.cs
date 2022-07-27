using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using CodeMonkey.Utils;

public class CinemachineShake : MonoBehaviour {

    public static CinemachineShake Instance { get; private set; }

    public static void ScreenShake_Static(float intensity = 1f, float timer = .1f) {
        if (Instance == null) {
            CinemachineVirtualCamera cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            if (cinemachineVirtualCamera == null) {
                Debug.LogError("No Cinemachine Camera in the scene!");
            } else {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                if (cinemachineBasicMultiChannelPerlin == null) {
                    cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                    cinemachineBasicMultiChannelPerlin.m_NoiseProfile = GameAssets.i.cinemachineDefaultNoiseProfile;
                    cinemachineBasicMultiChannelPerlin.m_FrequencyGain = 5f;
                }
                cinemachineVirtualCamera.gameObject.AddComponent<CinemachineShake>();
            }
        }
        Instance.ScreenShake(intensity, timer);
    }




    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Awake() {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ScreenShake(float intensity = 1f, float timer = .1f) {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        FunctionTimer.Create(() => { cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f; }, timer);
    }

}
