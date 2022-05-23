using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
public class BarrelMovement : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 2.0f)]
    float frictionConstant;
    [SerializeField]
    float kickRadius;
    [SerializeField]
    [Tooltip("The minimum magnitude needed to explode on contact")]
    float magnitudeForExplosion;
    [SerializeField]
    float damageAmount;
    [SerializeField]
    float damageRadius;
    [SerializeField]
    LayerMask damageMask;

    [Header("Effects")]
    [SerializeField]
    float screenShakeIntensity = 8.0f;
    [Range(0.01f, 5.0f)]
    [SerializeField]
    float screenShakeTime = 0.4f;
    [SerializeField]
    AudioClip explosionSound;

    bool closeToPlayer;

    PlayerPowers playerPowers;

    Rigidbody2D rb;
    LineRenderer storedEnergyLine;
    AudioSource audioSrc;
    SpriteRenderer spriteRenderer;

    Vector2 storedEnergy;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerPowers = player.GetComponent<PlayerPowers>();

        rb = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        storedEnergyLine = GetComponent<LineRenderer>();
        storedEnergyLine.positionCount = 2;

        closeToPlayer = false;

        storedEnergy = Vector2.zero;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(playerPowers.transform.position, transform.position);

        //On player enter radius
        if (distanceToPlayer <= kickRadius && !closeToPlayer)
        {
            closeToPlayer = true;
            playerPowers.SetClosestBarrel(this);
        }

        //On player exit radius
        else if (distanceToPlayer > kickRadius && closeToPlayer)
        {
            closeToPlayer = false;

            //Removes the closest barrel from the player if this is the closest barrel and it's leaving the player's radius
            if(playerPowers.GetClosestBarrel() == this)
                playerPowers.SetClosestBarrel(null);
        }

        //If the player is in the barrels radius but this isn't the closest barrel
        else if (distanceToPlayer <= kickRadius && playerPowers.GetClosestBarrel() != this)
        {
            //Update to make sure this isn't the closest barrel
            playerPowers.SetClosestBarrel(this);
        }

        //Render line to show direction and power on time stop
        if (playerPowers.GetTimestop().AbilityIsActive())
        {
            if (!storedEnergyLine.enabled)
            {
                storedEnergyLine.enabled = true;
            }

            Vector3 secondLinePoint = transform.position + ((Vector3)storedEnergy/10.0f);

            storedEnergyLine.SetPosition(0, transform.position);
            storedEnergyLine.SetPosition(1, secondLinePoint);
        }

        //Disables line renderer if ability is not active, and applies stored energy
        else if (storedEnergyLine.enabled)
        {
            storedEnergyLine.enabled = false;

            rb.AddForce(storedEnergy, ForceMode2D.Impulse);
            storedEnergy = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity != Vector2.zero)
        {
            rb.AddForce(-rb.velocity * frictionConstant);
        }
    }

    public void Kick(Vector3 velocity)
    {
        if (!playerPowers.GetTimestop().AbilityIsActive())
        {
            rb.AddForce(velocity, ForceMode2D.Impulse);
            closeToPlayer = false;
        }

        //Stores energy if time is stopped to be applied on time continue
        else
        {
            //sets velocity to zero
            //this is so that the barrel's stored energy line is actually accurate.
            rb.velocity = Vector3.zero;

            storedEnergy += (Vector2)velocity;
            closeToPlayer = false;
        }
    }


    private void Explode()
    {
        Debug.Log("Explode!");
        CameraShake.Instance.Shake(screenShakeIntensity, screenShakeTime);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, damageRadius, damageMask);

        foreach(Collider2D collider in hitColliders)
        {
            if(collider.gameObject.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(damageAmount);
            }
        }  

        storedEnergyLine.positionCount = 0;

        //removes barrel from player since it has exploded
        if (playerPowers.GetClosestBarrel() == this)
            playerPowers.SetClosestBarrel(null);

        //Disable visuals, and make sound.
        audioSrc.PlayOneShot(explosionSound);
        spriteRenderer.enabled = false;
        rb.simulated = false;

        Destroy(gameObject, explosionSound.length);

        this.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If the collision gameobject is a bullet or hits something with high magnitude, explode
        if (collision.gameObject.CompareTag("playerBullet") || rb.velocity.magnitude > magnitudeForExplosion)
        {
            Explode();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.0f, 1.0f, .0f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, kickRadius);

        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
