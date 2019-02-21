using UnityEngine;

public class ObstacleAvoidance : SteeringBehavior
{
    [SerializeField]
    float maxSeeAhead;
    float xSize, ySize;

    Vector3 topLeft, topRight, bottomLeft, bottomRight;

    new void Start()
    {
        base.Start();

        SetInitialBoundingBoxInfo();

        /* Set the current velocity of the object relative to it's up vector. */
        velocity = new Vector3(transform.up.x, transform.up.y, 0);
    }

    void Update()
    {
        WrapAroundCameraView();
        CreateVirtualBoundingBox();
        CheckForCollisionDetected();
        ApplySteeringToMotion();
    }

    private void CheckForCollisionDetected()
    {
        RaycastHit2D[] hit2D = new RaycastHit2D[2];

        /* 2 raycasts are used for this, one points from the bottom left corner to the top left corner of the agent and
        the other from the bottom right to the top right */
        hit2D[0] = Physics2D.Raycast(bottomLeft, topLeft - bottomLeft, maxSeeAhead, 1 << 9);
        hit2D[1] = Physics2D.Raycast(bottomRight, topRight - bottomRight, maxSeeAhead, 1 << 9);

        Vector3 dirOfMovementToAvoidObstacle;

        if (hit2D[0])
        {
            /* if a collision was detected on the left side of the bounding box, the direction of movement (to
            steer away from the obstacle) will be to the right. */
            dirOfMovementToAvoidObstacle = topRight - hit2D[0].collider.transform.position;

            /* Make the direction of vector to avoid obtacle, point away from it as much as possible to ensure the obstacle doesnt collide with it
             This can obviously be changed to make your own direction of movement when an obstacle is detected.*/
            dirOfMovementToAvoidObstacle *= Vector2.Distance(transform.position, hit2D[0].collider.transform.position);

            Steer(dirOfMovementToAvoidObstacle);

            Debug.DrawRay(hit2D[0].collider.transform.position, topRight - hit2D[0].collider.transform.position, Color.white);
        }
        else if (hit2D[1])
        {
            dirOfMovementToAvoidObstacle = topLeft - hit2D[1].collider.transform.position;
            dirOfMovementToAvoidObstacle *= Vector2.Distance(transform.position, hit2D[1].collider.transform.position);

            Steer(dirOfMovementToAvoidObstacle);

            Debug.DrawRay(hit2D[1].collider.transform.position, topLeft - hit2D[1].collider.transform.position, Color.white);
        }
        /* If no obstacle was detected, then just steer it towards it's current velocity */
        else Steer(location + (velocity.normalized * velocity.magnitude));
    }

    private void CreateVirtualBoundingBox()
    {
        /* Create the bounding box around the sprite for collision detection */
        bottomRight = transform.position + (transform.right * (xSize / 2)) + (-transform.up * (ySize / 2));
        bottomLeft = transform.position + (-transform.right * (xSize / 2)) + (-transform.up * (ySize / 2));

        topRight = transform.position + ((transform.right * (xSize / 2)) + (transform.up * maxSeeAhead));
        topLeft = transform.position + (-transform.right * (xSize / 2)) + (transform.up * maxSeeAhead);

        Debug.DrawRay(bottomRight, topRight - bottomRight, Color.green);
        Debug.DrawRay(bottomLeft, topLeft - bottomLeft, Color.green);

        Debug.DrawRay(bottomRight, bottomLeft - bottomRight, Color.green);
        Debug.DrawRay(topRight, topLeft - topRight, Color.green);
    }

    private void SetInitialBoundingBoxInfo()
    {
        /* Calculate some data that will be used to create the bounding box around the agent */
        float currentZRotation = transform.eulerAngles.z;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        xSize = GetComponent<SpriteRenderer>().bounds.size.x;
        ySize = GetComponent<SpriteRenderer>().bounds.size.y;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentZRotation));
    }
}