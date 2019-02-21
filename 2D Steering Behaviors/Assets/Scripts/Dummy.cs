using UnityEngine;

public class Dummy : SteeringBehavior
{
    Vector3 futurelocation;

    new void Start()
    {
        base.Start();
        velocity = transform.up * 10;
    }

    void Update()
    {
        futurelocation = location + (velocity.normalized * 10);

        WrapAroundCameraView();
        Steer(futurelocation);
        ApplySteeringToMotion();
    }
}