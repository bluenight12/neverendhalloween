using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Define.CameraMode _mode = Define.CameraMode.Scroll;
    [SerializeField]
    private Vector3 _delta = new Vector3(0.0f, 0.0f, -10.0f);
    [SerializeField]
    public GameObject _player = null;
    Camera _camera;
    float _xsize;
    float _ysize;
    GameObject _scene;
    float _x_limit_start;
    float _x_limit_end;
    float _y_limit;
    void Start()
    {
        _camera = GetComponent<Camera>();
        _scene = GameObject.Find("@Scene");
        _x_limit_start = _scene.transform.GetChild(0).position.x - 3;
        _x_limit_end = _scene.transform.GetChild(1).position.x + 3.5f;
        _y_limit = _scene.transform.GetChild(2).position.y + 2;
        _xsize = _camera.orthographicSize * Screen.width / Screen.height;
        _ysize = _camera.orthographicSize;
    }
    void LateUpdate()
    { 
        if ((_player == null)||(_player.activeSelf == false))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            _player = player;
        }
        else if (_mode == Define.CameraMode.Scroll)
        {
            _camera.orthographicSize = 8;
            transform.position = new Vector3(
                Mathf.Clamp(_player.transform.position.x, _x_limit_start + _xsize, _x_limit_end - _xsize),
                Mathf.Clamp(_player.transform.position.y, 2, _y_limit - _ysize),
                _player.transform.position.z) + _delta;
        }
    }
    public void SetScroll(){
        _mode = Define.CameraMode.Scroll;
    }
    public void SetDialog()
    {
        _mode = Define.CameraMode.Dialog;
    }
}
