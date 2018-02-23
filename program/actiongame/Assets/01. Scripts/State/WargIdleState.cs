using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WargIdleState : IdleState
{
    override public void Update()
    {
        if(_character.GetRefreshTime() <= _duration )
        {
            _character.Patrol();
        }
        _duration += Time.deltaTime;
    }


    // Attr

    float _duration = 0.0f;
}
