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

    // Start is called before the first frame update
    void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        originalFollow = vCam.Follow;

        //Create new game object to use as a target for the camera when aiming.
        altCamFollow = new GameObject().GetComponent<Transform>();
        altCamFollow.gameObject.name = "Aiming camera follow";
    }

    // Update is called once per frame
    void Update()
    {
        //Update alt cam follow position while aim button is down
        if (Input.GetButton("Fire2"))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;

            altCamFollow.position = (Camera.main.ScreenToWorldPoint(mousePos) + originalFollow.transform.position) / 2;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            vCam.Follow = altCamFollow;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            vCam.Follow = originalFollow;
        }
    }
}
