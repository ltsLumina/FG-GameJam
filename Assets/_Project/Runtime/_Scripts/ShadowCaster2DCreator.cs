#region
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
#endregion

#if UNITY_EDITOR

[RequireComponent(typeof(CompositeCollider2D))]
public class ShadowCaster2DCreator : MonoBehaviour
{
    readonly static FieldInfo meshField = typeof(ShadowCaster2D).GetField("m_Mesh", BindingFlags.NonPublic | BindingFlags.Instance);
    readonly static FieldInfo shapePathField = typeof(ShadowCaster2D).GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);
    readonly static FieldInfo shapePathHashField = typeof(ShadowCaster2D).GetField("m_ShapePathHash", BindingFlags.NonPublic | BindingFlags.Instance);
    readonly static MethodInfo generateShadowMeshMethod = typeof(ShadowCaster2D).Assembly.GetType("UnityEngine.Rendering.Universal.ShadowUtility").GetMethod
        ("GenerateShadowMesh", BindingFlags.Public | BindingFlags.Static);
    [SerializeField] bool selfShadows = true;

    CompositeCollider2D tilemapCollider;

    public void Create()
    {
        DestroyOldShadowCasters();
        tilemapCollider = GetComponent<CompositeCollider2D>();

        for (int i = 0; i < tilemapCollider.pathCount; i++)
        {
            var pathVertices = new Vector2[tilemapCollider.GetPathPointCount(i)];
            tilemapCollider.GetPath(i, pathVertices);
            var shadowCaster = new GameObject("shadow_caster_" + i);
            shadowCaster.transform.parent = gameObject.transform;
            var shadowCasterComponent = shadowCaster.AddComponent<ShadowCaster2D>();
            shadowCasterComponent.selfShadows = selfShadows;

            var testPath = new Vector3[pathVertices.Length];
            for (int j = 0; j < pathVertices.Length; j++) { testPath[j] = pathVertices[j]; }

            shapePathField.SetValue(shadowCasterComponent, testPath);
            shapePathHashField.SetValue(shadowCasterComponent, Random.Range(int.MinValue, int.MaxValue));
            meshField.SetValue(shadowCasterComponent, new Mesh());

            generateShadowMeshMethod.Invoke
            (shadowCasterComponent, new[]
             { meshField.GetValue(shadowCasterComponent), shapePathField.GetValue(shadowCasterComponent) });
        }
    }
    public void DestroyOldShadowCasters()
    {
        List<Transform> tempList = transform.Cast<Transform>().ToList();
        foreach (Transform child in tempList) { DestroyImmediate(child.gameObject); }
    }
}

[CustomEditor(typeof(ShadowCaster2DCreator))]
public class ShadowCaster2DTileMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Create"))
        {
            var creator = (ShadowCaster2DCreator) target;
            creator.Create();
        }

        if (GUILayout.Button("Remove Shadows"))
        {
            var creator = (ShadowCaster2DCreator) target;
            creator.DestroyOldShadowCasters();
        }

        EditorGUILayout.EndHorizontal();
    }
}

#endif
