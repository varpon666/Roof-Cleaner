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
using CodeMonkey.Utils;

public class ProjectileBolt : MonoBehaviour {

    public static void Create(Vector3 spawnPosition, Vector3 moveDir) {
        Transform boltTransform = Instantiate(GameAssets.i.pfBolt, spawnPosition, Quaternion.identity);

        ProjectileBolt projectileBolt = boltTransform.GetComponent<ProjectileBolt>();
        projectileBolt.Setup(moveDir);
    }


    private Vector3 moveDir;
    private float destroyTimer;

    private void Setup(Vector3 moveDir) {
        this.moveDir = moveDir;
        destroyTimer = 1f;
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(moveDir));
    }

    private void Update() {
        float moveSpeed = 200f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        destroyTimer -= Time.deltaTime;
        if (destroyTimer < 0f) {
            Destroy(gameObject);
        }
    }

}
