using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
// using UnityEngine.Experimental.;

public class ReceiveInput : MonoBehaviour
{
    public bool AllKey = false;
    public void OnMove(InputValue _input)
    {
        if(Forklift.Instance)
        {
            Vector2 _vec = _input.Get<Vector2>();
            Forklift.Instance.MoveInput = new Vector2Int(Mathf.RoundToInt(_vec.x), Mathf.RoundToInt(_vec.y));
        }
    }

    public void OnPickUp()
    {
        if(Forklift.Instance)
        {
            Forklift.Instance.PickUp(AllKey);
        }
        if(Title.Instance)
        {
            Title.Instance.PulessKey();
        }
    }

    public void OnDropDown()
    {
        if(Forklift.Instance)
        {
            Forklift.Instance.DropDown(AllKey);
        }
    }

    public void OnAllKey(InputValue _input)
    {
        // Debug.Log("A : " + _input.Get<float>());
        AllKey = 0 < _input.Get<float>();
    }

    public void OnNext()
    {
        if(Tutrial.Instance)
        {
            Tutrial.Instance.Next();
        }
    }

    public void OnBack()
    {
        if(Tutrial.Instance)
        {
            Tutrial.Instance.Next(-1);
        }
    }

    public void OnTurn(InputValue _input)
    {
        // Debug.Log(_input.Get<float>());
        if(CameraMover.Instance)
        {
            CameraMover.Instance.InputValue = _input.Get<float>();
        }
    }

    public void OnReset()
    {
        SceneManager.LoadScene("Title");
    }
}
