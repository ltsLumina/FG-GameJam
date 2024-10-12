#region
using DG.Tweening;
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
        if (other.TryGetComponent(out Player player))
        {
            var spriteRenderer = player.GetComponentInChildren<SpriteRenderer>();
            Sequence sequence = DOTween.Sequence();

            player.Death();

            // sequence.AppendCallback
            // (() =>
            // {
            //     Logger.LogWarning("Player has entered the sunlight. \nRespawning player at spawn point.");
            //     player.Death();
            // });

            //sequence.Append(spriteRenderer.DOFade(0, .5f));
        }
    }

    void OnValidate()
    {
        index = transform.GetSiblingIndex() - 1;
        if (index < 0) index = 0;

        name = $"Static Sunlight - {transform.position} - #{index}";
    }
}
