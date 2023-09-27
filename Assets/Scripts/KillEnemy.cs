using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpOnEnemy : MonoBehaviour
{
    // Altura mínima desde la cual el jugador puede "aplastar" al enemigo.
    public float minimumHeight = 1f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Si el jugador cae desde una altura suficiente...
            if (rb.velocity.y <= 0 && transform.position.y - minimumHeight > collision.transform.position.y)
            {
                // Destruye el enemigo
                Destroy(collision.gameObject);
            }
        }
    }
}
