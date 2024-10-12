﻿#region
using System.Collections;
using DG.Tweening;
using Lumina.Essentials.Attributes;
using TransitionsPlus;
using UnityEngine;
using UnityEngine.InputSystem;
#endregion

public partial class Player : MonoBehaviour
{
    [Header("Spawn Point")]
    [SerializeField] bool raycastSpawnPoint;
    [SerializeField] Vector2 spawnPoint;

    [Space, Header("Stats")]
    [SerializeField] float speed = 10f;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float jumpBufferTime = 0.2f;
    [SerializeField] float gravityScale;
    [SerializeField] float wallSlideMult = .95f;
    [SerializeField] float wallJumpMult = 1.15f;
    [SerializeField] float wallJumpLerp = 1f;
    [SerializeField] float dashSpeed = 25f;
    [SerializeField] float dashDrag = 3;

    [Space, Header("Coyote Time")]
    [SerializeField] float coyoteTimeDuration = 0.2f;

    [Space, Header("Booleans")]
    [SerializeField, ReadOnly] bool canMove;
#pragma warning disable CS0414 // Field is assigned but its value is never used
    [SerializeField, ReadOnly] bool coyote;
#pragma warning restore CS0414 // Field is assigned but its value is never used
    [SerializeField, ReadOnly] bool wallGrab;
    [SerializeField, ReadOnly] bool wallJumped;
    [SerializeField, ReadOnly] bool wallSlide;
    [SerializeField, ReadOnly] bool isDashing;
    [SerializeField, ReadOnly] bool groundTouch;
    [SerializeField, ReadOnly] bool hasDashed;

    /// <summary>
    /// 1 == right, -1 == left
    /// </summary>
    [SerializeField, ReadOnly] int facing = 1;

    [Space, Header("Polish")]
    [SerializeField] ParticleSystem dashParticle;
    [SerializeField] ParticleSystem jumpParticle;
    [SerializeField] ParticleSystem slideParticle;
    [SerializeField] ParticleSystem wallJumpParticle;

    [Header("Transition")]
    [SerializeField] TransitionAnimator deathTransition;
    [SerializeField] TransitionAnimator loadTransition;

    AnimationScript anim;
    Collision coll;

    float coyoteTimeTimer;
    float jumpBufferTimer;

    bool onGUI;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<AnimationScript>();
        deathTransition = GetComponentInChildren<TransitionAnimator>();
        loadTransition = GetComponentInChildren<TransitionAnimator>();

