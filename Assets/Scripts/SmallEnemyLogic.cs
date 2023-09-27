using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemyLogic : MonoBehaviour
{
    [SerializeField] private float rayDistance;
    [SerializeField] private Transform raycastPoint;

    public Transform player;
    public float attackDamage = 0.5f; // Reducido para reflejar la potencia de un enemigo pequeño
    public float attackCooldown = 0.30f;
    private Rigidbody2D rb;
    private float seguirX = 1;

    public bool dying = false;

    RaycastHit2D hit;
    RaycastHit2D seguir;

    public bool isFlipped = false;

    void Start()
    {
        attackDamage = 0.5f;
        rb = GetComponent<Rigidbody2D>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        hit = Physics2D.Raycast(transform.position, -transform.right, 3f);
        Debug.DrawRay(transform.position, transform.right * -3f, Color.red);

        seguir = Physics2D.Raycast(transform.position, -transform.right, 10f);
        //Debug.DrawRay(transform.position, transform.right * -10f, Color.blue);

        LookAtPlayer();

        if (seguir)
        {
            if (seguir.collider.name == "Player")
            {
                if (!isFlipped)
                {
                    seguirX = 1;
                }
                else
                {
                    seguirX = -1;
                }

                rb.velocity = new Vector2(-5f * seguirX, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
        }

        if (hit.collider != null && hit.collider.gameObject != null && hit.collider.gameObject.CompareTag("Player") && attackCooldown <= 0f)
        {
            Debug.Log("HIT Enemy");
            Attack();
            attackCooldown = 0.3f;
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    public void Attack()
    {
        Debug.Log("Attack small");
        player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
    }
}
