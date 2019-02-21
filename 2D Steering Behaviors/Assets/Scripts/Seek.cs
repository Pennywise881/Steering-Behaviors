using UnityEngine;

public class Seek : SteeringBehavior
{
    [SerializeField]
    Transform target;

    void Update()
    {
        Steer(target.position);
        ApplySteeringToMotion();
    }
}