using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : SteeringBehaviour
{
    [SerializeField]
    float distanceToCircle;
    [SerializeField]
    float circleRadius;

    public bool wanderEnabled = true;

    public override Vector3 CalculateSteeringForce()
    {
        if (wanderEnabled)
        {
            float orientation = rb.rotation * Mathf.Deg2Rad;
            Vector3 circlePoint = transform.position + new Vector3(Mathf.Cos(-orientation), 0.0f, Mathf.Sin(-orientation)) * distanceToCircle;

            float angle = Random.Range(0, Mathf.PI * 2);
            float x = Mathf.Sin(angle) * circleRadius;
            float z = Mathf.Cos(angle) * circleRadius;

            Vector3 wanderTarget = new Vector3(circlePoint.x + x, transform.position.y, circlePoint.z + z);

            Debug.DrawLine(wanderTarget, circlePoint);

            Vector3 dir = wanderTarget - transform.position;
            Vector3 desiredVelo = dir.normalized * maxSpeed;

            Vector3 steer = Vector3.ClampMagnitude(desiredVelo - (Vector3)rb.velocity, maxForce);

            return steer / mass;
        }
        else
        {
            //Slowly stops the wanderer
            return -rb.velocity;
        }
    }
}
