using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    Slider bossHealth;
    [SerializeField]
    Image playerHealthFirst;
    [SerializeField]
    Image playerHealthSecond;
    [SerializeField]
    Image playerHealthThird;



    private void Start()
    {
        if (Instance == null)
            Instance = this;

        bossHealth.gameObject.SetActive(false);
    }

    public void SetBossMaxHealthUI(float max)
    {
        bossHealth.maxValue = max;
        bossHealth.value = max;
    }

    //Used for updating the bosses current health (NOT max health)
    public void SetBossHealthUI(float current)
    {
        //Disables healthbar when value is less than 0. Re-enables if it is greater
        if (bossHealth.gameObject.activeSelf && current <= 0.0f)
            bossHealth.gameObject.SetActive(false);
        else if (!bossHealth.gameObject.activeSelf && current > 0.0f)
            bossHealth.gameObject.SetActive(true);

        bossHealth.value = current;
    }

    public void RemovePlayerHealthUI(int index)
    {
        switch (index)
        {
            case 0:
                playerHealthFirst.enabled = false;
                break;

            case 1:
                playerHealthSecond.enabled = false;
                break;

            case 2:
                playerHealthThird.enabled = false;
                break;
            default:
                Debug.LogWarning("Index out of bounds in RemovePlayerHealthUI");
                break;
        }
    }
    public void ResetPlayerHealthUI()
    {
        playerHealthFirst.enabled = true;
        playerHealthSecond.enabled = true;
        playerHealthThird.enabled = true;
    }

}
