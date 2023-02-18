using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game;
        Managers.UI.ShowSceneUI<UI_Status>();
        Managers.UI.ShowSceneUI<UI_Skill>();

        //PlayerController player = Managers.Game.GetPlayer().GetOrAddComponent<PlayerController>();
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        GameObject go = Util.FindChild(gameObject, "Start");
        player.gameObject.transform.position = go.transform.position;
        //Managers.Sound.Play("Bgm/Undertale", Define.Sound.Bgm);
        //Managers.UI.ShowSceneUI<UI_Inven>();
        //Managers.UI.ShowPopupUI<UI_Button>();
    }
    public override void Clear()
    {

    }

    void Update()
    {
        /*i += Time.deltaTime;
        if (i > 2)
        {
            GameObject go = Managers.Game.Spawn(Define.WorldObject.Monster, "Slime");
            go.transform.position = Util.FindChild(gameObject, "Start").transform.position;
            i = 0;
        }*/
    }
}
