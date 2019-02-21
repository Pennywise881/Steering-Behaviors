using UnityEngine;

public class Arrive : SteeringBehavior
{
    [SerializeField]
    Transform target;
    float maxRadius;

    private void Awake()
    {
        //set the maxRadius to the user defined value from the Target.cs class
        if (target != null) maxRadius = target.gameObject.GetComponent<Target>().Radius;
    }

    void Update()
    {
        Steer(target.position);
        ApplySteeringToMotion();
    }

    protected override void Steer(Vector3 targetPosition)
    {
        Vector3 desiredVelocity = target.position - transform.position;
        desiredVelocity.Normalize();

        //calculate the distance between the target and the agent's current location
        float distanceFromTarget = Vector3.Distance(target.position, location);

        //if the agent is close to the target, reduce the desired velocity
        if (distanceFromTarget < maxRadius) desiredVelocity *= distanceFromTarget;

        //else move towards the target at maximum speed
        else desiredVelocity *= maxSpeed;

        Vector3 steer = Vector3.ClampMagnitude(desiredVelocity - velocity, maxForce);
        ApplyForce(steer);
    }
}
