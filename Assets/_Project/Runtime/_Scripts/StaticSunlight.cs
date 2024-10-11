using System.Collections;
using DG.Tweening;
using UnityEngine;

public class StaticSunlight : MonoBehaviour
{
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

            var color = spriteRenderer.color;
            color.a                   = 1;
            spriteRenderer.color      = color;
            player.transform.position = GameManager.Instance.SpawnPoint;
        }
    }
}
