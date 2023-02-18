using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Scene : BaseScene
{
    PlayerController player;
    bool event1 = false;
    bool event2 = false;
    bool event3 = false;
    bool eventEnd = false;
    bool sceneEnd = false;
    UI_ItemAlert alert;
    UI_Dialog dialog;
    GameObject boss;

    protected override void Init()
    {
        base.Init();
        StartCoroutine(Game_Start());
        SceneType = Define.Scene.Game;
        Managers.UI.ShowSceneUI<UI_Status>();
        UI_Skill skill = Managers.UI.ShowSceneUI<UI_Skill>();
        player = Managers.Game.GetPlayer().GetOrAddComponent<PlayerController>();
        PlayerStat p_stat = player.GetComponent<PlayerStat>();
        p_stat.Hp = p_stat.MaxHp;
        //player = GameObject.Find("Player").GetComponent<PlayerController>();
        player.GetComponent<Animator>().ResetTrigger("ReturnBase");
        string wpname = player.GetComponent<Weapon>().Name;
        skill.ImageChange(wpname);
        GameObject go = Util.FindChild(gameObject, "Start");
        player.SceneChange();
        player.gameObject.transform.position = go.transform.position;

    }
    void Update()
    {
        
        if(player.transform.position.x > 7 && !event1)
        {
            event1 = true;
            StartCoroutine(Event1());
        }
        if(event1 && Input.GetKeyDown(KeyCode.F) && alert != null)
        {
            Managers.Resource.Destroy(alert.gameObject);
        }
        if(player.transform.position.x > 26)
        {
            if (!event2)
            {
                event2 = true;
                StartCoroutine(Event2());
            }
        }
        if(event1 && event2 && !Managers.Talk.isTalking && event3 && !eventEnd)
        {
            eventEnd = true;
            player.State = Define.State.Idle;
            boss.GetComponent<Boss1Controller>().State = Define.State.Idle;
        }
        if (eventEnd)
        {
            if (!sceneEnd)
            {
                if (boss.GetComponent<Stat>().Hp <= 0)
                {
                    //끝나고 대사넣기
                    sceneEnd = true;
                    UI_Alert alert = Managers.UI.ShowPopupUI<UI_Alert>();
                    alert.PopupCheckWindowOpen(GoTown, "Clear", "Clear ! 수고하셨습니다");
                }
            }
        }
    }
    void Talk()
    {
        Managers.Talk.isTalking = true;
        dialog = Managers.UI.MakeWorldSpaceUI<UI_Dialog>(boss.transform);
        if (dialog != null)
        {
            dialog.SetTalk(3100);
        }
    }
    IEnumerator Event1()
    {
        GameObject go = Managers.Resource.Load<GameObject>($"Prefabs/Structure/Stage1/Message");
        go.GetOrAddComponent<Item>().SetInfo("Message", 0, 0, "여긴 대체 어딜까\n우리 연구소 직원들은 어디로 간 거지..\n나 혼자 여기로 온 걸까?\n빨리 집에 가고 싶어...", "알 수 없는 쪽지");
        alert = Managers.UI.MakeWorldSpaceUI<UI_ItemAlert>(GameObject.Find("BookShelf").transform);
        alert.SetInfo(go.GetComponent<Item>());
        yield return null;
    }
    IEnumerator Event2()
    {
        GameObject spawn = GameObject.Find("MonsterSpawn");
        foreach (Transform child in spawn.transform)
        {
            boss = Managers.Game.Spawn(Define.WorldObject.Monster, $"Monster/{child.name}");
            boss.transform.position = child.position;
        }
        boss.GetComponent<Boss1Controller>().State = Define.State.Die;
        player.GetComponent<Animator>().SetFloat("Speed", 0);
        player.State = Define.State.Die;
        UI_Fade.FadeOut();
        yield return new WaitForSeconds(1);
        player.transform.position = boss.transform.position - new Vector3(4.0f, 0.0f, 0.0f);
        yield return new WaitForSeconds(1);
        UI_Fade.FadeIn();
        Talk();
        event3 = true;
        yield return null;
    }
    IEnumerator Game_Start()
    {
        yield return new WaitForSeconds(1);
        UI_Fade.FadeIn();
        Managers.Sound.Play("Bgm/Boss1BGM", Define.Sound.Bgm);
        yield return null;
    }
    void GoTown()
    {
        Managers.Scene.LoadScene(Define.Scene.Town);
        Managers.UI.CloseAllPopupUI();
    }
    public override void Clear()
    {
        event1 = false;
        event2 = false;
        event3 = false;
        eventEnd = false;
        sceneEnd = false;
        Managers.Game.MonsterClear();
    }
}
