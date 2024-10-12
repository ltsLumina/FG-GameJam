#region
using UnityEngine;
using UnityEngine.InputSystem;
#endregion

public class BetterJumping : MonoBehaviour
{
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;

    Player player;
    Rigidbody2D rb;

    void Start()
    {
        player = GetComponentInParent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var jumpAction = player.GetComponentInChildren<PlayerInput>().actions["Jump"];

        switch (rb.velocity.y)
        {
            case < 0:
                rb.velocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
                break;

            case > 0 when !jumpAction.IsPressed():
                rb.velocity += Vector2.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
                break;
        }
    }
}
