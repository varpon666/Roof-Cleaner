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
using TMPro;
using CodeMonkey.Utils;

public class HitCounter : MonoBehaviour {

    //[SerializeField] private Player player;
    private MeshRenderer meshRenderer;
    private TextMeshPro textMeshPro;
    private int hitCount;
    private Vector3 baseLocalPosition;
    private float shakeIntensity;

    private void Awake() {
        textMeshPro = GetComponent<TextMeshPro>();
        meshRenderer = GetComponent<MeshRenderer>();
        baseLocalPosition = transform.localPosition;

        HideHitCounter();
    }

    private void Start() {
        /*
        player.OnAttacked += Player_OnAttacked;
        player.OnDamaged += Player_OnDamaged;
        player.OnEnemyHit += Player_OnEnemyHit;
        player.OnEnemyKilled += Player_OnEnemyKilled;
        */
    }

    private void Update() {
        if (shakeIntensity > 0) {
            float shakeIntensityDropAmount = .5f;
            shakeIntensity -= shakeIntensityDropAmount * Time.deltaTime;
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            transform.localPosition = baseLocalPosition + randomDirection * shakeIntensity;
        }
    }

    private void Player_OnEnemyKilled(object sender, System.EventArgs e) {
        // Player killed an Enemy
        IncreaseHitCount();
        
        UtilsClass.ShakeCamera(3f, .1f);
    }

    private void Player_OnEnemyHit(object sender, System.EventArgs e) {
        // Player hit an Enemy
        IncreaseHitCount();

        float baseIntensity = .2f;
        float perHitIntensity = .02f;
        shakeIntensity = Mathf.Clamp(baseIntensity + perHitIntensity * hitCount, baseIntensity, 1.2f);

        UtilsClass.ShakeCamera(.3f, .1f);
    }

    private void Player_OnDamaged(object sender, System.EventArgs e) {
        // Player took Damage
        hitCount = 0;
        HideHitCounter();
    }

    private void Player_OnAttacked(object sender, System.EventArgs e) {
        // Player did an Attack
        IncreaseHitCount();
    }

    private void IncreaseHitCount() {
        hitCount++;
        SetHitCounter(hitCount);
    }

    private void SetHitCounter(int hitCount) {
        textMeshPro.SetText(hitCount.ToString());
        meshRenderer.enabled = true;

        Color textColor = Color.white;

        if (hitCount >= 10) textColor = UtilsClass.GetColorFromString("00A0FF");
        if (hitCount >= 20) textColor = UtilsClass.GetColorFromString("24E100");
        if (hitCount >= 30) textColor = UtilsClass.GetColorFromString("FFE300");
        if (hitCount >= 40) textColor = UtilsClass.GetColorFromString("FF7F1C");
        if (hitCount >= 50) textColor = UtilsClass.GetColorFromString("FF3AF2");

        textMeshPro.color = textColor;

        float startingFontSize = 6f;
        float perHitFontSize = .06f;
        textMeshPro.fontSize = Mathf.Clamp(startingFontSize + perHitFontSize * hitCount, startingFontSize, startingFontSize * 2f);
    }

    private void HideHitCounter() {
        meshRenderer.enabled = false;
    }

}
