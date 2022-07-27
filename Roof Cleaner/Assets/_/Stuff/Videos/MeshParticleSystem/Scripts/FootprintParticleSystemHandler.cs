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

public class FootprintParticleSystemHandler : MonoBehaviour {

    public static FootprintParticleSystemHandler Instance { get; private set; }

    private MeshParticleSystem meshParticleSystem;

    private void Awake() {
        Instance = this;
        meshParticleSystem = GetComponent<MeshParticleSystem>();
    }

    public void SpawnFootprint(Vector3 position, Vector3 direction) {
        Vector3 quadSize = new Vector3(3f, 3f);
        float rotation = UtilsClass.GetAngleFromVectorFloat(direction) + 90f;
        meshParticleSystem.AddQuad(position, rotation, quadSize, false, 0);
    }

}
