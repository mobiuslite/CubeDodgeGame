using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour
{
    public GameObject target;

    [SerializeField]
    Vector3 offset;
    [SerializeField]
    [Range(0.001f, 0.1f)]
    float smoothAmount;

    private void Update()
    {
        Vector3 toPosition;
        if (Input.GetAxisRaw("Fire2") > 0.25f)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;

            toPosition = (Camera.main.ScreenToWorldPoint(mousePos) + target.transform.position) / 2;
            toPosition += offset;
        }
        else
        {
            toPosition = target.transform.position;
            toPosition += offset;      
        }

        Vector3 camSmoothOut = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, toPosition, ref camSmoothOut, smoothAmount, Mathf.Infinity, Time.unscaledDeltaTime);
    }
}
