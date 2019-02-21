using UnityEngine;

public class WallAvoidance : SteeringBehavior
{
    [Range(5, 15)]
    public int radius;

    [SerializeField]
    Transform target;

    Vector2 offSet;
    bool positionReset;

    new void Start()
    {
        base.Start();

        //pick a random direction for the agent to move in when the program starts
        //take a look a the function SetTargetPosition()
        positionReset = true;
    }

    private void Update()
    {
        //check for wall collision
        CheckIfWallHit();
        //set the position of the target away from the agent for it to seek
        SetTargetPosition();

        //apply the default steering behavior to the agent
        Steer(target.position);
        ApplySteeringToMotion();

        Debug.DrawRay(transform.position, target.position - transform.position, Color.black);
    }

    private void CheckIfWallHit()
    {
        //calculate the distance between the target and agent
        float distanceFromTarget = Vector2.Distance(transform.position, target.position);

        //do a raycast from the agent's position towards the position of the target
        //check if any wall objects were hit. The walls are in layer '8' called 'Wall'
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, target.position - transform.position, distanceFromTarget, 1 << 8);

        //If the distance is too small which could occur if the 'radius' is small aswell,
        //then the agent might get stuck within a wall object and won't be able to move.
        //therefore the position of the agent is set to the middle of the area surronded by walls.
        if (distanceFromTarget <= 0)
        {
            transform.position = Vector3.zero;
            acceleration = Vector3.zero;
            velocity = Vector3.zero;
            location = transform.position;
            positionReset = true;
        }
        else if (hit2D)
        {
            //use scalar projection to find the reflected vector off the wall that was hit
            Vector2 vectorToProject = (Vector2)transform.position - hit2D.point;
            Vector2 vectorToProjectOn = (hit2D.point + hit2D.normal) - hit2D.point;
            Vector2 reflectedVector = GetReflectedVector(ref vectorToProject, ref vectorToProjectOn);

            //get the direction of the target relative to the reflected vector to set it's new position
            //within the circle's circumference.
            Vector2 dirOfTarget = (hit2D.point + reflectedVector) - hit2D.point;
            dirOfTarget.Normalize();

            //calculate the new offset
            offSet = dirOfTarget * radius;
        }
    }

    private void SetTargetPosition()
    {
        //if the position has been reset, then set the position of the target at a random point within the circle's circumference.
        if (positionReset)
        {
            offSet = (Random.insideUnitCircle).normalized * radius;
            positionReset = false;
        }

        target.position = (Vector2)transform.position + offSet;
    }

    private Vector2 GetReflectedVector(ref Vector2 u, ref Vector2 v)
    {
        //In order to get the reflected vector we need to do scalar projection.
        //formulae for scalar projection is: Project U onto V = ( Dot product of U and V / Magnitude of V squared ) * the Vector V

        float scalar = Vector3.Dot(u, v) / Mathf.Pow(Vector3.Magnitude(v), 2);

        Vector2 scalarProjection = (v * scalar);

        //formula for finding the reflected vector: (2 * projected vector)-the vector to reflect, which
        //in the previous line was the vector that was projected onto another vector i.e: 'u'
        return (2 * scalarProjection) - u;
    }
}