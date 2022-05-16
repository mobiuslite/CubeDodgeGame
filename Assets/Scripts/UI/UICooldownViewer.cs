using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICooldownViewer : MonoBehaviour
{
    [SerializeField]
    PowerType powerType;

    Image image;
    CooldownAbility ability;
    
    private void Start()
    {
        image = GetComponent<Image>();

        PlayerPowers powers = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPowers>();
        if(powerType == PowerType.Timestop)
        {
            ability = powers.GetTimestop();
        }
    }

    private void Update()
    {
        if (ability.CooldownIsActive())
        {
            image.fillAmount = ability.CooldownValue();
        }
    }
}
