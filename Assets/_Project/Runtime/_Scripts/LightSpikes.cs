#region
using UnityEngine;
#endregion

[SelectionBase]
public class LightSpikes : MonoBehaviour
{
    Player _player;

    void Start() => _player = FindObjectOfType<Player>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _player.Death();
    }
}
