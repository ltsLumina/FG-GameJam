﻿using DistantLands.Lumen;
using DistantLands.Lumen.Data;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LumenDynamicLightRay))]
[CanEditMultipleObjects]
public class E_LumenDynamicRay : Editor
{
    SerializedProperty flareData;
    SerializedProperty localColor;
    LumenDynamicLightRay lumenRay;
    bool settings;
    SerializedProperty updateFrequency;

    void OnEnable()
    {

        lumenRay = (LumenDynamicLightRay)target;
        flareData = serializedObject.FindProperty("rayData");
        updateFrequency = serializedObject.FindProperty("updateFrequency");
        localColor = serializedObject.FindProperty("localColor");

    }

    public override void OnInspectorGUI()
    {


        serializedObject.Update();
        EditorGUILayout.PropertyField(flareData);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Create New Data"))
        {
            string path = EditorUtility.SaveFilePanel("Save Location", "Asset/", "New Dynamic Ray", "asset");

            if (path.Length == 0)
                return;

            path = "Assets" + path.Substring(Application.dataPath.Length);

            DynamicRayData i = CreateInstance(typeof(DynamicRayData)) as DynamicRayData;

            AssetDatabase.CreateAsset(i, path);
            Debug.Log("Saved asset to " + path + "!");

            flareData.objectReferenceValue = LumenProjectSetup.GetAssets<DynamicRayData>(new string[1] { path.Substring(0, path.Length - (i.name.Length + 6)) }, i.name)[0];

        }


        if (serializedObject.hasModifiedProperties)
            lumenRay.RedoEffect();

        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();

        if (flareData.objectReferenceValue)
        {
            SerializedObject so = new SerializedObject(lumenRay.rayData);
            CreateEditor(lumenRay.rayData).OnInspectorGUI();

            GUIStyle foldoutStyle = new GUIStyle(GUI.skin.GetStyle("toolbarPopup"));
            foldoutStyle.fontStyle = FontStyle.Bold;
            foldoutStyle.margin = new RectOffset(30, 10, 5, 5);

            settings = EditorGUILayout.BeginFoldoutHeaderGroup(settings, "   Local Settings", foldoutStyle);
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (settings)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(updateFrequency);
                EditorGUILayout.PropertyField(localColor);
                EditorGUI.indentLevel--;
            }

            if (serializedObject.hasModifiedProperties || so.hasModifiedProperties || lumenRay.rayData.needsToBeUpdated)
                lumenRay.RedoEffect();


            serializedObject.ApplyModifiedProperties();
            so.ApplyModifiedProperties();
        }
        else
            EditorGUILayout.HelpBox("Set your Lumen Ray data here!", MessageType.Info, true);
    }
}