using UnityEditor;
using UnityEngine;

public class StaticRayInspector : ShaderGUI
{
    bool colorSettings;

    bool fadeSettings;
    bool noiseSettings;
    bool raylengthSettings;
    MaterialProperty style;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {


        style = FindProperty("_Style", properties);
        GUIStyle foldoutStyle = new GUIStyle(GUI.skin.GetStyle("toolbarPopup"));
        foldoutStyle.fontStyle = FontStyle.Bold;
        foldoutStyle.margin = new RectOffset(30, 10, 5, 5);


        
        Rect space = new Rect(0, 0, EditorGUIUtility.currentViewWidth, EditorGUIUtility.currentViewWidth  * 0.15f);

        EditorGUILayout.Space(space.height);

        Texture banner = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Distant Lands/Lumen - Stylized Light Effects/Contents/Scripts/Editor/Icons/Titlebar.png", typeof(Texture));

        GUI.DrawTexture(space, banner);

        colorSettings = EditorGUILayout.BeginFoldoutHeaderGroup(colorSettings, "    Main Settings", foldoutStyle);
        EditorGUILayout.EndFoldoutHeaderGroup();
        if (colorSettings)
        {
            MaterialProperty colorProperty = FindProperty("_MainColor", properties);
            materialEditor.ShaderProperty(colorProperty, colorProperty.displayName);
            MaterialProperty sun = FindProperty("_UseLightColor", properties);
            materialEditor.ShaderProperty(sun, sun.displayName);
            MaterialProperty intensity = FindProperty("_Intensity", properties);
            materialEditor.ShaderProperty(intensity, intensity.displayName);
            EditorGUILayout.Space();
            materialEditor.ShaderProperty(style, style.displayName);

        }


        fadeSettings = EditorGUILayout.BeginFoldoutHeaderGroup(fadeSettings, "    Fading Settings", foldoutStyle);
        EditorGUILayout.EndFoldoutHeaderGroup();
        if (fadeSettings)
        {
            MaterialProperty useCameraDepth = FindProperty("_UseCameraDepthFade", properties);
            materialEditor.ShaderProperty(useCameraDepth, useCameraDepth.displayName);
            EditorGUI.BeginDisabledGroup(useCameraDepth.floatValue != 1);
            MaterialProperty cameraDepthStart = FindProperty("_CameraDepthFadeStart", properties);
            materialEditor.ShaderProperty(cameraDepthStart, cameraDepthStart.displayName);
            MaterialProperty cameraDepthEnd = FindProperty("_CameraDepthFadeEnd", properties);
            materialEditor.ShaderProperty(cameraDepthEnd, cameraDepthEnd.displayName);
            EditorGUILayout.Space();
            EditorGUI.EndDisabledGroup();

            MaterialProperty useCameraDistance = FindProperty("_UseCameraDistanceFade", properties);
            materialEditor.ShaderProperty(useCameraDistance, useCameraDistance.displayName);
            EditorGUI.BeginDisabledGroup(useCameraDistance.floatValue != 1);
            MaterialProperty cameraDistanceStart = FindProperty("_CameraDistanceFadeStart", properties);
            materialEditor.ShaderProperty(cameraDistanceStart, cameraDistanceStart.displayName);
            MaterialProperty cameraDistanceEnd = FindProperty("_CameraDistanceFadeEnd", properties);
            materialEditor.ShaderProperty(cameraDistanceEnd, cameraDistanceEnd.displayName);
            EditorGUILayout.Space();
            EditorGUI.EndDisabledGroup();

            MaterialProperty useSceneDepth = FindProperty("_UseSceneDepthFade", properties);
            materialEditor.ShaderProperty(useSceneDepth, useSceneDepth.displayName);
            EditorGUI.BeginDisabledGroup(useSceneDepth.floatValue != 1);
            MaterialProperty sceneDepthStart = FindProperty("_DepthFadeStartDistance", properties);
            materialEditor.ShaderProperty(sceneDepthStart, sceneDepthStart.displayName);
            MaterialProperty sceneDepthEnd = FindProperty("_DepthFadeEndDistance", properties);
            materialEditor.ShaderProperty(sceneDepthEnd, sceneDepthEnd.displayName);
            EditorGUILayout.Space();
            EditorGUI.EndDisabledGroup();

            MaterialProperty useAngleDepth = FindProperty("_UseAngleBasedFade", properties);
            materialEditor.ShaderProperty(useAngleDepth, useAngleDepth.displayName);
            EditorGUI.BeginDisabledGroup(useAngleDepth.floatValue != 1);
            MaterialProperty angleFadeStart = FindProperty("_AngleFadeStart", properties);
            materialEditor.ShaderProperty(angleFadeStart, angleFadeStart.displayName);
            MaterialProperty angleFade = FindProperty("_AngleFade", properties);
            materialEditor.ShaderProperty(angleFade, angleFade.displayName);
            EditorGUILayout.Space();
            EditorGUI.EndDisabledGroup();
        }


        noiseSettings = EditorGUILayout.BeginFoldoutHeaderGroup(noiseSettings, "    Variation Settings", foldoutStyle);
        EditorGUILayout.EndFoldoutHeaderGroup();
        if (noiseSettings)
        {
            MaterialProperty useVariation = FindProperty("_UseVariation", properties);
            materialEditor.ShaderProperty(useVariation, useVariation.displayName);
            EditorGUI.BeginDisabledGroup(useVariation.floatValue != 1);

            MaterialProperty uniformVariation = FindProperty("_UseUniformVariation", properties);
            materialEditor.ShaderProperty(uniformVariation, uniformVariation.displayName);
            MaterialProperty variationColor = FindProperty("_VariationColor", properties);
            materialEditor.ShaderProperty(variationColor, variationColor.displayName);
            MaterialProperty variationSpeed = FindProperty("_VariationSpeed", properties);
            materialEditor.ShaderProperty(variationSpeed, variationSpeed.displayName);
            MaterialProperty variationScale = FindProperty("_VariationScale", properties);
            materialEditor.ShaderProperty(variationScale, variationScale.displayName);
            EditorGUILayout.Space();
            EditorGUI.EndDisabledGroup();
        }

        if (style.floatValue == 1)
        {
            raylengthSettings = EditorGUILayout.BeginFoldoutHeaderGroup(raylengthSettings, "    Dynamic Ray Settings", foldoutStyle);
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (raylengthSettings)
            {

                MaterialProperty bidirectional = FindProperty("_Bidirectional", properties);
                materialEditor.ShaderProperty(bidirectional, bidirectional.displayName);
                MaterialProperty raylength = FindProperty("_RayLength", properties);
                materialEditor.ShaderProperty(raylength, raylength.displayName);
                EditorGUILayout.Space();

                //MaterialProperty useRaylength = ShaderGUI.FindProperty("_UseRaylengthFade", properties);
                //materialEditor.ShaderProperty(useRaylength, useRaylength.displayName);
                //EditorGUI.BeginDisabledGroup(useRaylength.floatValue != 1);
                MaterialProperty angleOpacity = FindProperty("_AngleOpacityEffect", properties);
                materialEditor.ShaderProperty(angleOpacity, angleOpacity.displayName);
                MaterialProperty angleRaylength = FindProperty("_AngleRaylengthEffect", properties);
                materialEditor.ShaderProperty(angleRaylength, angleRaylength.displayName);
                EditorGUILayout.Space();
                MaterialProperty autoAssign = FindProperty("_AutoAssignSun", properties);
                materialEditor.ShaderProperty(autoAssign, autoAssign.displayName);
                if (autoAssign.floatValue == 0)
                {
                    MaterialProperty sunDir = FindProperty("_SunDirection", properties);
                    materialEditor.ShaderProperty(sunDir, sunDir.displayName);
                }
                //EditorGUI.EndDisabledGroup();
            }
        }
    }
}