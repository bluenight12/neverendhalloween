using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Quest : UI_Popup
{
    Dictionary<int, Data.Quest> _questDict;
    enum GameObjects
    {
        QuestList,
        CloseButton
    }
    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        //questDict에서 수락했다면 까만색 or 다시 눌렀을 때 팝업 뜨게 하기
        //수락, 거절 했을 때 여기서 업데이트 할 수 있게 한번 초기화 해주기
        _questDict = Managers.Data.QuestDict;
        GameObject questList = Get<GameObject>((int)GameObjects.QuestList);
        Get<GameObject>((int)GameObjects.CloseButton).BindEvent((PointerEventData) => { Managers.Game.GetPlayer().GetComponent<PlayerController>().CloseNPC(); });
        foreach (Transform child in questList.transform)
            Managers.Resource.Destroy(child.gameObject);

        for (int i = 0; i < _questDict.Count; i++)
        {
            GameObject quest = Managers.UI.MakeSubItem<UI_Quest_Item>(questList.transform).gameObject;
            quest.transform.SetParent(questList.transform);
            Data.Quest questData;
            _questDict.TryGetValue(i, out questData);
            UI_Quest_Item questitem = quest.GetOrAddComponent<UI_Quest_Item>();
            questitem.SetInfo(questData.phrase, i);
            questitem.ChangeStatus(questData.status);
        }
    }
    public void ChangeQuestStatus(int _questID, int status)
    {
        // 0: 미수주, 1: 진행중, 2: 완료
        GameObject questList = Get<GameObject>((int)GameObjects.QuestList);
        foreach(Transform child in questList.transform)
        {
            UI_Quest_Item _item = child.GetComponent<UI_Quest_Item>();
            if (_item._id == _questID)
            {
                _item.ChangeStatus(status);
            }
        }
    }
}
