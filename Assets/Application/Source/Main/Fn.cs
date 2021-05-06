using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fn
{
    public static float Limit(float X, float Max, float Min)
    {
        return Mathf.Min(Max, Mathf.Max(Min, X));
    }
    public static int Limit(int X, int Max, int Min)
    {
        return Mathf.Min(Max, Mathf.Max(Min, X));
    }

    public static Vector3 Bezier3(Vector3 _posA, Vector3 _dirA, Vector3 _posB, Vector3 _dirB, float x = 0)
    {
        return Vector3.Lerp(Vector3.Lerp(_posB, _posB + _dirB, x), Vector3.Lerp(_posA + _dirA, _posA, x), x);
    }
}
