using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScene : BaseScene
{
    int[] _dialog1 = new int[] { 0, 1, 0 };
    int[] _dialog2 = new int[] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 0 };
    Transform[] talk = new Transform[2];
    int _conv = 3000;
    int _queue = 0;
    bool event1 = false;
    bool event2 = false;
    UI_Dialog dialog;
    GameObject _tutorial;
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Tutorial;
        UI_Fade.FadeIn();

        _tutorial = GameObject.Find("Tutorial");
        _tutorial.SetActive(false);

        //NPC 세팅
        GameObject a_npc = GameObject.FindGameObjectWithTag("NPC");
        talk[0] = a_npc.transform;
        a_npc.GetComponent<Animator>().SetTrigger("Bind");

        //Player 세팅
        GameObject _player = Managers.Game.GetPlayer();
        talk[1] = _player.transform;
        PlayerController player = _player.GetOrAddComponent<PlayerController>();
        player.SceneChange();
        player.ResetPlayer();
        _player.transform.position = Util.FindChild(gameObject, "Start").transform.position;
        _player.GetComponent<Animator>().SetTrigger("Lying");
        _player.GetComponent<PlayerController>().State = Define.State.Die;
        StartCoroutine(Tutorial_Start());

    }
    void Update()
    {
        if (!Managers.Talk.isTalking)
        {
            if (event1)
            {
                talk[1].GetComponent<PlayerController>().State = Define.State.Idle;
                event1 = false;
                _tutorial.SetActive(true);
                //UI_Press press = Managers.UI.MakeWorldSpaceUI<UI_Press>(talk[0]);
                //press.SetPosition(talk[0].position + new Vector3(0.0f, 2.3f));
            }
            if(_queue == _dialog1.Length + _dialog2.Length)
            {
                talk[1].GetComponent<PlayerController>().State = Define.State.Idle;
                _queue++;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_dialog1.Length > _queue && event2)
                {
                    Talk(1);
                }
                else if (_dialog2.Length > _queue - _dialog1.Length && talk[0].GetComponent<Animator>().GetBool("IsUnBind"))
                {
                    if (event2)
                    {
                        event2 = false;
                        StartCoroutine(Event1_Start());
                    }
                    Talk(2);
                }
            }
        }
    }
    public override void Clear()
    {
        event1 = false;
        event2 = false;
    }
    void Talk(int i)
    {
        Managers.Talk.isTalking = true;
        if (i == 1)
        {
            dialog = Managers.UI.MakeWorldSpaceUI<UI_Dialog>(talk[_dialog1[_queue]]);
        }
        else if(i == 2)
        {
            dialog = Managers.UI.MakeWorldSpaceUI<UI_Dialog>(talk[_dialog2[_queue - _dialog1.Length]]);
        }
        if (dialog != null)
        {
            dialog.SetTalk(_conv);
            _queue++;
            _conv++;
        }
        if (i == 1)
        {
            if (_queue == _dialog1.Length)
            {
                event1 = true;
            }
        }
    }
    IEnumerator Tutorial_Start()
    {
        yield return new WaitForSeconds(3);
        UI_Fade.FadeOut();
        yield return new WaitForSeconds(1);
        talk[1].GetComponent<Animator>().SetTrigger("Wake");
        yield return new WaitForSeconds(1);
        UI_Fade.FadeIn();
        yield return new WaitForSeconds(1f);
        float walk = 2f;
        while (walk > 0)
        {
            talk[1].GetComponent<PlayerController>().State = Define.State.EventMove;
            walk -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        Talk(1);
        talk[1].GetComponent<Animator>().SetFloat("Speed", 0);
        talk[1].GetComponent<PlayerController>().State = Define.State.Die;
        event2 = true;
        yield return null;
    }
    IEnumerator Event1_Start()
    {
        GameObject effect = Managers.Resource.Instantiate("NPC/Effect/Effect", talk[0]);
        effect.transform.position = effect.transform.parent.position;
        Destroy(effect, 0.4f);
        talk[1].rotation = Quaternion.LookRotation(Vector3.back);
        float walk = 0.2f;
        while (walk > 0)
        {
            talk[1].GetComponent<PlayerController>().State = Define.State.EventMove;
            walk -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        talk[1].rotation = Quaternion.LookRotation(Vector3.forward);
        talk[1].GetComponent<Animator>().SetFloat("Speed", 0);
        talk[1].GetComponent<PlayerController>().State = Define.State.Die;
        yield return null;
    }
}
