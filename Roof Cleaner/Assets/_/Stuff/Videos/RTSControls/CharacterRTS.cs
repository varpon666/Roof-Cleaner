using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class CharacterRTS : MonoBehaviour, IGetPosition {

    [SerializeField] private bool isPlayer = false;

    private HealthSystem healthSystem;
    private IMovePosition movePosition;
    private CharacterRTS targetCharacterRTS;
    private SwordAttack attack;
    private GameObject selectedGameObject;

    private void Awake() {
        healthSystem = new HealthSystem(100);
        movePosition = GetComponent<IMovePosition>();
        attack = GetComponent<SwordAttack>();
        selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedGameObjectVisible(false);
    }

    private void Start() {
        SetMovePosition(GetPosition());
    }

    public void SetSelectedGameObjectVisible(bool visible) {
        selectedGameObject.SetActive(visible);
    }

    public void Damage(CharacterRTS attacker) {
        healthSystem.Damage(56);

        Vector3 dirFromAttacker = (GetPosition() - attacker.GetPosition()).normalized;
        Blood_Handler.SpawnBlood(GetPosition(), dirFromAttacker);

        if (healthSystem.IsDead()) {
            FlyingBody.Create(GameAssets.i.pfEnemyFlyingBody, GetPosition(), dirFromAttacker);
            Destroy(gameObject);
        }
    }

    public void SetMovePosition(Vector3 moveTargetPosition) {
        movePosition.SetMovePosition(moveTargetPosition, () => { });
    }

    public void SetTarget(CharacterRTS targetCharacterRTS) {
        this.targetCharacterRTS  = targetCharacterRTS;

        if (targetCharacterRTS != null) {
            SetMovePosition(targetCharacterRTS.GetPosition());
        }
    }

    private void Update() {
        if (targetCharacterRTS != null) {
            float attackDistance = 14f;
            if (Vector3.Distance(GetPosition(), targetCharacterRTS.GetPosition()) < attackDistance) {
                Vector3 attackDir = (targetCharacterRTS.GetPosition() - GetPosition()).normalized;
                FunctionTimer.Create(() => targetCharacterRTS.Damage(this), .05f);
                this.enabled = false;
                attack.Attack(attackDir, () => {
                    this.enabled = true;
                });
            }
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public HealthSystem GetHealthSystem() {
        return healthSystem;
    }

    public bool IsPlayer() {
        return isPlayer;
    }

}
