using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Status : UI_Scene
{
    enum GameObjects
    {
        //HP_Panel,
        SoulCounter,
        //Count
    }
    PlayerStat _stat;
    GameObject player;
    GameObject HP_Panel;
    TextMeshProUGUI SoulCounter;
    //TextMeshProUGUI _monCount;
    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        player = Managers.Game.GetPlayer();
        _stat = player.gameObject.GetComponent<PlayerStat>();
        SoulCounter = GetObject((int)GameObjects.SoulCounter).GetComponent<TextMeshProUGUI>();
        //_monCount = GetObject((int)GameObjects.Count).GetComponent<TextMeshProUGUI>();
        /*
        HP_Panel = GetObject((int)GameObjects.HP_Panel);
        foreach (Transform child in HP_Panel.transform)
            Managers.Resource.Destroy(child.gameObject);
        for (int i = 0; i < 10; i++)
        {
            GameObject item = Managers.UI.MakeSubItem<UI_HP_Item>(HP_Panel.transform).gameObject;
            UI_HP_Item heart = item.GetOrAddComponent<UI_HP_Item>();
            heart.SetRatio(0);
        }
        */
    }
    private void Update()
    {
        float currentHP = _stat.Hp;
        SoulCounter.text = _stat.Soul.ToString();
        //_monCount.text = "남은 몬스터 수 : " + Managers.Game.MonCount;
        // HP 롤백 할 시
        /*foreach (Transform child in HP_Panel.transform)
        {
            if (currentHP <= 0)
            {
                child.GetComponent<UI_HP_Item>().SetRatio(0);
                Util.FindChild(child.GetComponent<UI_HP_Item>().gameObject, "Background", true).GetComponent<Image>().enabled = false;
            }
            else
            {
                child.GetComponent<UI_HP_Item>().SetRatio(currentHP);
                Util.FindChild(child.GetComponent<UI_HP_Item>().gameObject, "Background", true).GetComponent<Image>().enabled = true;
            }
            currentHP -= 20;
        }*/
    }
}
