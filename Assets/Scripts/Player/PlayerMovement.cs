using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles player movement, dashing, and collision
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    [Range(0.001f, 0.1f)]
    float smoothing;
    [SerializeField]
    LayerMask damageMask;
    [Space]
    [SerializeField]
    LayerMask knockbackMask;
    [SerializeField]
    float knockbackAmount;

    [Header("Abilities (Don't update on runtime)")]
    [SerializeField]
    [Range(0.001f, 1.0f)]
    float dashLength;
    [SerializeField]
    float dashMultiplier;
    [SerializeField]
    float dashCooldown;
    [SerializeField]
    float dashTrailLength;

    Controller controller;
    BoxCollider2D boxCollider;

    TrailRenderer dashTrail;

    Vector2 velocity;

    CooldownAbility dash;

    void Start()
    {
        controller = GetComponent<Controller>();
        boxCollider = GetComponent<BoxCollider2D>();
        dashTrail = GetComponent<TrailRenderer>();

        velocity = Vector2.zero;

        dash = new CooldownAbility(dashCooldown, dashLength);

        dash.OnAbilityEnter += (sender, args) => DisableCollision();
        dash.OnAbilityEnter += (sender, args) => controller.UseDashMask();
        dash.OnAbilityEnter += (sender, args) => dashTrail.time = dashTrailLength;

        dash.OnAbilityExit += (sender, args) => EnableCollision();
        dash.OnAbilityExit += (sender, args) => controller.UseRegularMask();
        dash.OnAbilityExit += (sender, args) => dashTrail.time = 0.0f;
    }

    private void Update()
    {
        Vector2 movementDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


        if (dash.CanUseAbility() && Input.GetKey(KeyCode.Space) && movementDir != Vector2.zero)
        {
            dash.Use();
        }

        movementDir.Normalize();
        movementDir *= dash.AbilityIsActive() ? speed * dashMultiplier : speed;

        float veloXSmoothing = 0.0f;
        float veloYSmoothing = 0.0f;

        float usedSmoothing = dash.AbilityIsActive() ? 0.0f : smoothing;

        velocity.x = Mathf.SmoothDamp(velocity.x, movementDir.x, ref veloXSmoothing, usedSmoothing, Mathf.Infinity, Time.unscaledDeltaTime);
        velocity.y = Mathf.SmoothDamp(velocity.y, movementDir.y, ref veloYSmoothing, usedSmoothing, Mathf.Infinity, Time.unscaledDeltaTime);

        controller.Move(velocity * Time.unscaledDeltaTime);
        //transform.Translate(velocity * Time.unscaledDeltaTime, Space.World);
        dash.Update(Time.unscaledDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!dash.AbilityIsActive())
        {
            if (LayerTools.IsInLayerMask(collision.gameObject, damageMask))
            {
                Debug.Log("Take Damage!");
            }

            if (LayerTools.IsInLayerMask(collision.gameObject, knockbackMask))
            {
                Vector2 dir = transform.position - collision.gameObject.transform.position;
                Knockback(dir.normalized * knockbackAmount);
            }
        }
    }

    public void Knockback(Vector2 velocity)
    {
        this.velocity = velocity;
    }

    void EnableCollision()
    {
        boxCollider.isTrigger = false;
    }

    void DisableCollision()
    {
        boxCollider.isTrigger = true;
    }
}
