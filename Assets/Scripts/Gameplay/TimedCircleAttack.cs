using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedCircleAttack : MonoBehaviour
{
    [Header("Game objects")]
    [SerializeField]
    GameObject circle;
    [SerializeField]
    GameObject visuals;

    [Header("Variables")]
    [SerializeField]
    float radius;
    [SerializeField]
    float timeToExplosion;
    [SerializeField]
    LayerMask damageMask;
    [SerializeField]
    float damageAmount;

    [Header("Effects")]
    [SerializeField]
    float screenShakeIntensity = 8.0f;
    [Range(0.01f, 5.0f)]
    [SerializeField]
    float screenShakeTime = 0.4f;

    private void Start()
    {
        SetVisualsScale(radius);
        circle.transform.localScale = Vector3.zero;
    }

    public void StartAttack()
    {
        LTDescr scaleTween = circle.transform.LeanScale(Vector3.one, timeToExplosion);

        scaleTween.setEaseLinear();
        scaleTween.setOnComplete(() =>
        {
            Explode();
        });
    }

    void Explode()
    {
        Debug.Log("Timed Attack Explode!");
        CameraShake.Instance.Shake(screenShakeIntensity, screenShakeTime);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, damageMask);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.gameObject.TryGetComponent(out Health health))
            {
                health.TakeDamage(damageAmount);
            }
        }

        Destroy(gameObject);
    }

    void SetVisualsScale(float scale)
    {
        visuals.transform.localScale = new Vector3(scale * 2.0f, scale * 2.0f, 1.0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, radius);

        if (visuals != null)
            SetVisualsScale(radius);
    }
}
