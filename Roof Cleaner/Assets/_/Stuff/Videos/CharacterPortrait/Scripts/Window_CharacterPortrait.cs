/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class Window_CharacterPortrait : MonoBehaviour {

    private static Dictionary<Character, Window_CharacterPortrait> windowDictionary;

    private Transform cameraTransform;
    private Transform followTransform;
    private Character character;
    
    private Text levelText;
    private Text strText;
    private Text dexText;
    private Text conText;
    private Text wisText;
    private Transform experienceBar;

    private void Awake() {
        cameraTransform = transform.Find("camera");

        transform.Find("closeBtn").GetComponent<Button_UI>().ClickFunc = DestroyWindow;
        
        levelText = transform.Find("levelText").GetComponent<Text>();
        strText = transform.Find("strText").GetComponent<Text>();
        dexText = transform.Find("dexText").GetComponent<Text>();
        conText = transform.Find("conText").GetComponent<Text>();
        wisText = transform.Find("wisText").GetComponent<Text>();

        experienceBar = transform.Find("experienceBar");
    }

    private void Update() {
        cameraTransform.position = new Vector3(followTransform.position.x, followTransform.position.y, Camera.main.transform.position.z);
    }

    private void UpdateExperienceBar() {
        experienceBar.localScale = new Vector3(character.GetExperienceNormalized(), 1, 1);
    }

    private void UpdateStats() {
        levelText.text = "Level: " + character.level.ToString();
        strText.text = character.STR.ToString();
        dexText.text = character.DEX.ToString();
        conText.text = character.CON.ToString();
        wisText.text = character.WIS.ToString();
    }

    private void Show(Character character) {
        this.character = character;
        followTransform = character.transform;

        RenderTexture renderTexture = new RenderTexture(512, 512, 16);
        transform.Find("camera").GetComponent<Camera>().targetTexture = renderTexture;
        transform.Find("rawImage").GetComponent<RawImage>().texture = renderTexture;

        transform.Find("nameText").GetComponent<Text>().text = character.name;
        
        UpdateExperienceBar();
        UpdateStats();

        character.OnExperienceGained += delegate (object sender, EventArgs e) { UpdateExperienceBar(); };
        character.OnLeveledUp += delegate (object sender, EventArgs e) {
            UpdateExperienceBar();
            UpdateStats();
        };
    }

    private void DestroyWindow() {
        windowDictionary.Remove(character);
        Destroy(gameObject);
    }

    public static void Show_Static(Character character) {
        if (windowDictionary == null) {
            windowDictionary = new Dictionary<Character, Window_CharacterPortrait>();
        }

        if (!windowDictionary.ContainsKey(character)) {
            Transform windowCharacterPortraitTransform = Instantiate(CharacterPortrait_GameHandler.instance.pfWindow_CharacterPortrait);
            windowCharacterPortraitTransform.SetParent(CharacterPortrait_GameHandler.instance.canvas.transform, false);
            windowCharacterPortraitTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(UnityEngine.Random.Range(-500, 500), UnityEngine.Random.Range(-200, 200));

            Window_CharacterPortrait windowCharacterPortrait = windowCharacterPortraitTransform.GetComponent<Window_CharacterPortrait>();
            windowCharacterPortrait.Show(character);

            windowDictionary[character] = windowCharacterPortrait;
        }
    }
}
