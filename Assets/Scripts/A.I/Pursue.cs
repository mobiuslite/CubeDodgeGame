using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : SteeringBehaviour
{
    [SerializeField]
    float maximumSteps = 1.0f;

    public override Vector3 CalculateSteeringForce()
    {      
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;
        float speed = rb.velocity.magnitude;

        float T = (speed <= distance / maximumSteps) ? maximumSteps : distance / speed;

        Vector3 futurePosition = target.position + target.gameObject.GetComponent<Rigidbody>().velocity * T;

        Vector3 dirToFuturePosition = futurePosition - transform.position;
        dirToFuturePosition = dirToFuturePosition.normalized;

        Vector3 desiredVelo = dirToFuturePosition * maxSpeed;
        Vector3 steer = Vector3.ClampMagnitude(desiredVelo - (Vector3)rb.velocity, maxForce);

        return steer / mass;
    }

    public void ChangeRatio(float r)
    {
        ratio = r;
    }
}
