using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ContainerMap", menuName = "my/ContainerMap")]
public class ContainerMap : ScriptableObject
{
    public ContainerX[] LowZ;
    public ContainerX[] HighZ;

    public int GetNumber(Vector3Int _pos)
    {
        if(_pos.y == 0)
        {
            return LowZ[_pos.z].X[_pos.x];
        }
        return HighZ[_pos.z].X[_pos.x];
    }
}

[System.Serializable]
public class ContainerX
{
    public int[] X;
}
