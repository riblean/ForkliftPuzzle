using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class MapData : MonoBehaviour
{
    public static MapData Instance{get; private set;}

    [SerializeField] Type[][][] data;
    public bool Complete = false;
    [TextArea(3, 99)]
    public string csvString = "";
    public Vector3Int Size = new Vector3Int(0, 2, 0);
    public string StageName = "";

    void Awake()
    {
        Instance = this;
        if(csvString != "")
        {
            StartCoroutine(Load(csvString));
        }
    }

    public IEnumerator WebLoad(string _url)
    {
        // UnityWebRequest www = UnityWebRequest.Get(_url);
        // yield return www.SendWebRequest();
        // if(www.isNetworkError || www.isHttpError)
        // {
        //     Debug.Log(www.error);
        //     yield break;
        // }

        // yield return Load(www.downloadHandler.text);
        yield return Load(_url);
    }

    public IEnumerator Load(string _data)
    {
        string[] _tempA = _data.Split('\n');
        data = new Type[_tempA.Length][][];
        Size.x = _tempA.Length;
        for(int i = 0; i < _tempA.Length; i++)
        {
            string[] _tempB = _tempA[i].Split(',');
            Size.z = Mathf.Max(Size.z, _tempB.Length);
            data[i] = new Type[_tempB.Length][];
            for(int j = 0; j < _tempB.Length; j++)
            {
                string[] _tempC = _tempB[j].Split(':');
                data[i][j] = new Type[2];
                data[i][j][0] = (Type)Enum.Parse(typeof(MapData.Type), _tempC[0]);
                data[i][j][1] = (Type)Enum.Parse(typeof(MapData.Type), _tempC[1]);
            }
            yield return null;
        }
        Complete = true;
    }

    public Type Get(Vector3Int _pos)
    {
        return Get(_pos.x, _pos.y, _pos.z);
    }

    public Type Get(int _x, int _y, int _z)
    {
        if(_x < 0 || _x > data.Length - 1 || _z < 0 || _z > data[_x].Length - 1 || _y < 0 || _y > 1){ return Type.Empty; }
        return data[_x][_z][_y];
    }





    public enum Type
    {
        Empty,
        Floor,
        StartNorth,
        StartEast,
        StartSouth,
        StartWest,
        Em_A,
        Em_B,
        Em_C,
        Em_D,
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        End_A,
        End_B,
        End_C,
        End_D,
        End_E,
        End_F,
        End_G,
        End_H,
        End_I,
        End_J,
    }
}
