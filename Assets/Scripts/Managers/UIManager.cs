using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int _order = 10;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }
    public int GetPopupStack()
    {
        return _popupStack.Count;
    }
    public void SetCanvas(GameObject go, bool sort = true) // Popup 우선순위
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }
    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");

        if (parent != null)
        {
            go.transform.SetParent(parent.transform);
            if(typeof(T).Name == "UI_Dialog")
            {
                go.transform.position = parent.position + Vector3.up * 2/*(int)(parent.GetComponent<BoxCollider2D>().bounds.size.y)*/;
                go.transform.rotation = Camera.main.transform.rotation;
            }
        }
        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        // 받은 T에 맞는 컴포넌트를 달아준다
        return Util.GetOrAddComponent<T>(go);
    }
    public T MakeSubItem<T>(Transform parent = null ,string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
            go.transform.SetParent(parent.transform);
        // 받은 T에 맞는 컴포넌트를 달아준다
        return Util.GetOrAddComponent<T>(go);
    }
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");

        T scene = Util.GetOrAddComponent<T>(go);
        _sceneUI = scene;

        go.transform.SetParent(Root.transform);
        return scene;
    }
    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");

        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);
     
        go.transform.SetParent(Root.transform);       
        return popup;
    }
    public void ClosePopupUI(UI_Popup popup) // 잘못 된 팝업 삭제 방지(크로스체킹)
    {
        if (_popupStack.Count == 0)
            return;
        if(_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed");
            return;
        }
        ClosePopupUI();
    }
    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;
        UI_Popup popup = _popupStack.Pop();

        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
    }
    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }
    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
