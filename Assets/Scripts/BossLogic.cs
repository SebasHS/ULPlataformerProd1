using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLogic : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float rayDistance;
    [SerializeField] private Transform raycastPoint;

    public Transform player;
    public int attackDamage = 20;
    public int enragedAttackDamage = 40;
    public Vector3 attackOffset;
    public float attackRange = 30f;
    private Rigidbody2D rb;
    private float seguirX = 1;

    RaycastHit2D hit;
    RaycastHit2D seguir;

    //public LayerMask attackMask;
    public bool isFlipped = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hit = Physics2D.Raycast(
            transform.position,
            -transform.right,
            3f
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

        Debug.DrawRay( // hit
            transform.position,
            transform.right * -3f,
            Color.red
        );
        
    }

    // Update is called once per frame
    void Update()
    {

        hit = Physics2D.Raycast(
            transform.position,
            -transform.right,
            3f
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

        Debug.DrawRay( // hit
            transform.position,
            transform.right * -3f,
            Color.red
        );

        LookAtPlayer();
        //Attack();
        
        if (seguir)
        {
            //player = seguir.collider.transform;
            if (seguir.collider.name == "Player")
            {
                if(!isFlipped)
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

        if (hit)
        {
            Attack();
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
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;
        //Debug.Log("POS :: " + pos);

        //Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        //Debug.Log("Attack mask: " + attackMask);
    }

    public void EnragedAttack()
    {
        /*Vector3 pos = transform.position;
		pos += transform.right * attackOffset.x;
		pos += transform.up * attackOffset.y;

		Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
		if (colInfo != null)
		{
			colInfo.GetComponent<PlayerHealth>().TakeDamage(enragedAttackDamage);
		}*/
    }

}
