using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Pause : UI_Popup
{
    enum GameObjects
    {
        Back,
        Restart,
        Home,
        Control,
        Setting,
        Exit
    }
    Color textColor = new Color32(132, 146, 172, 255);
    public override void Init()
    {
        base.Init();
        Managers.Sound.Play("Effect/UI/ESC");
        Bind<GameObject>(typeof(GameObjects));
        for (int i = 0; i < 6; i++) {
            GameObject go = Get<GameObject>(i);
            BindEvent(go, (PointerEventData data) => { go.GetComponent<TextMeshProUGUI>().color = Color.white; }, Define.UIEvent.Enter);
            BindEvent(go, (PointerEventData data) => { go.GetComponent<TextMeshProUGUI>().color = textColor; }, Define.UIEvent.Exit);
            BindEvent(go, (PointerEventData) => { Managers.Sound.Play("Effect/UI/Hover"); }, Define.UIEvent.Enter);
            BindEvent(go, (PointerEventData) => { Managers.Sound.Play("Effect/UI/Click"); });
        }
        GetObject((int)GameObjects.Back).BindEvent(OnBackClick);
        GetObject((int)GameObjects.Restart).BindEvent(OnRestartClick);
        GetObject((int)GameObjects.Home).BindEvent(OnHomeClick);
        GetObject((int)GameObjects.Control).BindEvent(OnControlClick);
        GetObject((int)GameObjects.Setting).BindEvent((PointerEventData) => { Managers.UI.ShowPopupUI<UI_Setting>(); });
        GetObject((int)GameObjects.Exit).BindEvent(OnExitClick);
    }
    // 돌아가기
    void OnBackClick(PointerEventData data)
    {
        Time.timeScale = 1.0f;
        Managers.UI.ClosePopupUI();
    }
    // 새 게임
    void OnRestartClick(PointerEventData data)
    {
        UI_Alert alert = Managers.UI.ShowPopupUI<UI_Alert>();
        alert.PopupCheckWindowOpen(GoTown, "Town", "마을로 돌아갑니다");
    }
    void OnHomeClick(PointerEventData data)
    {
        UI_Alert alert = Managers.UI.ShowPopupUI<UI_Alert>();
        alert.PopupCheckWindowOpen(GoHome, "Home", "첫 화면으로 돌아갑니다");
    }
    void OnControlClick(PointerEventData data)
    {

    }
    void OnExitClick(PointerEventData data)
    {
        UI_Alert alert = Managers.UI.ShowPopupUI<UI_Alert>();
        alert.PopupCheckWindowOpen(GoExit, "Exit", "게임을 종료합니다");
    }
    void GoTown()
    {
        Managers.Scene.LoadScene(Define.Scene.Town);
        Managers.UI.CloseAllPopupUI();
        Time.timeScale = 1.0f;
    }
    void GoHome()
    {
        Managers.Scene.LoadScene(Define.Scene.Login);
        Managers.UI.CloseAllPopupUI();
        Time.timeScale = 1.0f;
    }
    void GoExit()
    {
        Application.Quit();
    }
}
