using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    Slider bossHealth;

    private void Start()
    {
        if (Instance == null)
            Instance = this;

        bossHealth.gameObject.SetActive(false);
    }

    public void SetBossMaxHealth(float max)
    {
        bossHealth.maxValue = max;
        bossHealth.value = max;
    }

    //Used for updating the bosses current health (NOT max health)
    public void SetBossHealth(float current)
    {
        //Disables healthbar when value is less than 0. Re-enables if it is greater
        if (bossHealth.gameObject.activeSelf && current <= 0.0f)
            bossHealth.gameObject.SetActive(false);
        else if (!bossHealth.gameObject.activeSelf && current > 0.0f)
            bossHealth.gameObject.SetActive(true);

        bossHealth.value = current;
    }

}
