using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThorn : MonoBehaviour
{
    bool _scanPlayer = false;
    private void Update()
    {
        if(_scanPlayer)
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(2.0f, 2.0f), 0.0f, Vector2.up, 0.0f, 1 << (int)Define.Layer.Player);
            if (hit)
            {
                hit.transform.GetComponent<PlayerController>().OnHitEvent(10, transform);
            }
        }
    }
    void SetScan()
    {
        _scanPlayer = true;
    }
    void DestroyThorn()
    {
        Managers.Resource.Destroy(gameObject);
    }
}
