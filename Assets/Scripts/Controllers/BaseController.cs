using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    protected Define.State _state = Define.State.Idle;
    private void Start()
    {
        Init();
    }
    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;
    public abstract void Init();
    public virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;
            Animator anim = GetComponent<Animator>();

            switch (_state)
            {
                case Define.State.Die:
                    break;
                case Define.State.Knockback:
                    break;
                case Define.State.Jumping:
                    anim.SetBool("IsJumping", true);
                    break;
                case Define.State.Attack:
                    anim.SetTrigger("IsAttacking");
                    break;                    
                case Define.State.Idle:
                    break;
                case Define.State.Moving:
                    break;
            }
        }
    }
    protected virtual void Update()
    {
        switch (State)
        {
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Knockback:
                UpdateKnockBack();
                break;
            case Define.State.EventMove:
                UpdateEventMoving();
                break;
            case Define.State.Jumping:
                UpdateJumping();
                break;
            case Define.State.Attack:
                UpdateAttack();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;
            case Define.State.Idle:
                UpdateIdle();
                break;
        }
    }
    protected virtual void FixedUpdate()
    {
        if(State == Define.State.Moving)
        {
            UpdateMoving();
        }
    }
    protected virtual void UpdateDie() { }
    protected virtual void UpdateJumping() { }
    protected virtual void UpdateAttack() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateSkill() { }
    protected virtual void UpdateKnockBack() { }
    protected virtual void UpdateEventMoving() { }
    public virtual void OnHitEvent(int damage, Transform Atktransform, string atk_type = null)
    {
        Stat _stat = gameObject.GetComponent<Stat>();
        int lastDamage = damage - _stat.Defense;
        _stat.Hp -= lastDamage >= 0 ? lastDamage : 0;
    }
}
