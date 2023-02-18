using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_Setting_Sound : UI_Popup
{
    public AudioMixer audioMixer;
    Slider _bgmSlider;
    Slider _effectSlider;
    float _bgmvolume;
    float _effectvolume;
    enum GameObjects
    {
        BGM_Slider,
        Effect_Slider,
        Back
    }
    enum Buttons 
    {
        BGM_Minus,
        BGM_Plus,
        Effect_Minus,
        Effect_Plus
    }
    Color textColor = new Color32(132, 146, 172, 255);
    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        _bgmSlider = GetObject((int)GameObjects.BGM_Slider).GetComponent<Slider>();
        _effectSlider = GetObject((int)GameObjects.Effect_Slider).GetComponent<Slider>();
        GetButton((int)Buttons.Effect_Plus).gameObject.BindEvent((PointerEventData) => { _effectSlider.value += 4; Managers.Sound.Play("Effect/UI/Click"); });
        GetButton((int)Buttons.Effect_Minus).gameObject.BindEvent((PointerEventData) => { _effectSlider.value -= 4; Managers.Sound.Play("Effect/UI/Click"); });
        GetButton((int)Buttons.BGM_Plus).gameObject.BindEvent((PointerEventData) => { _bgmSlider.value += 4; Managers.Sound.Play("Effect/UI/Click"); });
        GetButton((int)Buttons.BGM_Minus).gameObject.BindEvent((PointerEventData) => { _bgmSlider.value -= 4; Managers.Sound.Play("Effect/UI/Click"); });
        GameObject go = GetObject((int)GameObjects.Back);
        BindEvent(go, (PointerEventData data) => { go.GetComponent<TextMeshProUGUI>().color = Color.white; }, Define.UIEvent.Enter);
        BindEvent(go, (PointerEventData data) => { go.GetComponent<TextMeshProUGUI>().color = textColor; }, Define.UIEvent.Exit);
        BindEvent(go, (PointerEventData) => { Managers.Sound.Play("Effect/UI/Hover"); }, Define.UIEvent.Enter);
        BindEvent(go, (PointerEventData) => { Managers.Sound.Play("Effect/UI/Click"); });
        BindEvent(go, (PointerEventData data) => { Managers.UI.ClosePopupUI(); });
    }
    void Update()
    {
        _bgmvolume = _bgmSlider.value;
        audioMixer.SetFloat("Bgm", _bgmvolume <= -40 ? -80 : _bgmvolume);
        _effectvolume = _effectSlider.value;
        audioMixer.SetFloat("Effect", _effectvolume <= -40 ? -80 : _effectvolume);
    }

}
