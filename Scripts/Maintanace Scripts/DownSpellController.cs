using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownSpellController : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float hitForce;
    [SerializeField] float speed;
    [SerializeField] float lifetime = 1;

    // Start is called before the first frame update
    // need maintainace scripts
    public void Start()
    {
        /*
        if(PlayerController.Instance.GetComponent<Rigidbody2D>().GetComponentsInChildren<Transform>().Length > 0)
        {
            transform.position = PlayerController.Instance.GetComponent<Rigidbody2D>().GetComponentsInChildren<Transform>()[0].position;
        }
        else
        {
            transform.position = PlayerController.Instance.transform.position;
        }
        PlayerControll.AudioSettings.Instance.PlaySound("DownSpell");
        Destroy(gameObject, lifetime);
        */
    }

    private void FixedUpdate()
    {
         // transform.position += speed * transform.right;
    }
    //detect hit
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.tag == "Enemy")
            // if other tag Enemy, call enemyhit and can be destroyed enemy normally but doesnt cost hitpoint
        {
            _other.GetComponent<Enemy>().EnemyHit(damage, (_other.transform.position - transform.position).normalized, -hitForce);
        }
    }
}
