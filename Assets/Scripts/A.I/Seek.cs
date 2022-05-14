using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : SteeringBehaviour
{
    public override Vector3 CalculateSteeringForce()
    {
        Vector3 dir = target.position - transform.position;
        Vector3 desiredVelo = dir.normalized * maxSpeed;

        Vector3 steer = Vector3.ClampMagnitude(desiredVelo - (Vector3)rb.velocity, maxForce);
        return steer / mass;
    }


}
