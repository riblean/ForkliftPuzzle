using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckCtrl : MonoBehaviour
{
    public Transform DoorR;
    public Transform DoorL;
    public Transform DoorDR;
    public Transform DoorDL;

    public float DoorOpen = 0;
    public float DoorSpeed = 1f;
    public float DoorTarget = 0;
    public float DoorAngle = 90.0f;
    public float DoorDAngle = -90.0f;

    void Update()
    {
        DoorOpen = DoorOpen + Fn.Limit(DoorTarget - DoorOpen, DoorSpeed * Time.deltaTime, -DoorSpeed * Time.deltaTime);
        DoorR.rotation = Quaternion.AngleAxis(DoorAngle * DoorOpen, Vector3.forward);
        DoorL.rotation = Quaternion.AngleAxis(-DoorAngle * DoorOpen, Vector3.forward);
        DoorDR.rotation = Quaternion.AngleAxis(DoorDAngle * DoorOpen, Vector3.forward);
        DoorDL.rotation = Quaternion.AngleAxis(-DoorDAngle * DoorOpen, Vector3.forward);
    }
}