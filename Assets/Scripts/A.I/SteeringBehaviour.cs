using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SteeringBehaviour : MonoBehaviour
{
    [SerializeField]
    protected Transform target;
    [SerializeField]
    protected float weight = 1.0f;
    [SerializeField]
    protected float mass = 1.0f;

    [SerializeField]
    protected float maxSpeed; //How fast does the agent move to/away from target
    [SerializeField]
    protected float maxForce; // Caps how much steering force is applied
    [SerializeField]
    protected float maxTurnSpeed;

    protected Rigidbody2D rb;

    protected float ratio = 1.0f;

    protected bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if(rb == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
            rb = GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            Vector2 steer = CalculateSteeringForce();
            ApplySteer(steer);
        }
    }

    //Enables and disables the steering behaviour
    public void SetActive(bool state)
    {
        active = state;
    }

    public virtual Vector3 CalculateSteeringForce()
    {
        return Vector3.zero;
    }

    public virtual void SetTarget(Transform t)
    {
        this.target = t;
    }

    public void ApplySteer(Vector2 steer)
    {
        rb.velocity += steer * Time.deltaTime * ratio;

        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public void LookWhereYouAreGoing()
    {
        Vector3 dir = rb.velocity;

        LookAtDirection(dir);
    }

    public void LookAtDirection(Vector3 dir)
    {
        dir.Normalize();

        if(dir.sqrMagnitude > float.Epsilon)
        {
            float rotY = -1f * Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            float rotAmount = Mathf.LerpAngle(transform.rotation.eulerAngles.y, rotY, maxTurnSpeed * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0, rotAmount, 0);
        }
    }
}
