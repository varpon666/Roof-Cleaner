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
using UnityEngine.Experimental.Rendering.Universal;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;

namespace TopDownShooter {
    public class Player : MonoBehaviour, Enemy.IEnemyTargetable {

        public static Player Instance { get; private set; }

        public event EventHandler OnWeaponChanged;
        public event EventHandler OnPickedUpWeapon;
        public event EventHandler OnDodged;

        private enum State {
            Normal,
        }

        private PlayerMain playerMain;

        private State state;
        private HealthSystem healthSystem;
        private World_Bar healthBar;
        private Transform aimLightTransform;
        private float invulnerableTime;
        private MaterialTintColor materialTintColor;

        private Weapon weapon;
        private Weapon weaponPistol;
        private Weapon weaponShotgun;
        private Weapon weaponRifle;
        private Weapon weaponPunch;

        private bool canUseShotgun;
        private bool canUseRifle;

        private void Awake() {
            Instance = this;
            playerMain = GetComponent<PlayerMain>();
            materialTintColor = GetComponent<MaterialTintColor>();
            aimLightTransform = transform.Find("AimLight");
            state = State.Normal;
            healthSystem = new HealthSystem(100);
            healthBar = new World_Bar(transform, new Vector3(0, 10), new Vector3(12, 1.5f), Color.grey, Color.red, 1f, 10000, new World_Bar.Outline { color = Color.black, size = .5f });
            healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
            weaponPistol = new Weapon(Weapon.WeaponType.Pistol);
            weaponShotgun = new Weapon(Weapon.WeaponType.Shotgun);
            weaponRifle = new Weapon(Weapon.WeaponType.Rifle);
            weaponPunch = new Weapon(Weapon.WeaponType.Punch);
        }

        private void Start() {
            materialTintColor.SetMaterial(transform.Find("Body").GetComponent<MeshRenderer>().material);
            SetWeapon(weaponRifle);
            playerMain.PlayerSwapAimNormal.OnShoot += PlayerSwapAimNormal_OnShoot;

            switch (state) {
                default:
                    break;
            }
        }

        public void SetCanUseShotgun() {
            canUseShotgun = true;
            SetWeapon(weaponShotgun);
        }

        public void SetCanUseRifle() {
            canUseRifle = true;
            SetWeapon(weaponRifle);
        }

        public void SetWeapon(Weapon weapon) {
            this.weapon = weapon;
            //playerMain.PlayerSwapAimNormal.SetWeapon(weapon);
            OnWeaponChanged?.Invoke(this, EventArgs.Empty);
        }

        public Weapon GetWeapon() {
            return weapon;
        }

        private void PlayerSwapAimNormal_OnShoot(object sender, CharacterAim_Base.OnShootEventArgs e) {
            Shoot_Flash.AddFlash(e.gunEndPointPosition);
            WeaponTracer.Create(e.gunEndPointPosition, e.shootPosition);
            UtilsClass.ShakeCamera(.6f, .05f);
            SpawnBulletShellCasing(e.gunEndPointPosition, e.shootPosition);

            if (weapon.GetWeaponType() == Weapon.WeaponType.Shotgun) {
                // Shotgun spread
                int shotgunShells = 4;
                for (int i = 0; i < shotgunShells; i++) {
                    WeaponTracer.Create(e.gunEndPointPosition, e.shootPosition + UtilsClass.GetRandomDir() * UnityEngine.Random.Range(-20f, 20f));
                    if (i % 2 == 0) {
                        SpawnBulletShellCasing(e.gunEndPointPosition, e.shootPosition);
                    }
                }
            }

            switch (weapon.GetWeaponType()) {
            default:
                break;
            //case Weapon.WeaponType.Pistol: Sound_Manager.PlaySound(Sound_Manager.Sound.Pistol_Fire, GetPosition()); break;
            //case Weapon.WeaponType.Rifle: Sound_Manager.PlaySound(Sound_Manager.Sound.Rifle_Fire, GetPosition()); break;
            //case Weapon.WeaponType.Shotgun: Sound_Manager.PlaySound(Sound_Manager.Sound.Shotgun_Fire, GetPosition()); break;
            }

            // Any enemy hit?
            if (e.hitObject != null) {
                Enemy enemy = e.hitObject.GetComponent<Enemy>();
                if (enemy != null) {
                    enemy.Damage(this, weapon.GetDamageMultiplier());
                }
                ShieldFieldTransformer shieldFieldTransformer = e.hitObject.GetComponent<ShieldFieldTransformer>();
                if (shieldFieldTransformer != null) {
                    shieldFieldTransformer.Damage(this, weapon.GetDamageMultiplier());
                }
            }
        }

