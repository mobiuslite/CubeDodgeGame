using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : SteeringBehaviour
{
    [Space(10)]
    [SerializeField]
    float minDistance = 5.0f;

    public override Vector3 CalculateSteeringForce()
    {
        Vector3 dir = transform.position - target.position;

        if (dir.magnitude < minDistance)
        {

            Vector3 desiredVelo = dir.normalized * maxSpeed;

            Vector3 steer = Vector3.ClampMagnitude(desiredVelo - (Vector3)rb.velocity, maxForce);

            steer /= mass;

            return steer;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
