using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : BaseScene
{
    int _conv;
    //Dictionary<string, Transform> _NPC = new Dictionary<string, Transform>();
    Transform[] _NPC = new Transform[11];
    int[] _dialog1 = new int[] { 9, 8, 10, 8, 10, 8, 8, 10, 0, 1, 2, 3, 4, 5, 6, 7, 6, 10 };
    int[] _dialog2 = new int[] { 0, 7, 4, 6, 2 };
    UI_Dialog dialog;
    int _queue = 0;
    bool _intro_start = false;
    bool _intro1 = false;
    bool _introEnd = false;
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Intro;
        _conv = 2000;
        int i = 0;
        foreach (Transform npc in GameObject.Find("NPC").transform)
        {
            _NPC[i] = npc;
            i++;
        }
        StartCoroutine(Monologue());
        if (Managers.Game.GetPlayer())
        {
            Managers.Game.GetPlayer().transform.position = new Vector3(0, -20, 0);
        }
    }
    public override void Clear()
    {
        if (!Managers.Game.GetPlayer())
            Managers.Game.Spawn(Define.WorldObject.Player, "Player");
        _intro_start = false;
        _intro1 = false;
        _introEnd = false;
    }
    void Update()
    {
        if (!Managers.Talk.isTalking && Input.GetKeyDown(KeyCode.F))
        {
            if (_intro_start)
            {
                _intro_start = false;
                StartCoroutine(Intro_Start());
            }
            else if (_conv == 2010 && _intro1)
            {
                _intro1 = false;
                _conv++;
            }
            else if (_dialog1.Length > _queue && _conv >= 2011)
            {
                Talk();
            }
            else if (_dialog1.Length == _queue)
            {
                _queue++;
                StartCoroutine(Intro2_Start());
            }
            else if (_introEnd)
            {
                Managers.Scene.LoadScene(Define.Scene.Tutorial);
            }
        }
        /*if (_intro_start && Input.GetKeyDown(KeyCode.F) && !Managers.Talk.isTalking)
        {
            _intro_start = false;
            StartCoroutine(Intro_Start());
        }
        if (!Managers.Talk.isTalking && Input.GetKeyDown(KeyCode.F) && _conv >= 2011 && !_intro2)
        {
            if (_dialog1.Length > _queue)
            {
                Talk();
            }
            else if(!_intro2)
            {
                _intro2 = true;
                StartCoroutine(Intro2_Start());
            }
        }
        else if(Input.GetKeyDown(KeyCode.F) && _conv == 2010 && _intro1)
        {
            _intro1 = false;
            _conv++;
        }
        else if(Input.GetKeyDown(KeyCode.F) && _introEnd == true)
        {
            Managers.Scene.LoadScene(Define.Scene.Tutorial);
        }*/
    }
    void Talk()
    {
        Managers.Talk.isTalking = true;
        dialog = Managers.UI.MakeWorldSpaceUI<UI_Dialog>(_NPC[_dialog1[_queue]]);
        if (dialog != null)
        {
            dialog.SetTalk(_conv);
            _queue++;
            _conv++;
        }
    }
    IEnumerator Monologue()
    {
        Managers.Talk.isTalking = true;
        UI_ReadText rt = Managers.UI.ShowPopupUI<UI_ReadText>();
        rt.SetTalk(100);
        _intro_start = true;
        yield return null;
    }
    IEnumerator Intro_Start()
    {
        UI_Fade.FadeIn();
        Managers.Sound.Play("Effect/Structure/DoorOpen");
        yield return new WaitForSeconds(1);
        Managers.Sound.Play("Bgm/IntroBGM", Define.Sound.Bgm);
        Managers.Talk.isTalking = true;
        for (int i = 0; i < 10; i++)
        {
            dialog = Managers.UI.MakeWorldSpaceUI<UI_Dialog>(_NPC[i], "UI_Dialog_Small");
            dialog.SetTalk(_conv);
            _conv++;
            yield return new WaitForSeconds(0.1f);
        }
        dialog = Managers.UI.MakeWorldSpaceUI<UI_Dialog>(_NPC[10], "UI_Dialog_Small");
        dialog.SetTalk(_conv);
        _intro1 = true;
        yield return null;
    }
    IEnumerator Intro2_Start()
    {
        Managers.Sound.Clear();
        SpriteRenderer blink = GameObject.Find("Blink").GetComponent<SpriteRenderer>();
        blink.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(0.1f);
        blink.color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(0.1f);
        blink.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(0.1f);
        blink.color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(0.1f);
        blink.color = new Color(0, 0, 0, 1);
        Managers.Talk.isTalking = true;
        for (int i = 0; i < 5; i++)
        {
            dialog = Managers.UI.MakeWorldSpaceUI<UI_Dialog>(_NPC[_dialog2[i]]);
            dialog.SetTalk(_conv);
            _conv++;
            yield return new WaitForSeconds(0.5f);
        }
        _introEnd = true;
        yield return null;
    }
}
