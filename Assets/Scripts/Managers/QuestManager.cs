using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using UnityEngine;

public class QuestManager
{
    public SortedList<int, Data.Quest> _questList { get; private set; } = new SortedList<int, Data.Quest>();
    Dictionary<int, Data.Quest> _questDict;
    public void Init()
    {
        _questDict = Managers.Data.QuestDict;
    }
    public void Accept(int id)
    {
        //퀘스트 리스트에 추가
        //받고 나서 수주 버튼 변경
        Data.Quest quest;
        _questDict.TryGetValue(id, out quest);
        if(quest != null)
        {
            _questList.Add(id, quest);
        }
        GameObject.Find("UI_Quest").GetComponent<UI_Quest>().ChangeQuestStatus(id, 1);

    }
    public void Reject(int id)
    {
        //퀘스트 리스트에서 제거
        //취소 하면 다시 버튼 변경
        _questList.Remove(id);
        GameObject.Find("UI_Quest").GetComponent<UI_Quest>().ChangeQuestStatus(id, 0);
    }
    public void Success(int id)
    {
        //완료 되면 수주 버튼을 완료 혹은 검은색
        GameObject.Find("UI_Quest").GetComponent<UI_Quest>().ChangeQuestStatus(id, 3);
        Managers.Game.GetPlayer().GetComponent<PlayerStat>().Soul += Managers.Data.QuestDict[id].reward;
    }
    public void OnMonsterDeath(string name)
    {
        //퀘스트 돌면서 name 과 같은 퀘스트는 +1
        //만약 목표값과 일치한다면 그 이후로는 X or 달성시키기
        List<int> successID = new List<int>();
        foreach (var quest in _questList)
        {
            if (quest.Value.name.Equals(name))
            {
                if(++quest.Value.amount == quest.Value.goal)
                {
                    Managers.Data.QuestDict[quest.Key].status = 2;
                }
            }
        }
    }
}
