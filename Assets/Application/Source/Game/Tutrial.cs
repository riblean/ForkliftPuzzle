using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutrial : MonoBehaviour
{
    public GameObject UIObje;
    public GameObject EnterObj;
    public ContainerManager CM;
    public BattleManager BM;
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
        text.text = TextData[Curretn];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Next();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Next(-1);
        }
    }

    void Next(int _add = 1)
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
            for(int i = 3; i < 8; i++)
            {

                CM.AddContainer(new Vector3Int(i, 0, 2), i % 4);
                CM.AddContainer(new Vector3Int(i, 0, 8), i % 4);
            }
        }
        if (Curretn == AddTruckNumber && isTruck == false)
        {
            isTruck= true;
            BM.gameObject.SetActive(true);
        }
    }
}
