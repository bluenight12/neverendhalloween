using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fire : MonoBehaviour
{
    float _firetime = 0.0f;
    bool _enabled = false;
    Animator anim;
    BoxCollider2D box;
    void Start()
    {
        anim = gameObject.GetOrAddComponent<Animator>();
        box = gameObject.GetOrAddComponent<BoxCollider2D>();
    }
    void FixedUpdate()
    {
        _firetime += Time.deltaTime;
        if (_firetime >= 3.0f)
        {
            _enabled = !_enabled;
            anim.SetFloat("Fire", Convert.ToInt32(_enabled));
            _firetime = 0.0f;
        }
        if (_enabled)
        {
            float a = Mathf.Sin((transform.rotation.eulerAngles.z % 360) * Mathf.Deg2Rad);
            float b = Mathf.Cos((transform.rotation.eulerAngles.z % 360) * Mathf.Deg2Rad);
            int mask = 1 << (int)Define.Layer.Player;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(-a , b), box.size.y, mask);
            if (hit)
            {
                hit.transform.gameObject.GetComponent<PlayerController>().OnHitEvent(10, transform);
            }
        }
    }
}
