using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    [SerializeField]
    float slowFlySpeed = 20f;
    [SerializeField]
    float distanceToSlowdown = 7.5f;
    [SerializeField]
    float lifeTime = 5.0f;

    CooldownAbility timestop;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (timestop != null)
        {
            if (timestop.AbilityIsActive())
            {
                float distance = GetRaycastObjectDistance();
                if (distance > .5f)
                    transform.Translate(Vector3.right * (distance > distanceToSlowdown ? slowFlySpeed : distance / distanceToSlowdown * slowFlySpeed) * Time.unscaledDeltaTime);
                else if (distance < -0.5f)
                    transform.Translate(Vector3.right * slowFlySpeed * Time.unscaledDeltaTime);
            }
        }
    }
    public void SetTimestop(CooldownAbility timestop)
    {
        this.timestop = timestop;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DeleteBullet();
    }

    private void DeleteBullet()
    {
        Destroy(gameObject);
    }

    private float GetRaycastObjectDistance()
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right, Mathf.Infinity,

            //Detects all layers except bullet, bullet casing, and player
            ~LayerMask.GetMask(new string[] { "PlayerBullet", "Player"}));

        Debug.DrawRay(transform.position, transform.right * raycast.distance, Color.green);

        if (raycast.collider != null)
            return raycast.distance;
        else
            return -1f;
    }

}
