using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMov : MonoBehaviour
{
    [SerializeField]
    private float runSpeed = -2f;
    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider;
    
    void Move(){
        rb.velocity = new Vector2(
            runSpeed, rb.velocity.y
        );
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
