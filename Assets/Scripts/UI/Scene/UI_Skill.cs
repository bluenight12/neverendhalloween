using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UI_Skill : UI_Scene
{
    public Image ASkillImage;
    public Image SSkillImage;
    enum Images
    {
        ASkillIcon,
        SSkillIcon,
        ASkillImage,
        SSkillImage,
    }
    enum GameObjects
    {
        Panel,
        Fill,
        HP
    }
    Animator _panel;
    Image _askillIcon;
    Image _sskillIcon;
    Image _fillArea;
    TextMeshProUGUI _hp;
    GameObject player;
    PlayerStat _stat;
    public override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
        ASkillImage = GetImage((int)Images.ASkillImage);
        SSkillImage = GetImage((int)Images.SSkillImage);
        _askillIcon = GetImage((int)Images.ASkillIcon);
        _sskillIcon = GetImage((int)Images.SSkillIcon);
        _fillArea = GetObject((int)GameObjects.Fill).GetComponent<Image>();
        _panel = GetObject((int)GameObjects.Panel).GetComponent<Animator>();
        _hp = GetObject((int)GameObjects.HP).GetComponent<TextMeshProUGUI>();
        player = Managers.Game.GetPlayer();
        _stat = player.gameObject.GetComponent<PlayerStat>();
        ImageSetting(ASkillImage);
        ImageSetting(SSkillImage);        
    }
    private void Update()
    {
        float currentHP = _stat.Hp;
        float amount = currentHP / _stat.MaxHp;
        _hp.text = currentHP.ToString();
        _fillArea.fillAmount = amount;
    }

    // 스킬 쿨타임 받고 360 radius
    IEnumerator CoolTime(Image image, float coolTime)
    {
        float T = coolTime;
        while(coolTime > 0)
        {
            image.fillAmount = coolTime / T;
            coolTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        image.fillAmount = 0.0f;
        yield break;
    }
    public void ASkillEvent(float coolTime)
    {
        StartCoroutine(CoolTime(ASkillImage, coolTime));
    }
    public void SSkillEvent(float coolTime)
    {
        StartCoroutine(CoolTime(SSkillImage, coolTime));
    }
    private void ImageSetting(Image image)
    {
        image.type = Image.Type.Filled;
        image.fillMethod = Image.FillMethod.Radial360;
        image.fillOrigin = (int)Image.Origin360.Top;
        image.fillClockwise = false;
    }
    //Image 바꾸는 함수
    public void ImageChange(string name)
    {
        _askillIcon.sprite = Managers.Resource.Load<Sprite>($"Art/UI/Scene/UI_Skill/Skill Icon/{name}_A");
        _sskillIcon.sprite = Managers.Resource.Load<Sprite>($"Art/UI/Scene/UI_Skill/Skill Icon/{name}_S");
        _fillArea.sprite = Managers.Resource.Load<Sprite>($"Art/UI/Scene/UI_Skill/Bar/{name}_bar");
        switch (name)
        {
            case "Punch":
                _panel.SetFloat("State", 0);
                break;
            case "Cat":
                _panel.SetFloat("State", 1);
                break;
            case "Water":
                _panel.SetFloat("State", 2);
                break;
        }
    }
}
