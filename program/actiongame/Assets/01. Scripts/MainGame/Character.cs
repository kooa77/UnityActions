using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum eCharacterType
    {
        NONE,
        PLAYER,
        MONSTER,
    }
    protected eCharacterType _characterType = eCharacterType.NONE;

    public GameObject CharacterVisual;

    // Use this for initialization
    void Start ()
    {
        Init();
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


    // Type

    virtual public void Init()
    {
        InitAttackInfo();
        InitDamageInfo();
        InitState();
    }

    public eCharacterType GetCharacterType()
    {
        return _characterType;
    }


    // State

    public enum eState
    {
        IDLE,
        MOVE,
        ATTACK,
        CHASE,
    }
    protected eState _stateType = eState.IDLE;
    eState _nextStateType = eState.IDLE;

    protected Dictionary<eState, State> _stateList = new Dictionary<eState, State>();

    void InitState()
    {
        State idleState = new IdleState();
        State moveState = new MoveState();
        State attackState = new AttackState();
        State chaseSate = new ChaseState();

        idleState.Init(this);
        moveState.Init(this);
        attackState.Init(this);
        chaseSate.Init(this);

        _stateList.Add(eState.IDLE, idleState);
        _stateList.Add(eState.MOVE, moveState);
        _stateList.Add(eState.ATTACK, attackState);
        _stateList.Add(eState.CHASE, chaseSate);
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


    // Attack

    AttackArea[] _attackAreas;

    void InitAttackInfo()
    {
        _attackAreas = GetComponentsInChildren<AttackArea>();
    }

    public void AttackStart()
    {
        for (int i = 0; i < _attackAreas.Length; i++)
            _attackAreas[i].Enable();
    }

    public void AttackEnd()
    {
        for (int i = 0; i < _attackAreas.Length; i++)
            _attackAreas[i].Disable();
    }


    // Damage Info

    void InitDamageInfo()
    {
        HitArea[] hitAreas = GetComponentsInChildren<HitArea>();
        for(int i=0; i< hitAreas.Length; i++)
        {
            hitAreas[i].Init(this);
        }
    }


    // Animation

    public void SetAnimationTrigger(string triggerName)
    {
        CharacterVisual.GetComponent<Animator>().SetTrigger(triggerName);
    }

    public AnimationPlayer GetAnimationPlayer()
    {
        return CharacterVisual.GetComponent<AnimationPlayer>();
    }
}
