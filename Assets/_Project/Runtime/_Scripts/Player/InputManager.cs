#region
using UnityEngine;
using UnityEngine.InputSystem;
#endregion

public partial class Player // InputManager
{
    PlayerInput playerInput;

    public Vector2 MoveInput { get; private set; }
    public Vector2 MoveInputRaw { get; private set; }

    void Awake()
    {
        playerInput = GetComponentInChildren<PlayerInput>();
        Debug.Assert(playerInput != null, "Player Input component not found.");
    }

    // -- Player Input Actions --

    public void OnMove(InputAction.CallbackContext context)
    {
        //Logger.Log("Move Vector: " + MoveInput);
        MoveInputRaw = context.ReadValue<Vector2>();
        MoveInput = MoveInputRaw.normalized;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            anim.SetTrigger("jump");

            if (coll.OnGround || coyoteTimeTimer > 0)
            {
                Jump(Vector2.up, false);
                coyoteTimeTimer = 0; // Reset coyote time after jumping
            }
            else if (coll.OnWall && !coll.OnGround) { WallJump(); }
            else { jumpBufferTimer = jumpBufferTime; }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        float xRaw = MoveInputRaw.x;
        float yRaw = MoveInputRaw.y;

        if (!context.performed || hasDashed) return;
        if (xRaw != 0 || yRaw != 0) Dash(xRaw, yRaw);
    }

    // -- UI Input Actions --

    /// <summary>
    ///     Toggles the pause state of the game.
    ///     <remarks> Although the name is "OnPause", it is used to toggle the pause state of the game. </remarks>
    /// </summary>
    /// <param name="context"></param>
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed) GameManager.TogglePause();
    }
}
