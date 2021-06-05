using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonCtrl : MonoBehaviour
{
    public Button Button;
    public Text Text;

    public float space = 30.0f;
    public float Deff = -20.0f;

    public string Content;
    public string SceneName = "Stage";

    public void Set(string _text, string _Content, int _count)
    {
        Text.text = _text;
        // _Content
        transform.localPosition = new Vector3(0, space * _count + Deff);

        Content = _Content;
    }

    public void StageLoad()
    {
        StartCoroutine(StageLoadRoutine());
    }

    IEnumerator StageLoadRoutine()
    {
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName);
        while(!operation.isDone)
        {
            yield return null;
        }
        while(MapData.Instance == null)
        {
            yield return null;
        }
        
        MapData.Instance.StageName = Text.text;
        yield return MapData.Instance.WebLoad(Content);
        Destroy(gameObject);
    }
}
