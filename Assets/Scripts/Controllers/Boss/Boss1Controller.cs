using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : BaseController
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
    GameObject player;
    Rigidbody2D _rigid;
    Animator anim;
    float _atkTime = 0.0f;
    bool isSkilling = false;
    public override void Init()
    {
        _stat = gameObject.GetComponent<Stat>();
        _rigid = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        monsterDict = Managers.Data.MonsterDict;
        player = GameObject.FindGameObjectWithTag("Player");
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
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (State == Define.State.Die)
            return;
        _atkTime += Time.deltaTime;
        if (_atkTime > 10.0f)
        {
            int rand = Random.Range(1, 4);
            isSkilling = true;
            anim.SetFloat("Status", 0);
            StartCoroutine($"Attack_Pattern{rand}");
            _atkTime = 0.0f;
        }
    }
    protected override void UpdateDie()
    {

    }
    protected override void UpdateIdle()
    {
        if (State == Define.State.Die)
            return;
        anim.SetFloat("Status", 0);
        // 잠깐 쉬다가 지정 Seoncds 지나면 Move -> Attack
        if (!isSkilling)
        {
            //Chase
            State = Define.State.Moving;
            return;
        }
    }
    protected override void UpdateMoving()
    {
        if (State == Define.State.Die)
            return;
        float distance = (player.transform.position - transform.position).magnitude;
        bool isRight = player.transform.position.x - transform.position.x > 0 ? true : false;
        if (distance <= _attackRange)
        {
            if (AttackCount > 0)
            {
                AttackCount--;
                anim.SetInteger("State", 1);
                State = Define.State.Attack;
            }
            return;
        }
        if (Mathf.Abs(player.transform.position.x - transform.position.x) > 0.1)
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
        int mask = 1 << (int)Define.Layer.Player;
        Vector2 look = transform.TransformDirection(Vector3.left);
        GameObject effect = Managers.Resource.Instantiate("Monster/Effect/Boss1/Attack");
        effect.transform.position = transform.position + new Vector3(0.0f, -0.5f);
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 3.0f, Vector2.up, 0.0f, mask);
        Managers.Sound.Play("Effect/Boss/Stage1/Attack");
        if (hit)
        {
            player.GetComponent<PlayerController>().OnHitEvent(10, transform);
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
        if (atk_type == "Crit")
        {
            takeDamage.SetText($"<b><color=red>{lastDamage}!!</color></b>");
        }
        else if (atk_type == "Poison")
        {
            takeDamage.SetText($"<b><color=purple>{lastDamage}</color></b>");
        }
        else
        {
            takeDamage.SetText($"{lastDamage}");
        }
        takeDamage.SetPosition(gameObject);
        StartCoroutine(Knockback());
        if (_stat.Hp <= 0)
        {
            State = Define.State.Die;
            anim.SetTrigger("IsDeath");
            StartCoroutine(DestroyMonster());
        }
    }
    IEnumerator Knockback()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.4f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.4f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
        yield return new WaitForSeconds(0.1f);
        yield return null;
    }
    IEnumerator DestroyMonster()
    {
        float animTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        while (animTime > 0)
        {
            animTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        Managers.Game.Despawn(gameObject);
        _stat.Hp = _stat.MaxHp;
        yield return null;
    }
    IEnumerator Attack_Pattern1()
    {
        //플레이어 머리 위로 순간이동
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        transform.position = player.transform.position + new Vector3(0.0f, 5.0f, 0.0f);
        anim.SetInteger("State", 2);
        State = Define.State.Attack;
        rb.gravityScale = 0.0f;
        yield return new WaitForSeconds(0.5f);
        //엉덩방아 찍기
        anim.SetTrigger("Falling");
        rb.gravityScale = 1.0f;
        int mask = 1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Platform;
        RaycastHit2D check;
        do
        {
            //몬스터 크기 확인하고 raycast 범위 조정
            check = Physics2D.BoxCast(transform.position - new Vector3(0.0f, 3.0f), new Vector2(2.0f, 0.1f), 0.0f, Vector2.up, 0.0f, mask);
            //땅을 만나면 거기서 멈추기
            transform.position += Vector3.down * Time.deltaTime * 30;
            yield return new WaitForFixedUpdate();
        } while (!check);
        anim.SetTrigger("IsGround");
        GameObject effect = Managers.Resource.Instantiate("Monster/Effect/Boss1/Skill1");
        Managers.Sound.Play("Effect/Boss/Stage1/Skill1");
        effect.transform.position = transform.position;
        //몬스터 크기 Size X는 그대로 하더라도 Y는 조정
        check = Physics2D.BoxCast(transform.position - new Vector3(0.0f, 1.5f), new Vector2(5.0f, 3f), 0.0f, Vector2.up, 0.0f, 1 << (int)Define.Layer.Player);
        if (check)
        {
            player.GetComponent<PlayerController>().OnHitEvent(20, transform);
        }
        State = Define.State.Idle;
        yield return new WaitForSeconds(3);
        isSkilling = false;
        yield return null;
    }
    IEnumerator Attack_Pattern2()
    {
        //발구르기 (1초 기다리고)
        anim.SetInteger("State", 3);
        State = Define.State.Attack;
        yield return new WaitForSeconds(1);
        //땅에서 가시 나오게
        //가시는 애니메이션 바로 위로 올라오게 바꾸기
        //가시 자체적으로 Destroy 해야함
        for (int i = 0; i < 3; i++)
        {
            float x = player.transform.position.x;
            GameObject effect = Managers.Resource.Instantiate("Monster/Effect/Boss1/Skill2");
            effect.transform.position = new Vector3(x, -0.5f);
            yield return new WaitForSeconds(0.5f);
            GameObject go = Managers.Resource.Instantiate("Monster/Boss1-Thron");
            go.transform.position = new Vector3(x, -0.5f);
            yield return new WaitForSeconds(1.5f);
        }
        //Player 발밑에서 가시 3번
        State = Define.State.Idle;
        yield return new WaitForSeconds(3);
        isSkilling = false;
        yield return null;
    }
    IEnumerator Attack_Pattern3()
    {
        anim.SetInteger("State", 4);
        State = Define.State.Attack;
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 4; i++)
        {
            GameObject go = Managers.Game.Spawn(Define.WorldObject.Monster, "Monster/Slime");
            go.transform.position = transform.position;
            yield return new WaitForSeconds(0.5f);
        }
        State = Define.State.Idle;
        yield return new WaitForSeconds(3);
        isSkilling = false;
        yield return null;
    }
    void EndHitEvent()
    {
        //gameObject.GetComponent<Animator>().Play("Idle");
        State = Define.State.Idle;
    }
}
