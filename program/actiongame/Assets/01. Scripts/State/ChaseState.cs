﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    Vector3 _velocity = Vector3.zero;

    override public void Start()
    {
        _character.SetAnimationTrigger("move");
    }

    override public void Stop()
    {
    }

    override public void Update()
    {
        //Vector3 destination = _character.GetTargetPosition();
        Vector3 destination = _character.GetTargetObject().transform.position;

        destination.y = _character.GetPosition().y;
        Vector3 direction = (destination - _character.GetPosition()).normalized;
        _velocity = direction * 6.0f;

        Vector3 snapGround = Vector3.zero;
        if (_character.IsGround())
            snapGround = Vector3.down;

        // 목적지와 현재 위치가 일정 거리 이상이면 -> 이동
        float distance = Vector3.Distance(destination, _character.GetPosition());
        if (distance < _character.GetAttackRange())
        {
            _character.ChangeState(Player.eState.ATTACK);
        }
        else
        {
            _character.Rotate(direction);
            _character.Move(_velocity * Time.deltaTime + snapGround);
        }
    }

    override public void UpdateInput()
    {
        //_destination = _character.GetTargetPosition();
    }
}
