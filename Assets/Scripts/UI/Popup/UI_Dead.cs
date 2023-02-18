using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Dead : UI_Popup
{
    enum GameObjects
    {
        DeadText,
        ReturnText
    }
    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));

        GetObject((int)GameObjects.ReturnText).gameObject.BindEvent(HomeReturn);
    }
    public void HomeReturn(PointerEventData eventData)
    {
        Managers.Sound.Play("Effect/UI/Confirm");
        Managers.Scene.LoadScene(Define.Scene.Town);
    }
}
