using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forklift : MonoBehaviour
{
    public static Forklift Instance{get; private set;}
    public bool Active = false;
    public ContainerManager CM;
    public PuzzleManager PuzzleManager;
    public ForkliftArmAnimation arm;
    public GameObject MoveGuide;

    public float[] MoveTime = {0.5f, 0.3f, 0.1f};
    public int MoveConboCount = 0;
    public float MoveTimer = 0;

    public Vector3Int Position = new Vector3Int(5, 0, 5);
    public int Direction = 0;

    public int PrevDir;
    public Vector3 PrevPos;
    public float isBack = 0;

    public Vector2Int MoveInput;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        Instance = this;
        yield return CM.Load();
        for(int x = 0; x < CM.mapData.Size.x; x++)
        {
            for(int z = 0; z < CM.mapData.Size.z; z++)
            {
                yield return null;
                if((int)CM.mapData.Get(x, 0, z) < 6 && (int)CM.mapData.Get(x, 0, z) > 1)
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

            transform.rotation = Quaternion.Lerp(CM.WorldDirection(Direction), CM.WorldDirection(PrevDir), MoveTimer / MoveTime[MoveConboCount]);

            transform.position = Fn.Bezier3(PrevPos, isBack * CM.Direction(PrevDir), CM.WorldPosition(Position), - isBack * CM.Direction(Direction), MoveTimer / MoveTime[MoveConboCount]);
            MoveGuide.SetActive(false);
        }
        else
        {
            MoveGuide.SetActive(true);
            PrevDir = Direction;
            PrevPos = CM.WorldPosition(Position);
            if (MoveInput.y != 0)
            {
                Vector3Int _nextPos;
                int _nextDir = Direction;

                _nextPos = Position + CM.DirectionInt(Direction) * MoveInput.y;
                isBack = MoveInput.y;
                if (MoveInput.x > 0)
                {
                    _nextDir = (_nextDir + 1 * MoveInput.y + 4) % 4;
                    _nextPos += CM.DirectionInt(_nextDir) * MoveInput.y;
                    MoveConboCount = 0;
                }
                if (MoveInput.x < 0)
                {
                    _nextDir = (_nextDir + 3 * MoveInput.y + 4) % 4;
                    _nextPos += CM.DirectionInt(_nextDir) * MoveInput.y;
                    MoveConboCount = 0;
                }

                bool _isCanMove = true;
                if (!CM.GetCanMove(_nextPos.x, _nextPos.z))
                {
                    _isCanMove = false;
                }
                if (!CM.GetCanMove((_nextPos - CM.DirectionInt(_nextDir)).x, (_nextPos - CM.DirectionInt(_nextDir)).z ))
                {
                    _isCanMove = false;
                }

                if(_isCanMove)
                {
                    MoveTimer += MoveTime[MoveConboCount];
                    MoveConboCount = Fn.Limit(MoveConboCount + 1, 2, 0);
                    PrevDir = Direction;
                    PrevPos = CM.WorldPosition(Position);
                    Direction = _nextDir;
                    Position = _nextPos;
                }
                else
                {
                    MoveConboCount = 0;
                }
            }
            else
            {
                MoveConboCount = 0;
            }
        }
    }

    public void PickUp(bool _all)
    {
        if (!Active || MoveTimer > 0) { return; }
        Vector3Int _target = Position + CM.DirectionInt(Direction);
        if(!(_target.x >= 0 && _target.x < CM.mapData.Size.x && _target.z >= 0 && _target.z < CM.mapData.Size.z)){return;}

        if(_all)
        {
            int _numA = CM.GetContainer(_target.x, _target.y, _target.z);
            int _numB = CM.GetContainer(_target.x, _target.y + 1, _target.z);
            if(_numA >= 0 && _numB >= 0 && CM.Containers[_numA].Type - 10 > 0 && CM.Containers[_numB].Type - 10 > 0)
            {
                GameObject _temp = Instantiate(CM.Prefabs[CM.Containers[_numA].Type - 10]);
                GameObject _tempB = Instantiate(CM.Prefabs[CM.Containers[_numB].Type - 10]);
                if (arm.LowType == -1)
                {
                    arm.PickUp(CM.Containers[_numB].Type - 10, _tempB);
                    arm.PickUp(CM.Containers[_numA].Type - 10, _temp);
                    MoveTimer += MoveTime[MoveConboCount];
                    MoveConboCount = Fn.Limit(MoveConboCount + 1, 2, 0);
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
            int _num = CM.GetContainer(_target.x, _target.y + 1, _target.z);
            if (_num < 0) { _num = CM.NumberMap[_target.x, _target.y, _target.z]; }
            if(_num < 0){ return; }
            if (CM.Containers[_num].Type - 10 == 0) { 
                _num = CM.NumberMap[_target.x, _target.y, _target.z];
            }
            if (_num >= 0 && CM.Containers[_num].Type - 10 > 0)
            {
                GameObject _temp = Instantiate(CM.Prefabs[CM.Containers[_num].Type - 10]);
                if (arm.PickUp(CM.Containers[_num].Type - 10, _temp))
                {
                    MoveTimer += MoveTime[MoveConboCount];
                    MoveConboCount = Fn.Limit(MoveConboCount + 1, 2, 0);
                    CM.DeleteContainer(_num);
                }
                else
                {
                    Destroy(_temp);
                }
            }
        }
        PuzzleManager.ContainerCheck();
    }

    public void DropDown(bool _all)
    {
        if (!Active || MoveTimer > 0) { return; }
        Vector3Int _target = Position + CM.DirectionInt(Direction);
        if(!(_target.x >= 0 && _target.x < CM.mapData.Size.x && _target.z >= 0 && _target.z < CM.mapData.Size.z)){return;}
        if (_all)
        {
            int _numA = CM.GetContainer(_target.x, _target.y, _target.z);
            int _numB = CM.GetContainer(_target.x, _target.y + 1, _target.z);
            if (_numA < 0 && _numB < 0)
            {
                if (arm.LowType >= 0 && arm.HighType >= 0)
                {
                    CM.AddContainer(new Vector3Int(_target.x, _target.y, _target.z), arm.LowType + 10);
                    CM.AddContainer(new Vector3Int(_target.x, _target.y + 1, _target.z), arm.HighType + 10);
                    arm.DropDown();
                    arm.DropDown();
                    MoveTimer += MoveTime[MoveConboCount];
                    MoveConboCount = Fn.Limit(MoveConboCount + 1, 2, 0);
                }
            }
        }
        else
        {
            int _num = CM.GetContainer(_target.x, _target.y, _target.z);
            if(_num < 0)
            {
                int _type = arm.DropDown();
                if(_type >= 0)
                {
                    MoveTimer += MoveTime[MoveConboCount];
                    MoveConboCount = Fn.Limit(MoveConboCount + 1, 2, 0);
                    CM.AddContainer(new Vector3Int(_target.x, _target.y, _target.z), _type + 10);
                }
            }
            else
            {
                _num = CM.GetContainer(_target.x, _target.y + 1, _target.z);
                if(_num < 0 && arm.HighType == -1)
                {
                    int _type = arm.DropDown();
                    if (_type >= 0)
                    {
                        MoveTimer += MoveTime[MoveConboCount];
                        MoveConboCount = Fn.Limit(MoveConboCount + 1, 2, 0);
                        CM.AddContainer(new Vector3Int(_target.x, _target.y + 1, _target.z), _type + 10 );
                    }
                }
                else
                {
                    Message.AddMessage("おけません");
                }
            }
        }
        PuzzleManager.ContainerCheck();
    }
}
