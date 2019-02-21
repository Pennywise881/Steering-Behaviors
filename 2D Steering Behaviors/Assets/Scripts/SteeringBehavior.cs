using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehavior : MonoBehaviour
{
    [SerializeField] protected float _maxSpeed, maxForce;
    protected float maxSpeed;

    protected Vector3 acceleration, velocity, location, startPosition;

    private Camera mainCamera;
    private float cameraOffSet;

    void Awake()
    {
        //Get a reference to the main camera
        mainCamera = Camera.main;
        cameraOffSet = 2;
    }

    protected void Start()
    {
        //set both the acceleration and velocity to Vector3.zero to
        //ensure that at the start of the program there are no forces acting on the object
        //and it starts at an initial velocity of 0
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        location = transform.position;
        maxSpeed = _maxSpeed;
        startPosition = transform.position;
    }

    protected void ApplySteeringToMotion()
    {
        //add the acceleration to the velocity as we want acceleration a.k.a the force to act on the agent
        //acceleration is the rate of change of velocity and therefore, we want it to effect the velocity and change it.
        //limit the magnitude of the velocity to the user defined maximum speed
        velocity = Vector3.ClampMagnitude(velocity + acceleration, maxSpeed);

        //the location of the agent is calculated after the steering force is added to it's velocity
        //add the velocity to the location as velocity is the rate of change in location
        //multiply it with Time.deltaTime to get a smooth transition
        location += velocity * Time.deltaTime;

        //it is important to set the acceleration to zero as we dont want the acceleration (the force)
        //to pile up and get too large eventually making it very hard for the agent to move
        //before the start of the next frame we want to get rid of all the forces from the previous frame
        //and calculate a new net force to act on the agent in the next frame.
        acceleration = Vector3.zero;

        //make the agent rotate towards the target
        RotateTowardTarget();

        //set the new position to the calculated location based on the velocity and steering force
        //this causes the agent to steer and smoothly move towards the targets location every frame
        transform.position = location;
    }

    protected virtual void Steer(Vector3 targetPosition)
    {
        //Calculate the desired velocity, which is a vector pointing from the target to the position of this object
        //Note that the position is refering to the position of the agent in the previous frame as this function
        //is called before the position of the agent in the current frame is set.
        Vector3 desiredVelocity = targetPosition - location;

        //get a unit lenght vector as all we want is the direction of the desired velocity, which points to the target's location
        desiredVelocity.Normalize();

        //and then scale it by max speed to control how fast the agent moves towards this desired velocity (target's position)
        desiredVelocity *= maxSpeed;


        //make sure the steering force does not exceed the limit of the maxforce
        //what this means is that we don't want the magnitude of the steering force to be too large
        //causing a rapid change in the motion of the agent. In order to achieve a consistent and smooth motion
        //we limit the magnitude of the steering force to make sure it does not exceed the user defined maxforce.
        Vector3 steer = Vector3.ClampMagnitude(desiredVelocity - velocity, maxForce);

        //call the function to apply the steering force
        ApplyForce(steer);
    }

    protected void ApplyForce(Vector3 force)
    {
        //acceleration is a force, and this is the only kind of force we want to be acting on the agent
        //the steering force is thus added to the acceleration which is the net force acting on the agent
        acceleration += force;
    }

    protected void RotateTowardTarget()
    {
        //instead of rotating the agent to the target's position directly,
        //we want it to smoothly rotate towards it. For that,
        //instead of using the target as the 'to' vector, we use the calculated location
        //as that gradually changes and adjusts itself to eventually point towards the target's location
        Vector3 directionToDesiredLocation = location - transform.position;

        //normalize as we want a unit lenght vector to get the direction only
        directionToDesiredLocation.Normalize();

        //calculate the angle of rotation
        float rotZ = Mathf.Atan2(directionToDesiredLocation.y, directionToDesiredLocation.x) * Mathf.Rad2Deg;
        rotZ -= 90;

        //set the angle of rotation to the agent to make it rotate towards the target.
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    protected virtual void WrapAroundCameraView()
    {
        //In order to wrap an object around a camera in this project, it is important
        //to know the height and width of the orthographic camera, the height is the size of the camera,
        //which can be seen in the 'Main Camera' object's 'Camera' Component. The width is cacualted by multiplying
        //the height/orthographic size and the aspect of the camera. See below for more information

        /* Use this to wrap the agent around the view of the camera. If the agent goes out of view, then bring it back in */

        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (!onScreen)
        {
            /* Goes off the top edge of the screen */
            if (screenPoint.y > 1) transform.position = new Vector3(transform.position.x, -mainCamera.orthographicSize + cameraOffSet, transform.position.z);
            /* Goes off the bottom edge of the screen */
            else if (screenPoint.y < 0) transform.position = new Vector3(transform.position.x, mainCamera.orthographicSize - cameraOffSet, transform.position.z);
            /* Goes off the right edge of the screen */
            else if (transform.position.x > 1) transform.position = new Vector3((-mainCamera.orthographicSize * mainCamera.aspect) + cameraOffSet, transform.position.y, transform.position.z);
            /* Goes off the left edge of the screen */
            else if (transform.position.x < 0) transform.position = new Vector3((mainCamera.orthographicSize * mainCamera.aspect) - cameraOffSet, transform.position.y, transform.position.z);

            /* Reset the location of the agent to the new location after it goes out of the view of the camera */
            location = transform.position;
            onScreen = false;
        }
    }
}
