using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : SteeringBehavior
{
    [SerializeField]
    float radius;

    Vector3 futureLocation;

    bool flockingOn;

    new void Start()
    {
        base.Start();

        velocity = Random.insideUnitCircle * 10;
        maxSpeed = _maxSpeed = 20;
        maxForce = Random.Range(0.8f, 2);
        flockingOn = true;
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            flockingOn = !flockingOn;

            if (!flockingOn) velocity = Random.insideUnitCircle * 5;
        }

        futureLocation = location + (velocity.normalized * 5);

        WrapAroundCameraView();

        if (flockingOn)
        {
            Align();
            Separate();
            Cohesion();
        }
        else Steer(futureLocation);

        ApplySteeringToMotion();
    }

    private void Separate()
    {
        Vector3 separationForce = Vector3.zero;
        int count = 0;

        foreach (Transform a in transform.parent)
        {
            float d = Vector3.Distance(location, a.position);

            if (d > 0 && d < radius)
            {
                Vector3 diff = location - a.position;
                diff.Normalize();
                diff /= d;
                separationForce += diff;
                count++;
            }
        }

        if (count > 0) separationForce /= count;

        if (separationForce.magnitude > 0)
        {
            separationForce.Normalize();
            separationForce *= maxSpeed;
            separationForce = Vector3.ClampMagnitude(separationForce - velocity, maxForce);
            separationForce *= 5;
            ApplyForce(separationForce);
        }
    }

    private void Align()
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (Transform a in transform.parent)
        {
            float d = Vector3.Distance(transform.position, a.position);

            if (d > 0 && d < 20)
            {
                sum += a.GetComponent<Flocking>().GetVelocity;
                count++;
            }
        }

        if (count > 0)
        {
            sum /= count;
            sum.Normalize();
            sum *= maxSpeed;
            Vector3 alignmentForce = sum - velocity;
            alignmentForce = Vector3.ClampMagnitude(alignmentForce, maxForce);
            ApplyForce(alignmentForce);
        }
    }

    private void Cohesion()
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (Transform a in transform.parent)
        {
            float d = Vector3.Distance(location, a.position);

            if (d > 0 && d < radius)
            {
                sum += a.position;
                count++;
            }
        }

        if (count > 0)
        {
            sum /= count;
            Steer(sum);
        }
    }

    public Vector3 GetVelocity
    {
        get
        {
            return velocity;
        }
    }
}
