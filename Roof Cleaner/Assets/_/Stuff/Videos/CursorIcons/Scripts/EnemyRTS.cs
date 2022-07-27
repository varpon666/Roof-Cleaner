using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRTS : MonoBehaviour {

    private HealthSystem healthSystem;

    private void Awake() {
        healthSystem = new HealthSystem(100);
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void Damage(UnitRTS unitRTS, int damageAmount) {
        healthSystem.Damage(damageAmount);

        Vector3 bloodDir = (GetPosition() - unitRTS.GetPosition()).normalized;
        Blood_Handler.SpawnBlood(GetPosition(), bloodDir);

        DamagePopup.Create(GetPosition(), damageAmount, false);
        healthSystem.Damage(damageAmount);
        if (healthSystem.IsDead()) {
            FlyingBody.Create(GameAssets.i.pfEnemyFlyingBody, GetPosition(), bloodDir);
            Destroy(gameObject);
        } else {
            // Knockback
            //transform.position += bloodDir * 5f;
        }
    }

}
