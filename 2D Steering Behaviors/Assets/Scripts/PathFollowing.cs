using UnityEngine;

public class PathFollowing : SteeringBehavior
{
    [SerializeField]
    Transform projectedVectorPoint;

    [SerializeField]
    Path path;

    Vector3 futureLocation;
    Vector3 p1, p2;
    Vector3 target;

    int index;

    new void Start()
    {
        base.Start();
        index = 0;
    }

    private void Update()
    {
        //start moving along the path when it has been set up 
        if (path.pathIsSet)
        {
            //get the start and end points of a line segement along the path
            p1 = path.GetLineSegmentPoint(index);
            p2 = path.GetLineSegmentPoint(index + 1);

            MoveAlongPath();

            //apply standard seek steering behavior once target is calculated
            Steer(target);
            ApplySteeringToMotion();
        }
    }

    private void MoveAlongPath()
    {
        //predict the future location of the agent based on it's current velocity.
        futureLocation = location + (velocity.normalized * 10);

        //calculate the projected vector along the line segment
        Vector3 projectedVector = GetProjectedVector();

        //set the position of the projected vector point a little bit further than the actual projected vector
        projectedVectorPoint.position = projectedVector + ((p2 - p1).normalized * 10);

        //calculate distance between the agent's future location and projected vector
        float distance = Vector3.Distance(futureLocation, projectedVector);

        //if the distance is greater than a given value, then that means the agent is off the path and needs
        //to steer towards the projected vector to get back on the path
        if (distance > 10) target = projectedVectorPoint.position;

        //else steer towards the future location
        else target = futureLocation;

        //check to see if the agent has reaced the end of a path segment.
        if (Vector2.Distance(p1, projectedVector) > Vector2.Distance(p1, p2))
        {
            if (index + 2 < path.numberOfPointsOnPath) index++;

            //if it reaches the end of the entire path then moving it towards the first line segment of the path
            else if (index == path.numberOfPointsOnPath - 2) index = 0;
        }

        Debug.DrawRay(transform.position, futureLocation - transform.position, Color.blue);
        Debug.DrawRay(transform.position, target - transform.position, Color.yellow);
    }

    private Vector3 GetProjectedVector()
    {
        /* Standard fomula used to project one vector onto another */

        //get a vector pointing from the starting point of  a line 
        Vector3 fromP1ToFutureLocation = futureLocation - p1;

        //get a normalized vector pointing from the start of the line segment to the end of the line segment
        Vector3 pathDirection = (p2 - p1).normalized;

        //multiply the above vector by the dot product of itself and the vector pointing from start of line segment to future location
        pathDirection *= Vector3.Dot(fromP1ToFutureLocation, pathDirection);

        Vector3 projectedVector = p1 + pathDirection;

        return projectedVector;
    }
}