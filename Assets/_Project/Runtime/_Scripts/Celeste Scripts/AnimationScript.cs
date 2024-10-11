using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    Animator anim;
    Player move;
    Collision coll;

    public SpriteRenderer SpriteRenderer { get; set; }

    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponentInParent<Collision>();
        move = GetComponentInParent<Player>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        anim.SetBool("onGround", coll.OnGround);
        anim.SetBool("onWall", coll.OnWall);
        anim.SetBool("onRightWall", coll.OnRightWall);
        anim.SetBool("wallGrab", move.WallGrab);
        anim.SetBool("wallSlide", move.WallSlide);
        anim.SetBool("canMove", move.CanMove);
        anim.SetBool("isDashing", move.IsDashing);

    }

    public void SetHorizontalMovement(float x,float y, float yVel)
    {
        anim.SetFloat("HorizontalAxis", x);
        anim.SetFloat("VerticalAxis", y);
        anim.SetFloat("VerticalVelocity", yVel);
    }

    public void SetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    public void Flip(int side)
    {

        if (move.WallGrab || move.WallSlide)
        {
            switch (side)
            {
                case -1 when SpriteRenderer.flipX:
                case 1 when !SpriteRenderer.flipX:
                    return;
            }
        }

        bool state = side != 1;
        SpriteRenderer.flipX = state;
    }
}
