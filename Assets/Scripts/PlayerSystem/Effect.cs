using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    float animTime;
    void Start()
    {
        animTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
    }
    void Update()
    {
        animTime -= Time.deltaTime;
        if(animTime <= 0) 
        {
            Managers.Resource.Destroy(gameObject);
        }
    }
}
