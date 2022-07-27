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

namespace TopDownShooter {
    public class ShieldField : MonoBehaviour {

        [Serializable]
        public class ShieldFieldTransformerPowerLines {
            public Transform[] powerLineArray;
        }

        [Serializable]
        public class ShieldFieldTransformerLink {
            public ShieldFieldTransformer shieldFieldTransformer;
            public ShieldFieldTransformerPowerLines shieldFieldTransformerPowerLines;
        }

        public ShieldFieldTransformerLink[] shieldFieldTransformerLinkArray;


        private void Start() {
            foreach (ShieldFieldTransformerLink shieldFieldTransformerLink in shieldFieldTransformerLinkArray) {
                shieldFieldTransformerLink.shieldFieldTransformer.OnDestroyed += ShieldFieldTransformer_OnDestroyed;
            }
        }

        private void ShieldFieldTransformer_OnDestroyed(object sender, EventArgs e) {
            ShieldFieldTransformer shieldFieldTransformer = sender as ShieldFieldTransformer;
            ShieldFieldTransformerLink shieldFieldTransformerLink = GetShieldFieldTransformerLink(shieldFieldTransformer);
            foreach (Transform powerLine in shieldFieldTransformerLink.shieldFieldTransformerPowerLines.powerLineArray) {
                Destroy(powerLine.gameObject);
            }
            TestAllTransformersDead();
        }

        public ShieldFieldTransformerLink GetShieldFieldTransformerLink(ShieldFieldTransformer shieldFieldTransformer) {
            foreach (ShieldFieldTransformerLink shieldFieldTransformerLink in shieldFieldTransformerLinkArray) {
                if (shieldFieldTransformerLink.shieldFieldTransformer == shieldFieldTransformer) {
                    return shieldFieldTransformerLink;
                }
            }
            return null;
        }

        private void TestAllTransformersDead() {
            bool allDead = true;
            foreach (ShieldFieldTransformerLink shieldFieldTransformerLink in shieldFieldTransformerLinkArray) {
                if (shieldFieldTransformerLink.shieldFieldTransformer.IsAlive()) {
                    allDead = false;
                    break;
                }
            }

            if (allDead) {
                // All transformers are dead!
                Destroy(gameObject);
            }
        }

    }
}