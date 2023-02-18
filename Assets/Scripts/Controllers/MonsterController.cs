using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    Stat _stat;
    Dictionary<string, Data.Monster> monsterDict;
    Data.Monster _monster;
    [SerializeField]
    float _scanRange;

    [SerializeField]
    float _attackRange;
    int AttackCount = 1;
    Vector3 _destPos;
    GameObject _lockTarget;
    Rigidbody2D _rigid;
    Animator anim;    
    public override void Init()
    {
        _stat = gameObject.GetComponent<Stat>();
        _rigid = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        monsterDict = Managers.Data.MonsterDict;
        _lockTarget = Managers.Game.GetPlayer();
        if (monsterDict.TryGetValue(gameObject.name, out _monster))
        {
            _stat.Attack = _monster.damage;
            _scanRange = _monster.scanrange;
            _attackRange = _monster.attackrange;
            _stat.MoveSpeed = _monster.speed;
            _stat.MaxHp = _monster.maxhp;
            _stat.Hp = _stat.MaxHp;
            _stat.Defense = 5;
        }
        //Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
        WorldObjectType = Define.WorldObject.Monster;
    }
    protected override void UpdateDie()
    {

    }
    IEnumerator Knockback()
    {
        State = Define.State.Knockback;
        anim.SetTrigger("IsKnockBack");
        if(_stat.MoveSpeed != 0)
            _rigid.velocity = new Vector2(0, 1) * 3;
        //SpriteRenderer currentSprite = GetComponent<SpriteRenderer>();
        //currentSprite.color = new Color(1, 1, 1, 0.4f);
        yield return new WaitForSeconds(1);
        if(State != Define.State.Die)
            State = Define.State.Idle;
        yield return null;
        //currentSprite.color = new Color(1, 1, 1, 1);
    }
    protected override void UpdateIdle()
    {
        if (State == Define.State.Die)
            return;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;
        //Idle 상태에 움직이는거
        int mask = (1 << 12) | (1 << 8);

        Vector2 look = transform.TransformDirection(Vector3.left);
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position + new Vector3(look.x * (box.size.x / 1.9f), 0), look, 0.1f, mask);
        if (hit.Length > 0)
        {
            foreach (RaycastHit2D hits in hit)
            {
                if ((hits.collider.gameObject.layer == (int)Define.Layer.MonsterBlock) && (look.x == -1))
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.back);
                }
                else if ((hits.collider.gameObject.layer == (int)Define.Layer.MonsterBlock) && (look.x == 1))
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.forward);
                }
                else if(hits.collider.gameObject.layer == (int)Define.Layer.Monster)
                {
                    if(look.x == -1)
                    {
                        transform.rotation = Quaternion.LookRotation(Vector3.back);
                    }
                    else
                    {
                        transform.rotation = Quaternion.LookRotation(Vector3.forward);
                    }
                }
            }
        }
        anim.SetFloat("Speed", 0);
        anim.SetFloat("Status", 1);
        transform.Translate(Vector3.left * Time.deltaTime * _stat.MoveSpeed);
        float distance = (player.transform.position - transform.position).magnitude;
        if(distance <= _scanRange)
        {
            //Chase
            anim.SetFloat("Speed", 1);
            _lockTarget = player;
            State = Define.State.Moving;
            return;
        }
        //Todo : 플레이어를 찾는건 매니저가 생기면 옮기기
        
    }
    protected override void UpdateMoving()
    {
        if (State == Define.State.Die)
            return;
        // chase 중일때는 사정거리 안에 들어올 때 까지 모든 작업 return
        // 플레이어가 내 사정거리보다 가까우면 공격
        // chase 중이 아니라면 알아서 돌아다니기
        float distance = (_lockTarget.transform.position - transform.position).magnitude;
        bool isRight = _lockTarget.transform.position.x - transform.position.x > 0 ? true : false;
        if (distance <= _attackRange)
        {
            anim.SetFloat("Status", 0);
            if (AttackCount > 0)
            {
                State = Define.State.Attack;
            }
            return;
        }
        if (distance > _scanRange)
        {
            State = Define.State.Idle;
        }
        if (Mathf.Abs(_lockTarget.transform.position.x - transform.position.x) > 0.1)
        {
            anim.SetFloat("Status", 1);
            if (isRight)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.back);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(Vector3.forward);
            }
            transform.Translate(Vector3.left * Time.deltaTime * _stat.MoveSpeed);
        }
    }
    
    protected override void UpdateAttack()
    {        
    }
    IEnumerator Attack()
    {
        int mask = 1 << 7;
        AttackCount--;
        if (_monster.name == "Slime")
        {
            Vector2 look = transform.TransformDirection(Vector3.left);
            RaycastHit2D hit = Physics2D.CircleCast(transform.position,_monster.range,Vector2.up, 0.0f ,mask);
            if (hit)
            {
                PlayerController player = hit.collider.gameObject.GetComponent<BaseController>() as PlayerController;
                player.OnHitEvent(_monster.damage, transform);
            }
        }
        else if (_monster.name == "Crystal")
        {
            Vector2 look = _lockTarget.transform.position - transform.position;
            GameObject go = Managers.Resource.Instantiate($"MonsterBullet/{gameObject.name}Bullet");
            go.GetOrAddComponent<MonsterBullet>().Damage = _stat.Attack;
            go.transform.position = transform.position;
            float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            go.transform.rotation = rotation;
            Rigidbody2D rigid = go.GetOrAddComponent<Rigidbody2D>();
            look.x -= transform.TransformDirection(Vector3.left).x;
            float distance = (_lockTarget.transform.position - transform.position).magnitude;
            rigid.AddForce(look / distance * 350);
        }
        float atkCount = 1 / _monster.rate;
        float animTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        if (atkCount > animTime)
        {
            yield return new WaitForSeconds(animTime);
            State = Define.State.Idle;
            yield return new WaitForSeconds(atkCount - animTime);
            AttackCount++;
        }
        else
        {
            yield return new WaitForSeconds(atkCount);
            AttackCount++;
            yield return new WaitForSeconds(animTime - atkCount);
            State = Define.State.Idle;
        }
    }
    void AttackStartEvent()
    {
        StartCoroutine(Attack());
    }
    void AttackEndEvent()
    {
        //애니메이션이 끝났을 때
        State = Define.State.Idle;
    }
    public override void OnHitEvent(int damage, Transform Atktransform, string atk_type = null)
    {
        base.OnHitEvent(damage, Atktransform, atk_type);
        UI_TakeDamage takeDamage = Managers.UI.MakeWorldSpaceUI<UI_TakeDamage>();
        int lastDamage = damage - _stat.Defense >= 0 ? damage - _stat.Defense : 0;
        if(atk_type == "Crit")
        {
            takeDamage.SetText($"<b><color=red>{lastDamage}!!</color></b>");
        } 
        else if(atk_type == "Poison")
        {
            takeDamage.SetText($"<b><color=purple>{lastDamage}</color></b>");
        }
        else
        {
            takeDamage.SetText($"{lastDamage}");
        }
        StartCoroutine(Knockback());
        takeDamage.SetPosition(gameObject);
        if (_stat.Hp <= 0)
        {
            State = Define.State.Die;
            anim.SetTrigger("IsDeath");
            StartCoroutine(DestroyMonster());
        }
    }
    IEnumerator DestroyMonster()
    {
        float animTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        while(animTime > 0)
        {
            animTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        Managers.Game.Despawn(gameObject);
        State = Define.State.Idle;
        _stat.Hp = _stat.MaxHp;
        yield return null;
    }
    void EndHitEvent()
    {
        //gameObject.GetComponent<Animator>().Play("Idle");
        State = Define.State.Idle;
    }
}
