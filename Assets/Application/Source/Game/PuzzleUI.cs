using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleUI : MonoBehaviour
{
    public Text text;
    public Transform UI;
    public Material material;
    public Vector2Int Pos;
    public int Type = 0;

    int goalCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        text.color = material.color;
        GetComponentInChildren<MeshRenderer>().material = material;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UI.LookAt(Camera.main.transform);
    }

    public int GoalCount
    {
        get{ return goalCount;}
        set{
            goalCount = value;
            text.text = goalCount.ToString();
        }
    }

    public int CurCount(int _cur)
    {

        text.text = (goalCount - _cur).ToString();
        return goalCount - _cur;
    }

    // public void SetColor(Color _col)
    // {
    // }
}
