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
using CodeMonkey.Utils;

public class Window_QuestPointer : MonoBehaviour {

    [SerializeField] private Camera uiCamera = null;
    [SerializeField] private Sprite arrowSprite = null;
    [SerializeField] private Sprite crossSprite = null;

    private List<QuestPointer> questPointerList;

    private void Awake() {
        questPointerList = new List<QuestPointer>();
    }

    private void Update() {
        foreach (QuestPointer questPointer in questPointerList) {
            questPointer.Update();
        }
    }

    public QuestPointer CreatePointer(Vector3 targetPosition, Color arrowColor, Color crossColor, Sprite arrowSprite = null, Sprite crossSprite = null) {
        if (arrowSprite == null) {
            arrowSprite = this.arrowSprite;
        }
        if (crossSprite == null) {
            crossSprite = this.crossSprite;
        }
        GameObject pointerGameObject = Instantiate(transform.Find("pointerTemplate").gameObject);
        pointerGameObject.SetActive(true);
        pointerGameObject.transform.SetParent(transform, false);
        QuestPointer questPointer = new QuestPointer(targetPosition, pointerGameObject, uiCamera, arrowSprite, crossSprite, arrowColor, crossColor);
        questPointerList.Add(questPointer);
        return questPointer;
    }

    public void DestroyPointer(QuestPointer questPointer) {
        questPointerList.Remove(questPointer);
        questPointer.DestroySelf();
    }

    public class QuestPointer {

        private Vector3 targetPosition;
        private GameObject pointerGameObject;
        private Sprite arrowSprite;
        private Sprite crossSprite;
        private Camera uiCamera;
        private RectTransform pointerRectTransform;
        private Image pointerImage;
        private Color arrowColor;
        private Color crossColor;

        public QuestPointer(Vector3 targetPosition, GameObject pointerGameObject, Camera uiCamera, Sprite arrowSprite, Sprite crossSprite, Color arrowColor, Color crossColor) {
            this.targetPosition = targetPosition;
            this.pointerGameObject = pointerGameObject;
            this.uiCamera = uiCamera;
            this.arrowSprite = arrowSprite;
            this.crossSprite = crossSprite;
            this.arrowColor = arrowColor;
            this.crossColor = crossColor;
            
            pointerRectTransform = pointerGameObject.GetComponent<RectTransform>();
            pointerImage = pointerGameObject.GetComponent<Image>();
        }

        public void Update() {
            float borderSize = 100f;
            Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
            bool isOffScreen = targetPositionScreenPoint.x <= borderSize || targetPositionScreenPoint.x >= Screen.width - borderSize || targetPositionScreenPoint.y <= borderSize || targetPositionScreenPoint.y >= Screen.height - borderSize;

            if (isOffScreen) {
                RotatePointerTowardsTargetPosition();

                pointerImage.sprite = arrowSprite;
                pointerImage.color = arrowColor;
                Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
                cappedTargetScreenPosition.x = Mathf.Clamp(cappedTargetScreenPosition.x, borderSize, Screen.width - borderSize);
                cappedTargetScreenPosition.y = Mathf.Clamp(cappedTargetScreenPosition.y, borderSize, Screen.height - borderSize);

                Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
                pointerRectTransform.position = pointerWorldPosition;
                pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
            } else {
                pointerImage.sprite = crossSprite;
                pointerImage.color = crossColor;
                Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(targetPositionScreenPoint);
                pointerRectTransform.position = pointerWorldPosition;
                pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);

                pointerRectTransform.localEulerAngles = Vector3.zero;
            }
        }

        private void RotatePointerTowardsTargetPosition() {
            Vector3 toPosition = targetPosition;
            Vector3 fromPosition = Camera.main.transform.position;
            fromPosition.z = 0f;
            Vector3 dir = (toPosition - fromPosition).normalized;
            float angle = UtilsClass.GetAngleFromVectorFloat(dir);
            pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
        }

        public void DestroySelf() {
            Destroy(pointerGameObject);
        }

    }
}
