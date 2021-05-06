using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Transform Target;
    public float RotateSpeed = 60.0f;

    void LateUpdate()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            transform.rotation *= Quaternion.AngleAxis(RotateSpeed * Time.deltaTime, Vector3.up);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.rotation *= Quaternion.AngleAxis(-RotateSpeed * Time.deltaTime, Vector3.up);
        }
        Camera.main.transform.LookAt(Target);
    }
}
