using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    float bulletSpeed;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float bulletFireSpeed;

    float lastFireTime;

    PlayerPowers powers;

    // Start is called before the first frame update
    void Start()
    {
        powers = GetComponent<PlayerPowers>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if(Input.GetAxisRaw("Fire1") > 0.25f && lastFireTime < Time.unscaledTime)
        {
            FireBullet();
            lastFireTime = Time.unscaledTime + bulletFireSpeed;
        }
    }

    void FireBullet()
    {
        GameObject newBulletObj = Instantiate(bulletPrefab, transform.position, transform.rotation);

        Vector2 test = transform.right;
        test *= bulletSpeed;

        newBulletObj.GetComponent<Rigidbody2D>().AddForce(test, ForceMode2D.Impulse);
        newBulletObj.GetComponent<BulletHandler>().SetTimestop(powers.GetTimestop());
    }
}
