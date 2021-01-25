using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ntp_Bullet : MonoBehaviour
{
    float life = 3f;

    // Update is called once per frame
    void Update()
    {
        life -= Time.deltaTime;
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
