using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey.InventorySystem;

public class UnitRTS : MonoBehaviour {

    [SerializeField] private LayerMask wallLayermask = default(LayerMask);

    private Character_Base characterBase;
    private GameObject selectedGameObject;
    private IMovePosition movePosition;
    private EnemyRTS enemyRTS;
    private State state;

    private enum State {
        Normal,
        Attacking
    }

    private void Awake() {
        characterBase = GetComponent<Character_Base>();
        selectedGameObject = transform.Find("Selected").gameObject;
        movePosition = GetComponent<IMovePosition>();
        SetSelectedVisible(false);
        state = State.Normal;
    }

    private void Update() {
        switch (state) {
            case State.Normal:
                if (enemyRTS != null) {
                    float attackRange = 50f;
                    if (Vector3.Distance(transform.position, enemyRTS.GetPosition()) < attackRange) {
                        Vector3 dirToEnemy = (enemyRTS.GetPosition() - GetPosition()).normalized;
                        if (!Physics2D.Raycast(GetPosition(), dirToEnemy, Vector3.Distance(GetPosition(), enemyRTS.GetPosition()), wallLayermask)) {
                            MoveTo(transform.position);
                            GetComponent<IMoveVelocity>().Disable();
                            Vector3 attackDir = (enemyRTS.GetPosition() - transform.position).normalized;
                            UtilsClass.ShakeCamera(.6f, .1f);
                            CinemachineShake.ScreenShake_Static(20f, .1f);

                            characterBase.PlayShootAnimation(attackDir, (Vector3 vec) => {
                                if (enemyRTS == null) return;
                                Shoot_Flash.AddFlash(vec);
                                WeaponTracer.Create(vec, enemyRTS.GetPosition());
                                enemyRTS.Damage(this, Random.Range(5, 15));
                            }, () => {
                                characterBase.PlayIdleAnim();
                                GetComponent<IMoveVelocity>().Enable();
                                state = State.Normal;
                            });
                            state = State.Attacking;
                        } else {
                            // Something blocking path, move closer
                            MoveTo(enemyRTS.GetPosition());
                        }
                    } else {
                        // Move Closer
                        MoveTo(enemyRTS.GetPosition());
                    }
                } else {
                    // No enemy
                }
                break;
            case State.Attacking:
                break;
        }
    }

    public void SetSelectedVisible(bool visible) {
        selectedGameObject.SetActive(visible);
    }

    public void MoveTo(Vector3 targetPosition) {
        movePosition.SetMovePosition(targetPosition, () => { });
    }

    public void SetTarget(EnemyRTS enemyRTS) {
        this.enemyRTS = enemyRTS;
    }

    public void ClearTarget() {
        this.enemyRTS = null;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if (itemWorld != null) {
            itemWorld.DestroySelf();
        }
    }

}
