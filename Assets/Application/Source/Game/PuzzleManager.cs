using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PuzzleManager : MonoBehaviour
{
    public ContainerManager CM;
    public PuzzleUI[] GoalPrefabs;

    public PuzzleUI[] uIs = new PuzzleUI[0];

    public Text stageName;
    public Text LargeMessage;
    public bool Active = false;

    IEnumerator Start()
    {
        while(!CM.Active)
        {
            yield return null;
        }

        stageName.text = CM.mapData.StageName;
        
        for (int x = 0; x < CM.mapData.Size.x; x++)
        {
            for (int z = 0; z < CM.mapData.Size.z; z++)
            {
                int _num = (int)CM.mapData.Get(x, 0, z);
                if(_num >= 21 && _num < 30)
                {
                    Array.Resize(ref uIs, uIs.Length + 1);
                    uIs[uIs.Length - 1] = Instantiate(GoalPrefabs[_num - 21], transform);
                    uIs[uIs.Length - 1].transform.position = CM.WorldPosition(new Vector3Int(x, 0, z));
                    uIs[uIs.Length - 1].Pos = new Vector2Int(x, z);
                    uIs[uIs.Length - 1].Type = _num - 11;

                    int _numB = (int)CM.mapData.Get(x, 1, z);
                    if(_numB >= 21)
                    {
                        uIs[uIs.Length - 1].GoalCount = 2;
                    }
                    else
                    {
                        uIs[uIs.Length - 1].GoalCount = 1;
                    }
                }
            }
            yield return null;
        }
        Active = true;
    }
    
    public void ContainerCheck()
    {
        if(!Active){return;}
        bool _Complete = true;
        for(int i = 0; i < uIs.Length; i++)
        {
            int _D = CM.GetContainer(uIs[i].Pos.x, 0, uIs[i].Pos.y);
            int _U = CM.GetContainer(uIs[i].Pos.x, 1, uIs[i].Pos.y);
            int _num = 0;

            if(_D >= 0 && CM.Containers[_D].Type - 1 == uIs[i].Type)
            {
                _num ++;
            }
            if(_U >= 0 && CM.Containers[_U].Type - 1 == uIs[i].Type)
            {
                _num ++;
            }
            _num = uIs[i].CurCount(_num);
            if(_num != 0)
            {
                _Complete = false;
            }

        }

        if(_Complete)
        {
            
            LargeMessage.text = CM.mapData.StageName + " クリア！";
            
            stageName.text = "Rでタイトルに戻る。";
        }
    }
}
