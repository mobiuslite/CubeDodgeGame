using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraFollow : MonoBehaviour
{
    Transform originalFollow;
    CinemachineVirtualCamera vCam;

    Transform altCamFollow;

    [SerializeField]
    Transform bossTransform;

    bool followBoss = true;

    // Start is called before the first frame update
    void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        originalFollow = vCam.Follow;

        //Create new game object to use as a target for the camera when aiming.
        altCamFollow = new GameObject().GetComponent<Transform>();
        altCamFollow.gameObject.name = "Camera follow";

        vCam.Follow = altCamFollow;
    }

    // Update is called once per frame
    void Update()
    {
        //Update alt cam follow position while aim button is down
        if (Input.GetButtonDown("Fire2"))
        {
            //Switch between following mouse and boss
            followBoss = !followBoss;
        }

        Vector3 secondPoint;
        if (followBoss)
        {
            if (bossTransform != null)
                secondPoint = bossTransform.position;
            else
                secondPoint = originalFollow.position;
        }
        else
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;
            secondPoint = Camera.main.ScreenToWorldPoint(mousePos);
        }
        altCamFollow.position = (secondPoint + originalFollow.transform.position) / 2;
    }

    public void SetBossFollow(Transform boss)
    {
        bossTransform = boss;
    }
}
