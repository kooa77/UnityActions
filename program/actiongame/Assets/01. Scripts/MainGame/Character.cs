using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject CharacterVisual;

    // Use this for initialization
    void Start ()
    {
        InitState();
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateCharacter();
    }

    virtual public void UpdateCharacter()
    {
        UpdateChangeState();
        _stateList[_stateType].Update();
    }


    // State

    public enum eState
    {
        IDLE,
        MOVE,
    }
    protected eState _stateType = eState.IDLE;
    eState _nextStateType = eState.IDLE;

    protected Dictionary<eState, State> _stateList = new Dictionary<eState, State>();

    void InitState()
    {
        State idleState = new IdleState();
        State moveState = new MoveState();

        idleState.Init(this);
        moveState.Init(this);

        _stateList.Add(eState.IDLE, idleState);
        _stateList.Add(eState.MOVE, moveState);
    }

    public void ChangeState(eState stateType)
    {
        _nextStateType = stateType;
    }

    void UpdateChangeState()
    {
        if (_nextStateType != _stateType)
        {
            _stateType = _nextStateType;
            _stateList[_stateType].Start();
        }
    }


    // Move

    protected Vector3 _targetPosition = Vector3.zero;

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector3 GetTargetPosition()
    {
        return _targetPosition;
    }

    public bool IsGround()
    {
        return gameObject.GetComponent<CharacterController>().isGrounded;
    }

    public void Rotate(Vector3 direction)
    {
        Quaternion targetRotate = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotate, 600.0f * Time.deltaTime);
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    public void Move(Vector3 velocity)
    {
        gameObject.GetComponent<CharacterController>().Move(velocity);
    }


    // Animation

    public void SetAnimationTrigger(string triggerName)
    {
        CharacterVisual.GetComponent<Animator>().SetTrigger(triggerName);
    }
}
