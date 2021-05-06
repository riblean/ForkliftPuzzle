using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkliftArmAnimation : MonoBehaviour
{
    public Transform Arm;
    public Transform Frame;
    public Transform FrameB;
    public Transform PickUpPointLow;
    public Transform PickUpPointUp;

    public float Length = 1.2f;

    public float Speed = 1.0f;
    public Quaternion Angle_A = Quaternion.identity;
    public Quaternion Angle_B = Quaternion.identity;
    public float AngleSpeed = 0.5f;

    public float X = 0;
    public float Y = 0;

    public int TargetHight = 0;
    public float[] TargetHightFloat = new float[] { 0.0f, 0.5f, 1.0f };

    public int TargetAngle = 0;
    public float[] TargetAngleFloat = new float[] { 0f, 0.3f };

    public int LowType = -1;
    public GameObject LowObj;
    public int HighType = -1;
    public GameObject HighObj;

    void Start()
    {
    }

    void Update()
    {
        X = Fn.Limit(X + Fn.Limit(TargetHightFloat[TargetHight] - X,
            Speed * Time.deltaTime, - Speed * Time.deltaTime), Length, 0);
        Y = Fn.Limit(Y + Fn.Limit(TargetAngleFloat[TargetAngle] - Y,
            AngleSpeed * Time.deltaTime, -AngleSpeed * Time.deltaTime), 1f, 0f);

        Arm.localPosition = Vector3.up * X;
        Frame.localRotation = Quaternion.Lerp(Angle_A, Angle_B, Y);
        FrameB.localPosition = Vector3.up * X * 0.5f;
    }

    public bool PickUp(int _type, GameObject _obj)
    {
        if(HighType == -1)
        {
            if(LowType == -1)
            {
                LowType = _type;
                LowObj = _obj;
                LowObj.transform.SetParent(PickUpPointLow);
                LowObj.transform.localPosition = new Vector3();
            }
            else
            {
                LowObj.transform.SetParent(PickUpPointUp);
                LowObj.transform.localPosition = new Vector3();
                HighType = LowType;
                HighObj = LowObj;

                LowType = _type;
                LowObj = _obj;
                LowObj.transform.SetParent(PickUpPointLow);
                LowObj.transform.localPosition = new Vector3();
            }
            return true;
        }
        return false;
    }

    public int DropDown()
    {
        if(LowType != -1)
        {
            int _temp = LowType;
            Destroy(LowObj);
            LowType = HighType;
            LowObj = HighObj;
            if(LowObj != null)
            {
                LowObj.transform.SetParent(PickUpPointLow);
                LowObj.transform.localPosition = new Vector3();
            }
            HighType = -1;
            HighObj = null;
            return _temp;
        }
        return -1;
    }
}
