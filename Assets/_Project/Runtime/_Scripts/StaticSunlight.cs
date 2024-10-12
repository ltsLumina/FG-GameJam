#region
using System.Collections;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
#endregion

public class StaticSunlight : MonoBehaviour
{
    int index;

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
        SpriteRenderer spriteRenderer;

        if (other.TryGetComponent(out Player player))
        {
            spriteRenderer = player.GetComponentInChildren<SpriteRenderer>();
            StartCoroutine(Wait());
        }

        return;

        IEnumerator Wait()
        {
            Logger.LogWarning("Player has entered the sunlight. \nRespawning player at spawn point.");

            spriteRenderer.DOFade(0, .5f);
            yield return new WaitForSeconds(.5f);

            Color color = spriteRenderer.color;
            color.a = 1;
            spriteRenderer.color = color;
            player.Death();
        }
    }

    void OnValidate()
    {
        index = transform.GetSiblingIndex() - 1;
        if (index < 0) index = 0;

        name = $"Static Sunlight - {transform.position} - #{index}";
    }
}
