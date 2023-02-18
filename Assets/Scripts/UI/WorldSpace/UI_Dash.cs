using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dash : UI_Base
{
    Slider _dash;
    enum GameObjects
    {
        Dash
    }
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        _dash = GetObject((int)GameObjects.Dash).GetComponent<Slider>();
    }
    public void StartDash()
    {
        gameObject.SetActive(true);
        StartCoroutine(Dash());
    }
    IEnumerator Dash()
    {
        float _startTime = 0.0f;
        _dash.value = 0.0f;
        while (_startTime < 5.0f)
        {
            _startTime += Time.deltaTime;            
            _dash.value = _startTime / 5.0f;
            yield return new WaitForFixedUpdate();
        }
        gameObject.SetActive(false);
        yield return null;
    }
}
