using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Title : MonoBehaviour
{
    public static Title Instance{get;private set;}
    public Transform Rotation;
    public float RotateSpeed = 90f;

    public int State = 0;

    public RectTransform TitleRect;

    public GameObject[] MenuObjs;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Instance = this;
        SetMenu(-1);
        yield return new WaitForSeconds(1.0f);
        if(State == 0)
        {
            SetMenu(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Rotation.rotation *= Quaternion.AngleAxis(RotateSpeed * Time.deltaTime, Vector3.up);
    }

    public void SetMenu(int _num)
    {
        for(int i = 0; i < MenuObjs.Length; i++)
        {
            MenuObjs[i].SetActive(i == _num);
            if(MenuObjs[i].transform.childCount > 0)
            {
                
            }
            EventSystem.current.SetSelectedGameObject(MenuObjs[i].transform.GetChild(0).gameObject);
        }
    }

    public void LoadScene(string _name)
    {
        SceneManager.LoadScene(_name);
    }

    public void PulessKey()
    {
        State = 1;
        SetMenu(1);
    }
}
