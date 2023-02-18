using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UI_QuestAlert : UI_Scene
{
    SortedList<int, Data.Quest> _qList = new SortedList<int, Data.Quest>();
    TextMeshProUGUI _questText;
    enum GameObjects
    {
        QuestList
    }
    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        _questText = Get<GameObject>((int)GameObjects.QuestList).GetComponent<TextMeshProUGUI>();
        _qList = Managers.Quest._questList;
        UpdateQuestList("Test");
    }
    
    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateQuestList(string name)
    {
        _questText.text = "";
        foreach (var quest in _qList)
        {
            if (quest.Value.amount >= quest.Value.goal)
            {
                _questText.text += $"<#B4E9AB>{quest.Value.name} : ¼º°ø!</color>\n";
            }
            else
            {
                _questText.text += $"<#E59f9F>{quest.Value.name} : {quest.Value.amount} / {quest.Value.goal}</color>\n";
            }
        }
    }
}
