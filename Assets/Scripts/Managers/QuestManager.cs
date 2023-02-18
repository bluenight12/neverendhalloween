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
        //����Ʈ ����Ʈ�� �߰�
        //�ް� ���� ���� ��ư ����
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
        //����Ʈ ����Ʈ���� ����
        //��� �ϸ� �ٽ� ��ư ����
        _questList.Remove(id);
        GameObject.Find("UI_Quest").GetComponent<UI_Quest>().ChangeQuestStatus(id, 0);
    }
    public void Success(int id)
    {
        //�Ϸ� �Ǹ� ���� ��ư�� �Ϸ� Ȥ�� ������
        GameObject.Find("UI_Quest").GetComponent<UI_Quest>().ChangeQuestStatus(id, 3);
        Managers.Game.GetPlayer().GetComponent<PlayerStat>().Soul += Managers.Data.QuestDict[id].reward;
    }
    public void OnMonsterDeath(string name)
    {
        //����Ʈ ���鼭 name �� ���� ����Ʈ�� +1
        //���� ��ǥ���� ��ġ�Ѵٸ� �� ���ķδ� X or �޼���Ű��
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
