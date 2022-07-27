using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskMeshUIDone : MaskableGraphic {

    [SerializeField] private Transform maskContainer = null;
    private List<MaskMeshLogicSingle> maskMeshLogicSingleList;

    protected override void Start() {
        base.Start();

        if (!Application.isPlaying) return; // Don't run in the Editor

        maskMeshLogicSingleList = new List<MaskMeshLogicSingle>();

        GetComponent<Mask>().enabled = true;

        if (maskContainer != null) {
            AddMaskMeshContainer(maskContainer);
        }
    }

    protected override void OnPopulateMesh(VertexHelper vertexHelper) {
        vertexHelper.Clear();

        if (!Application.isPlaying) return; // Don't run in the Editor

        if (maskMeshLogicSingleList != null) {
            foreach (MaskMeshLogicSingle maskMeshLogicSingle in maskMeshLogicSingleList) {
                maskMeshLogicSingle.PopulateMesh(vertexHelper);
            }
        }
    }

    private void Update() {
        if (!Application.isPlaying) return; // Don't run in the Editor

        if (maskMeshLogicSingleList != null) {
            foreach (MaskMeshLogicSingle maskMeshLogicSingle in maskMeshLogicSingleList) {
                maskMeshLogicSingle.Update();
            }
        }

        SetVerticesDirty();
    }


    public void AddMaskMeshContainer(Transform maskContainer) {
        Action<Transform> handleTransform = (Transform child) => {
            MaskMeshUIDoneSingle maskMeshUISingle = child.GetComponent<MaskMeshUIDoneSingle>();
            if (maskMeshUISingle != null) {
                maskMeshLogicSingleList.Add(new MaskMeshLogicSingle {
                    rectTransform = child.GetComponent<RectTransform>(),
                    startTimer = maskMeshUISingle.startTime,
                    timeMax = maskMeshUISingle.time
                });
            }
            child.gameObject.SetActive(false);
        };

        handleTransform(maskContainer);
        foreach (Transform child in maskContainer) {
            handleTransform(child);
        }
    }

    public float GetTotalTime(Transform maskContainer) {
        float totalTime = 0f;

        Action<Transform> handleTransform = (Transform child) => {
            MaskMeshUIDoneSingle maskMeshUISingle = child.GetComponent<MaskMeshUIDoneSingle>();
            if (maskMeshUISingle != null) {
                float testTotalTime = maskMeshUISingle.startTime + maskMeshUISingle.time;
                if (testTotalTime > totalTime) {
                    totalTime = testTotalTime;
                }
            }
        };

        handleTransform(maskContainer);
        foreach (Transform child in maskContainer) {
            handleTransform(child);
        }

        return totalTime;
    }


    // Contains the running logic for a single Mask Mesh
    public class MaskMeshLogicSingle {

        public RectTransform rectTransform;
        public float startTimer;
        public float time;
        public float timeMax;

        public void Update() {
            startTimer -= Time.deltaTime;
            if (startTimer <= 0f) {
                time += Time.deltaTime;
                if (time > timeMax) {
                    time = timeMax;
                }
            }
        }

        public void PopulateMesh(VertexHelper toFill) {
            Vector3 vec_00 = rectTransform.anchoredPosition;
            Vector3 vec_01 = rectTransform.anchoredPosition + new Vector2(0, rectTransform.sizeDelta.y);
            Vector3 vec_11 = rectTransform.anchoredPosition + new Vector2(Mathf.Lerp(0f, rectTransform.sizeDelta.x, time / timeMax), rectTransform.sizeDelta.y);
            Vector3 vec_10 = rectTransform.anchoredPosition + new Vector2(Mathf.Lerp(0f, rectTransform.sizeDelta.x, time / timeMax), 0);

            toFill.AddUIVertexQuad(new UIVertex[] {
                new UIVertex { position = vec_00, color = Color.green },
                new UIVertex { position = vec_01, color = Color.green },
                new UIVertex { position = vec_11, color = Color.green },
                new UIVertex { position = vec_10, color = Color.green },
            });
        }
    }

}
