using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Setting : UI_Popup
{
    enum GameObjects
    {
        Back,
        Graphic,
        Audio,
        Data,
        Language,
        Difficulty
    }
    Color textColor = new Color32(132, 146, 172, 255);
    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        for (int i = 0; i < 6; i++)
        {
            GameObject go = Get<GameObject>(i);
            BindEvent(go, (PointerEventData data) => { go.GetComponent<TextMeshProUGUI>().color = Color.white; }, Define.UIEvent.Enter);
            BindEvent(go, (PointerEventData data) => { go.GetComponent<TextMeshProUGUI>().color = textColor; }, Define.UIEvent.Exit);
            BindEvent(go, (PointerEventData) => { Managers.Sound.Play("Effect/UI/Hover"); }, Define.UIEvent.Enter);
            BindEvent(go, (PointerEventData) => { Managers.Sound.Play("Effect/UI/Click"); });
        }
        GetObject((int)GameObjects.Back).BindEvent(OnBackClick);
        GetObject((int)GameObjects.Graphic).BindEvent(OnGraphicClick);
        GetObject((int)GameObjects.Audio).BindEvent((PointerEventData) => { Managers.UI.ShowPopupUI<UI_Setting_Sound>(); });
        GetObject((int)GameObjects.Data).BindEvent(OnDataClick);
        GetObject((int)GameObjects.Language).BindEvent(OnLanguageClick);
        GetObject((int)GameObjects.Difficulty).BindEvent(OnDifficultyClick);
    }
    void OnBackClick(PointerEventData data)
    {
        Managers.UI.ClosePopupUI();
    }
    // ªı ∞‘¿”
    void OnGraphicClick(PointerEventData data)
    {

    }
    void OnLanguageClick(PointerEventData data)
    {

    }
    void OnDataClick(PointerEventData data)
    {

    }
    void OnDifficultyClick(PointerEventData data)
    {

    }
}
