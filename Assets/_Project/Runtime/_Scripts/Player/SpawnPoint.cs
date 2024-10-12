#region
using UnityEngine;
#endregion

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] bool raycastSpawnPoint;
    [SerializeField] Vector2 spawnPoint;

    public Vector2 Position => spawnPoint;

    void Start()
    {
        var player = FindObjectOfType<Player>();
        player.transform.position = spawnPoint;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnPoint, new (1, 1, 1));
    }

    void OnValidate()
    {
        if (!raycastSpawnPoint) return;

        // Perform a raycast downwards from the spawn point to find the ground
        RaycastHit2D hit = Physics2D.Raycast(new (spawnPoint.x, spawnPoint.y), Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));

        if (hit.collider != null)

            // Set the spawn point's Y position to the ground position
            spawnPoint = new (spawnPoint.x, hit.point.y + 1f);
        else

            // If no ground is found, set the spawn point to the default value
            spawnPoint = new (spawnPoint.x, 0f);

        transform.position = spawnPoint;
    }
}
