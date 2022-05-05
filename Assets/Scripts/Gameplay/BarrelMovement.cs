using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BarrelMovement : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float frictionConstant;
    [SerializeField]
    float kickRadius;

    bool closeToPlayer;

    PlayerPowers playerPowers;

    Rigidbody2D rb;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerPowers = player.GetComponent<PlayerPowers>();

        rb = GetComponent<Rigidbody2D>();

        closeToPlayer = false;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(playerPowers.transform.position, transform.position);
        if (distanceToPlayer <= kickRadius && !closeToPlayer)
        {
            closeToPlayer = true;
            playerPowers.SetClosestBarrel(this);

            Debug.Log("Set closest Barrel");
        }
        else if (distanceToPlayer > kickRadius && closeToPlayer)
        {
            closeToPlayer = false;
            playerPowers.SetClosestBarrel(null);

            Debug.Log("removed closest Barrel");
        }

        if(rb.velocity != Vector2.zero)
        {
            rb.AddForce(-rb.velocity * frictionConstant);
        }
    }

    public void Kick(Vector3 velocity)
    {
        rb.AddForce(velocity, ForceMode2D.Impulse);
        closeToPlayer = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.0f, 1.0f, .0f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, kickRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If the collision gameobject is a bullet, explode
        if (collision.gameObject.CompareTag("playerBullet"))
        {
            Debug.Log("Explode!");
            Destroy(gameObject);
        }


    }
}
