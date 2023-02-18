using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Dialog : UI_Base
{
    Queue<string> _dialog;
    enum GameObjects
    {
        DialogImage,
        DialogText,
    }
    TextMeshProUGUI text;
    string sentence;
    Coroutine coroutine;
    NPCController currentNPC;
    bool isNPC;
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        text = GetObject((int)GameObjects.DialogText).GetComponent<TextMeshProUGUI>();
        _dialog = new Queue<string>();
    }
    private void Update()
    {
        Transform parent = transform.parent;
        if (parent.name == "Boss1")
        {
            transform.position = parent.position + Vector3.up * 3/*(int)(parent.GetComponent<BoxCollider2D>().bounds.size.y)*/;
        }
        else
        {
            transform.position = parent.position + Vector3.up * 2/*(int)(parent.GetComponent<BoxCollider2D>().bounds.size.y)*/;
        }
        transform.rotation = Camera.main.transform.rotation;
        if (text.text.Equals(sentence) && Input.GetKeyDown(KeyCode.F))
        {
            Talk();
        }
        else if(!text.text.Equals(sentence) && Input.GetKeyDown(KeyCode.F))
        {
            text.text = sentence;
            StopCoroutine(coroutine);
        }
    }
    public void SetTalk(int id, bool isNPC = false, NPCController npc = null)
    {
        string[] talkData = Managers.Talk.GetTalk(id);
        currentNPC = npc;
        this.isNPC = isNPC;
        _dialog.Clear();
        foreach (string talk in talkData)
        {
            _dialog.Enqueue(talk);
        }
        Talk();
    }
    public void Talk()
    {
        text.text = "";
        if (_dialog.Count != 0)
        {
            sentence = _dialog.Dequeue();
            coroutine = StartCoroutine(Typing(sentence));
        }
        else
        {
            Managers.Talk.isTalking = false;
            Managers.Resource.Destroy(gameObject);
            if (isNPC)
            {
                currentNPC.ShowUI();
            }
        }
    }
    IEnumerator Typing(string sentence)
    {
        yield return null;
        foreach(char letter in sentence.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
