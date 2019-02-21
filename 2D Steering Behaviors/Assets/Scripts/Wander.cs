using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : SteeringBehavior
{
    [SerializeField]
    float _timeBeforeNewDirectionIsChosen;
    float timeBeforeNewDirectionIsChosen;

    [Range(5, 10)]
    public int radius;

    [Range(10, 20)]
    public int maxSeeAhead;

    [SerializeField]
    Transform target;

    Vector2 center, dir;

    new void Start()
    {
        base.Start();

        //choose a random direction 
        dir = Random.insideUnitCircle.normalized;

        //set the timer before a new direction is chosen
        timeBeforeNewDirectionIsChosen = _timeBeforeNewDirectionIsChosen;
    }

    void Update()
    {
        //count down the timer every frame
        timeBeforeNewDirectionIsChosen -= Time.deltaTime;

        //the center is the circle's center that is away from the agent
        center = (Vector2)transform.position + ((Vector2)transform.up * maxSeeAhead);

        if (timeBeforeNewDirectionIsChosen <= 0)
        {
            //pick a new direction to move in
            dir = Random.insideUnitCircle.normalized;
            //reset the timer
            timeBeforeNewDirectionIsChosen = _timeBeforeNewDirectionIsChosen;
        }

        //place the target's position based on the calculated values of 'center' & 'dir'
        //to place the target at a random point within the circle's circumference,
        target.position = center + (dir * radius);

        //steer towards the target's position
        WrapAroundCameraView();
        Steer(target.position);
        ApplySteeringToMotion();

        Debug.DrawRay(center, (Vector2)target.position - center, Color.green);
    }
}
