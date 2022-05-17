using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICooldownViewer : MonoBehaviour
{
    [SerializeField]
    PowerType powerType;
    [SerializeField]
    bool transparentOnFinish;

    Image image;
    CooldownAbility ability;
    
    private void Start()
    {
        image = GetComponent<Image>();
        image.enabled = false;

        PlayerPowers powers = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPowers>();
        if(powerType == PowerType.Timestop)
        {
            ability = powers.GetTimestop();
        }

        if (transparentOnFinish)
        {
            ability.OnCooldownEnter += (sender, args) => EnableIcon();
            ability.OnCooldownExit += (sender, args) => DisableIcon();
        }
    }

    private void Update()
    {
        if (ability.CooldownIsActive())
        {
            image.fillAmount = ability.CooldownValue();
        }
    }

    void EnableIcon()
    {
        image.fillAmount = ability.CooldownValue();
        image.enabled = true;
    }

    void DisableIcon()
    {
        image.enabled = false;
    }
}
