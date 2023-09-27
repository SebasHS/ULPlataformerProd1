 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aplastar : MonoBehaviour
{

    public float enemyHealth;
    public float damageFoot = 100f; 
    private GameObject enemy;
    [SerializeField] public GameObject HealthBarBoss;
    public Slider slider;

    [SerializeField] private BossLogic bossLogic_code;

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "WeakPoint")
        {
            Debug.Log("Aplastado");
            HealthSystem.Instance.RestoreMana(34f);
            aplastado(collision);
        }
    }
    void Start()
    {
        enemyHealth = 700f;
        enemy = GameObject.Find("Boss");
        HealthBarBoss = GameObject.Find("HealthBarBoss");

    }

    void Update()
    {
        
    }

    void aplastado(Collision2D collision)
    {
        enemyHealth -= damageFoot;
        slider.value = enemyHealth; 
        if (enemyHealth <= 0)
        {
            bossLogic_code.animator.SetBool("IsDying", true);
            
            // Invoca MiMetodoDespuesDeEsperar después de 1 segundo
            Invoke("DeathActions", 1.0f);
        } 
    }

    private void DeathActions()
    {
        // Este método se llama después de esperar 1 segundo
        Debug.Log("Enemy se muere");

        //Se desactiva el enemy porque muere uu
        //Destroy(gameObject);
        enemy.SetActive(false);

    }



}
