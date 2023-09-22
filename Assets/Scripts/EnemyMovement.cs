using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float rayDistance;
    [SerializeField] private float speed = 4f;
    [SerializeField] private Transform raycastPoint;
    private UnityEngine.Vector2 direction = UnityEngine.Vector2.right;
    private Transform player;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            transform.right,
            3f
        );

        Debug.DrawRay(
            transform.position,
            transform.right * 3f,
            Color.magenta
        );

        if (hit)
        {
            player = hit.collider.transform;
            Attack();
            //Debug.Log("Collision Raycast");
        }

        if(ShouldFall())
        {
            rb.velocity = new UnityEngine.Vector2(
                0f,
                rb.velocity.y
            );
        }
    }

    void Attack()
    {
        rb.velocity = new UnityEngine.Vector2(
            speed,
            rb.velocity.y
        );
    }

    bool ShouldFall()
    {
        UnityEngine.Vector2 dir = new UnityEngine.Vector2(1f, -1f);
        RaycastHit2D hit = Physics2D.Raycast(
            raycastPoint.position,
            dir.normalized,
            2f
        );

        Debug.DrawRay(
            raycastPoint.position,
            dir.normalized * 2f,
            Color.blue
        );

        if(hit) return false;
        return true;
    }
}
