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
using UnityEditor;

[CustomEditor(typeof(CustomAction))]
public class CustomActionEditor : Editor {

    public enum ClickToSelectType {
        UnitMovePosition,
        CustomAction,
        Door,
        ChatBubbleGameObject
    }

    private bool clickToSelect;
    private ClickToSelectType clickToSelectType;
    private System.Action<UnityEngine.Object> clickToSelectAction;

    public override void OnInspectorGUI() {
        serializedObject.Update();

        CustomAction customAction = (CustomAction)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("actionType"));

        EditorGUILayout.Space();

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.blue;
        style.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("" + customAction.actionType, style);

        if (clickToSelect) {
            style.normal.textColor = Color.red;
            switch (clickToSelectType) {
                case ClickToSelectType.UnitMovePosition:
                    EditorGUILayout.LabelField("Click to Select Unit Move Position", style);
                    break;
                case ClickToSelectType.CustomAction:
                    EditorGUILayout.LabelField("Click to Select CustomAction", style);
                    break;
                case ClickToSelectType.Door:
                    EditorGUILayout.LabelField("Click to Select Door", style);
                    break;
                case ClickToSelectType.ChatBubbleGameObject:
                    EditorGUILayout.LabelField("Click to Select ChatBubbleGameObject", style);
                    break;
            }
        }

