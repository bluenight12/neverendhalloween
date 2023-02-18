using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Quest_Item : UI_Base
{
    enum GameObjects
    {
        Accept,
        QuestText
    }
    string _dialog;
    public int _id { get; private set; }
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
    }
    public void SetInfo(string dialog,int id)
    {
        _dialog = dialog;
        Get<GameObject>((int)GameObjects.QuestText).GetComponent<TextMeshProUGUI>().text = $"{_dialog}";
        _id = id;
    }
    public void ChangeStatus(int status)
    {
        TextMeshProUGUI btntext = Get<GameObject>((int)GameObjects.Accept).GetComponent<Button>().transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (status == 0)
        {
            Get<GameObject>((int)GameObjects.Accept).GetComponent<Button>().onClick.RemoveAllListeners();
            Get<GameObject>((int)GameObjects.Accept).GetComponent<Button>().onClick.AddListener(OnButtonClick);
            Managers.Data.QuestDict[_id].status = 0;
            btntext.text = "����";
        }
        else if(status == 1)
        {
            Get<GameObject>((int)GameObjects.Accept).GetComponent<Button>().onClick.RemoveAllListeners();
            Get<GameObject>((int)GameObjects.Accept).GetComponent<Button>().onClick.AddListener(DoingQuest);
            Managers.Data.QuestDict[_id].status = 1;
            btntext.text = "������";
        }
        else if(status == 2)
        {
            Get<GameObject>((int)GameObjects.Accept).GetComponent<Button>().onClick.RemoveAllListeners();
            Get<GameObject>((int)GameObjects.Accept).GetComponent<Button>().onClick.AddListener(QuestEnd);
            Managers.Data.QuestDict[_id].status = 2;
            btntext.text = "����";
        }
        else if(status == 3)
        {
            Get<GameObject>((int)GameObjects.Accept).GetComponent<Button>().onClick.RemoveAllListeners();
            Managers.Data.QuestDict[_id].status = 3;
            btntext.text = "�Ϸ�";
        }
    }
    public void OnButtonClick()
    {
        Managers.Quest.Accept(_id);
    }
    public void DoingQuest()
    {
        UI_Alert alert = Managers.UI.ShowPopupUI<UI_Alert>();
        alert.PopupCheckWindowOpen(() => Managers.Quest.Reject(_id), "reject", "����Ʈ�� ����Ͻðڽ��ϱ�?");
    }
    public void QuestEnd()
    {
        UI_Alert alert = Managers.UI.ShowPopupUI<UI_Alert>();
        alert.PopupCheckWindowOpen(() => Managers.Quest.Success(_id), "Success", "����Ʈ�� �Ϸ��Ͻðڽ��ϱ�?");
    }
}
