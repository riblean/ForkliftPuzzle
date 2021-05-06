using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public static Message Instance { get; private set; }
    public Text text;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        AddMessage("");
    }

    void Update()
    {
        
    }

    public static void AddMessage(string _str)
    {
        if(Instance)
        {
            Instance.text.text = _str;
            Instance.StartCoroutine(Instance.lateDelete());
        }
    }

    IEnumerator lateDelete()
    {
        yield return new WaitForSeconds(3.0f);
        text.text = "";
    }
}
