using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField]
    UI_Popup popup;
    bool isOpen = false;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void ShowUI()
    {
        if (!isOpen)
        {
            Managers.UI.ShowPopupUI<UI_Popup>(popup.name);
            isOpen = true;
        }
    }
    public void CloseUI()
    {
        isOpen = false;
        Managers.UI.ClosePopupUI();
    }
}
