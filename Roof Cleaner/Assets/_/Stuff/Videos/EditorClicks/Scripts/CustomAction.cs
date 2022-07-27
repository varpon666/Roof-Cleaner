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
#if UNITY_EDITOR
using UnityEditor;
#endif
using CodeMonkey.Utils;

public class CustomAction : MonoBehaviour {

    public enum ActionType {
        ChatBubble,
        Move,
        Door,
    }

    public ActionType actionType;

    // Type = ChatBubble
    public ChatBubbleParams chatBubbleParams;
    [System.Serializable]
    public class ChatBubbleParams {

        public GameObject gameObject;
        public string text;
        public CustomAction onChatBubbleComplete;

        public void ExecuteAction() {
            ChatBubble.Create(gameObject.transform, new Vector3(1f, 1.7f), ChatBubble.IconType.Happy, text);
            FunctionTimer.Create(() => {
                onChatBubbleComplete.ExecuteAction();
            }, 2f);
        }

    }

    // Type = Move
    public MoveParams moveParams;
    [System.Serializable]
    public class MoveParams {

        public GameObject unitGameObject;
        public Vector3 movePosition;
        public CustomAction onReachedPosition;

        public void ExecuteAction() {
            unitGameObject.GetComponent<IMovePosition>().SetMovePosition(movePosition, 
                () => { onReachedPosition.ExecuteAction(); }
            );
        }

    }

    // Type = Door
    public DoorParams doorParams;
    [System.Serializable]
    public class DoorParams {

        public DoorAnims3D door;
        public bool open;
        public CustomAction onDoorAnimComplete;

        public void ExecuteAction() {
            if (open) {
                door.OpenDoor();
            } else {
                door.CloseDoor();
            }
            if (onDoorAnimComplete != null) {
                FunctionTimer.Create(onDoorAnimComplete.ExecuteAction, 1.2f);
            }
        }

    }

    public void ExecuteAction() {
        Debug.Log("ExecuteAction: " + name);

        switch (actionType) {
            case ActionType.ChatBubble:
                chatBubbleParams.ExecuteAction();
                break;
            case ActionType.Move:
                moveParams.ExecuteAction();
                break;
            case ActionType.Door:
                doorParams.ExecuteAction();
                break;
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos() {
        CustomAction customAction = this;

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;
        style.fontStyle = FontStyle.Bold;
        Handles.Label(customAction.transform.position + new Vector3(-.2f, -.2f), "" + customAction.actionType, style);

        switch (actionType) {
            case ActionType.ChatBubble:
                if (chatBubbleParams.gameObject != null) {
                    GizmoDrawLine(
                        customAction.transform.position,
                        chatBubbleParams.gameObject.transform.position,
                        Color.blue, chatBubbleParams.text, Color.white
                    );

                    if (chatBubbleParams.onChatBubbleComplete != null) {
                        GizmoDrawLine(
                            customAction.transform.position,
                            chatBubbleParams.onChatBubbleComplete.transform.position,
                            Color.black, "Next", Color.black
                        );
                    }
                }
                break;
            case ActionType.Move:
                if (moveParams.unitGameObject != null) {
                    GizmoDrawLine(
                        customAction.transform.position,
                        moveParams.unitGameObject.transform.position,
                        Color.white, "", Color.white
                    );

                    GizmoDrawLine(
                        moveParams.unitGameObject.transform.position,
                        moveParams.movePosition,
                        Color.green, "MovePosition", Color.white
                    );

                    if (moveParams.onReachedPosition != null) {
                        GizmoDrawLine(
                            customAction.transform.position,
                            moveParams.onReachedPosition.transform.position,
                            Color.black, "Next", Color.black
                        );
                    }
                }
                break;
            case ActionType.Door:
                if (doorParams.door != null) {
                    GizmoDrawLine(
                        customAction.transform.position,
                        doorParams.door.transform.position,
                        Color.yellow, doorParams.open ? "Open" : "Close", Color.green
                    );

                    if (doorParams.onDoorAnimComplete != null) {
                        GizmoDrawLine(
                            customAction.transform.position,
                            doorParams.onDoorAnimComplete.transform.position,
                            Color.black, "Next", Color.black
                        );
                    }
                }
                break;
        }
    }


    private void GizmoDrawLine(Vector3 from, Vector3 to, Color lineColor, string text, Color textColor) {
        Handles.color = lineColor;
        Handles.DrawAAPolyLine(5f, from, to);

        Vector3 dir = (to - from).normalized;
        float distance = Vector3.Distance(from, to);

        for (float i = 0; i < distance; i += 1f) {
            Handles.DrawAAPolyLine(
                5f, 
                from + dir * i, 
                from + (dir * (i - .15f)) + Quaternion.AngleAxis(Time.realtimeSinceStartup * 360f, dir.normalized * 300f) * Vector3.up * .05f
            );
            Handles.DrawAAPolyLine(
                5f, 
                from + dir * i, 
                from + (dir * (i - .15f)) + Quaternion.AngleAxis(Time.realtimeSinceStartup * 360f + 180, dir.normalized * 300f) * Vector3.up * .05f
            );
        }

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = textColor;
        Handles.Label(from + (dir * distance * .5f), text, style);
    }
#endif

}
