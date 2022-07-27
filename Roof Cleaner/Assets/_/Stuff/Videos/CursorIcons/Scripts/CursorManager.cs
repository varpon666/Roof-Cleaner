using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {

    public static CursorManager Instance { get; private set; }

    public event EventHandler<OnCursorChangedEventArgs> OnCursorChanged;
    public class OnCursorChangedEventArgs : EventArgs {
        public CursorType cursorType;
    }

    [SerializeField] private List<CursorAnimation> cursorAnimationList = null;

    private CursorAnimation cursorAnimation;

    private int currentFrame;
    private float frameTimer;
    private int frameCount;

    public enum CursorType {
        Arrow,
        Grab,
        Select,
        Attack,
        Move
    }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        SetActiveCursorType(CursorType.Arrow);
    }

    private void Update() {
        frameTimer -= Time.deltaTime;
        if (frameTimer <= 0f) {
            frameTimer += cursorAnimation.frameRate;
            currentFrame = (currentFrame + 1) % frameCount;
            Cursor.SetCursor(cursorAnimation.textureArray[currentFrame], cursorAnimation.offset, CursorMode.Auto);
        }
    }

    public void SetActiveCursorType(CursorType cursorType) {
        SetActiveCursorAnimation(GetCursorAnimation(cursorType));
        OnCursorChanged?.Invoke(this, new OnCursorChangedEventArgs { cursorType = cursorType });
    }

    private CursorAnimation GetCursorAnimation(CursorType cursorType) {
        foreach (CursorAnimation cursorAnimation in cursorAnimationList) {
            if (cursorAnimation.cursorType == cursorType) {
                return cursorAnimation;
            }
        }
        // Couldn't find this CursorType!
        return null;
    }

    private void SetActiveCursorAnimation(CursorAnimation cursorAnimation) {
        this.cursorAnimation = cursorAnimation;
        currentFrame = 0;
        frameTimer = 0f;
        frameCount = cursorAnimation.textureArray.Length;
    }


    [System.Serializable]
    public class CursorAnimation {

        public CursorType cursorType;
        public Texture2D[] textureArray;
        public float frameRate;
        public Vector2 offset;

    }

}
