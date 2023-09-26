using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLogic : MonoBehaviour
{
    [SerializeField] private float rayDistance;
    [SerializeField] private Transform raycastPoint;
    [SerializeField] private Aplastar aplastar_code;

    public Transform player;
    public float attackDamage = 0.5f;
    public float enragedAttackDamage = 0.7f;
    public float attackCooldown = 0.30f;
    private Rigidbody2D rb;
    private float seguirX = 1;
    

    RaycastHit2D hit;
    RaycastHit2D seguir;

    //public LayerMask attackMask;
    public bool isFlipped = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Aplastar aplastar_code = new Aplastar();
        //aplastar_code = GetComponent<Aplastar>();
        attackDamage = 0.5f;
        enragedAttackDamage = 0.7f;
    }

    // Update is called once per frame
    void Update()
    {

        hit = Physics2D.Raycast(
            transform.position,
            -transform.right,
            3f
        );

        Debug.DrawRay( // hit
            transform.position,
            transform.right * -3f,
            Color.red
        );

        seguir = Physics2D.Raycast(
            transform.position,
            -transform.right,
            10f
        );

        Debug.DrawRay( //Seguir
            transform.position,
            transform.right * -10f,
            Color.blue
        );


        LookAtPlayer();
        //Attack();

        if (seguir)
        {
            //player = seguir.collider.transform;
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

                rb.velocity = new Vector2(
                    -5f * seguirX,
                    rb.velocity.y
                );
            }
            else
            {
                rb.velocity = new Vector2(
                    0f,
                    rb.velocity.y
                );
                //LookAtPlayer();
            }
        }

        if (hit && Time.time > attackCooldown)
        {
            Debug.Log("HIT");
            Debug.Log(aplastar_code.enemyHealth);
            if(aplastar_code.enemyHealth < 250)
            {
                Debug.Log("Enraged");
                EnragedAttack();
            }
            else
            {
                Attack();
            }
        }
    }

    public void LookAtPlayer()
    {
        //Debug.Log("Mirando");
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
        player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
    }

    public void EnragedAttack()
    {
        player.GetComponent<PlayerHealth>().TakeDamage(enragedAttackDamage);
    }

}
