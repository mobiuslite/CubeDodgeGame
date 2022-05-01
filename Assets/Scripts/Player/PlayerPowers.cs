using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowers : MonoBehaviour
{
    [SerializeField]
    float timestopLength;
    [SerializeField]
    float timestopCooldown;

    CooldownAbility timestop;

    private void Start()
    {
        timestop = new CooldownAbility(timestopCooldown, timestopLength);
        timestop.OnAbilityEnter += (sender, args) => StopTime();
        timestop.OnAbilityEnter += (sender, args) => PPManager.Instance.StartTimestopPP();

        timestop.OnAbilityExit += (sender, args) => StartTime();
        timestop.OnAbilityExit += (sender, args) => PPManager.Instance.StopTimestopPP();
    }

    private void Update()
    {
        if(Input.GetAxisRaw("Power") > 0.25f && timestop.CanUseAbility())
        {
            timestop.Use();
        }

        timestop.Update(Time.unscaledDeltaTime);
    }

    public CooldownAbility GetTimestop()
    {
        return timestop;
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
