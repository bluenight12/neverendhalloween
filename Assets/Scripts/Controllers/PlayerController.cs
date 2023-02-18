
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerController : BaseController
{
    [SerializeField]
    private RuntimeAnimatorController Resetanimator;

    PlatformEffector2D _platform;

    PlayerStat _stat;
    Dictionary<string, Data.Weapon> weaponDict;
    Dictionary<string, Data.Skill> skillDict;
    protected Weapon _weapon;
    WeaponAbility _wpAbility;
    Rigidbody2D _rigid;
    NPCController _currentNPC;
    List<Collider2D> _triggerCol;
    Animator anim;
    UI_Dash _dash;
    bool _jumpAttack = true;
    int _attackcount = 1;
    int _askillCount = 1;
    int _sskillCount = 1;
    int _dashCount = 1;
    float _attackAnim = 0.0f;
    bool _isGround = false;
    float _lastAttack = 0.0f;
    bool _isDead = false;
    bool _isLadder = false;
    bool _isDamage = false;
    int footstep = 1;
    int jumpSound = 1;
    float _moveTime = 0.0f;
    float _regenTime = 0.0f;
    bool exitTime = false;
    public bool IsDamage { set { _isDamage = value; } }
    public override void Init()
    {
        //Stat 정보 추출
        _rigid = gameObject.GetOrAddComponent<Rigidbody2D>();
        weaponDict = Managers.Data.WeaponDict;
        skillDict = Managers.Data.SkillDict;
        _stat = gameObject.GetOrAddComponent<PlayerStat>();
        _weapon = gameObject.GetOrAddComponent<Weapon>();
        _wpAbility = gameObject.GetOrAddComponent<WeaponAbility>();
        anim = GetComponent<Animator>();
        _triggerCol = new List<Collider2D>();
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
        DontDestroyOnLoad(gameObject);
        WorldObjectType = Define.WorldObject.Player;
        Data.Weapon wp;
        weaponDict.TryGetValue("Punch", out wp);
        Managers.Weapon.ChangeWeapon(gameObject, wp);
        _dash = Util.FindChild<UI_Dash>(gameObject, "UI_Dash");
        //_platform = GameObject.Find("Platform").GetComponent<PlatformEffector2D>();
        //Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        IsFalling();
        _lastAttack += Time.deltaTime;
        _regenTime += Time.deltaTime;
        if (_regenTime > 5.0f)
        {
            if (_stat.Hp + _wpAbility.HealthRegen > _stat.MaxHp)
            {
                _stat.Hp = _stat.MaxHp;
            }
            else
            {
                _stat.Hp += _wpAbility.HealthRegen;
            }
            _regenTime = 0;
        }
        if (_lastAttack >= 3.0f)
        {
            _attackAnim = 0;
        }
        if (!_isLadder)
        {
            _rigid.gravityScale = 3.0f;
        }
    }
    protected override void UpdateDie()
    {
        //아무것도 못함

    }
    protected override void UpdateKnockBack()
    {
        //아무것도 못함
        transform.Translate(Vector3.left * Time.deltaTime * _stat.MoveSpeed);
    }
    protected override void UpdateEventMoving()
    {
        transform.Translate(Vector3.right * Time.deltaTime * _stat.MoveSpeed);
        anim.SetFloat("Speed", _stat.MoveSpeed);
    }
    protected override void UpdateJumping()
    {

    }
    protected override void UpdateMoving()
    {
        // 움직일 때 실행해야 하는 것들
        transform.Translate(Vector3.right * Time.deltaTime * _stat.MoveSpeed);
        if (!(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
            State = Define.State.Idle;
        anim.SetFloat("Speed", _stat.MoveSpeed);
    }
    protected override void UpdateAttack()
    {
    }
    protected override void UpdateIdle()
    {
        State = Define.State.Idle;
        anim.SetFloat("Speed", 0);
        if (!(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
            anim.SetFloat("LadderSpeed", 0);
    }
    // 넉백 다시 만들기 add force 말고 velocity로
    public override void OnHitEvent(int damage, Transform Atktransform, string atk_type = null)
    {
        if (_isDead)
            return;
        if (_isDamage)
        {
            return;
        }
        base.OnHitEvent(damage, Atktransform, atk_type);
        //KnockBack 전 캐릭터 회전
        if (transform.position.x - Atktransform.position.x > 0)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.back);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
        StartCoroutine(Knockback());
        if (_stat.Hp <= 0 && !_isDead)
        {
            // 죽는 모션 이후에 삭제 플레이어는 팝업
            State = Define.State.Die;
            Managers.UI.ShowPopupUI<UI_Dead>();
            _isDead = true;
            anim.SetTrigger("IsDead");
            Managers.Sound.Play($"Effect/Death/{_weapon.Name}_Death");
            return;
        }
    }
    public void ResetPlayer()
    {
        _wpAbility.ResetValue();
        _stat.Hp = _stat.MaxHp;
        _isDead = false;
        _isDamage = false;
        Data.Weapon wp;
        if (weaponDict.TryGetValue("Punch", out wp))
            Managers.Weapon.ChangeWeapon(gameObject, wp);
        State = Define.State.Idle;
        anim.runtimeAnimatorController = Resetanimator;
        anim.SetTrigger("ReturnBase");
    }
    public void SceneChange()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
        GameObject grid = GameObject.Find("Grid");
        if (grid != null)
            _platform = Util.FindChild(grid, "Platform", true).GetOrAddComponent<PlatformEffector2D>();
    }
    IEnumerator Attack()
    {
        _lastAttack = 0.0f;
        int mask = 1 << 8;
        bool Crit = false;
        if ((100 - _wpAbility.CritPer) < Random.Range(1, 101))
        {
            Crit = true;
        }
        if (_weapon.WpType == "melee")
        {
            // 데미지 계산식 따로 리팩토링
            Vector2 look = transform.TransformDirection(Vector3.right);
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, look, _weapon.Range, mask);
            //사운드 설정
            if (hits.Length > 0)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    BaseController monster = hit.collider.gameObject.GetComponent<BaseController>();
                    if (Crit)
                    {
                        int HitDamage = (int)(_attackAnim == 3 ? _weapon.Damage * 2 : _weapon.Damage * _wpAbility.CriticalDamage) + _stat.Attack + _wpAbility.AddDamage;
                        monster.OnHitEvent(HitDamage, transform, "Crit");
                    }
                    else
                    {
                        int HitDamage = (int)(_attackAnim == 3 ? _weapon.Damage * 2 : _weapon.Damage) + _stat.Attack + _wpAbility.AddDamage;
                        monster.OnHitEvent(HitDamage, transform);
                    }
                    if ((100 - _wpAbility.PoisonPer) < Random.Range(1, 101))
                        StartCoroutine(PoisonAttack(monster));
                    // 체력 빨아오는 Particle Effect
                    int StealHp = (_wpAbility.LifeSteal * (_weapon.Damage + _stat.Attack)) / 100;
                    _stat.Hp = _stat.Hp + StealHp <= _stat.MaxHp ? _stat.Hp + StealHp : _stat.MaxHp;
                }
            }
        }
        else if (_weapon.WpType == "range")
        {
            ShootBullet(Convert.ToInt32(Crit));
        }
        float atkCount = 1 / (_weapon.Rate + _wpAbility.AttackSpeed);
        float animTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        float t = 0;
        while((atkCount > animTime ? atkCount : animTime) > t)
        {
            t += Time.deltaTime;
            if(t >= animTime)
            {
                _stat.MoveSpeed = 5.0f;
                animTime = 99;
            }
            if(t >= atkCount)
            {
                _attackcount = 1;
                atkCount = 99;
            }
            if(animTime == 99 && atkCount == 99)
            {
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }
    IEnumerator CoolTime(float coolTime, string name)
    {
        float t = 0;
        while (coolTime >= t)
        {
            t += Time.deltaTime;

        }
        if (name == "count")
        {
            _attackcount = 1;
        }
        else if (name == "speed")
        {
            _stat.MoveSpeed = 5.0f;
        }
        yield return null;
    }
    void ShootBullet(int Crit)
    {
        Vector2 look = transform.TransformDirection(Vector3.right);
        GameObject go = Managers.Resource.Instantiate($"Bullet/{_weapon.Name}Bullet");
        //go.transform.parent = gameObject.transform;
        go.transform.position = transform.position + new Vector3(look.x, 0.0f, 0.0f);
        go.transform.rotation = transform.rotation;
        int HitDamage;
        if (Crit != 0)
        {
            HitDamage = (int)(_weapon.Damage * _wpAbility.CriticalDamage) + _stat.Attack + _wpAbility.AddDamage;
        }
        else
        {
            HitDamage = _weapon.Damage + _stat.Attack + _wpAbility.AddDamage;
        }
        go.GetComponent<Bullet>().Damage = HitDamage;
        //go.GetComponent<Bullet>().Crit = Convert.ToBoolean(Crit);
        Rigidbody2D rigid = go.GetOrAddComponent<Rigidbody2D>();
        rigid.AddForce(look * _weapon.Speed);
    }
    public IEnumerator PoisonAttack(BaseController monster)
    {
        float coolTime = 3;
        while (coolTime > 0)
        {
            if (monster == null)
            {
                yield break;
            }
            monster.OnHitEvent(5, transform, "Poison");
            coolTime--;
            yield return new WaitForSeconds(1);
        }
        yield break;
    }
    void Dash()
    {
        _dashCount++;
    }
    IEnumerator Knockback()
    {
        State = Define.State.Knockback;
        anim.SetTrigger("IsKnockBack");
        _rigid.velocity = new Vector2(0, 1) * 3;
        //SpriteRenderer currentSprite = GetComponent<SpriteRenderer>();
        //currentSprite.color = new Color(1, 1, 1, 0.4f);
        Managers.Sound.Play($"Effect/KnockBack/{_weapon.Name}_KnockBack");
        _isDamage = true;
        yield return new WaitForSeconds(1.5f);
        _isDamage = false;
        if (!_isDead)
            State = Define.State.Idle;
        //currentSprite.color = new Color(1, 1, 1, 1);
    }
    void AttackStartEvent()
    {

    }
    void AttackEndEvent()
    {
        //애니메이션이 끝났을 때
        _stat.MoveSpeed = 5.0f;
    }
    IEnumerator ASkillStartEvent()
    {
        Skill skill = gameObject.GetOrAddComponent<Skill>();
        Data.Skill askill;
        skillDict.TryGetValue($"{_weapon.Name}A", out askill);
        skill.UseASkill(_weapon.Name, askill.damage);
        float coolTime = askill.coolTime / _wpAbility.SkillSpeed;
        Managers.Sound.Play($"Effect/Skill/{_weapon.Name}/A");
        GameObject.Find("UI_Skill").GetOrAddComponent<UI_Skill>().ASkillEvent(coolTime);
        yield return new WaitForSeconds(coolTime);
        _askillCount++;
        yield return null;
    }
    IEnumerator SSkillStartEvent()
    {
        Skill skill = gameObject.GetOrAddComponent<Skill>();
        Data.Skill sskill;
        skillDict.TryGetValue($"{_weapon.Name}S", out sskill);
        skill.UseSSkill(_weapon.Name, sskill.damage);
        float coolTime = sskill.coolTime / _wpAbility.SkillSpeed;
        Managers.Sound.Play($"Effect/Skill/{_weapon.Name}/S");
        GameObject.Find("UI_Skill").GetOrAddComponent<UI_Skill>().SSkillEvent(coolTime);
        yield return new WaitForSeconds(coolTime);
        _sskillCount++;
        yield return null;
    }
    void OnKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Managers.UI.GetPopupStack() != 0)
            {
                Managers.UI.ClosePopupUI();
                if (Managers.UI.GetPopupStack() == 0)
                {
                    Time.timeScale = 1.0f;
                }
                return;
            }
            else if (Time.timeScale != 0)
            {
                Managers.UI.ShowPopupUI<UI_Pause>();
                Time.timeScale = 0.0f;
            }
        }
        if (State == Define.State.Die || State == Define.State.Knockback || Time.timeScale == 0 || State == Define.State.EventMove)
        {
            return;
        }
        if (!anim.GetBool("IsLadder"))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                //캐릭터를 돌려서 앞, 뒤를 보게해줌
                transform.rotation = Quaternion.LookRotation(Vector3.back);
                _dash.transform.rotation = Quaternion.LookRotation(Vector3.forward);
                State = Define.State.Moving;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.rotation = Quaternion.LookRotation(Vector3.forward);
                _dash.transform.rotation = Quaternion.LookRotation(Vector3.forward);
                State = Define.State.Moving;
            }
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _stat.JumpCount = 2;
            int mask = 1 << (int)Define.Layer.Ladder;
            BoxCollider2D Boxcol = GetComponent<BoxCollider2D>();
            RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0.0f, 1.0f), Vector2.down, 0.5f, mask);
            if (!hit)
            {
                anim.SetBool("IsLadder", false);
                _rigid.gravityScale = 3.0f;
                if (_platform.colliderMask != ~0)
                {
                    _platform.colliderMask = ~0;
                }
                return;
            }
            else
            {
                RaycastHit2D laddercheck = Physics2D.Raycast(transform.position, Vector2.down, 1.49f, mask);
                if (!laddercheck)
                {
                    transform.position += new Vector3(0, 0.015f, 0.0f);
                }
                _rigid.gravityScale = 0;
                State = Define.State.Idle;
                _rigid.velocity = new Vector2(0.0f, 0.0f);
                if (!anim.GetBool("IsLadder"))
                    anim.SetTrigger("Laddering");
                anim.SetBool("IsLadder", true);
                transform.position = new Vector3(Mathf.Floor(transform.position.x) + 0.5f, transform.position.y, 0.0f);
                transform.Translate(Vector3.up * Time.deltaTime * 3.5f);
                _rigid.gravityScale = 0;
                anim.SetFloat("LadderSpeed", 10.0f);
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (_isLadder)
            {
                _stat.JumpCount = 2;
                int mask = 1 << (int)Define.Layer.Ladder;
                BoxCollider2D Boxcol = GetComponent<BoxCollider2D>();
                RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0.0f, 1.55f), Vector2.down, 1.0f, mask);
                if (!hit)
                {
                    anim.SetBool("IsLadder", false);
                    _rigid.gravityScale = 3.0f;
                    return;
                }
                else
                {
                    _platform.colliderMask = ~(1 << (int)Define.Layer.Player);
                    State = Define.State.Idle;
                    _rigid.velocity = new Vector2(0.0f, 0.0f);
                    if (!anim.GetBool("IsLadder"))
                        anim.SetTrigger("Laddering");
                    anim.SetBool("IsLadder", true);
                    transform.position = new Vector3(Mathf.Floor(transform.position.x) + 0.5f, transform.position.y, 0.0f);
                    transform.Translate(Vector3.down * Time.deltaTime * 3.5f);
                    anim.SetFloat("LadderSpeed", 10.0f);
                    _rigid.gravityScale = 0;
                }
            }
        }
        if ((Input.GetKeyDown(KeyCode.C)) && !(Input.GetKey(KeyCode.DownArrow)))
        {
            _rigid.gravityScale = 3.0f;
            State = Define.State.Jumping;
            anim.SetBool("IsLadder", false);
            if (_stat.JumpCount != 0)
            {
                Vector2 jumpVelocity = new Vector2(0, 1);
                //_rigid.AddForce(jumpVelocity * _stat.JumpPower, ForceMode2D.Impulse);
                jumpSound = jumpSound == 1 ? 2 : 1;
                Managers.Sound.Play($"Effect/Jump/{_weapon.Name}_Jump{jumpSound}");
                _rigid.velocity = Vector2.up * _stat.JumpPower;
                _stat.JumpCount--;

            }
        }
        if (_rigid.gravityScale != 0)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (_attackcount > 0)
                {
                    bool jumping = anim.GetBool("IsJumping") || anim.GetBool("IsFalling");
                    if (jumping)
                    {
                        if (_jumpAttack)
                        {
                            _jumpAttack = false;
                            JumpAttack jump = gameObject.GetOrAddComponent<JumpAttack>();
                            jump.Attack(_weapon.Name, (int)(_weapon.Damage * _wpAbility.AttackRatio));
                            State = Define.State.Attack;
                            Managers.Sound.Play($"Effect/Attack/{_weapon.Name}/1");
                            anim.SetFloat("AttackAnim", 6);
                        }
                        return;
                    }
                    _attackcount--;
                    _stat.MoveSpeed = 1.0f;
                    State = Define.State.Attack;
                    anim.SetFloat("AttackAnim", _attackAnim++ < 3 ? _attackAnim : _attackAnim = 1);
                    StartCoroutine(Attack());
                    switch (_attackAnim)
                    {
                        case 1:
                            Managers.Sound.Play($"Effect/Attack/{_weapon.Name}/1");
                            break;
                        case 2:
                            Managers.Sound.Play($"Effect/Attack/{_weapon.Name}/2");
                            break;
                        case 3:
                            Managers.Sound.Play($"Effect/Attack/{_weapon.Name}/3");
                            break;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (_dashCount > 0)
                {
                    anim.SetTrigger("IsDashing");
                    Invoke("Dash", 5);
                    _dash.StartDash();
                    Managers.Sound.Play($"Effect/Dash/{_weapon.Name}_Dash");
                    StartCoroutine(AfterImage(0.08f));
                    _dashCount--;
                }
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (_askillCount > 0)
                {
                    _askillCount--;
                    State = Define.State.Attack;
                    anim.SetFloat("AttackAnim", 4);
                    StartCoroutine(ASkillStartEvent());
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (_sskillCount > 0)
                {
                    _sskillCount--;
                    State = Define.State.Attack;
                    anim.SetFloat("AttackAnim", 5);
                    StartCoroutine(SSkillStartEvent());
                }
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_triggerCol.Count > 0)
                {
                    Collider2D collision = _triggerCol[0];
                    if (_triggerCol[0].gameObject.layer == (int)Define.Layer.WeaponItem)
                    {
                        //A, S Skill Icon 교체
                        Destroy(collision.gameObject);
                        _triggerCol.Remove(collision);
                        Data.Weapon wp;
                        if (weaponDict.TryGetValue(collision.name, out wp))
                        {
                            WeaponItem wpItem = collision.gameObject.GetOrAddComponent<WeaponItem>();
                            anim.runtimeAnimatorController = wpItem.animator;
                            Managers.Weapon.ChangeWeapon(gameObject, wp);
                            GameObject.Find("UI_Skill").GetOrAddComponent<UI_Skill>().ImageChange(_weapon.Name);
                            Managers.Item.CloseItem();
                            //WeaponManager로 이동
                        }
                    }
                    else if (_triggerCol[0].gameObject.layer == (int)Define.Layer.NPC)
                    {
                        if (_triggerCol[0].gameObject.name == "GotchaMachine")
                        {
                            Managers.Item.OpenItem();
                            Managers.Sound.Play("Effect/Structure/Gotcha");
                            _triggerCol[0].GetComponent<BoxCollider2D>().enabled = false;
                            return;
                        }
                        else if (_triggerCol[0].gameObject.name == "ShopNPC")
                        {
                            UI_Dialog dialog = Managers.UI.MakeWorldSpaceUI<UI_Dialog>(collision.transform);
                            _currentNPC = collision.gameObject.GetComponent<NPCController>();
                            //NPC Code 받아와서 안에 넣기
                            dialog.SetTalk(1000, true, _currentNPC);
                            //dialog.SetTalk(100, false, _currentNPC);
                            Managers.Input.KeyAction -= OnKeyboard;
                        }
                        else if (_triggerCol[0].gameObject.name == "A_npc" && !_triggerCol[0].gameObject.GetComponent<Animator>().GetBool("IsUnBind"))
                        {
                            _triggerCol[0].gameObject.GetComponent<Animator>().SetBool("IsUnBind", true);
                            _triggerCol[0].gameObject.GetComponent<Animator>().SetTrigger("UnBind");
                            Destroy(_triggerCol[0].transform.Find("UI_Press").gameObject);
                        }
                        else if (_triggerCol[0].gameObject.name == "QuestNPC")
                        {
                            _currentNPC = collision.gameObject.GetComponent<NPCController>();
                            _currentNPC.ShowUI();
                            Managers.Input.KeyAction -= OnKeyboard;
                        }
                    }
                    else if (_triggerCol[0].gameObject.layer == (int)Define.Layer.Item)
                    {
                        // Item에서 받아오기
                        ChangeWeaponStat(collision.name, 0);
                        _triggerCol.Remove(collision);
                        // 아이템 지우는 매니저
                        Managers.Item.CloseItem();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                SaveStatData();
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                LoadData();
            }
            if(Input.GetKeyDown(KeyCode.O) && Input.GetKey(KeyCode.RightShift))
            {
                SceneManager.LoadScene("Stage1-Bonus");
            }
        }
    }
    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (evt != Define.MouseEvent.Click)
            return;
        /*
        int mask = (1 << 8) | (1 << 9);
        Vector2 look = transform.TransformDirection(Vector3.right);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, look, 10.0f, mask);
        Debug.DrawRay(transform.position, look + new Vector2(10.0f, 0.0f), Color.red);
        if (hit)
        {
            Debug.Log($"Raycast @ {hit.collider.gameObject.name}");
        }*/
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (State == Define.State.Die)
        {
            return;
        }
        if ((collision.gameObject.layer == (int)Define.Layer.Ground || collision.gameObject.layer == (int)Define.Layer.Platform) && _rigid.velocity.y <= 0 && !_isGround)
        {
            _stat.JumpCount = 2;
            _jumpAttack = true;
            State = Define.State.Idle;
            Managers.Sound.Play("Effect/FootStep/Land", Define.Sound.FootStep);
        }
        if (!_triggerCol.Find(getCol => getCol.name == collision.name))
        {
            if (collision.gameObject.layer == (int)Define.Layer.WeaponItem)
            {
                _triggerCol.Add(collision);
                UI_ItemAlert itemAlert = Managers.UI.MakeWorldSpaceUI<UI_ItemAlert>(collision.transform);
                itemAlert.SetWpInfo(collision.gameObject);
            }
            if (collision.gameObject.layer == (int)Define.Layer.NPC)
            {
                _triggerCol.Add(collision);
                UI_Press press = Managers.UI.MakeWorldSpaceUI<UI_Press>(collision.transform);
                if (collision.name == "GotchaMachine")
                {
                    press.SetPosition(collision.transform.position + new Vector3(-0.7f, 0.0f));
                }
                else
                {
                    press.SetPosition(collision.transform.position + new Vector3(0, 1.5f));
                }
            }
            if (collision.gameObject.layer == (int)Define.Layer.Item)
            {
                _triggerCol.Add(collision);
                UI_ItemAlert itemAlert = Managers.UI.MakeWorldSpaceUI<UI_ItemAlert>(collision.transform);
                itemAlert.SetInfo(collision.gameObject.GetComponent<Item>());
            }
        }
        if (collision.gameObject.layer == (int)Define.Layer.Exit)
        {
            if (collision.gameObject.GetComponent<BoxCollider2D>().enabled)
            {
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(End_Scene(collision.name));
            }
        }
        if (collision.gameObject.layer == (int)Define.Layer.Door)
        {
            if (Managers.Game.MonCount == 0)
            {
                collision.GetComponent<Animator>().SetTrigger("IsCome");
                collision.GetComponent<BoxCollider2D>().enabled = false;
                Managers.Sound.Play("Effect/Structure/Door");
            }
        }
        if (collision.gameObject.layer == (int)Define.Layer.Ladder)
        {
            _isLadder = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Define.Layer.WeaponItem)
        {
            _triggerCol.Remove(collision);
            GameObject go = Util.FindChild(collision.gameObject, "UI_ItemAlert");
            Managers.Resource.Destroy(go);
        }
        if (collision.gameObject.layer == (int)Define.Layer.NPC)
        {
            _triggerCol.Remove(collision);
            if (collision.transform.Find("UI_Press"))
            {
                Destroy(collision.transform.Find("UI_Press").gameObject);
            }
        }
        if (collision.gameObject.layer == (int)Define.Layer.Item)
        {
            _triggerCol.Remove(collision);
            GameObject go = Util.FindChild(collision.gameObject, "UI_ItemAlert");
            Managers.Resource.Destroy(go);
        }
        if (collision.gameObject.layer == (int)Define.Layer.Ladder)
        {
            _isLadder = false;
            anim.SetBool("IsLadder", false);
            _platform.colliderMask = ~0;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == (int)Define.Layer.Ground || collision.gameObject.layer == (int)Define.Layer.Platform) && _rigid.velocity.y <= 0 && !_isGround)
        {
            _stat.JumpCount = 2;
            if (!_weapon.Name.Equals("Water"))
            {
                if (State == Define.State.Moving)
                {
                    _moveTime += Time.deltaTime;
                    if (_moveTime > 0.4f)
                    {

                        footstep = footstep == 1 ? 2 : 1;
                        Managers.Sound.Play($"Effect/FootStep/Punch_Run{footstep}", Define.Sound.FootStep);
                        _moveTime = 0.0f;
                    }
                }
            }
        }
        if (collision.gameObject.layer == (int)Define.Layer.Trap && gameObject.layer == (int)Define.Layer.Player)
        {
            OnHitEvent(10, collision.transform);
        }
        if (collision.gameObject.layer == (int)Define.Layer.Exit)
        {
            if (collision.gameObject.GetComponent<BoxCollider2D>().enabled)
            {
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(End_Scene(collision.name));
            }
        }
        if (collision.gameObject.layer == (int)Define.Layer.Door)
        {
            if (Managers.Game.MonCount == 0)
            {
                collision.GetComponent<Animator>().SetTrigger("IsCome");
                collision.GetComponent<BoxCollider2D>().enabled = false;
                Managers.Sound.Play("Effect/Structure/Door");
            }
        }
    }
    void IsFalling()
    {
        BoxCollider2D Boxcol = GetComponent<BoxCollider2D>();
        int mask = 1 << 17;
        Vector2 size = new Vector2(Boxcol.size.x, Boxcol.size.y / 4.0f);
        ExtDebug.DrawBoxCast2D(transform.position - new Vector3(0.0f, (Boxcol.size.y * 3 / 8) + 0.25f), size, 0.0f, Vector2.up, 0.0f, Color.red);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position - new Vector3(0.0f, (Boxcol.size.y * 3 / 8) + 0.25f), size, 0.0f, Vector2.up, 0.0f, mask);
        if (hit)
        {
            _isGround = true;
        }
        else
        {
            _isGround = false;
        }
        if (_rigid.velocity.y < -0.5f)
        {
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", true);
        }
        else if (_rigid.velocity.y == 0)
        {
            anim.SetBool("IsFalling", false);
            anim.SetBool("IsJumping", false);
        }
        else if (_rigid.velocity.y >= 0)
        {
            anim.SetBool("IsFalling", false);
        }
    }
    public void ChangeStat(string content, int value)
    {
        switch (content)
        {
            case "Attack":
                _stat.Attack += value;
                break;
            case "Hp":
                _stat.MaxHp += value;
                _stat.Hp = _stat.MaxHp;
                break;
            case "Defense":
                _stat.Defense += value;
                break;
        }
    }
    public void CloseNPC()
    {
        Managers.Input.KeyAction += OnKeyboard;
        _currentNPC.CloseUI();
        _currentNPC = null;
    }
    public void ChangeWeaponStat(string content, float value)
    {
        switch (content)
        {
            //이후에 Value로 전부 수정
            case "AttackSpeed":
                _wpAbility.AttackSpeed += value;
                break;
            case "AttackRatio":
                _wpAbility.AttackRatio += value;
                break;
            case "SkillRatio":
                _wpAbility.SkillRatio += value;
                break;
            case "SkillSpeed":
                _wpAbility.SkillSpeed += value;
                break;
            case "CriticalPer":
                _wpAbility.CritPer += (int)value;
                break;
            case "AddDamage":
                _wpAbility.AddDamage += (int)value;
                break;
            case "PoisonPer":
                _wpAbility.PoisonPer += (int)value;
                break;
            case "LifeSteal":
                _wpAbility.LifeSteal += (int)value;
                break;
            case "CriticalDamage":
                _wpAbility.CriticalDamage += value;
                break;
            case "HealthRegen":
                _wpAbility.HealthRegen += (int)value;
                break;
            case "Defense":
                _stat.Defense += (int)value;
                break;
        }
    }
    IEnumerator AfterImage(float coolTime)
    {
        int i = 0;
        _isDamage = true;
        while (coolTime > 0)
        {
            if (i % 2 == 0)
            {
                GameObject currentGhost = Managers.Resource.Instantiate($"Ghost/Ghost", transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                Destroy(currentGhost, 1f);
                coolTime -= Time.deltaTime;
            }
            i++;
            yield return new WaitForFixedUpdate();
        }
        _isDamage = false;
        yield return null;
    }
    IEnumerator End_Scene(string name)
    {
        UI_Fade.FadeOut();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(name);
        SceneChange();
        yield return null;
    }
    void SaveStatData()
    {
        Managers.Data.SaveFile.id = _stat.ID;
        Managers.Data.SaveFile.attack = _stat.Attack;
        Managers.Data.SaveFile.soul = _stat.Soul;
        Managers.Data.SaveFile.hp = _stat.MaxHp;
        Managers.Data.SaveData();
    }
    void LoadData()
    {
        _stat.Attack = Managers.Data.SaveFile.attack;
        _stat.Soul = Managers.Data.SaveFile.soul;
        _stat.MaxHp = Managers.Data.SaveFile.hp;
    }
}
