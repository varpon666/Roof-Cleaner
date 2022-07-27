using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CodeMonkey.CraftingSystem;

[CustomEditor(typeof(RecipeScriptableObject))]
public class RecipeScriptableObjectEditor : Editor {

    public override void OnInspectorGUI() {
        serializedObject.Update();

        RecipeScriptableObject recipeScriptableObject = (RecipeScriptableObject)target;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("OUTPUT", new GUIStyle { fontStyle = FontStyle.Bold });
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginVertical();

        Texture texture = null;
        if (recipeScriptableObject.output != null) {
            texture = recipeScriptableObject.output.itemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("output"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();



        EditorGUILayout.Space();



        GUILayout.Label("RECIPE", new GUIStyle { fontStyle = FontStyle.Bold });
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeScriptableObject.item_02 != null) {
            texture = recipeScriptableObject.item_02.itemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_02"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeScriptableObject.item_12 != null) {
            texture = recipeScriptableObject.item_12.itemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_12"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeScriptableObject.item_22 != null) {
            texture = recipeScriptableObject.item_22.itemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_22"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();



        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeScriptableObject.item_01 != null) {
            texture = recipeScriptableObject.item_01.itemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_01"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeScriptableObject.item_11 != null) {
            texture = recipeScriptableObject.item_11.itemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_11"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeScriptableObject.item_21 != null) {
            texture = recipeScriptableObject.item_21.itemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_21"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();




        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeScriptableObject.item_00 != null) {
            texture = recipeScriptableObject.item_00.itemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_00"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeScriptableObject.item_10 != null) {
            texture = recipeScriptableObject.item_10.itemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_10"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        texture = null;
        if (recipeScriptableObject.item_20 != null) {
            texture = recipeScriptableObject.item_20.itemSprite.texture;
        }
        GUILayout.Box(texture, GUILayout.Width(150), GUILayout.Height(150));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("item_20"), GUIContent.none, true, GUILayout.Width(150));
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();


        serializedObject.ApplyModifiedProperties();
    }


}