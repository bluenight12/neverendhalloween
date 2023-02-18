using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TakeDamage : UI_Base
{
    enum GameObjects
    {
        DamageText
    }
    string _currentText;
    TextMeshProUGUI text;
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        text = GetObject((int)GameObjects.DamageText).GetComponent<TextMeshProUGUI>();
    }
    public void SetText(string value)
    {
        text.text = value;
    }
    public void SetPosition(GameObject go)
    {
        float rand = Random.Range(-1.0f, 1.0f);
        transform.position = go.transform.position + Vector3.up * (int)(go.GetComponent<BoxCollider2D>().bounds.size.y) + new Vector3(rand, 0.0f);
        StartCoroutine("UpText");
    }
    IEnumerator UpText()
    {
        float coolTime = 1;
        while (coolTime > 0)
        {
            transform.position += Vector3.up * 0.03f;
            coolTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        Managers.Resource.Destroy(gameObject);
        yield break;
    }
}
