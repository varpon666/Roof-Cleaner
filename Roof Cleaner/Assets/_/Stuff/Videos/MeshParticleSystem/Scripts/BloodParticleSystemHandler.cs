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

public class BloodParticleSystemHandler : MonoBehaviour {

    public static BloodParticleSystemHandler Instance { get; private set; }

    private MeshParticleSystem meshParticleSystem;
    private List<Single> singleList;

    private void Awake() {
        Instance = this;
        meshParticleSystem = GetComponent<MeshParticleSystem>();
        singleList = new List<Single>();
    }

    private void Update() {
        for (int i=0; i<singleList.Count; i++) {
            Single single = singleList[i];
            single.Update();
            if (single.IsParticleComplete()) {
                singleList.RemoveAt(i);
                i--;
            }
        }
    }

    public void SpawnBlood(Vector3 position, Vector3 direction) {
        SpawnBlood(3, position, direction);
    }

    public void SpawnBlood(int bloodParticleCount, Vector3 position, Vector3 direction) {
        for (int i = 0; i < bloodParticleCount; i++) {
            Vector3 dir;
            if (meshParticleSystem.GetAxis() == MeshParticleSystem.Axis.XY) {
                dir = UtilsClass.ApplyRotationToVector(direction, Random.Range(-15f, 15f));
            } else {
                dir = UtilsClass.ApplyRotationToVectorXZ(direction, Random.Range(-15f, 15f));
            }
            singleList.Add(new Single(position, dir, meshParticleSystem));
        }
    }


    /*
     * Represents a single Dirt Particle
     * */
    private class Single {

        private MeshParticleSystem meshParticleSystem;
        private Vector3 position;
        private Vector3 direction;
        private int quadIndex;
        private Vector3 quadSize;
        private float moveSpeed;
        private float rotation;
        private int uvIndex;

        public Single(Vector3 position, Vector3 direction, MeshParticleSystem meshParticleSystem) {
            this.position = position;
            this.direction = direction;
            this.meshParticleSystem = meshParticleSystem;

            quadSize = new Vector3(2.5f, 2.5f);
            if (meshParticleSystem.GetAxis() == MeshParticleSystem.Axis.XZ) {
                quadSize = new Vector3(quadSize.x, 0, quadSize.y);
            }
            rotation = Random.Range(0, 360f);
            moveSpeed = Random.Range(50f, 70f);
            uvIndex = Random.Range(0, 8);

            quadIndex = meshParticleSystem.AddQuad(position, rotation, quadSize, false, uvIndex);
        }

        public void Update() {
            position += direction * moveSpeed * Time.deltaTime;
            rotation += 360f * (moveSpeed / 10f) * Time.deltaTime;

            meshParticleSystem.UpdateQuad(quadIndex, position, rotation, quadSize, false, uvIndex);

            float slowDownFactor = 3.5f;
            moveSpeed -= moveSpeed * slowDownFactor * Time.deltaTime;
        }

        public bool IsParticleComplete() {
            return moveSpeed < .1f;
        }

    }

}
