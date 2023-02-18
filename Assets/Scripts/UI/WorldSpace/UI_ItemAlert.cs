using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemAlert : UI_Base
{
    enum GameObjects
    {
        PopupText,
        ItemImage,
        ItemTitle
    }
    TextMeshProUGUI phrase;
    Image itemImage;
    TextMeshProUGUI ItemTitle;
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        phrase = GetObject((int)GameObjects.PopupText).GetComponent<TextMeshProUGUI>();
        itemImage = GetObject((int)GameObjects.ItemImage).GetComponent<Image>();
        ItemTitle = GetObject((int)GameObjects.ItemTitle).GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * 2.5f;
    }
    public void SetInfo(Item item)
    {
        phrase.text = item.Phrase;
        itemImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
        ItemTitle.text = item.Title;
    }
    public void SetWpInfo(GameObject go)
    {
        phrase.text = $"{go.name}으로 변신";
        itemImage.sprite = go.GetComponent<SpriteRenderer>().sprite;
        ItemTitle.text = go.name;
    }
}
