using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSkip : MonoBehaviour
{
    [SerializeField]
    float pitchEffectTime = 1.0f;

    AudioSource src;
    private void Start()
    {
        src = GetComponent<AudioSource>();
    }

    //ONLY USE ON DECOMPRESSED AUDIO
    public void StartSkipTime(float amount)
    {
        LeanTween.cancel(gameObject);

        //Eases the pitch from 1 to zero, then sets it back to 1 in the end;
        LeanTween.value(gameObject, 1.0f, 0.0f, pitchEffectTime).setEaseInQuad().setIgnoreTimeScale(true).setOnUpdate((value) => 
        {
            src.pitch = value;
        }
        ).setOnComplete(() =>
        {
            src.pitch = 1.0f;
            SkipTime(amount);
        }
        );
    }

    void SkipTime(float amount)
    {
        if (src.loop && src.time + amount >= src.clip.length)
        {
            //Loop time back around
            src.time = src.time - src.clip.length + amount;
        }

        //Stop from an invalid position being set
        else if (src.time + amount < src.clip.length)
        {
            src.time += amount;
        }
    }
}
