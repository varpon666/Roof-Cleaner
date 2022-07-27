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

public class Key : MonoBehaviour {

    [SerializeField] private KeyType keyType = KeyType.Green;

    public enum KeyType {
        Red,
        Green,
        Blue
    }

    public KeyType GetKeyType() {
        return keyType;
    }

    public static Color GetColor(KeyType keyType) {
        switch (keyType) {
        default:
        case KeyType.Red:       return Color.red;
        case KeyType.Green:     return Color.green;
        case KeyType.Blue:      return Color.blue;
        }
    }

}
