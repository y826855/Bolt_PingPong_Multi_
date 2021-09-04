using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBullet : MonoBehaviour
{
    float Speed = 5f;

    private void OnEnable()
    {
        Invoke("SelfDestroy", 3f);
    }

    void Update()
    {
        this.transform.position += this.transform.forward * Time.deltaTime * Speed;
    }

    void SelfDestroy()
    {
        GameObject.Destroy(this.gameObject);
    }
    
}
