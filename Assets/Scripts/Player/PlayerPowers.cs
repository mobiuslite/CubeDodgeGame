using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles player timestop
public class PlayerPowers : MonoBehaviour
{
    [Range(0.0f, 30.0f)]
    [SerializeField]
    float timestopLength;

    [Range(0.0f, 30.0f)]
    [SerializeField]
    float timestopCooldown;

    [Range(0.0f, 100.0f)]
    [SerializeField]
    float kickStrength;

    [SerializeField]
    float barrelLineLength;

    CooldownAbility timestop;

    BarrelMovement closestBarrel;
    LineRenderer barrelDirLine;
    TrailRenderer dashTrail;

    private void Start()
    {
        dashTrail = GetComponent<TrailRenderer>();

        timestop = new CooldownAbility(timestopCooldown, timestopLength);
        timestop.OnAbilityEnter += (sender, args) => StopTime();
        timestop.OnAbilityEnter += (sender, args) => PPManager.Instance.StartTimestopPP();
        timestop.OnAbilityEnter += (sender, args) => dashTrail.enabled = false;

        timestop.OnAbilityExit += (sender, args) => StartTime();
        timestop.OnAbilityExit += (sender, args) => PPManager.Instance.StopTimestopPP();
        timestop.OnAbilityExit += (sender, args) => dashTrail.enabled = true;

        closestBarrel = null;

        barrelDirLine = GetComponent<LineRenderer>();
        barrelDirLine.positionCount = 2;
    }

    private void Update()
    {
        bool powerButtonDown = Input.GetButtonDown("Power");

        if (powerButtonDown && timestop.CanUseAbility())
        {
            timestop.Use();
        }
        else if(powerButtonDown && timestop.AbilityIsActive())
        {
            timestop.CancelAbility();
        }

        if (Input.GetButtonDown("Kick") && closestBarrel != null)
        {
            Vector3 direction = (closestBarrel.transform.position - transform.position).normalized;

            closestBarrel.Kick(direction * kickStrength);
            closestBarrel = null;

            barrelDirLine.enabled = false;
        }

        if(closestBarrel != null)
        {
            Vector3 barrelPos = closestBarrel.transform.position;
            Vector3 dir = (barrelPos - transform.position).normalized;

            barrelDirLine.SetPosition(0, barrelPos);         
            barrelDirLine.SetPosition(1, barrelPos + (dir * barrelLineLength));
        }

        timestop.Update(Time.unscaledDeltaTime);
    }

    public CooldownAbility GetTimestop()
    {
        return timestop;
    }
    
    //Receives a barrel, and updates the closest barrel if it is closer.
    public void SetClosestBarrel(BarrelMovement barrel)
    {
        if(barrel == null)
        {
            closestBarrel = null;
            barrelDirLine.enabled = false;
            return;
        }

        if(closestBarrel == null)
        {
            closestBarrel = barrel;          
        }
        else if((transform.position - barrel.transform.position).sqrMagnitude < (transform.position - closestBarrel.transform.position).sqrMagnitude)
        {
            closestBarrel = barrel;
        }

        barrelDirLine.enabled = true;
    }

    public BarrelMovement GetClosestBarrel()
    {
        return closestBarrel;
    }

    void StopTime()
    {
        Time.timeScale = 0.0f;
    }

    void StartTime()
    {
        Time.timeScale = 1.0f;
    }
}
