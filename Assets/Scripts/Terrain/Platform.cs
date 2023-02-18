using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    bool _playerCheck;
    PlatformEffector2D _platform;
    void Start()
    {
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
        _platform = GetComponent<PlatformEffector2D>();

    }
    void OnKeyboard()
    {
        if ((Input.GetKeyDown(KeyCode.C)) && (Input.GetKey(KeyCode.DownArrow)))
        {
            if(_playerCheck)
                StartCoroutine(KeyDown());
        }
    }
    IEnumerator KeyDown()
    {
        _platform.colliderMask = ~(1 << (int)Define.Layer.Player);
        yield return new WaitForSeconds(0.5f);
        _platform.colliderMask = ~0;
        yield return null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == (int)Define.Layer.Player)
        {
            _playerCheck = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Define.Layer.Player)
        {
            _playerCheck = false;
        }
    }
}
