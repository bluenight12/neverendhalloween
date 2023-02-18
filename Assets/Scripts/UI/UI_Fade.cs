using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Fade : UI_Base
{
    static Animator anim;
    public override void Init()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        DontDestroyOnLoad(gameObject);
    }
    public static void FadeIn()
    {
        anim.SetTrigger("FadeIn");
    }
    public static void FadeOut()
    {
        anim.SetTrigger("FadeOut");
    }
    public static void Blink()
    {
        anim.SetTrigger("Blink");
    }
}
