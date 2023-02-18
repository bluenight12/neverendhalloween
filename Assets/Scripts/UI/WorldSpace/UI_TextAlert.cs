using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_TextAlert : UI_Base
{
    enum GameObjects
    {
        AlertText
    }
    TextMeshProUGUI text;
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        text = GetObject((int)GameObjects.AlertText).GetComponent<TextMeshProUGUI>();
    }
    public void SetText(string value, Vector3 vector)
    {
        text.text = value;
        transform.position = transform.parent.position + vector;
    }
}
