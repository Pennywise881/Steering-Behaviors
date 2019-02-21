using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldFollowing : SteeringBehavior
{
    Vector3 arrowAheadVector;

    [SerializeField] private float scalar;

    [HideInInspector]
    public Vector2[,] arrowDirections, arrowPositions;

    void Update()
    {
        if (arrowDirections != null)
        {
            Steer(Vector3.zero);
            ApplySteeringToMotion();
        }
    }

    protected override void Steer(Vector3 targetPosition)
    {
        // Divide the x and y values of the agent's location by the number of pixels
        // that ONE arrow occupies. In this case its 10 pixels. This gives us the row and column
        // values of the particular arrow that the agent is on top of.

        int row = (int)location.y / 10;
        int col = (int)location.x / 10;


        // The desired velocity in this case is pointing towards the local "up" vector of the arrow
        // that the agent is on top of

        Vector3 desiredVelocity = GetArrowDirection(ref row, ref col) - location;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;

        Vector3 steer = Vector3.ClampMagnitude(desiredVelocity - velocity, maxForce);

        ApplyForce(steer);
    }

    private Vector3 GetArrowDirection(ref int row, ref int col)
    {

        // we use the row and column values to get the up vector of the arrow
        // since the local directional vectors of an object (tranform.up/forward/right) are
        // normalized vectors, we scale it and increase the length of the desired vector to prevent
        // the agent from making sharp turns

        // the desired vector will always be pointing at the directon of an arrow's local up vector
        // and in order for it to stay relative to the agent's location, we add it to it's current position


        Vector3 arrowDirection = Vector3.zero;

        try
        {
            arrowDirection = arrowDirections[row, col];
            Debug.DrawRay(transform.position, arrowAheadVector - transform.position, Color.white);
        }
        catch (Exception e)
        {
            int x = UnityEngine.Random.Range(0, arrowPositions.GetLength(0));
            int y = UnityEngine.Random.Range(0, arrowPositions.GetLength(1));

            location = arrowPositions[x, y];
            arrowDirection = arrowDirections[x, y];
        }


        arrowDirection *= scalar;
        arrowDirection += transform.position;
        arrowAheadVector = arrowDirection;
        return arrowDirection;
    }
}
