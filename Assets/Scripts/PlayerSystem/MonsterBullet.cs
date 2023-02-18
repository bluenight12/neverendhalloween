using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    int _damage;
    public int Damage { set { _damage = value; } }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Define.Layer.Player)
        {
            // 부하 걸릴 것 같음
            //Weapon playerwp = GameObject.FindGameObjectWithTag("Player").GetComponent<Weapon>();
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            Destroy(gameObject);
            player.OnHitEvent(_damage, transform);
        }
        else if (collision.gameObject.layer == (int)Define.Layer.Ground || collision.gameObject.layer == (int)Define.Layer.Block)
        {
            Destroy(gameObject);
        }
    }
}
