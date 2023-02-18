using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_MonCount : UI_Scene
{
    enum GameObjects
    {
        Count
    }
    TextMeshProUGUI _monCount;
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        _monCount = GetObject((int)GameObjects.Count).GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        _monCount.text = "남은 몬스터 수 : " + Managers.Game.MonCount;
    }
}