        transform.position = spawnPoint;
    }

    // void OnGUI()
    // {
    //     // Draw all the booleans from the inspector on right of the screen
    //     if (GUI.Button(new Rect(Screen.width - 100, 0, 100, 50), "Toggle GUI")) onGUI = !onGUI;
    //     
    //     if (!onGUI) return;
    // }

    // Update is called once per frame

    void Update()
    {
        if (jumpBufferTimer > 0) jumpBufferTimer -= Time.deltaTime;

        {
        }

        float x = MoveInput.x;
        float y = MoveInput.y;
        var dir = new Vector2(x, y);

        Walk(dir);
        anim.SetHorizontalMovement(x, y, rb.velocity.y);

        InputAction dashAction = playerInput.actions["Dash"];

        if (coll.OnWall && dashAction.WasReleasedThisFrame() && canMove)
        {
            if (facing != coll.WallFacing) anim.Flip(facing * -1);
            wallGrab = true;
            wallSlide = false;
        }

        if (dashAction.WasReleasedThisFrame() || !coll.OnWall || !canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }

        if (coll.OnGround)
        {
            coyoteTimeTimer = coyoteTimeDuration;
            coyote = true;
        }
        else
        {
            coyoteTimeTimer -= Time.deltaTime;
            if (coyoteTimeTimer <= 0) coyote = false;
        }

        if (coll.OnGround && !isDashing)
        {
            wallJumped = false;
            GetComponent<BetterJumping>().enabled = true;
        }

        if (wallGrab && !isDashing)
        {
            rb.gravityScale = 0;
            if (x is > .2f or < -.2f) rb.velocity = new (rb.velocity.x, 0);

            float speedModifier = y > 0 ? .5f : 1;

            rb.velocity = new (rb.velocity.x, y * (speed * speedModifier));
        }

        else { rb.gravityScale = gravityScale; }

        if (coll.OnWall && !coll.OnGround)
            if (x != 0 && !wallGrab)
            {
                wallSlide = true;
                WallSliding();
            }

        if (!coll.OnWall || coll.OnGround) wallSlide = false;

        if (coll.OnGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!coll.OnGround && groundTouch) groundTouch = false;

        WallParticle(y);

        if (wallGrab || wallSlide || !canMove) return;

        if (x > 0)
        {
            facing = 1;
            anim.Flip(facing);
        }

        if (x < 0)
        {
            facing = -1;
            anim.Flip(facing);
        }
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

    public override string ToString()
    {
        string[] strings =
        { $"{name} Booleans:",
          $"Can Move: {canMove}",
          $"Wall Grab: {wallGrab}",
          $"Wall Jumped: {wallJumped}",
          $"Wall Slide: {wallSlide}",
          $"Is Dashing: {isDashing}" };

        return string.Join("\n", strings);
    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        facing = anim.SpriteRenderer.flipX ? -1 : 1;

        jumpParticle.Play();

        // Check if jump buffer is still valid
        if (jumpBufferTimer > 0)
        {
            Jump(Vector2.up, false);
            jumpBufferTimer = 0; // Reset the buffer timer after jumping
        }
    }

    void Dash(float x, float y)
    {
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .5f, 14);
        FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

        hasDashed = true;

        anim.SetTrigger("dash");

        rb.velocity = Vector2.zero;
        var dir = new Vector2(x, y);

        rb.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());
        DOVirtual.Float(dashDrag, 0, .8f, RigidbodyDrag);

        dashParticle.Play();
        rb.gravityScale = 0;
        GetComponent<BetterJumping>().enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        dashParticle.Stop();
        rb.gravityScale = gravityScale;
        GetComponent<BetterJumping>().enabled = true;
        wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (coll.OnGround) hasDashed = false;
    }

    void WallJump()
    {
        if ((coll.OnRightWall && facing == 1) || (coll.OnLeftWall && facing == -1))
        {
            facing *= -1;
            anim.Flip(facing);

            StopCoroutine(DisableMovement(0));
            StartCoroutine(DisableMovement(.1f));

            Vector2 wallDir = coll.OnRightWall ? Vector2.left : Vector2.right;

            Jump(Vector2.up * wallJumpMult + wallDir / 1.5f, true);

            wallJumped = true;
        }
    }

    bool WallSliding()
    {
        if (coll.WallFacing != facing) anim.Flip(facing * -1);

        if (!canMove) return false;

        bool pushingWall = (rb.velocity.x < 0 && coll.OnLeftWall) || (rb.velocity.x > 0 && coll.OnRightWall);
        float push = pushingWall ? 0 : rb.velocity.x;
        rb.velocity = new (push, rb.velocity.y * wallSlideMult);

        return true;
    }

    void Walk(Vector2 dir)
    {
        if (!canMove) return;
        if (wallGrab) return;

        rb.velocity = !wallJumped ? new (dir.x * speed, rb.velocity.y) : Vector2.Lerp(rb.velocity, new (dir.x * speed, rb.velocity.y), wallJumpLerp * Time.deltaTime);
    }

    void Jump(Vector2 dir, bool wall)
    {
        slideParticle.transform.parent.localScale = new (ParticleSide(), 1, 1);
        ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        rb.velocity = new (rb.velocity.x * 2, 0);
        rb.velocity += dir * jumpForce;

        particle.Play();
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void RigidbodyDrag(float x) => rb.drag = x;

    void WallParticle(float vertical)
    {
        ParticleSystem.MainModule main = slideParticle.main;

        if (wallSlide || (wallGrab && vertical < 0))
        {
            slideParticle.transform.parent.localScale = new (ParticleSide(), 1, 1);
            main.startColor = Color.white;
        }
        else { main.startColor = Color.clear; }
    }

    int ParticleSide()
    {
        int particleSide = coll.OnRightWall ? 1 : -1;
        return particleSide;
    }

    public void Death()
    {
        if (!enabled) return;

        //anim.SetTrigger("death");
        enabled = false;
        StopAllCoroutines();
        rb.velocity = Vector2.zero;
        deathTransition.Play();
    }

    #region Properties
    public bool CanMove => canMove;
    public bool WallGrab => wallGrab;
    public bool WallJumped => wallJumped;
    public bool WallSlide => wallSlide;
    public bool IsDashing => isDashing;

    public Vector2 SpawnPoint => spawnPoint;
    #endregion
}
