using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
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

    AudioSource audioSrc;

    [Header("Audio")]
    [SerializeField]
    AudioClip riser;
    [SerializeField]
    AudioClip explosion;

    private void Awake()
    {
        SetVisualsScale(radius);
        circle.transform.localScale = Vector3.zero;

        audioSrc = GetComponent<AudioSource>();
    }

    public void StartAttack()
    {
        LeanTween.cancel(gameObject);
        LTDescr scaleTween = LeanTween.value(gameObject, 0.0f, 1.0f, timeToExplosion).setOnUpdate((value) =>
        {
            circle.transform.localScale = new Vector3(value, value);
        });

        scaleTween.setEaseLinear();
        scaleTween.setOnComplete(() =>
        {
            Explode();
        });

        audioSrc.PlayOneShot(riser);
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

        //Make sound and disable visuals
        audioSrc.PlayOneShot(explosion);
        visuals.SetActive(false);

        //Delete after sound is finished
        Destroy(gameObject, explosion.length);
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
