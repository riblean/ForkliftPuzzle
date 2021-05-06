using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string InputY = "Vertical";
    public string InputX = "Horizontal";

    public KeyCode PickUpKey = KeyCode.Space;
    public KeyCode DropDownKey = KeyCode.LeftShift;
    public KeyCode CtrlKey = KeyCode.LeftControl;

    //public 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector = new Vector2Int(
        //    (int)Input.GetAxisRaw(InputX), (int)Input.GetAxisRaw(InputY));
    }

    public Vector2Int Vector
    {
        get { return new Vector2Int((int)Input.GetAxisRaw(InputX), (int)Input.GetAxisRaw(InputY)); }
    }

    public bool PickUp
    {
        get { return Input.GetKeyDown(PickUpKey); }
    }

    public bool DropDown
    {
        get { return Input.GetKeyDown(DropDownKey); }
    }

    public bool Ctrl
    {
        get { return Input.GetKey(CtrlKey); }
    }
}
