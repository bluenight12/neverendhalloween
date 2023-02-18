using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class JumpAttack : MonoBehaviour
{
    int mask = 1 << 8;
    public void Attack(string name,int damage)
    {
        switch (name)
        {
            case "Punch":
                {

                    Vector2 look = transform.TransformDirection(Vector3.right);
                    RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0.0f, 1.0f, 0.0f), look + new Vector2(0.0f, 1.0f), 2.0f, mask);
                    if (hit)
                    {
                        BaseController monster = hit.collider.gameObject.GetComponent<BaseController>();
                        monster.OnHitEvent(15, transform);
                    }
                }
                break;
            case "Water":
                {
                    RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 2.0f, Vector2.right, 0, mask);
                    foreach (RaycastHit2D hit in hits)
                    {
                        BaseController monster = hit.collider.gameObject.GetComponent<BaseController>();
                        monster.OnHitEvent(15, transform);
                    }
                }
                break;
            case "Cat":
                {
                    Vector2 size = new Vector2(3.0f, 2.0f);
                    RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position - new Vector3(0.0f, 1.5f), size, 0.0f, Vector2.up, 0.0f, mask);
                    foreach (RaycastHit2D hit in hits)
                    {
                        BaseController monster = hit.collider.gameObject.GetComponent<BaseController>();
                        monster.OnHitEvent(15, transform);
                    }
                }
                break;
        }
    }
}
