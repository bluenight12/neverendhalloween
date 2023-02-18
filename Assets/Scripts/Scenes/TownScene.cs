using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TownScene : BaseScene
{
    // 시작 전에 Player 초기화 HP와 무기
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Town;
        ResetPlayer();
        UI_Fade.FadeIn();
        Managers.UI.ShowSceneUI<UI_Status>();
        Managers.UI.ShowSceneUI<UI_Skill>();
    }
    void ResetPlayer()
    {
        PlayerController player = Managers.Game.GetPlayer().GetOrAddComponent<PlayerController>();
        player.SceneChange();
        player.ResetPlayer();
        GameObject go = Util.FindChild(gameObject, "Start");
        player.gameObject.transform.position = go.transform.position;
    }
    void Update()
    {
    }
    public override void Clear()
    {
        
    }
}
