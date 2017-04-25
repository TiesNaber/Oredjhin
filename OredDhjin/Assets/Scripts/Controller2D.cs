using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    public LayerMask collisionMask;


    private float thickness;
    const float skinWidth = 0.015f;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D collider;
    RayCastOrigins raycastOrigins;
    public CollisionInfo collisions;

	void Start ()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }
	
    void Update()
    {
        
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Debug.DrawRay(raycastOrigins.topMiddle + Vector2.down * horizontalRaySpacing * i, Vector2.right * thickness * -1, Color.blue);
            Debug.DrawRay(raycastOrigins.topMiddle + Vector2.down * horizontalRaySpacing * i, Vector2.right * thickness, Color.green);
        }
        
    }

    public void Move(Vector3 velocity)
    {
        UpdateRayCastOrigins();
        collisions.Reset();

        if (velocity.x != 0)
        {
            horizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }           
        transform.Translate(velocity);
    }

    void horizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance; // This shortens the future casted array lengths to avoid detecting grounds lower than the first detection

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance; // This shortens the future casted array lengths to avoid detecting grounds lower than the first detection

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    void UpdateRayCastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);
        thickness = bounds.extents.x;

        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);

        raycastOrigins.topMiddle = new Vector2(bounds.center.x, bounds.max.y);
        raycastOrigins.bottomMiddle = new Vector2(bounds.center.x, bounds.min.y);

    }

    void CalculateRaySpacing()  // This calculate how much space there is between the rays based on the raycount and collider size.
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.y / (verticalRayCount - 1);
    }

    struct RayCastOrigins   // Simply contains the new locations of the boundaries of the collider after calculating the skinwidth.
    {
        public Vector2 topLeft, topMiddle, topRight;           
        public Vector2 bottomLeft, bottomMiddle, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }
}
