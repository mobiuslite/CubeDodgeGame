using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//handles changing between different types of post-processing
public class PPManager : MonoBehaviour
{
    public static PPManager Instance { get; private set; }

    Volume currentPP;
    Volume timestopPP;

    [SerializeField]
    float switchLength;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        timestopPP = GetComponentInChildren<Volume>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PP"))
        {
            currentPP = other.gameObject.GetComponent<Volume>();
        }
    }

    public void StartTimestopPP()
    {
        LeanTween.cancel(gameObject);
        LeanTween.value(gameObject, 0.0f, 1.0f, switchLength)
            .setEaseInOutSine().setIgnoreTimeScale(true).setOnUpdate((value) => 
            {
                timestopPP.weight = value;
                currentPP.weight = 1.0f - value;       
            });
    }

    public void StopTimestopPP()
    {
        LeanTween.cancel(gameObject);

        LeanTween.value(gameObject, 0.0f, 1.0f, switchLength)
            .setEaseInOutSine().setIgnoreTimeScale(true).setOnUpdate((value) =>
            {
                currentPP.weight = value;
                timestopPP.weight = 1.0f - value;
            });
    }
}
