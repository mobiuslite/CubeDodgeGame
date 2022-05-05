using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles player timestop
public class PlayerPowers : MonoBehaviour
{
    [SerializeField]
    float timestopLength;
    [SerializeField]
    float timestopCooldown;
    [SerializeField]
    float kickStrength;

    [SerializeField]
    float barrelLineLength;

    CooldownAbility timestop;

    BarrelMovement closestBarrel;
    LineRenderer barrelDirLine;

    private void Start()
    {
        timestop = new CooldownAbility(timestopCooldown, timestopLength);
        timestop.OnAbilityEnter += (sender, args) => StopTime();
        timestop.OnAbilityEnter += (sender, args) => PPManager.Instance.StartTimestopPP();

        timestop.OnAbilityExit += (sender, args) => StartTime();
        timestop.OnAbilityExit += (sender, args) => PPManager.Instance.StopTimestopPP();

        closestBarrel = null;

        barrelDirLine = GetComponent<LineRenderer>();
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

    void StopTime()
    {
        Time.timeScale = 0.0f;
    }

    void StartTime()
    {
        Time.timeScale = 1.0f;
    }
}
