using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;
using System;

public class DataDownload : MonoBehaviour
{
    public Text Message;

    public RectTransform Target;
    public ButtonCtrl ButtonPrefab;
    public string[] Names = new string[0];
    public string[] Datas = new string[0];
    public bool isDownloaded = false;
    public IEnumerator Start()
    {
        
        Message.text = "マップデータ取得中…。";
        
        yield return null;

        bool isError = false;
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("MapData");
        query.OrderByDescending("number");
        query.Limit = 99;
        query.FindAsync ((List<NCMBObject> list ,NCMBException e) => 
        {
            if(e != null)
            {
                Message.text = "サーバー接続に失敗しました。";
                isError = true;
            }
            else
            {
                foreach(NCMBObject obj in list)
                {
                    Array.Resize(ref Names, Names.Length + 1);
                    Names[Names.Length - 1] = System.Convert.ToString(obj["name"]);
                    Array.Resize(ref Datas, Datas.Length + 1);
                    Datas[Datas.Length - 1] = System.Convert.ToString(obj["data"]);
                }
            }
            isDownloaded = true;
        });

        while(!isDownloaded)
        {
            yield return null;
        }
        if(isError){
            yield break;
        }
        Message.text = "取得完了。";

        for(int i = 0; i < Names.Length; i++)
        {
            Instantiate(ButtonPrefab, Target.transform).Set(Names[i], Datas[i], i + 1);
            yield return null;
        }
        Target.sizeDelta = new Vector2 (0f, - ButtonPrefab.space * ((float)Names.Length + 2) );
        Message.text = "読み込み完了。";
        yield return new WaitForSeconds(3.0f);
        Message.text = "";
    }
}
