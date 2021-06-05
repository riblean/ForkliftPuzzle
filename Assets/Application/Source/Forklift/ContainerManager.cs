using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerManager : MonoBehaviour
{
    public MapData mapData;
    public GameObject[] Prefabs;
    public GameObject Floor;
    public float Size = 1.3f;
    public float Hight = 1.65f;
    public ContainerInstance[] Containers;

    public int[,,] NumberMap;
    // -1 Floor
    // -2 Empty
    // 0~ CheckContainer

    public bool Active = false;
    
    public IEnumerator Load()
    {
        while(MapData.Instance == null)
        {
            yield return null;
        }
        while(!MapData.Instance.Complete)
        {
            yield return null;
        }
        Containers = new ContainerInstance[16*16*2];
        yield return null;
        NumberMap = new int[mapData.Size.x, 2, mapData.Size.z];
        for (int x = 0; x < mapData.Size.x; x++)
        {
            for (int z = 0; z < mapData.Size.z; z++)
            {
                NumberMap[x, 0, z] = -2;
                AddContainer(new Vector3Int(x, 0, z), (int)mapData.Get(x, 0, z));
                NumberMap[x, 1, z] = -2;
                AddContainer(new Vector3Int(x, 1, z), (int)mapData.Get(x, 1, z));
            }
            yield return null;
        }
        Active = true;
    }

    public Vector3 WorldPosition(Vector3Int _pos)
    {
        return new Vector3((float)_pos.x * Size, (float)_pos.y * Hight, (float)_pos.z * Size);
    }

    public Quaternion WorldDirection(int _dir)
    {
        return Quaternion.AngleAxis(90.0f * _dir, Vector3.up);
    }
    public Vector3Int DirectionInt(int _dir)
    {
        switch (_dir)
        {
            case 0:
                return new Vector3Int(0, 0, 1);
            case 1:
                return new Vector3Int(1, 0, 0);
            case 2:
                return new Vector3Int(0, 0, -1);
            case 3:
                return new Vector3Int(-1, 0, 0);
        }
        return Vector3Int.zero;
    }
    public Vector3 Direction(int _dir)
    {
        switch (_dir)
        {
            case 0:
                return new Vector3(0, 0, 1);
            case 1:
                return new Vector3(1, 0, 0);
            case 2:
                return new Vector3(0, 0, -1);
            case 3:
                return new Vector3(-1, 0, 0);
        }
        return Vector3.zero;
    }

    public void AddContainer( Vector3Int _pos, int _type = 0)
    {
        if(_type > 0 && _pos.y == 0)
        {
            Transform _obj = Instantiate(Floor, transform).transform;
            _obj.position = WorldPosition(new Vector3Int(_pos.x, 0, _pos.z));
        }
        if(_type - 10 < 0 || _type - 10 >= Prefabs.Length) { 
            if(_type > 0)
            {
                NumberMap[_pos.x, _pos.y, _pos.z] = -2;
            }
            return; 
        }

        if(_type - 10 >= 0 && _type -10 < Prefabs.Length)
        {
            int _cur = -2;
            for(int i = 0; i < Containers.Length; i++)
            {
                if(Containers[i] == null)
                {
                    _cur = i;
                    break;
                }
            }
            if(_cur == -2) { return; }
            NumberMap[_pos.x, _pos.y, _pos.z] = _cur;

            Containers[_cur] = new ContainerInstance();
            Containers[_cur].Type = _type;
            Containers[_cur].Position = _pos;
            Containers[_cur].Tra = Instantiate(Prefabs[_type - 10], transform).transform;
            Containers[_cur].Tra.position = WorldPosition(new Vector3Int(_pos.x, _pos.y, _pos.z));
        }
    }

    public void DeleteContainer( int _cur)
    {
        if(Containers[_cur] == null)
        {
            return;
        }

        if(Containers[_cur].Tra != null)
        {
            Destroy(Containers[_cur].Tra.gameObject);
        }

        if(Containers[_cur].Position.y == 0)
        {
            NumberMap[Containers[_cur].Position.x, Containers[_cur].Position.y, Containers[_cur].Position.z] = -1;
        }
        else
        {
            NumberMap[Containers[_cur].Position.x, Containers[_cur].Position.y, Containers[_cur].Position.z] = -2;
        }
        Containers[_cur] = null;
    }

    public bool GetCanMove(int _x, int _z)
    {
        int _typeD = (int)mapData.Get(_x, 0, _z);
        if( (_typeD > 0 && _typeD < 30) )
        {
            if(NumberMap[_x, 0, _z] < 0 && NumberMap[_x, 1, _z] < 0)
            {
                return true;
            }
        }
        return false;
    }

    public int GetContainer(int _x, int _y, int _z)
    {
        if(_x >= 0 && _x < mapData.Size.x && _z >= 0 && _z < mapData.Size.z && _y >= 0 && _y < 2)
        {
            
            return NumberMap[_x, _y, _z];
        }
        return -1;
    }
}

public class ContainerInstance
{
    public Transform Tra;
    public int Type = 0;
    public Vector3Int Position;
}