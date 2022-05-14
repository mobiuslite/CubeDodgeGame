using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Approach : SteeringBehaviour
{
    [SerializeField]
    float maintainRadius = 5.0f;

    float curDistanceToTarget = 10.0f;

    public override Vector3 CalculateSteeringForce()
    {
        Vector3 dir = target.position - transform.position;
        Vector3 desiredVelo = dir.normalized * maxSpeed;

        curDistanceToTarget = dir.magnitude;
        if (curDistanceToTarget < maintainRadius)
        {
            desiredVelo *= -1.0f;
        }

        Vector3 steer = Vector3.ClampMagnitude(desiredVelo - (Vector3)rb.velocity, maxForce);
        return steer / mass;
    }

    public bool IsInFiringRange()
    {
        //Adding 0.25f just to give some leeway
        return curDistanceToTarget <= maintainRadius + 0.25f;
    }
    public float GetDistanceToTarget()
    {
        return curDistanceToTarget;
    }
}
