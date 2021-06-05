using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public static CameraMover Instance{ get; private set;}
    public Transform Target;
    public float RotateSpeed = 60.0f;

    public float InputValue = 0;

    public void Awake()
    {
        Instance = this;
    }
    
    void LateUpdate()
    {
        transform.rotation *= Quaternion.AngleAxis( InputValue  * RotateSpeed * Time.deltaTime, Vector3.up);
        Camera.main.transform.LookAt(Target);
    }
}
