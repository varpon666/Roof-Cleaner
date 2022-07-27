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

public class KeyHolder : MonoBehaviour {

    public event EventHandler OnKeysChanged;

    private List<Key.KeyType> keyList;

    private void Awake() {
        keyList = new List<Key.KeyType>();
    }

    public List<Key.KeyType> GetKeyList() {
        return keyList;
    }

    public void AddKey(Key.KeyType keyType) {
        Debug.Log("Added Key: " + keyType);
        keyList.Add(keyType);
        OnKeysChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveKey(Key.KeyType keyType) {
        keyList.Remove(keyType);
        OnKeysChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool ContainsKey(Key.KeyType keyType) {
        return keyList.Contains(keyType);
    }


    private void OnTriggerEnter2D(Collider2D collider) {
        Key key = collider.GetComponent<Key>();
        if (key != null) {
            AddKey(key.GetKeyType());
            Destroy(key.gameObject);
        }

        KeyDoor keyDoor = collider.GetComponent<KeyDoor>();
        if (keyDoor != null) {
            if (ContainsKey(keyDoor.GetKeyType())) {
                // Currently holding Key to open this door
                RemoveKey(keyDoor.GetKeyType());
                keyDoor.OpenDoor();
            } else {
                keyDoor.PlayOpenFailAnim();
            }
        }
    }

}
