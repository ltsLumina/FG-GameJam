#region
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
#endregion

public class StaticSunlight : MonoBehaviour
{
    int index;

    void OnEnable()
    {
#if UNITY_EDITOR

        // Move this component to the top of the component list
        Component[] components = gameObject.GetComponents(typeof(Component));

        for (int i = 0; i < components.Length - 1; i++) { ComponentUtility.MoveComponentUp(this); }
#endif
    }

    void OnDrawGizmos()
    {
        // draw the name of the object
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, .45f);

        // Draw a number next to the object
        var style = new GUIStyle();
        style.normal.textColor = Color.yellow;
        style.fontSize = 8;
        style.alignment = TextAnchor.MiddleCenter;
        Handles.Label(transform.position + new Vector3(0, 1.5f), $"{transform.position} - #{index}", style);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player)) player.Death();
    }

    void OnValidate()
    {
        index = transform.GetSiblingIndex() - 1;
        if (index < 0) index = 0;

        name = $"Static Sunlight - {transform.position} - #{index}";
    }
}

public static class MenuIntegration
{
    [MenuItem("GameObject/Light/Static Sunlight")]
    public static void CreateStaticSunlightMenu(MenuCommand menuCommand)
    {
        GameObject o = CreateStaticLight();

        Undo.RegisterCreatedObjectUndo(o, "Create Static Light");

        Selection.activeGameObject = o.GetComponentInChildren<StaticSunlight>().gameObject;
    }

    static GameObject CreateStaticLight()
    {
        var o = Resources.Load<GameObject>("_PREFABS/Static Sunlight");

        if (o == null)
        {
            Debug.LogError("Couldn't find Static Sunlight prefab.");
            return null;
        }

        o = Object.Instantiate(o);

        return o;
    }
}
