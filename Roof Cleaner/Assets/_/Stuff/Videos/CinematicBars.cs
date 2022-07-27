/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this Code Monkey project
    I hope you find it useful in your own projects
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

public class CinematicBars : MonoBehaviour {

    private static CinematicBars instance;

    private RectTransform topBar, bottomBar;
    private float changeSizeAmount;
    private float targetSize;
    private bool isActive;

    private void Awake() {
        instance = this;
        GameObject gameObject = new GameObject("topBar", typeof(Image));
        gameObject.transform.SetParent(transform, false);
        gameObject.GetComponent<Image>().color = Color.black;
        topBar = gameObject.GetComponent<RectTransform>();
        topBar.anchorMin = new Vector2(0, 1);
        topBar.anchorMax = new Vector2(1, 1);
        topBar.sizeDelta = new Vector2(0, 0);
        
        gameObject = new GameObject("bottomBar", typeof(Image));
        gameObject.transform.SetParent(transform, false);
        gameObject.GetComponent<Image>().color = Color.black;
        bottomBar = gameObject.GetComponent<RectTransform>();
        bottomBar.anchorMin = new Vector2(0, 0);
        bottomBar.anchorMax = new Vector2(1, 0);
        bottomBar.sizeDelta = new Vector2(0, 0);

        CMDebug.ButtonUI(new Vector2(-200, 0), "Show", () => {
            Show(300, .3f);
        });
        CMDebug.ButtonUI(new Vector2(200, 0), "Hide", () => {
            Hide(.3f);
        });
    }

    private void Update() {
        if (isActive) {
            Vector2 sizeDelta = topBar.sizeDelta;
            sizeDelta.y += changeSizeAmount * Time.deltaTime;
            if (changeSizeAmount > 0) {
                if (sizeDelta.y >= targetSize) {
                    sizeDelta.y = targetSize;
                    isActive = false;
                }
            } else {
                if (sizeDelta.y <= targetSize) {
                    sizeDelta.y = targetSize;
                    isActive = false;
                }
            }
            topBar.sizeDelta = sizeDelta;
            bottomBar.sizeDelta = sizeDelta;
        }
    }

    public void Show(float targetSize, float time) {
        this.targetSize = targetSize;
        changeSizeAmount = (targetSize - topBar.sizeDelta.y) / time;
        isActive = true;
    }

    public void Hide(float time) {
        targetSize = 0f;
        changeSizeAmount = (targetSize - topBar.sizeDelta.y) / time;
        isActive = true;
    }


    public static void Show_Static(float targetSize, float time) {
        if (instance != null) {
            instance.Show(targetSize, time);
        }
    }

    public static void Hide_Static(float time) {
        if (instance != null) {
            instance.Hide(time);
        }
    }

}
