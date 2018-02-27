using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    void OnTriggerEnter(Collider other)
    {
        if (LayerMask.NameToLayer("CharacterCtrl") == other.gameObject.layer)
        {
            Character character = other.gameObject.GetComponent<Character>();
            if( eCharacterType.PLAYER == character.GetCharacterType() )
            {
                _targetObject = other.gameObject;
                ChangeState(eState.CHASE);
            }
        }
    }

    override public void Init()
    {
        base.Init();
        _characterType = eCharacterType.MONSTER;
    }

    protected override void InitState()
    {
        base.InitState();

        State idleState = new WargIdleState();
        idleState.Init(this);
        _stateList[eState.IDLE] = idleState;
    }

    public List<WayPoint> _wayPointList;
    int _wayPointIndex = 0;

    override public void ArriveDestination()
    {
        // WayPoint에 도착을 하면 다음 웨이포인트로 목적지 변경
        WayPoint wayPoint = _wayPointList[_wayPointIndex];
        _wayPointIndex = (_wayPointIndex + 1) % _wayPointList.Count;
        _targetPosition = wayPoint.GetPosition();
    }
}
