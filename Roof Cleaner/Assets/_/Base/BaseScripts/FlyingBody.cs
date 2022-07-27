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

public class FlyingBody : MonoBehaviour {

    public static void Create(Transform prefab, Vector3 spawnPosition, Vector3 flyDirection) {
        Transform flyingBodyTransform = Instantiate(prefab, spawnPosition, Quaternion.identity);
        FlyingBody flyingBody = flyingBodyTransform.gameObject.AddComponent<FlyingBody>();
        flyingBody.Setup(flyDirection);
    }

    private Vector3 flyDirection;
    private float timer;
    private float eulerZ;
    private float spawnBloodTimer;

    private void Setup(Vector3 flyDirection) {
        this.flyDirection = flyDirection;
        transform.localScale = Vector3.one * 2f;
        eulerZ = 0f;
    }

    private void Update() {
        float flySpeed = 400f;
        transform.position += flyDirection * flySpeed * Time.deltaTime;

        float scaleSpeed = 7f;
        transform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;

        float eulerSpeed = 360f * 4f;
        eulerZ += eulerSpeed * Time.deltaTime;
        transform.localEulerAngles = new Vector3(0, 0, eulerZ);

        spawnBloodTimer -= Time.deltaTime;
        if (spawnBloodTimer <= 0f) {
            float spawnBloodTimerMax = .016f;
            spawnBloodTimer = spawnBloodTimerMax;
            Blood_Handler.SpawnBlood(5, transform.position, flyDirection * -1f);
        }

        timer += Time.deltaTime;
        if (timer >= 1f) {
            Destroy(gameObject);
        }
    }
}
