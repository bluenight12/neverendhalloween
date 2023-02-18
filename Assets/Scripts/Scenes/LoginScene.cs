using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    bool isPopup = false;
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Login;
        if(!GameObject.Find("UI_Fade"))
            Managers.Resource.Instantiate("UI/UI_Fade");
        UI_Fade.FadeIn();
        if (Managers.Game.GetPlayer())
        {
            Managers.Game.GetPlayer().GetComponent<PlayerController>().ResetPlayer();
            Managers.Game.GetPlayer().transform.position = Vector3.zero;
        }
    }
    private void Update()
    {
        
        if (Input.anyKeyDown && !isPopup)
        {
            isPopup = true;
            UI_Alert alert = Managers.UI.ShowPopupUI<UI_Alert>();
            alert.PopupCheckWindowOpenClose(NonSkip, Skip, "NonSkip", "Skip", "튜토리얼을 진행 하시겠습니까?");
            UI_Fade.FadeOut();
        }
    }
    void NonSkip()
    {
        Managers.Scene.LoadScene(Define.Scene.Intro);
    }
    void Skip()
    {
        Managers.Scene.LoadScene(Define.Scene.Town);
        if(!Managers.Game.GetPlayer())
            Managers.Game.Spawn(Define.WorldObject.Player, "Player");
    }
    public override void Clear()
    {
        Debug.Log("Login Scene Clear!");
        //Managers.Game.Spawn(Define.WorldObject.Player, "Player");
    }
}