        private void SpawnBulletShellCasing(Vector3 gunEndPointPosition, Vector3 shootPosition) {
            Vector3 shellSpawnPosition = gunEndPointPosition;
            Vector3 shootDir = (shootPosition - gunEndPointPosition).normalized;
            float backOffsetPosition = 8f;
            if (weapon.GetWeaponType() == Weapon.WeaponType.Pistol) {
                backOffsetPosition = 6f;
            }
            shellSpawnPosition += (shootDir * -1f) * backOffsetPosition;

            float applyRotation = UnityEngine.Random.Range(+130f, +95f);
            if (shootDir.x < 0) {
                applyRotation *= -1f;
            }
            //Sound_Manager.PlaySound(Sound_Manager.Sound.BulletShell, GetPosition());

            Vector3 shellMoveDir = UtilsClass.ApplyRotationToVector(shootDir, applyRotation);

            ShellParticleSystemHandler.Instance.SpawnShell(shellSpawnPosition, shellMoveDir);
        }

        private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e) {
            healthBar.SetSize(healthSystem.GetHealthNormalized());
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                SetWeapon(weaponPistol);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && canUseShotgun) {
                SetWeapon(weaponShotgun);
                //SetWeapon(weaponPunch);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) && canUseRifle) {
                SetWeapon(weaponRifle);
            }

            Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
            Vector3 aimDir = (mousePosition - GetPosition()).normalized;
            aimLightTransform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(aimDir));
        }

        public Vector3 GetPosition() {
            return transform.position;
        }

        public GameObject GetGameObject() {
            return gameObject;
        }

        public void Damage(Enemy attacker, float damageMultiplier) {
            Vector3 attackerPosition = GetPosition();
            if (attacker != null) {
                attackerPosition = attacker.GetPosition();
            }

            int damageAmount = Mathf.RoundToInt(8 * damageMultiplier * UnityEngine.Random.Range(.8f, 1.2f));

            bool isInvulnerable = Time.time < invulnerableTime;

            if (!isInvulnerable) {
                healthSystem.Damage(damageAmount);
                DamagePopup.Create(GetPosition(), damageAmount, true);
            }

            Sound_Manager.PlaySound(Sound_Manager.Sound.Player_Hit, GetPosition());

            Vector3 bloodDir = (GetPosition() - attackerPosition).normalized;
            BloodParticleSystemHandler.Instance.SpawnBlood(5, GetPosition(), bloodDir);
        }

        public void Knockback(Vector3 knockbackDir, float knockbackAmount) {
            //transform.position += knockbackDir * knockbackAmount;
            playerMain.PlayerRigidbody2D.MovePosition(transform.position + knockbackDir * knockbackAmount);
        }

        public void Dodged() {
            Sound_Manager.PlaySound(Sound_Manager.Sound.Dash, GetPosition());
            invulnerableTime = Time.time + .2f;
            OnDodged?.Invoke(this, EventArgs.Empty);
        }

        private void OnTriggerEnter2D(Collider2D collider) {
            if (collider.GetComponent<PickupHealth>() != null) {
                // Health item!
                materialTintColor.SetTintColor(Color.green);
                healthSystem.Heal(healthSystem.GetHealthMax());
                Destroy(collider.gameObject);
            }

            if (collider.GetComponent<PickupShotgun>() != null) {
                // Shotgun
                materialTintColor.SetTintColor(Color.blue);
                SetCanUseShotgun();
                Destroy(collider.gameObject);
                OnPickedUpWeapon?.Invoke(Weapon.WeaponType.Shotgun, EventArgs.Empty);
            }

            if (collider.GetComponent<PickupRifle>() != null) {
                // Shotgun
                materialTintColor.SetTintColor(Color.blue);
                SetCanUseRifle();
                Destroy(collider.gameObject);
                OnPickedUpWeapon?.Invoke(Weapon.WeaponType.Rifle, EventArgs.Empty);
            }

            if (collider.GetComponent<Star>() != null) {
                // Star!
                // Game Win!
                collider.gameObject.SetActive(false);
                playerMain.PlayerSwapAimNormal.PlayWinAnimation();
                playerMain.PlayerMovementHandler.Disable();
                //transform.Find("Body").GetComponent<MeshRenderer>().material = GameAssets.i.m_PlayerWinOutline;
                healthBar.Hide();
                CameraFollow.Instance.SetCameraFollowPosition(GetPosition());
                CameraFollow.Instance.SetCameraZoom(35f);
                CinematicBars.Show_Static(150f, .6f);

                transform.Find("AimLight").gameObject.SetActive(false);
            }
        }

    }
}