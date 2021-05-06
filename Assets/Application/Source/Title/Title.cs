using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public Transform Rotation;
    public float RotateSpeed = 90f;

    public int State = 0;

    public RectTransform TitleRect;

    public GameObject[] MenuObjs;
    // Start is called before the first frame update
    IEnumerator Start()
    {
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
        if(Input.anyKeyDown)
        {
            State = 1;
            SetMenu(1);
        }
    }

    public void SetMenu(int _num)
    {
        for(int i = 0; i < MenuObjs.Length; i++)
        {
            MenuObjs[i].SetActive(i == _num);
        }
    }

    public void LoadScene(string _name)
    {
        SceneManager.LoadScene(_name);
    }
}
