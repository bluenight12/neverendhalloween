using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        UI_Fade.FadeIn();
        SceneType = Define.Scene.Bonus;
        Managers.Sound.Play("Bgm/BonusBGM", Define.Sound.Bgm);
        Managers.UI.ShowSceneUI<UI_Status>();
        UI_Skill skill = Managers.UI.ShowSceneUI<UI_Skill>();
        PlayerController player = Managers.Game.GetPlayer().GetOrAddComponent<PlayerController>();
        string wpname = player.GetComponent<Weapon>().Name;
        skill.ImageChange(wpname);
        GameObject go = Util.FindChild(gameObject, "Start");
        player.SceneChange();
        player.gameObject.transform.position = go.transform.position;
    }
    
    public override void Clear()
    {

    }

}
