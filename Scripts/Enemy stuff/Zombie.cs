using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    // Start is called before the first frame update
   protected override void Start()
    {
        rb.gravityScale = 12f;        
    }
    // init gravity scale rigidbody
    protected override void Awake()
    {
        base.Awake();
    }
   /* protected override void StartAwake(){
        if(HealthBar != null)
        {
            HealthBar.SetMaxHealth(health);
        }
        StartAwake();
    }
   */
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(!isRecoiling)
        {
            transform.position = Vector2.MoveTowards
                (transform.position, new Vector2(PlayerController.Instance.transform.position.x, transform.position.y),
                speed * Time.deltaTime);
        }
        // move towards player, need neft, can be neft sp= 0.7*speedplayer
    }
    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce);
        // this function take recall the hitpoint of enemy, so the enemy can be destroyed when hitpoint <=0
    }

}
