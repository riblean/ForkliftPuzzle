using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class WebLoad : MonoBehaviour
{
    public string url = "http://mahriblean.starfree.jp/GameData/ForkliftPuzzle";
    public Text Message;

    public RectTransform Target;
    public ButtonCtrl ButtonPrefab;
    IEnumerator Start()
    {
        Message.text = "マップデータ取得中…。";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Message.text = "取得に失敗しました。";
            yield break;
        }
        
        Message.text = "取得完了。";
        string[] _lines = www.downloadHandler.text.Split('\n');

        int _count = 0;
        for(int i = 1; i < _lines.Length; i++)
        {
            if(_lines[i].Length > 2 && !( _lines[i][0] == '/' && _lines[i][1] == '/') )
            {
                string[] _str = _lines[i].Split(',');
                Instantiate(ButtonPrefab, Target.transform).Set(_str[0], url + "/" + _str[1], _count);
                _count++;
            }
        }
        yield return new WaitForSeconds(1.0f);
        Message.text = _lines[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
