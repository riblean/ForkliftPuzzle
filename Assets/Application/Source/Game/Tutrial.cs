using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutrial : MonoBehaviour
{
    public static Tutrial Instance{get; private set;}
    public GameObject UIObje;
    public GameObject EnterObj;
    public ContainerManager CM;
    public GameObject Puzzle;
    public Text text;

    public string[] TextData;
    public int Curretn = 0;

    public int AddContainerNumber = 8;
    public bool isContainer = false;
    public int AddTruckNumber = 10;
    public bool isTruck = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        text.text = TextData[Curretn];
    }

    public void Next(int _add = 1)
    {
        Curretn+=_add;
        Curretn = Fn.Limit(Curretn, TextData.Length, -1);
        if (Curretn >= TextData.Length || Curretn < 0)
        {
            UIObje.SetActive(false);
        }
        else
        {
            UIObje.SetActive(true);
            text.text = TextData[Curretn];
        }

        if (Curretn == AddContainerNumber && isContainer == false)
        {
            isContainer = true;
            CM.AddContainer(new Vector3Int(0, 0, 2), 11 + 1);
            CM.AddContainer(new Vector3Int(0, 0, 3), 11 + 2);
            CM.AddContainer(new Vector3Int(0, 1, 3), 11 + 2);
        }
        if (Curretn == AddTruckNumber && isTruck == false)
        {
            isTruck= true;
            Puzzle.SetActive(true);
        }
    }
}
