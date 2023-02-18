using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Press : UI_Base
{
    enum GameObjects
    {
        PressImage
    }
    float hover = 0.0f;
    float i = -1f;
    bool setPos = false;
    // Ã¹ A´Â 2.3
    Vector3 hoverPos;
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
    }

    private void Update()
    {
        if (setPos)
        {
            hover += i * Time.deltaTime;
            if (hover >= 0.2f)
            {
                i = -1f;
            }
            else if (hover <= -0.2f)
            {
                i = 1f;
            }
            transform.position = hoverPos + Vector3.up * hover;
        }
    }
    public void SetPosition(Vector3 value)
    {
        transform.position = value;
        hoverPos = value;
        setPos = true;
    }
}
