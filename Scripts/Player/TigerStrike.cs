using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerStrike : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float hitForce;
    [SerializeField] float speed;
    [SerializeField] float lifetime = 1;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
        // destroy this object after lifetime, can be set in unity
        // if lifetime is 1, it will be destroyed after 1 second
    }

    private void FixedUpdate()
    {
        transform.position += speed * transform.right;
        // transform the position of gameobject when deployed (left, right ....)
    }
    //detect hit
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.tag == "Enemy")
        {
            _other.GetComponent<Enemy>().EnemyHit(damage, (_other.transform.position - transform.position).normalized, -hitForce);
            // if any object with tag "Enemy" is hit, this object recall the Enemy.cs scripts and take the damage
        }
    }
}
