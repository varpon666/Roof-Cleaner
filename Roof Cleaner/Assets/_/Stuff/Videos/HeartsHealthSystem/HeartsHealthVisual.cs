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
using UnityEngine.UI;
using CodeMonkey;
using CodeMonkey.Utils;

public class HeartsHealthVisual : MonoBehaviour {

    public static HeartsHealthSystem heartsHealthSystemStatic;

    [SerializeField] private Sprite heart0Sprite = null;
    [SerializeField] private Sprite heart1Sprite = null;
    [SerializeField] private Sprite heart2Sprite = null;
    [SerializeField] private Sprite heart3Sprite = null;
    [SerializeField] private Sprite heart4Sprite = null;
    [SerializeField] private AnimationClip heartFullAnimationClip = null;

    private List<HeartImage> heartImageList;
    private HeartsHealthSystem heartsHealthSystem;
    private bool isHealing;

    private void Awake() {
        heartImageList = new List<HeartImage>();
    }

    private void Start() {
        FunctionPeriodic.Create(HealingAnimatedPeriodic, .05f);
        HeartsHealthSystem heartsHealthSystem = new HeartsHealthSystem(20);
        SetHeartsHealthSystem(heartsHealthSystem);
        
        /*
        //Testing buttons
        CMDebug.ButtonUI(new Vector2(-50, -100), "Damage 1", () => heartsHealthSystem.Damage(1));
        CMDebug.ButtonUI(new Vector2(50, -100), "Damage 4", () => heartsHealthSystem.Damage(4));
        
        CMDebug.ButtonUI(new Vector2(-50, -200), "Heal 1", () => heartsHealthSystem.Heal(1));
        CMDebug.ButtonUI(new Vector2(50, -200), "Heal 4", () => heartsHealthSystem.Heal(4));
        CMDebug.ButtonUI(new Vector2(150, -200), "Heal 50", () => heartsHealthSystem.Heal(50));
        */
    }

    public void SetHeartsHealthSystem(HeartsHealthSystem heartsHealthSystem) {
        this.heartsHealthSystem = heartsHealthSystem;
        heartsHealthSystemStatic = heartsHealthSystem;

        List<HeartsHealthSystem.Heart> heartList = heartsHealthSystem.GetHeartList();
        int row = 0;
        int col = 0;
        int colMax = 10;
        float rowColSize = 30f;

        for (int i = 0; i < heartList.Count; i++) {
            HeartsHealthSystem.Heart heart = heartList[i];
            Vector2 heartAnchoredPosition = new Vector2(col * rowColSize, -row * rowColSize);
            CreateHeartImage(heartAnchoredPosition).SetHeartFraments(heart.GetFragmentAmount());

            col++;
            if (col >= colMax) {
                row++;
                col = 0;
            }
        }

        heartsHealthSystem.OnDamaged += HeartsHealthSystem_OnDamaged;
        heartsHealthSystem.OnHealed += HeartsHealthSystem_OnHealed;
        heartsHealthSystem.OnDead += HeartsHealthSystem_OnDead;
    }

    private void HeartsHealthSystem_OnDead(object sender, System.EventArgs e) {
        CMDebug.TextPopupMouse("Dead!");
    }

    private void HeartsHealthSystem_OnHealed(object sender, System.EventArgs e) {
        // Hearts health system was healed
        //RefreshAllHearts();
        isHealing = true;
    }

    private void HeartsHealthSystem_OnDamaged(object sender, System.EventArgs e) {
        // Hearts health system was damaged
        RefreshAllHearts();
    }

    private void RefreshAllHearts() {
        List<HeartsHealthSystem.Heart> heartList = heartsHealthSystem.GetHeartList();
        for (int i = 0; i < heartImageList.Count; i++) {
            HeartImage heartImage = heartImageList[i];
            HeartsHealthSystem.Heart heart = heartList[i];
            heartImage.SetHeartFraments(heart.GetFragmentAmount());
        }
    }

    private void HealingAnimatedPeriodic() {
        if (isHealing) {
            bool fullyHealed = true;
            List<HeartsHealthSystem.Heart> heartList = heartsHealthSystem.GetHeartList();
            for (int i = 0; i < heartList.Count; i++) {
                HeartImage heartImage = heartImageList[i];
                HeartsHealthSystem.Heart heart = heartList[i];
                if (heartImage.GetFragmentAmount() != heart.GetFragmentAmount()) {
                    // Visual is different from logic
                    heartImage.AddHeartVisualFragment();
                    if (heartImage.GetFragmentAmount() == HeartsHealthSystem.MAX_FRAGMENT_AMOUNT) {
                        // This heart was fully healed
                        heartImage.PlayHeartFullAnimation();
                    }
                    fullyHealed = false;
                    break;
                }
            }
            if (fullyHealed) {
                isHealing = false;
            }
        }
    }

    private HeartImage CreateHeartImage(Vector2 anchoredPosition) {
        // Create Game Object
        GameObject heartGameObject = new GameObject("Heart", typeof(Image), typeof(Animation));

        // Set as child of this transform
        heartGameObject.transform.parent = transform;
        heartGameObject.transform.localPosition = Vector3.zero;
        heartGameObject.transform.localScale = Vector3.one;

        // Locate and Size heart
        heartGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        heartGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(35, 35);

        heartGameObject.GetComponent<Animation>().AddClip(heartFullAnimationClip, "HeartFull");

        // Set heart sprite
        Image heartImageUI = heartGameObject.GetComponent<Image>();
        heartImageUI.sprite = heart4Sprite;

        HeartImage heartImage = new HeartImage(this, heartImageUI, heartGameObject.GetComponent<Animation>());
        heartImageList.Add(heartImage);

        return heartImage;
    }


    // Represents a single Heart
    public class HeartImage {

        private int fragments;
        private Image heartImage;
        private HeartsHealthVisual heartsHealthVisual;
        private Animation animation;

        public HeartImage(HeartsHealthVisual heartsHealthVisual, Image heartImage, Animation animation) {
            this.heartsHealthVisual = heartsHealthVisual;
            this.heartImage = heartImage;
            this.animation = animation;
        }

        public void SetHeartFraments(int fragments) {
            this.fragments = fragments;
            switch (fragments) {
            case 0: heartImage.sprite = heartsHealthVisual.heart0Sprite; break;
            case 1: heartImage.sprite = heartsHealthVisual.heart1Sprite; break;
            case 2: heartImage.sprite = heartsHealthVisual.heart2Sprite; break;
            case 3: heartImage.sprite = heartsHealthVisual.heart3Sprite; break;
            case 4: heartImage.sprite = heartsHealthVisual.heart4Sprite; break;
            }
        }

        public int GetFragmentAmount() {
            return fragments;
        }

        public void AddHeartVisualFragment() {
            SetHeartFraments(fragments + 1);
        }

        public void PlayHeartFullAnimation() {
            animation.Play("HeartFull", PlayMode.StopAll);
        }
    }


}
