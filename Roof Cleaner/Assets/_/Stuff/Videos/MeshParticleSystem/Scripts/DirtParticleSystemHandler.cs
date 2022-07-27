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

public class DirtParticleSystemHandler : MonoBehaviour {

    public static DirtParticleSystemHandler Instance { get; private set; }

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
                single.DestroySelf();
                singleList.RemoveAt(i);
                i--;
            }
        }
    }

    public void SpawnDirt(Vector3 position, Vector3 direction) {
        singleList.Add(new Single(position, direction, meshParticleSystem));
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
        private int uvIndex;
        private float uvIndexTimer;
        private float uvIndexTimerMax;

        public Single(Vector3 position, Vector3 direction, MeshParticleSystem meshParticleSystem) {
            this.position = position;
            this.direction = direction;
            this.meshParticleSystem = meshParticleSystem;

            quadSize = new Vector3(2.5f, 2.5f);
            moveSpeed = Random.Range(20f, 30f);
            uvIndex = 0;
            uvIndexTimerMax = 1f / 10;

            quadIndex = meshParticleSystem.AddQuad(position, 0f, quadSize, false, uvIndex);
        }

        public void Update() {
            uvIndexTimer += Time.deltaTime;
            if (uvIndexTimer >= uvIndexTimerMax) {
                uvIndexTimer -= uvIndexTimerMax;
                uvIndex++;
            }
            position += direction * moveSpeed * Time.deltaTime;

            meshParticleSystem.UpdateQuad(quadIndex, position, 0f, quadSize, false, uvIndex);

            float slowDownFactor = 3.5f;
            moveSpeed -= moveSpeed * slowDownFactor * Time.deltaTime;
        }

        public bool IsParticleComplete() {
            return uvIndex >= 8 || moveSpeed < .1f;
        }

        public void DestroySelf() {
            meshParticleSystem.DestroyQuad(quadIndex);
        }

    }

}
