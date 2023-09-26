using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aplastar : MonoBehaviour
{

    public float enemyHealth;
    public float damageFoot = 100f; 

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "WeakPoint")
        {
            Debug.Log("Aplastado");
            aplastado(collision);
        }
    }
    void Start()
    {
        enemyHealth = 700f;
    }

    void Update()
    {
        
    }

    void aplastado(Collision2D collision)
    {
        enemyHealth -= damageFoot;
        if(enemyHealth <= 0)
        {
            Destroy(collision.gameObject);
        } 
    }
}
