using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Shop : UI_Popup
{
    enum GameObjects
    {
        AttackBar,
        HPBar
    }
    enum Texts
    {
        AttackName,
        AttackValue,
        HPName,
        HPValue
    }
    enum Buttons
    {
        Quit
    }
    enum Images
    {
        AttackUp,
        HPUp
    }
    int AttackNum;
    int HPNum;
    PlayerController _player;
    PlayerStat _stat;
    Slider _atkslider;
    Slider _hpslider;
    int _shopPoint;
    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        //Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        GetImage((int)Images.AttackUp).gameObject.BindEvent(AttackUp);
        GetImage((int)Images.HPUp).gameObject.BindEvent(HPUp);
        GetButton((int)Buttons.Quit).gameObject.BindEvent((PointerEventData) => { _player.CloseNPC();});
        _player = Managers.Game.GetPlayer().GetComponent<PlayerController>();
        _stat = _player.GetComponent<PlayerStat>();
        AttackNum = (_stat.Attack - 10);
        HPNum = _stat.MaxHp - 100;
        _atkslider = GetObject((int)GameObjects.AttackBar).GetComponent<Slider>();
        _atkslider.value = AttackNum / 20.0f;
        _hpslider = GetObject((int)GameObjects.HPBar).GetComponent<Slider>();
        _hpslider.value = HPNum / 100.0f;
        //ShopPoint를 미리 가져오기
        _shopPoint = _stat.Soul / 50;
    }
    public void AttackUp(PointerEventData data)
    {
        if (_shopPoint > 0)
        {
            AttackNum += 2;
            if (AttackNum > 20)
            {
                AttackNum -= 2;
                return;
            }
            _atkslider.value = AttackNum / 20.0f;
            _player.ChangeStat("Attack", 2);
            _stat.Soul -= 50;
            _shopPoint--;
            Managers.Sound.Play("Effect/UI/Click");
        }
        else
        {
            // 포인트가 부족합니다 띄우기?
        }
    }
    public void HPUp(PointerEventData data)
    {
        if (_shopPoint > 0)
        {
            HPNum += 10;
            if (HPNum > 100)
            {
                HPNum -= 10;
                return;
            }
            _hpslider.value = HPNum / 100.0f;
            _player.ChangeStat("Hp", 10);
            _stat.Soul -= 50;
            _shopPoint--;
            Managers.Sound.Play("Effect/UI/Click");
        }
        else
        {
            // 포인트가 부족합니다 띄우기?
        }
    }
}
