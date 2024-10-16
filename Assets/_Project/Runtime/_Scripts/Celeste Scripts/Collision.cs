﻿#region
using UnityEngine;
#endregion

public class Collision : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] LayerMask groundLayer;

    [Space]
    [SerializeField] bool onGround;
    [SerializeField] bool onWall;
    [SerializeField] bool onRightWall;
    [SerializeField] bool onLeftWall;
    [SerializeField] int wallFacing;

    [Space]
    [Header("Collision")]
    [SerializeField] float collisionRadius = 0.25f;
    [SerializeField] Vector2 bottomOffset, rightOffset, leftOffset;
    [SerializeField] Color gizmoColour = Color.red;

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapCircle((Vector2) transform.position + bottomOffset, collisionRadius, groundLayer | LayerMask.GetMask("Ignore Raycast"));

        onWall = Physics2D.OverlapCircle((Vector2) transform.position + rightOffset, collisionRadius, groundLayer | LayerMask.GetMask("Ignore Raycast")) || Physics2D.OverlapCircle
            ((Vector2) transform.position + leftOffset, collisionRadius, groundLayer | LayerMask.GetMask("Ignore Raycast"));

        onRightWall = Physics2D.OverlapCircle((Vector2) transform.position + rightOffset, collisionRadius, groundLayer | LayerMask.GetMask("Ignore Raycast"));
        onLeftWall = Physics2D.OverlapCircle((Vector2) transform.position + leftOffset, collisionRadius, groundLayer | LayerMask.GetMask("Ignore Raycast"));

        wallFacing = onRightWall ? -1 : 1;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColour;

        Gizmos.DrawWireSphere((Vector2) transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2) transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2) transform.position + leftOffset, collisionRadius);
    }

    #region Properties
    public bool OnGround => onGround;
    public bool OnWall => onWall;
    public bool OnRightWall => onRightWall;
    public bool OnLeftWall => onLeftWall;
    public int WallFacing => wallFacing;
    #endregion
}
