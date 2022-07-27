using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GuestSpritesheetData {

    public int hairIndex;
    public int beardIndex;
    public int bodyIndex;
    
    public Color hairColor;
    public Color skinColor;
    public Color bodyPrimaryColor;
    public Color bodySecondaryColor;

    public void Save() {
        string jsonString = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("GuestSpritesheetData", jsonString);
        Debug.Log(jsonString);
    }

    public static GuestSpritesheetData Load_Static() {
        string jsonString = PlayerPrefs.GetString("GuestSpritesheetData");
        Debug.Log(jsonString);
        return JsonUtility.FromJson<GuestSpritesheetData>(jsonString);
    }

    public static GuestSpritesheetData GenerateRandom() {
        Color[] skinColorArray = new [] {
            UtilsClass.GetColorFromString("FFE9C6"),
            UtilsClass.GetColorFromString("FFD8A0"),
            UtilsClass.GetColorFromString("D8C19F"),
            UtilsClass.GetColorFromString("D8AC6C"),
            UtilsClass.GetColorFromString("D89774"),
            UtilsClass.GetColorFromString("D1925F"),
            UtilsClass.GetColorFromString("BF8759"),
            UtilsClass.GetColorFromString("86644C"),
            UtilsClass.GetColorFromString("3D2D22"),
        };
        Color skinColor = skinColorArray[Random.Range(0, skinColorArray.Length)];
        
        Color[] hairColorArray = new[] {
            UtilsClass.GetColorFromString("503D30"),
            UtilsClass.GetColorFromString("D4B60C"),
            UtilsClass.GetColorFromString("5B4636"),
            UtilsClass.GetColorFromString("000000"),
            UtilsClass.GetColorFromString("5B5B5B"),
            UtilsClass.GetColorFromString("BCBCBC"),
            UtilsClass.GetColorFromString("564336"),
        };
        Color hairColor = hairColorArray[Random.Range(0, hairColorArray.Length)];
        
        Color bodyPrimaryColor = Color.red;
        Color bodySecondaryColor = Color.yellow;

        int hairIndex;
        bool hasHair = Random.Range(0, 100) < 70;
        if (hasHair) {
            hairIndex = Random.Range(0, 4);
        } else {
            hairIndex = -1;
        }

        int beardIndex;
        bool hasBeard = Random.Range(0, 100) < 70;
        if (hasBeard) {
            beardIndex = Random.Range(0, 4);
        } else {
            beardIndex = -1;
        }
        
        int bodyIndex = Random.Range(0, 4);

        return new GuestSpritesheetData { 
            bodyIndex = bodyIndex,
            beardIndex = beardIndex,
            hairIndex = hairIndex,

            bodyPrimaryColor = bodyPrimaryColor,
            bodySecondaryColor = bodySecondaryColor,

            skinColor = skinColor,
            hairColor = hairColor,
        };
    }
}
