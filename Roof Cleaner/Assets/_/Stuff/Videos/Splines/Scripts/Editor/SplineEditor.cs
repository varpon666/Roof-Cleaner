using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Spline))]
public class SplineEditor : Editor {

    public override void OnInspectorGUI() {
        serializedObject.Update();

        Spline spline = (Spline)target;

        if (GUILayout.Button("Add Anchor")) {
            Undo.RecordObject(spline, "Add Anchor");
            spline.AddAnchor();
            spline.SetDirty();
            serializedObject.Update();
        }

        if (GUILayout.Button("Remove Last Anchor")) {
            Undo.RecordObject(spline, "Remove Last Anchor");
            spline.RemoveLastAnchor();
            spline.SetDirty();
            serializedObject.Update();
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("dots"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("normal"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("closedLoop"));

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Set All Z = 0")) {
            Undo.RecordObject(spline, "Set All Z = 0");
            spline.SetAllZZero();
            spline.SetDirty();
            serializedObject.Update();
        }
        if (GUILayout.Button("Set All Y = 0")) {
            Undo.RecordObject(spline, "Set All Y = 0");
            spline.SetAllYZero();
            spline.SetDirty();
            serializedObject.Update();
        }
    }

    public void OnSceneGUI() {
        Spline spline = (Spline)target;

        Vector3 transformPosition = spline.transform.position;

        List<Spline.Anchor> anchorList = spline.GetAnchorList();
        if (anchorList != null) {
            foreach (Spline.Anchor anchor in spline.GetAnchorList()) {
                Handles.color = Color.white;
                Handles.DrawWireCube(transformPosition + anchor.position, Vector3.one * .5f);

                EditorGUI.BeginChangeCheck();
                Vector3 newPosition = Handles.PositionHandle(transformPosition + anchor.position, Quaternion.identity);
                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(spline, "Change Anchor Position");
                    anchor.position = newPosition - transformPosition;
                    spline.SetDirty();
                    serializedObject.Update();
                }

                Handles.color = Color.green;
                Handles.SphereHandleCap(0, transformPosition + anchor.handleAPosition, Quaternion.identity, .5f, EventType.Repaint);

                EditorGUI.BeginChangeCheck();
                newPosition = Handles.PositionHandle(transformPosition + anchor.handleAPosition, Quaternion.identity);
                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(spline, "Change Anchor Handle A Position");
                    anchor.handleAPosition = newPosition - transformPosition;
                    spline.SetDirty();
                    serializedObject.Update();
                }

                Handles.color = Color.blue;
                Handles.SphereHandleCap(0, transformPosition + anchor.handleBPosition, Quaternion.identity, .5f, EventType.Repaint);

                EditorGUI.BeginChangeCheck();
                newPosition = Handles.PositionHandle(transformPosition + anchor.handleBPosition, Quaternion.identity);
                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(spline, "Change Anchor Handle B Position");
                    anchor.handleBPosition = newPosition - transformPosition;
                    spline.SetDirty();
                    serializedObject.Update();
                }

                Handles.color = Color.white;
                Handles.DrawLine(transformPosition + anchor.position, transformPosition + anchor.handleAPosition);
                Handles.DrawLine(transformPosition + anchor.position, transformPosition + anchor.handleBPosition);
            }

            // Draw Bezier
            for (int i = 0; i < spline.GetAnchorList().Count - 1; i++) {
                Spline.Anchor anchor = spline.GetAnchorList()[i];
                Spline.Anchor nextAnchor = spline.GetAnchorList()[i + 1];
                Handles.DrawBezier(transformPosition + anchor.position, transformPosition + nextAnchor.position, transformPosition + anchor.handleBPosition, transformPosition + nextAnchor.handleAPosition, Color.grey, null, 3f);
            }

            if (spline.GetClosedLoop()) {
                // Spline is Closed Loop
                Spline.Anchor anchor = spline.GetAnchorList()[spline.GetAnchorList().Count - 1];
                Spline.Anchor nextAnchor = spline.GetAnchorList()[0];
                Handles.DrawBezier(transformPosition + anchor.position, transformPosition + nextAnchor.position, transformPosition + anchor.handleBPosition, transformPosition + nextAnchor.handleAPosition, Color.grey, null, 3f);
            }
        }
    }

}
