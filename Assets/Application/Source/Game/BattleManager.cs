using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public TruckCtrl InportTruck;
    public Vector3Int[] InportPosisions;
    public TruckCtrl OutportTruck;
    public Vector3Int[] OutportPosisions;

    public Text BMMessage;

    public ContainerManager CM;
    public Forklift Target;

    public float OverTimer = 9999f;
    public int InCount = 0;
    public int OutCount = 0;
    public int Score = 0;

    public float startTime = 0;

    public float OverTimeMax = 30f;
    public float OberTimeSum = 0.9f;

    public int RandomRange = 3;

    bool gameOver = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);
        OutportTruck.gameObject.SetActive(false);
        OutportTruck.gameObject.SetActive(false);
        startTime = Time.time;
        InportTruck.DoorTarget = 1;
        OutportTruck.DoorTarget = 1;

        for(int i = 0; i < InportPosisions.Length; i++)
        {
            int _num = getNum(InportPosisions[i]);
            if (_num != -1) { CM.DeleteContainer(_num); }
            //CM.AddContainer(InportPosisions[i], 1);
            _num = getNum(OutportPosisions[i]);
            if (_num != -1) { CM.DeleteContainer(_num); }
            CM.AddContainer(OutportPosisions[i], Random.Range(2, RandomRange));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(OutCount > 0)
        {
            OverTimer -= Time.deltaTime;
            if(OverTimer < 0)
            {
                gameOver = true;
                Target.Active = false;
                StartCoroutine(GameOverStream());
            }
        }
        if(!gameOver)
        {
            BMMessage.text = "残り時間：" + OverTimer.ToString("00.00") + "\n";
            BMMessage.text += "入荷に残ったコンテナ：" + OutCount.ToString() + "\n";
            BMMessage.text += "出荷に置いたコンテナ：" + InCount.ToString() + "\n";
            BMMessage.text += "現在のスコア：" + Score.ToString() + "\n";
        }

        if(Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("Title");
        }
    }

    IEnumerator GameOverStream()
    {
        BMMessage.text += "GameOver!";
        yield return null;
    }

    int getNum(Vector3Int _pos)
    {
        return CM.NumberMap[_pos.x, _pos.y, _pos.z];
    }

    public void TruckCheck()
    {
        if(OutCount < 0 || InCount > 4) { return; }
        InCount = 0;
        OutCount = 0;
        for (int i = 0; i < InportPosisions.Length; i++)
        {
            int _num = getNum(InportPosisions[i]);
            if(_num >= 0) { InCount++; }
            _num = getNum(OutportPosisions[i]);
            if (_num >= 0) { OutCount++; }

        }

        if(InCount == 5)
        {
            StartCoroutine(AddScore());
        }

        if(OutCount == 0)
        {
            StartCoroutine(UpdateInport());
        }
    }

    IEnumerator AddScore()
    {

        InportTruck.gameObject.SetActive(true);
        InportTruck.DoorTarget = 0;
        InportTruck.DoorOpen = 1;
        yield return new WaitForSeconds(3.0f);

        int[] Colors = new int[16];

        for (int i = 0; i < InportPosisions.Length; i++)
        {
            int _num = getNum(InportPosisions[i]);
            Colors[CM.Containers[_num].Type]++;
            if (_num != -1) { CM.DeleteContainer(_num); }
        }
        int _max = 0;
        for (int i = 0; i < Colors.Length; i++)
        {
            if (Colors[i] > _max)
            {
                _max = Colors[i];
            }
        }
        Score += (int)Mathf.Pow(2, _max);
        InportTruck.DoorTarget = 1;
        yield return new WaitForSeconds(2.0f);
        InportTruck.gameObject.SetActive(false);

        InCount = 0;
    }

    IEnumerator UpdateInport()
    {
        OutCount = -1;
        OutportTruck.gameObject.SetActive(true);
        OutportTruck.DoorTarget = 0;
        OutportTruck.DoorOpen = 1;
        yield return new WaitForSeconds(3.0f);
        OutportTruck.DoorTarget = 1;
        yield return new WaitForSeconds(2.0f);
        OutportTruck.gameObject.SetActive(false);
        OverTimeMax *= OberTimeSum;
        OverTimer = OverTimeMax;

        for (int i = 0; i < InportPosisions.Length; i++)
        {
            int _num = getNum(OutportPosisions[i]);
            CM.AddContainer(OutportPosisions[i], Random.Range(2, RandomRange));
        }

        OutCount = 5;
        TruckCheck();
    }
}
