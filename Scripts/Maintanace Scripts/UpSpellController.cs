using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class UpSpellController : MonoBehaviour
{
    [SerializeField] public float damage;
    [SerializeField] public float lifetime;
    [SerializeField] public float hitForce;
    // Start is called before the first frame update
    void Start()
    {
       Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.tag == "Enemy")
        {
            _other.GetComponent<Enemy>().EnemyHit(damage, (_other.transform.position - transform.position).normalized, -hitForce);
            // if any object with tag "Enemy" is hit, this object recall the Enemy.cs scripts and take the damage
        }
    }
}
