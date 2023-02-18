using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ReadText : UI_Popup
{
    Queue<string> _dialog;
    enum GameObjects
    {
        DialogText
    }
    TextMeshProUGUI text;
    string sentence;
    Coroutine coroutine;
    string checkSen;
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
        if (Input.GetKeyDown(KeyCode.F) && sentence.Equals(checkSen))
        {
            Talk();
        }
        /*        else if (!text.text.Equals(sentence) && Input.GetKeyDown(KeyCode.F))
                {
                    text.text = sentence;
                    StopCoroutine(coroutine);
                }*/
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
        if (_dialog.Count != 0)
        {
            sentence = _dialog.Dequeue();
            coroutine = StartCoroutine(Typing(sentence));
        }
        else
        {
            Managers.Talk.isTalking = false;
            Managers.UI.ClosePopupUI();
            if (isNPC)
            {
                currentNPC.ShowUI();
            }
        }
    }
    IEnumerator Typing(string sentence)
    {
        checkSen = "";
        int i = 0;
        foreach (char letter in sentence.ToCharArray())
        {
            if(i % 2 == 0)
            Managers.Sound.Play("Effect/UI/TypeMessage");
            checkSen += letter;
            i++;
            text.text += letter;
            yield return new WaitForSeconds(0.1f);
            
        }
    }
}
