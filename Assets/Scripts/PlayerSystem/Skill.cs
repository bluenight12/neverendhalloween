using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
    int mask = 1 << 8;
    public void UseASkill(string name, int damage)
    {
        Rigidbody2D _rigid = GetComponent<Rigidbody2D>();
        _rigid.velocity = new Vector2(0, 0);
        switch (name) {
            case "Punch":
                {

                    Vector2 look = transform.TransformDirection(Vector3.right);
                    RaycastHit2D hit = Physics2D.Raycast(transform.position-new Vector3(0.0f,1.0f,0.0f), look + new Vector2(0.0f, 1.0f), 2.0f, mask);
                    if (hit)
                    {
                        BaseController monster = hit.collider.gameObject.GetComponent<BaseController>();
                        monster.OnHitEvent(damage, transform);
                    }
                }
                break;
            case "Water":
                {
                    Vector2 look = transform.TransformDirection(Vector3.right);
                    GameObject go = Managers.Resource.Instantiate($"Skill/Water/SkillA");
                    go.GetComponent<Bullet>().Damage = damage;
                    go.transform.position = transform.position + new Vector3(look.x, 0.0f, 0.0f);
                    go.transform.rotation = transform.rotation;
                    Rigidbody2D rigid = go.GetOrAddComponent<Rigidbody2D>();
                    rigid.AddForce(look * 1000);
                }
                break;
            case "Cat":
                {
                    StartCoroutine("CatA", damage);
                }
                break;
        }
    }
    public void UseSSkill(string name, int damage)
    {
        Rigidbody2D _rigid = GetComponent<Rigidbody2D>();
        _rigid.velocity = new Vector2(0, 0);
        switch (name)
        {
            case "Punch":
                {
                    Vector2 look = transform.TransformDirection(Vector3.right);
                    Vector2 jumpVelocity = new Vector2(0, 1);
                    _rigid.AddForce(jumpVelocity * 10, ForceMode2D.Impulse);
                    RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0.0f, 1.0f, 0.0f), look + new Vector2(0.0f, 1.0f), 2.0f, mask);
                    if (hit)
                    {
                        BaseController monster = hit.collider.gameObject.GetComponent<BaseController>();
                        monster.OnHitEvent(damage, transform);
                    }
                }
                break;
            case "Water":
                {
                    StartCoroutine(WaterS(damage));
                }
                break;
            case "Cat":
                {
                    Vector2 look = transform.TransformDirection(Vector3.right);
                    GameObject go = Managers.Resource.Instantiate($"Skill/Cat/SkillS");
                    go.transform.position = transform.position + new Vector3(look.x * 4.5f, 0.0f, 0.0f);
                    go.transform.rotation = transform.rotation;
                    go.GetComponent<Bullet>().Damage = damage;
                    float onTime = go.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
                    Destroy(go, onTime);
                }
                break;
        }
    }
    IEnumerator WaterS(int damage)
    {
        // 1. 사이즈 Box Collider에 맞게 조정하기    
        Vector2 look = transform.TransformDirection(Vector3.right);
        int hitCount = 3;
        Vector2 position = transform.position + new Vector3(5.0f * look.x, 0.0f);
        GameObject cloud = Managers.Resource.Instantiate("Skill/Water/SkillS",position,new Quaternion());
        BoxCollider2D box = cloud.GetComponent<BoxCollider2D>();
        Vector2 size = new Vector2(box.size.x, box.size.y);
        cloud.transform.position = new Vector3(position.x, position.y + box.size.y / 2 + gameObject.GetComponent<SpriteRenderer>().size.y / 2, 0.0f);
        while (hitCount > 0)
        {
            RaycastHit2D[] hits = Physics2D.BoxCastAll(position, size, 0.0f, look, 0.0f, mask);
            foreach(RaycastHit2D hit in hits){
                BaseController monster = hit.collider.gameObject.GetComponent<BaseController>();
                monster.OnHitEvent(damage, transform);
            }
            hitCount--;
            if(hitCount <= 0)
            {
                Managers.Resource.Destroy(cloud);
            }
            yield return new WaitForSeconds(1);
        }
        yield return null;
    }
    IEnumerator CatA(int damage)
    {
        Vector2 look = transform.TransformDirection(Vector3.right);
        GameObject go = Managers.Resource.Instantiate($"Skill/Cat/SkillA");
        go.transform.position = transform.position + new Vector3(look.x * 4, 0.0f, 0.0f);
        go.transform.rotation = transform.rotation;
        float onTime = go.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        Destroy(go, onTime);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, look, 8.0f, mask);
        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                BaseController monster = hit.collider.gameObject.GetComponent<BaseController>();
                monster.OnHitEvent(damage, transform);
            }
        }
        gameObject.GetComponent<PlayerController>().IsDamage = true;
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<PlayerController>().IsDamage = false;
        yield return null;
    }
}
