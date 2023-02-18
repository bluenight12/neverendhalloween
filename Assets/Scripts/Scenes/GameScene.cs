using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    bool _monCheck = false;
    GameObject monster; 
    protected override void Init()
    {
        base.Init();
        StartCoroutine(Game_Start());
        SceneType = Define.Scene.Game;
        Managers.UI.ShowSceneUI<UI_Status>();
        UI_Skill skill = Managers.UI.ShowSceneUI<UI_Skill>();
        Managers.UI.ShowSceneUI<UI_MonCount>();
        UI_QuestAlert quest = Managers.UI.ShowSceneUI<UI_QuestAlert>();
        Managers.Game._monsterDie -= quest.UpdateQuestList;
        Managers.Game._monsterDie += quest.UpdateQuestList;
        PlayerController player = Managers.Game.GetPlayer().GetOrAddComponent<PlayerController>();
        player.GetComponent<Animator>().ResetTrigger("ReturnBase");
        string wpname = player.GetComponent<Weapon>().Name;
        skill.ImageChange(wpname);
        GameObject go = Util.FindChild(gameObject, "Start");
        player.SceneChange();
        player.gameObject.transform.position = go.transform.position;
        transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = false;
        GameObject spawn = GameObject.Find("MonsterSpawn");
        if (spawn != null)
        {
            foreach (Transform child in spawn.transform)
            {
                monster = Managers.Game.Spawn(Define.WorldObject.Monster, $"Monster/{child.name}");
                monster.transform.position = child.position;
            }
        }
        //Managers.UI.ShowSceneUI<UI_Inven>();
        //Managers.UI.ShowPopupUI<UI_Button>();
    }
    public override void Clear()
    {
        Managers.Game.MonsterClear();
    }
    
    void Update()
    {
        if (Managers.Game.MonCount == 0 && !_monCheck)
        {
            transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = true;
            _monCheck = true;
        }
    }
    IEnumerator Game_Start()
    {
        yield return new WaitForSeconds(1);
        UI_Fade.FadeIn();
        Managers.Sound.Play("Bgm/Stage1BGM", Define.Sound.Bgm);
        yield return null;
    }
}
