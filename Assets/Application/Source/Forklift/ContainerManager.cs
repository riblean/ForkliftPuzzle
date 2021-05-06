using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerManager : MonoBehaviour
{
    public ContainerMap Map;
    public GameObject[] Prefabs;
    public GameObject Floor;
    public Vector2Int MapSize = new Vector2Int(10, 10);
    public float Size = 1.3f;
    public float Hight = 1.65f;
    public ContainerInstance[] Containers;

    public int[,,] NumberMap;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        Containers = new ContainerInstance[16*16*2];
        yield return null;
        MapSize = new Vector2Int(Map.LowZ.Length, Map.LowZ[0].X.Length);
        NumberMap = new int[MapSize.x, 2, MapSize.y];
        for (int x = 2; x < MapSize.x - 2; x++)
        {
            for (int y = 2; y < MapSize.y - 2; y++)
            {
                Transform _obj = Instantiate(Floor, transform).transform;
                _obj.position = WorldPosition(new Vector3Int(x, 0, y));
            }
            yield return null;
        }
        for (int x = 0; x < MapSize.x; x++)
        {
            for (int z = 0; z < MapSize.y; z++)
            {
                NumberMap[x, 0, z] = -1;
                AddContainer(new Vector3Int(x, 0, z), Map.LowZ[z].X[x]);
                NumberMap[x, 1, z] = -1;
                AddContainer(new Vector3Int(x, 1, z), Map.HighZ[z].X[x]);
            }
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if(_type < 0 || _type >= Prefabs.Length) { return; }
        int _cur = -1;
        for(int i = 0; i < Containers.Length; i++)
        {
            if(Containers[i] == null)
            {
                _cur = i;
                break;
            }
        }
        if(_cur == -1) { return; }

        if(NumberMap[_pos.x, _pos.y, _pos.z] != -1) { Debug.Log("すでにあります。",transform); return; }
        NumberMap[_pos.x, _pos.y, _pos.z] = _cur;

        Containers[_cur] = new ContainerInstance();
        Containers[_cur].Type = _type;
        Containers[_cur].Position = _pos;
        if(_pos.x > MapSize.x - 3 || _pos.z > MapSize.y - 3 || _pos.x < 2 || _pos.z < 2)
        {
            if(_type == 0)
            {
                Containers[_cur].Type = 0;
                return;
            }
        }
        Containers[_cur].Tra = Instantiate(Prefabs[_type], transform).transform;
        Containers[_cur].Tra.position = WorldPosition(new Vector3Int(_pos.x, _pos.y, _pos.z));
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
        NumberMap[Containers[_cur].Position.x, Containers[_cur].Position.y, Containers[_cur].Position.z] = -1;
        Containers[_cur] = null;
    }
}

public class ContainerInstance
{
    public Transform Tra;
    public int Type = 0;
    public Vector3Int Position;
}