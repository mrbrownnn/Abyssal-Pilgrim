using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
public class butterfly : Enemy
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float weight;
    [SerializeField] protected float acceleration;
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float TimetoAttack;

    [SerializeField] private Transform SideAttackTransformButterfly; //the middle of the side attack area
    [SerializeField] private Vector2 SideAttackAreaButterfly; //how large the area of side attack is

    [SerializeField] private Transform UpAttackTransformButterfly; //the middle of the up attack area
    [SerializeField] private Vector2 UpAttackAreaButterfly; //how large the area of side attack is

    [SerializeField] private Transform DownAttackTransformButterfly; //the middle of the down attack area
    [SerializeField] private Vector2 DownAttackAreaButterfly; //how large the area of down attack is

   
    protected virtual void Update()
    {
        
    }
    protected virtual void OnCollisionEnter2D(Collision2D _other)
    {
        if (_other.gameObject.tag == "Player")
        {
            PlayerController.Instance.TakeDamage(damage);
        }
    }
    protected void BalanceForce( float maxSpeed, float acceleration, float weight)
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.right * acceleration * weight);
            // add a lift force to the butterfly
        }
        new Vector3(0, 0, 0);
    }
    protected void FixedUpdate()
    {
        BalanceForce(maxSpeed, acceleration, weight);
    }
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(SideAttackTransformButterfly.position, SideAttackAreaButterfly);
        Gizmos.DrawWireCube(UpAttackTransformButterfly.position, UpAttackAreaButterfly);
        Gizmos.DrawWireCube(DownAttackTransformButterfly.position, DownAttackAreaButterfly);
    }
    protected virtual void TimeAttack(float TimetoAttack)
    {
        Attack();
        TimetoAttack -= Time.deltaTime;
        if( TimetoAttack == 0)
        {
            Attack();
        }
        
    }
}
