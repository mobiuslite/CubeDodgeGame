using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraShake : MonoBehaviour
{

    public static CameraShake Instance { get; private set; }

    CinemachineVirtualCamera vCam;
    CinemachineBasicMultiChannelPerlin perlin;

    float elapsedShakeTime;
    float shakeTime;

    float intensity;

    bool shaking;
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        vCam = GetComponent<CinemachineVirtualCamera>();
        perlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        elapsedShakeTime = 0.0f;
        shaking = false;
    }

    private void Update()
    {
        if (shaking)
        {
            elapsedShakeTime += Time.unscaledDeltaTime;
            if(elapsedShakeTime >= shakeTime)
            {
                elapsedShakeTime = 0.0f;
                shaking = false;

                perlin.m_AmplitudeGain = 0.0f;
            }
            else
            {
                perlin.m_AmplitudeGain = Mathf.Lerp(intensity, 0.0f, elapsedShakeTime / shakeTime);
            }       
        }
    }

    public void Shake(float intensity, float time)
    {
        shakeTime = time;
        this.intensity = intensity;
        elapsedShakeTime = 0.0f;

        shaking = true;
    }
}
