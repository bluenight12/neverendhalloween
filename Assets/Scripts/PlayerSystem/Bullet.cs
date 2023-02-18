using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    int _damage = 0;
    bool _poison = false;
    public int Damage { set { _damage = value; } }
    public bool Poison { set { _poison = value; } }
    //데미지 설정하는 함수 만들고 PlayerController에서 데미지 주기
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)Define.Layer.Monster)
        {
            BaseController monster = collision.gameObject.GetComponent<BaseController>();
            Animator anim = GetComponent<Animator>();
            Rigidbody2D rigid = GetComponent<Rigidbody2D>();
            //Crit 없애기
            monster.OnHitEvent(_damage, transform);
            if(_poison)
                StartCoroutine(PoisonAttack(monster));
            anim.SetTrigger("IsHit");
            if(rigid)
                rigid.velocity = new Vector2(0, 0);
        }
        else if (collision.gameObject.layer == (int)Define.Layer.Ground)
        {
            Animator anim = GetComponent<Animator>();
            Rigidbody2D rigid = GetComponent<Rigidbody2D>();
            anim.SetTrigger("IsHit");
            if (rigid)
                rigid.velocity = new Vector2(0, 0);
        }else if(collision.gameObject.layer == (int)Define.Layer.Block)
        {
            Destroy(gameObject);
        }
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }
    IEnumerator PoisonAttack(BaseController monster)
    {
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        yield return player.StartCoroutine(player.PoisonAttack(monster));
    }
}
