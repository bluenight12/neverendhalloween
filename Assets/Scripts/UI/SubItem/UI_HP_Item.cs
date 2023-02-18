using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HP_Item : UI_Base
{
    enum GameObjects
    {
        Heart
    }
    Slider Heart;
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Heart = Get<GameObject>((int)GameObjects.Heart).GetComponent<Slider>();
    }
    public void SetRatio(float value)
    {
        Heart.value = value;
    }
}
