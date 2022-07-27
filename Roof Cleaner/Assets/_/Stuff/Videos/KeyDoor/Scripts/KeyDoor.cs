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

public class KeyDoor : MonoBehaviour {

    [SerializeField] private Key.KeyType keyType = Key.KeyType.Blue;

    private DoorAnims doorAnims;

    private void Awake() {
        doorAnims = GetComponent<DoorAnims>();
    }

    public Key.KeyType GetKeyType() {
        return keyType;
    }

    public void OpenDoor() {
        doorAnims.OpenDoor();
    }

    public void PlayOpenFailAnim() {
        doorAnims.PlayOpenFailAnim();
    }

}
