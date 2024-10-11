#region
using System;
using System.Collections;
using DG.Tweening;
using Lumina.Essentials.Attributes;
using Lumina.Essentials.Modules;
using UnityEngine;
using UnityEngine.InputSystem;
#endregion

public partial class Player : MonoBehaviour
{
    Collision coll;
    Rigidbody2D rb;
    AnimationScript anim;

    [Space, Header("Stats")]
    [SerializeField] float speed = 10;
    [SerializeField] float jumpForce = 50;
#pragma warning disable CS0414 // Field is assigned but its value is never used
    float UNUSED_slideSpeed = 5;
#pragma warning restore CS0414 // Field is assigned but its value is never used
    [SerializeField] float wallJumpLerp = 10;
    [SerializeField] float dashSpeed = 20;
    [SerializeField] int startDrag = 5;
    [SerializeField] int endDrag;

    [Space, Header("Coyote Time")]
    [SerializeField] float coyoteTimeDuration = 0.2f;
    float coyoteTimeTimer;

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

    #region Properties
    public bool CanMove => canMove;
    public bool WallGrab => wallGrab;
    public bool WallJumped => wallJumped;
    public bool WallSlide => wallSlide;
    public bool IsDashing => isDashing;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision>();
        rb   = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<AnimationScript>();
    }

    bool onGUI;

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
        float x    = MoveInput.x;
        float y    = MoveInput.y;
        var   dir  = new Vector2(x, y);

        Walk(dir);
        anim.SetHorizontalMovement(x, y, rb.velocity.y);

        InputAction dashAction = playerInput.actions["Dash"];

        if (coll.OnWall && dashAction.WasReleasedThisFrame() && canMove)
        {
            if (facing != coll.WallFacing) anim.Flip(facing * -1);
            wallGrab  = true;
            wallSlide = false;
        }

        if (dashAction.WasReleasedThisFrame() || !coll.OnWall || !canMove)
        {
            wallGrab  = false;
            wallSlide = false;
        }

        if (coll.OnGround)
        {
            coyoteTimeTimer = coyoteTimeDuration;
            coyote         = true;
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
        else { rb.gravityScale = 3; }

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

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        facing = anim.SpriteRenderer.flipX ? -1 : 1;

        jumpParticle.Play();
    }

    void Dash(float x, float y)
    {
        Helpers.CameraMain.transform.DOComplete();
        Helpers.CameraMain.transform.DOShakePosition(.2f, .5f, 14);
        FindObjectOfType<RippleEffect>().Emit(Helpers.CameraMain.WorldToViewportPoint(transform.position));

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
        DOVirtual.Float(startDrag, endDrag, .8f, RigidbodyDrag);

        dashParticle.Play();
        rb.gravityScale                       = 0;
        GetComponent<BetterJumping>().enabled = false;
        wallJumped                            = true;
        isDashing                             = true;

        yield return new WaitForSeconds(.3f);

        dashParticle.Stop();
        rb.gravityScale                       = 3;
        GetComponent<BetterJumping>().enabled = true;
        wallJumped                            = false;
        isDashing                             = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (coll.OnGround) hasDashed = false;
    }

    void WallJump()
    {
        if ((facing == 1 && coll.OnRightWall) || (facing == -1 && !coll.OnRightWall))
        {
            facing *= -1;
            anim.Flip(facing);
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = coll.OnRightWall ? Vector2.left : Vector2.right;

        Jump(Vector2.up / 1.5f + wallDir / 1.5f, true);

        wallJumped = true;
    }

    bool WallSliding()
    {
        if (coll.WallFacing != facing) anim.Flip(facing * -1);

        if (!canMove) return false;

        bool  pushingWall = (rb.velocity.x < 0 && coll.OnLeftWall) || (rb.velocity.x > 0 && coll.OnRightWall);
        float push        = pushingWall ? 0 : rb.velocity.x;

        rb.velocity = new (push, rb.velocity.y);
        
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

        rb.velocity =  new (rb.velocity.x, 0);
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
            main.startColor                           = Color.white;
        }
        else { main.startColor = Color.clear; }
    }

    int ParticleSide()
    {
        int particleSide = coll.OnRightWall ? 1 : -1;
        return particleSide;
    }
}