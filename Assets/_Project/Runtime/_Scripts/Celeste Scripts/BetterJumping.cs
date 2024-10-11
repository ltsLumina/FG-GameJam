using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BetterJumping : MonoBehaviour
{
    Player player;
    Rigidbody2D rb;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    void Start()
    {
        player = GetComponentInParent<Player>();
        rb     = GetComponent<Rigidbody2D>();
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