        switch (customAction.actionType) {
            case CustomAction.ActionType.ChatBubble:
                //EditorGUILayout.PropertyField(serializedObject.FindProperty("chatBubbleParams"));

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("chatBubbleParams").FindPropertyRelative("gameObject"));
                if (GUILayout.Button("+", GUILayout.Width(20f))) {
                    clickToSelect = true;
                    clickToSelectType = ClickToSelectType.ChatBubbleGameObject;
                    clickToSelectAction = (UnityEngine.Object obj) => {
                        customAction.chatBubbleParams.gameObject = obj as GameObject;
                    };
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("chatBubbleParams").FindPropertyRelative("text"));

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("chatBubbleParams").FindPropertyRelative("onChatBubbleComplete"));
                if (GUILayout.Button("+", GUILayout.Width(20f))) {
                    clickToSelect = true;
                    clickToSelectType = ClickToSelectType.CustomAction;
                    clickToSelectAction = (UnityEngine.Object obj) => {
                        customAction.chatBubbleParams.onChatBubbleComplete = obj as CustomAction;
                    };
                }
                EditorGUILayout.EndHorizontal();
                break;
            case CustomAction.ActionType.Move:
                //EditorGUILayout.PropertyField(serializedObject.FindProperty("moveParams"));
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moveParams").FindPropertyRelative("unitGameObject"));
                if (GUILayout.Button("+", GUILayout.Width(20f))) {
                    clickToSelect = true;
                    clickToSelectType = ClickToSelectType.UnitMovePosition;
                    clickToSelectAction = (UnityEngine.Object obj) => {
                        customAction.moveParams.unitGameObject = obj as GameObject;
                        customAction.moveParams.movePosition = customAction.moveParams.unitGameObject.transform.position + new Vector3(0, 0, -2f);
                    };
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("moveParams").FindPropertyRelative("movePosition"));

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("moveParams").FindPropertyRelative("onReachedPosition"));
                if (GUILayout.Button("+", GUILayout.Width(20f))) {
                    clickToSelect = true;
                    clickToSelectType = ClickToSelectType.CustomAction;
                    clickToSelectAction = (UnityEngine.Object obj) => {
                        customAction.moveParams.onReachedPosition = obj as CustomAction;
                    };
                }
                EditorGUILayout.EndHorizontal();
                break;
            case CustomAction.ActionType.Door:
                //EditorGUILayout.PropertyField(serializedObject.FindProperty("doorParams"));

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("doorParams").FindPropertyRelative("door"));
                if (GUILayout.Button("+", GUILayout.Width(20f))) {
                    clickToSelect = true;
                    clickToSelectType = ClickToSelectType.Door;
                    clickToSelectAction = (UnityEngine.Object obj) => {
                        customAction.doorParams.door = obj as DoorAnims3D;
                    };
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("doorParams").FindPropertyRelative("open"));

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("doorParams").FindPropertyRelative("onDoorAnimComplete"));
                if (GUILayout.Button("+", GUILayout.Width(20f))) {
                    clickToSelect = true;
                    clickToSelectType = ClickToSelectType.CustomAction;
                    clickToSelectAction = (UnityEngine.Object obj) => {
                        customAction.doorParams.onDoorAnimComplete = obj as CustomAction;
                    };
                }
                EditorGUILayout.EndHorizontal();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private bool IsMouseOverObjectRaycast<T>(Vector2 mousePosition, out T tOut, out Collider collider) {
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

        RaycastHit[] raycastHitArray = Physics.RaycastAll(ray);

        foreach (RaycastHit raycastHit in raycastHitArray) {
            if (raycastHit.collider.gameObject.TryGetComponent(out T t)) {
                collider = raycastHit.collider;
                tOut = t;
                return true;
            }
        }

        collider = null;
        tOut = default(T);
        return false;

    }

    private void OnSceneGUI() {
        CustomAction customAction = (CustomAction)target;

        Event e = Event.current;

        if (clickToSelect) {
            // Highlight selectable objects
            switch (clickToSelectType) {
                case ClickToSelectType.UnitMovePosition: {
                    if (IsMouseOverObjectRaycast(e.mousePosition, out IMovePosition movePosition, out Collider collider)) {
                        Handles.DrawWireCube(collider.bounds.center, collider.bounds.size);
                    }
                    break;
                }
                case ClickToSelectType.CustomAction: {
                    if (IsMouseOverObjectRaycast(e.mousePosition, out CustomAction customActionMouse, out Collider collider)) {
                        Handles.DrawWireCube(collider.bounds.center, collider.bounds.size);
                    }
                    break;
                }
                case ClickToSelectType.ChatBubbleGameObject: {
                    if (IsMouseOverObjectRaycast(e.mousePosition, out Character_Base characterBase, out Collider collider)) {
                        Handles.DrawWireCube(collider.bounds.center, collider.bounds.size);
                    }
                    break;
                }
                case ClickToSelectType.Door: {
                    if (IsMouseOverObjectRaycast(e.mousePosition, out DoorAnims3D doorAnims3D, out Collider collider)) {
                        Handles.DrawWireCube(collider.bounds.center, collider.bounds.size);
                    }
                    break;
                }
            }
        }

        if (e.type == EventType.MouseUp) {
            switch (clickToSelectType) {
                case ClickToSelectType.UnitMovePosition: {
                    if (IsMouseOverObjectRaycast(e.mousePosition, out IMovePosition movePosition, out Collider collider)) {
                        clickToSelectAction(collider.gameObject);
                        clickToSelect = false;
                    }
                    break;
                }
                case ClickToSelectType.CustomAction: {
                    if (IsMouseOverObjectRaycast(e.mousePosition, out CustomAction customActionMouse, out Collider collider)) {
                        clickToSelectAction(customActionMouse);
                        clickToSelect = false;
                    }
                    break;
                }
                case ClickToSelectType.ChatBubbleGameObject: {
                    if (IsMouseOverObjectRaycast(e.mousePosition, out Character_Base characterBase, out Collider collider)) {
                        clickToSelectAction(characterBase.gameObject);
                        clickToSelect = false;
                    }
                    break;
                }
                case ClickToSelectType.Door: {
                    if (IsMouseOverObjectRaycast(e.mousePosition, out DoorAnims3D doorAnims3D, out Collider collider)) {
                        clickToSelectAction(doorAnims3D);
                        clickToSelect = false;
                    }
                    break;
                }
            }
            clickToSelect = false;
            Repaint();
        }

        if (clickToSelect) {
            // Block Mouse Deselect
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
        }


        switch (customAction.actionType) {
            case CustomAction.ActionType.Move:
                EditorGUI.BeginChangeCheck();
                Vector3 newMovePosition = Handles.PositionHandle(customAction.moveParams.movePosition, Quaternion.identity);
                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(customAction, "Change Move Position");
                    customAction.moveParams.movePosition = newMovePosition;
                    serializedObject.Update();
                }
                break;
        }
    }

}
