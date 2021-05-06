using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forklift : MonoBehaviour
{
    public bool Active = false;
    public BattleManager BM;
    public ContainerManager CM;
    public PlayerInput inp;
    public ForkliftArmAnimation arm;
    public GameObject MoveGuide;

    public float MoveTime = 0.5f;
    public float MoveTimer = 0;

    public Vector3Int Position = new Vector3Int(5, 0, 5);
    public int Direction = 0;

    public int PrevDir;
    public Vector3 PrevPos;
    public float isBack = 0;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        for(int x = 0; x < CM.Map.LowZ[0].X.Length; x++)
        {
            for(int z = 0; z < CM.Map.LowZ.Length; z++)
            {
                yield return null;
                if(CM.Map.LowZ[z].X[x] < -9)
                {
                    Position = new Vector3Int(x, 0, z);
                    transform.position = CM.WorldPosition(Position);
                }
            }
        }
        Active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active) { return; }
        if(MoveTimer > 0)
        {
            MoveTimer -= Time.deltaTime;

            transform.rotation = Quaternion.Lerp(CM.WorldDirection(Direction), CM.WorldDirection(PrevDir), MoveTimer / MoveTime);

            transform.position = Fn.Bezier3(PrevPos, isBack * CM.Direction(PrevDir), CM.WorldPosition(Position), - isBack * CM.Direction(Direction), MoveTimer / MoveTime);
            MoveGuide.SetActive(false);
        }
        else
        {
            MoveGuide.SetActive(true);
            PrevDir = Direction;
            PrevPos = CM.WorldPosition(Position);
            if (inp.Vector.y != 0)
            {
                Vector3Int _nextPos;
                int _nextDir = Direction;

                _nextPos = Position + CM.DirectionInt(Direction) * (int)inp.Vector.y;
                isBack = inp.Vector.y;
                if (inp.Vector.x > 0)
                {
                    _nextDir = (_nextDir + 1 * inp.Vector.y + 4) % 4;
                    _nextPos += CM.DirectionInt(_nextDir) * (int)inp.Vector.y;
                }
                if (inp.Vector.x < 0)
                {
                    _nextDir = (_nextDir + 3 * inp.Vector.y + 4) % 4;
                    _nextPos += CM.DirectionInt(_nextDir) * (int)inp.Vector.y;
                }

                bool _isCanMove = true;
                if (CM.NumberMap[_nextPos.x, 0, _nextPos.z] >= 0)
                {
                    _isCanMove = false;
                }
                if (CM.NumberMap[(_nextPos - CM.DirectionInt(_nextDir)).x, 0, (_nextPos - CM.DirectionInt(_nextDir)).z] >= 0)
                {
                    _isCanMove = false;
                }
                if (CM.NumberMap[_nextPos.x, 1, _nextPos.z] >= 0)
                {
                    _isCanMove = false;
                }
                if (CM.NumberMap[(_nextPos - CM.DirectionInt(_nextDir)).x, 1, (_nextPos - CM.DirectionInt(_nextDir)).z] >= 0)
                {
                    _isCanMove = false;
                }

                if(_isCanMove)
                {
                    MoveTimer += MoveTime;
                    PrevDir = Direction;
                    PrevPos = CM.WorldPosition(Position);
                    Direction = _nextDir;
                    Position = _nextPos;
                }
            }
            else if (inp.PickUp)
            {
                Vector3Int _target = Position + CM.DirectionInt(Direction);
                if(inp.Ctrl)
                {
                    int _numA = CM.NumberMap[_target.x, _target.y, _target.z];
                    int _numB = CM.NumberMap[_target.x, _target.y + 1, _target.z];
                    if(_numA >= 0 && _numB >= 0 && CM.Containers[_numA].Type > 0 && CM.Containers[_numB].Type > 0)
                    {
                        GameObject _temp = Instantiate(CM.Prefabs[CM.Containers[_numA].Type]);
                        GameObject _tempB = Instantiate(CM.Prefabs[CM.Containers[_numB].Type]);
                        if (arm.LowType == -1)
                        {
                            arm.PickUp(CM.Containers[_numB].Type, _tempB);
                            arm.PickUp(CM.Containers[_numA].Type, _temp);
                            MoveTimer += MoveTime;
                            CM.DeleteContainer(_numA);
                            CM.DeleteContainer(_numB);
                        }
                        else
                        {
                            Destroy(_temp);
                            Destroy(_tempB);
                        }
                    }
                }
                else
                {
                    int _num = CM.NumberMap[_target.x, _target.y + 1, _target.z];
                    if (_num < 0) { _num = CM.NumberMap[_target.x, _target.y, _target.z]; }
                    if (CM.Containers[_num].Type == 0) { _num = CM.NumberMap[_target.x, _target.y, _target.z]; }
                    if (_num >= 0 && CM.Containers[_num].Type > 0)
                    {
                        GameObject _temp = Instantiate(CM.Prefabs[CM.Containers[_num].Type]);
                        if (arm.PickUp(CM.Containers[_num].Type, _temp))
                        {
                            MoveTimer += MoveTime;
                            CM.DeleteContainer(_num);
                        }
                        else
                        {
                            Destroy(_temp);
                        }
                    }
                }
                BM.TruckCheck();
            }
            else if (inp.DropDown)
            {
                Vector3Int _target = Position + CM.DirectionInt(Direction);
                if (inp.Ctrl)
                {
                    int _numA = CM.NumberMap[_target.x, _target.y, _target.z];
                    int _numB = CM.NumberMap[_target.x, _target.y + 1, _target.z];
                    if (_numA < 0 && _numB < 0)
                    {
                        if (arm.LowType >= 0 && arm.HighType >= 0)
                        {
                            CM.AddContainer(new Vector3Int(_target.x, _target.y, _target.z), arm.LowType);
                            CM.AddContainer(new Vector3Int(_target.x, _target.y + 1, _target.z), arm.HighType);
                            arm.DropDown();
                            arm.DropDown();
                            MoveTimer += MoveTime;
                        }
                    }
                }
                else
                {
                    int _num = CM.NumberMap[_target.x, _target.y, _target.z];
                    if(_num < 0)
                    {
                        int _type = arm.DropDown();
                        if(_type >= 0)
                        {
                            MoveTimer += MoveTime;
                            CM.AddContainer(new Vector3Int(_target.x, _target.y, _target.z), _type);
                        }
                    }
                    else
                    {
                        _num = CM.NumberMap[_target.x, _target.y + 1, _target.z];
                        if(_num < 0 && arm.HighType == -1)
                        {
                            int _type = arm.DropDown();
                            if (_type >= 0)
                            {
                                MoveTimer += MoveTime;
                                CM.AddContainer(new Vector3Int(_target.x, _target.y + 1, _target.z), _type);
                            }
                        }
                        else
                        {
                            Message.AddMessage("おけません");
                        }
                    }
                }
                BM.TruckCheck();
            }

        }
    }
}
