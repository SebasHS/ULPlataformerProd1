using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletLogic : MonoBehaviour
{

    private GameObject player;
    private Rigidbody2D rb;
    public float fuerza;
    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * fuerza;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);


        // Obtén el componente SpriteRenderer del objeto
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Comprueba si se encontró un SpriteRenderer
        if (spriteRenderer != null)
        {
            // Establece el Sorting Layer a "Player"
            spriteRenderer.sortingLayerName = "Player";
        }
        else
        {
            // Si no se encontró un SpriteRenderer, muestra un mensaje de error
            Debug.LogError("No se encontró un componente SpriteRenderer en este objeto.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 15)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerHealth>().TakeDamage(5f);
            Destroy(gameObject);

        }
    }
}
