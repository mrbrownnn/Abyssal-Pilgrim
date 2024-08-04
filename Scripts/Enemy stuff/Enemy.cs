using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float recoilLength;
    [SerializeField] protected float recoilFactor;
    [SerializeField] protected bool isRecoiling = false;

    [SerializeField] protected float speed;

    [SerializeField] protected float damage;

    protected bool checkingDead = false;

    protected float recoilTimer;
    protected Rigidbody2D rb;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);

        }
        if(isRecoiling)
        {
            if(recoilTimer < recoilLength)
            {
                recoilTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }
        // this funtion is to check if the enemy is recoiling or not, if health enemy <=0; the enemy will be defeated
        // need update funtion respawn enemy after destroyed then respawn after delta time
    }

    public virtual void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        health -= _damageDone;
        if(!isRecoiling)
        {
            rb.AddForce(-_hitForce * recoilFactor * _hitDirection);
        }
        // take damage from player, if take damage from player, rigidbody enemy will pushback with hitforce
    }
    protected void OnCollisionStay2D(Collision2D _other)
    {
        if(_other.gameObject.CompareTag("Player") && !PlayerController.Instance.pState.invincible)
        {
            Attack();
            PlayerController.Instance.HitStopTime(0, 5, 0.5f);
        }
        // this funtion is checking considering the player,if player is invicible , dont take damage to player
    }
    protected virtual void Attack()
    {
        PlayerController.Instance.TakeDamage(damage);
    }

}
