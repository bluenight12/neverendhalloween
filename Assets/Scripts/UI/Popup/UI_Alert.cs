using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class PopupCheckConfirmEventArgs : EventArgs
{
    public UnityEngine.Events.UnityAction events;
    public string eventName;
}

public class UI_Alert : UI_Popup {

    public static event EventHandler<PopupCheckConfirmEventArgs> ConfirmEvent;
    public static event EventHandler<PopupCheckConfirmEventArgs> CancelEvent;

    enum GameObjects
    {
        Text,
        OK,
        Cancel
    }
    
    TextMeshProUGUI _text;
    TextMeshProUGUI _ok;
    TextMeshProUGUI _cancel;

    Color textColor = new Color32(132, 146, 172, 255);
    public override void Init ()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        for (int i = 1; i < 3; i++)
        {
            GameObject go = Get<GameObject>(i);
            BindEvent(go, (PointerEventData data) => { go.GetComponent<TextMeshProUGUI>().color = Color.white; Managers.Sound.Play("Effect/UI/Hover"); }, Define.UIEvent.Enter);
            BindEvent(go, (PointerEventData data) => { go.GetComponent<TextMeshProUGUI>().color = textColor; }, Define.UIEvent.Exit);
        }
        _text = GetObject((int)GameObjects.Text).GetComponent<TextMeshProUGUI>();
        _ok = GetObject((int)GameObjects.OK).GetComponent<TextMeshProUGUI>();
        _cancel = GetObject((int)GameObjects.Cancel).GetComponent<TextMeshProUGUI>();
        ConfirmEvent = null;
        ConfirmEvent += OnConfirmOnClick;
        CancelEvent = null;
        CancelEvent += OnCancelOnClick;
    }

    public void CloseView()
    {
        Managers.UI.ClosePopupUI();
    }

    public void PopupCheckWindowOpen(UnityEngine.Events.UnityAction action, string actionName, string msg)
    {
        _ok.gameObject.BindEvent(delegate { ConfirmEvent(this, new PopupCheckConfirmEventArgs() { events = action, eventName = actionName }); });
        _cancel.gameObject.BindEvent((PointerEventData) => { CloseView(); });
        PopupWindowOpen(msg);
    }
    public void PopupCheckWindowOpenClose(UnityEngine.Events.UnityAction ok_action, UnityEngine.Events.UnityAction cancel_action, string ok_actionName, string cancel_actionName, string msg)
    {
        _ok.gameObject.BindEvent(delegate { ConfirmEvent(this, new PopupCheckConfirmEventArgs() { events = ok_action, eventName = ok_actionName }); });
        _cancel.gameObject.BindEvent(delegate { CancelEvent(this, new PopupCheckConfirmEventArgs() { events = cancel_action, eventName = cancel_actionName }); });
        PopupWindowOpen(msg);
    }
    public void OnConfirmOnClick(object sender, PopupCheckConfirmEventArgs e)
    {
        e.events();
        Managers.Sound.Play("Effect/UI/Confirm");
        CloseView();
    }
    public void OnCancelOnClick(object sender, PopupCheckConfirmEventArgs e)
    {
        e.events();
        Managers.Sound.Play("Effect/UI/Cancel");
        CloseView();
    }
    public void PopupWindowOpen(string msg)
    {
        _text.text = msg;
    }
}



/*using UnityEngine;
using UnityEngine.UI;
using System;

public class PopupCheckConfirmEventArgs : EventArgs
{
    public UnityEngine.Events.UnityAction events;
    public string eventName;
}

public class PopupWindow : MonoBehaviour
{

    public static PopupWindow instance;
    public static event EventHandler<PopupCheckConfirmEventArgs> ConfirmEvent;

    public enum MsgType { notice, error, warning, exit };

    public GameObject popupView;

    public Text m_MessageType;
    public Text m_Msg;
    public Button Btn_Exit;
    public Button Btn_OK;
    public Button Btn_Cancle;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;

        Btn_Exit.onClick.AddListener(CloseView);
        Btn_Cancle.onClick.AddListener(CloseView);
        ConfirmEvent += OnConfirmOnClick;
        if (transform.parent != null && transform.parent.GetComponent<Canvas>() != null)
            transform.SetSiblingIndex(transform.parent.childCount);
        else
            Debug.LogError("Popup Window 패키지가 Canvas 자식으로 설정되어있지 않습니다. Canvas 하위 자식으로 설정하세요.");

    }

    public void CloseView()
    {
        popupView.SetActive(false);
        Btn_OK.gameObject.SetActive(false);
        Btn_Cancle.gameObject.SetActive(false);
        Btn_OK.onClick.RemoveAllListeners();
    }

    public void PopupCheckWindowOpen(UnityEngine.Events.UnityAction action, string actionName, MsgType msgtype, string msg)
    {
        Btn_OK.gameObject.SetActive(true);
        Btn_Cancle.gameObject.SetActive(true);
        Btn_OK.onClick.AddListener(delegate { ConfirmEvent(this, new PopupCheckConfirmEventArgs() { events = action, eventName = actionName }); });
        PopupWindowOpen(msgtype, msg);
    }

    public void OnConfirmOnClick(object sender, PopupCheckConfirmEventArgs e)
    {
        e.events();
        Btn_OK.onClick.RemoveAllListeners();
        CloseView();
    }


    public void PopupWindowOpen(MsgType msgtype, string msg)
    {
        popupView.SetActive(true);

        if (msgtype == MsgType.error)
        {
            m_MessageType.text = "Error !";
            m_Msg.text = msg;
            m_MessageType.color = Color.red;
        }
        else if (msgtype == MsgType.warning)
        {
            m_MessageType.text = "Warning !";
            m_Msg.text = msg;
            m_MessageType.color = Color.yellow;
        }
        else if (msgtype == MsgType.exit)
        {
            m_MessageType.text = "EXIT";
            m_Msg.text = msg;
            m_MessageType.color = Color.white;
        }
        else
        {
            m_MessageType.text = "Notice";
            m_Msg.text = msg;
            m_MessageType.color = Color.white;
        }
    }
}
*/
