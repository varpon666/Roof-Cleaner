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
using CodeMonkey.Utils;

namespace TopDownShooter {
    public class ShieldFieldTransformer : MonoBehaviour {

        public event EventHandler OnDestroyed;

        private SpriteRenderer spriteRenderer;
        private HealthSystem healthSystem;
        private World_Bar healthBar;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            healthSystem = new HealthSystem(40);
            healthBar = new World_Bar(transform, new Vector3(0, 1), new Vector3(12, 1.5f), Color.grey, Color.red, 1f, 10000, new World_Bar.Outline { color = Color.black, size = .5f });
            healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
            healthSystem.OnDead += HealthSystem_OnDead;
        }

        private void HealthSystem_OnHealthChanged(object sender, EventArgs e) {
            healthBar.SetSize(healthSystem.GetHealthNormalized());
        }

        private void HealthSystem_OnDead(object sender, EventArgs e) {
            //spriteRenderer.sprite = GameAssets.i.s_ShieldTransformerDestroyed;
            //spriteRenderer.material = GameAssets.i.m_SpritesDefault;
            healthBar.Hide();

            OnDestroyed?.Invoke(this, EventArgs.Empty);
        }

        public void Damage(Player attacker, float damageMultiplier) {
            if (healthSystem.IsDead()) return; // Already dead
            healthSystem.Damage(Mathf.RoundToInt(10 * damageMultiplier));
        }

        public bool IsAlive() {
            return !healthSystem.IsDead();
        }

    }
}