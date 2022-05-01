using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : RaycastController
{
    public CollisionInfo collisionInfo;
    //PlayerMovement player;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        //player = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame

    public void Move(Vector2 velo)
    {
        UpdateRayOrigins();

        if (boxCollider.enabled)
        {
            if (velo.x != 0.0f)
                HorizontalCollisions(ref velo);
        
            if (velo.y != 0.0f)
                VerticleCollisions(ref velo);
        }

        transform.Translate(velo, Space.World);
    }

    void HorizontalCollisions(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;
        for (uint i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1.0f) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, mask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit.collider != null)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisionInfo.left = directionX == -1.0f;
                collisionInfo.right = directionX == 1.0f;
            }
        }
    }

    void VerticleCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        for (uint i = 0; i < verticleRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1.0f) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticleRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, mask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if(hit.collider != null)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisionInfo.below = directionY == -1.0f;
                collisionInfo.above = directionY == 1.0f;
            }
        }
    }

    public void SetVelocity(Vector2 velo)
    {
        //player.SetVelocity(velo);
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public Vector3 velocityOld;
    }

  
}
