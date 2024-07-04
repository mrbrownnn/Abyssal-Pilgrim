
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.Coroutine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using UnityEditor.Experimental.GraphView;



public class PlayerControler : MonoBehaviour
{


    //define layer for animation
    [Header("Horizontal Movement Settings")]

    private Rigidbody2D rb;
    private Animator anim;
    private float xAxis, yAxis;
    private float gravity;
    [SerializeField] private float Walkspeed = 1;// default setting walkspeed=1

    [Space(7)]



    [Header(" Settings Air Jump and Buffer Jump")]

    private int JumpBufferCounter;


    [SerializeField] int JumpBufferFrame;
    [SerializeField] private int AirJumpMax;// khai bao so buoc nhay tren khong toi da co the thuc hien
    private int airJumpcounter = 0; // dem so jump tren khong
    [SerializeField] private float JumpForceZ = 45;
    [Space(7)]



    [Header("Ground Checking")]

    [SerializeField] private Transform GroundCheck;
    [SerializeField] private float groundcheckY = 0.2f;
    [SerializeField] private float groundcheckX = 0.5f;
    [SerializeField] private LayerMask Whatground;
    [Space(7)]


    /////////////////////////////////////////////////////////////////////

    [Header("Health Settings")]

    public int health;
    public int maxHealth;
    [SerializeField] GameObject bloodSpurt;
    [SerializeField] float hitFlashSpeed;
    public delegate void OnHealthChangedDelegate();
    [HideInInspector] public OnHealthChangedDelegate onHealthChangedCallback;

    float healTimer;
    [SerializeField] float timeToHeal;





    /// //////////////////////////////////////////////////////////////


    [Header("Dashing Settings")]

    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;
    private bool canDash = true;
    private bool Dashed = false;
    [SerializeField] public GameObject dashEffect;
    [Space(7)]


    [Header("Attacking Settings")]

    private bool attack = false;
    //no operation for delay attack

    [SerializeField] public GameObject AttackEffect;
    [SerializeField] private Transform SideAttackTransform, UpAttackTransform, DownAttackTransform;
    [SerializeField] private Vector2 SideAttackArea, UpAttackArea, DownAttackArea;
    [SerializeField] public LayerMask attackableLayer;
    [SerializeField] private float damage;
    [SerializeField] public GameObject slashEffect;
    bool restoreTime;
    float restoreTimeSpeed;

    [SerializeField] private float TimeBetweenAttack = 0.1f ;
    public float TimeSinceLastAttack = 0 ;
    [Space(7)]


    ///////////////////////////////////////////////////////// 
    


    [Header("Recoil Settings")]

    [SerializeField] private float recoilXSteps = 5;
    [SerializeField] private float recoilYSteps = 5;
    [SerializeField] private float recoilspeedX = 100;
    [SerializeField] private float recoilspeedY = 100;

    private float stepsXRecoiled, stepsYRecoiled;
    [Space(7)]



    [Header("Mana Settings")]
    [SerializeField] float mana;
    [SerializeField] float manaDrainSpeed;
    [SerializeField] float manaReceived;
    [SerializeField] UnityEngine.UI.Image manaStorage;
    public delegate void OnManaChangedDelegate();
    [HideInInspector] public OnManaChangedDelegate onManaChangedCallback;
    [Space(7)]

    //for gameObject and main player

    [Header("Player Skills")]
    [SerializeField] float recoilTigerStrike;
    [SerializeField] float ManaToDeployedTigerStrike;
    [SerializeField] float DumpSkill;
    [SerializeField] float ManaToDeployedDumpSkill;
    [SerializeField] float TimeRecoilDumpSkill;
    [SerializeField] float FreezeSkill;
    [SerializeField] float ManaToDeployedFreezeSkill;






    [Header("Additional Mana and Hitpoint Management")]
    [SerializeField] GameObject GameObject;

    public static PlayerControler Instance;
    public PlayerSecondary playerState;
    private SpriteRenderer sr;
    public bool isTakingDamageFromPlayer = false;

    

    //main code


    private void Awake()
    {
        if (gameObject != null && Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        Health = maxHealth;
    }


    void Start()
    {
        playerState = GetComponent<PlayerSecondary>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        gravity = rb.gravityScale;
        ManaUpdate();
    }
    void ManaUpdate()
    {
        Mana = mana;
        manaStorage.fillAmount = mana;
    }


    void OnDrawGizmos()
    // xac dinh pham vi tan cong
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(SideAttackTransform.position, SideAttackArea);
        Gizmos.DrawWireCube(UpAttackTransform.position, UpAttackArea);
        Gizmos.DrawWireCube(DownAttackTransform.position, DownAttackArea);

        //done

    }


    void Update()
    {
        GetInputs();

        UpdateJump();

        if (playerState.dashing)
        {
            return;
        }
        flip();

        Move();

        Jump();

        StartDash();

        Attack();

        RestoreTimeScale();

        FlashWhileInvincible();

        Heal();
        
    }
    private void FixedUpdate()
    {
        if(playerState.dashing) { return; }

        Recoil();
        // used to call recoil function and update recoil
    }



    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");

        yAxis = Input.GetAxisRaw("Vertical");

        attack = Input.GetButtonDown("Attack");

        // Setting up inputs for horizontal movement, jumping, dashing, and attacking
    }


    void flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y); 
            
            playerState.lookingRight = false;
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y); 

            playerState.lookingRight = true;
        }
        // setting up flip function for player
    }


    void Move()
    {
        rb.velocity = new Vector2(Walkspeed * xAxis, rb.velocity.y);
        flip();
        anim.SetBool("Walking", rb.velocity.x != 0 && Grounded());
        //setting up move function for player
    }


    private void StartDash()
    {
        if (Input.GetButtonDown("Dash") && canDash && !Dashed)
        {
            StartCoroutine(Dash());

            Dashed = true;
        }
        if (Grounded())
        {
            Dashed = false;
        }
        // check conditional for dash, if dash is true, then start dash
    }
    private IEnumerator Dash()
    {
        canDash = false;

        playerState.dashing = true;

        if (playerState.dashing)
        {
            anim.SetTrigger("Dashing");
        }
        rb.gravityScale = 0;

        rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0);

        if (dashEffect != null && Grounded())
        {

            Instantiate(dashEffect, transform);
        }

        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = gravity;

        playerState.dashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
        // add conditional for dash, it can be used only once
    }

    /// ///////////////////////////////////////////////////////////////////////

    void Attack()
    {
         TimeSinceLastAttack += Time.deltaTime;
        if( attack &&  TimeSinceLastAttack > TimeBetweenAttack)
        {

            anim.SetTrigger("Attacking");

            if (yAxis == 0 || yAxis < 0 && Grounded())
            {

                Hit(SideAttackTransform, SideAttackArea, ref playerState.recoilingX, recoilspeedX);

                Instantiate(slashEffect, SideAttackTransform);
            }

            else if (yAxis > 0)
            {
                Hit(UpAttackTransform, UpAttackArea, ref playerState.recoilingY, recoilspeedY);
                SlashEffectAtAngle(slashEffect, 80, UpAttackTransform);
            }

            else if (yAxis < 0 && !Grounded())
            {

                Hit(DownAttackTransform, DownAttackArea, ref playerState.recoilingY, recoilspeedY);
                SlashEffectAtAngle(slashEffect, -90, DownAttackTransform);



                // add yAxis conditional for attack up and down
                // hold up and down and press Z to attack up and down
            }
            TimeSinceLastAttack = 0;
        }
    }

    ///////////////////////////////////////////////////////////
    void Hit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilDir, float _recoilStrength)
    {

        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);

        if (objectsToHit.Length > 0)
        {
            _recoilDir = true;
        }
        for (int i = 0; i < objectsToHit.Length; i++)
        {
            if (objectsToHit[i].GetComponent<Enermyscripts>() != null)
            {
                objectsToHit[i].GetComponent<Enermyscripts>().EnemyHit(damage, (transform.position - objectsToHit[i].transform.position).normalized, _recoilStrength);
                //update recoil when attack

               if (objectsToHit[i].CompareTag("Enermyscripts"))
                {
                    Mana = Mana + manaReceived;
                }
          
            }
        }
    }
    void RestoreTimeScale()
    {
        if (restoreTime)
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale += Time.deltaTime * restoreTimeSpeed;
            }
            else
            {
                Time.timeScale = 1;
                restoreTime = false;
            }
        }
        // add conditional for restore time scale
    }

    /// ///////////////////////////////////////////////////////////////////////

    public void HitStopTime(float _newTimeScale, int _restoreSpeed, float _delay)
    {
        restoreTimeSpeed = _restoreSpeed;
        if (_delay > 0)
        {
            StopCoroutine(StartTimeAgain(_delay));
            StartCoroutine(StartTimeAgain(_delay));
        }
        else
        {
            restoreTime = true;
        }
        Time.timeScale = _newTimeScale;
    }
    //update hit stop time for player
    IEnumerator StartTimeAgain(float _delay)
    {
        restoreTime = true;
        yield return new WaitForSeconds(_delay);
    }


    /// ////////////////////////////////////////////////////////////////

    public void TakeDamage(float _damage)
    {
        
        Health = health - (int)_damage;
        
        StartCoroutine(StopTakingDamage());
    }
    //had scripts debug


    // ham logic
    //kiem tra va cham bang Onconsider2d
    // set co trang thai takedamage trong playersecondary bang true
    //neu bang true thi bat trang thai invicible trong stoptaking damage
      IEnumerator StopTakingDamage()
        {
            playerState.invincible = true;

            GameObject _bloodSpurtParticles = Instantiate(bloodSpurt, transform.position, Quaternion.identity);

            Destroy(_bloodSpurtParticles, 1.5f);

            anim.SetTrigger("TakeDamage");

            yield return new WaitForSeconds(1f);

            playerState.invincible = false;

            hitFlashSpeed = 0.1f;
        }
    // had problem with take damage from player

    /// /////////////////////////////////////////////////////////////////

      void FlashWhileInvincible()
    {
        sr.material.color = playerState.invincible ? Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * hitFlashSpeed, 1.0f)) : Color.white;
    }
    // add animation flash while invincible
    
    /// issue

    public int Health
    {
        get { return health;
            // checking player die or no 
            // thay doi khi tan cong quai

                }
        set
        {
            if (health != value)
            {

                health = Mathf.Clamp(value, 0, maxHealth);
                if (onHealthChangedCallback != null)
                {
                    onHealthChangedCallback.Invoke();
                    Debug.Log("Health Changed");

                }
            }
        }
        // call back hitpoint to UI heartcontroller
    }
    // health checking hitpoint not working



    protected void CheckPlayerDie()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Player is Dead");
        }
    }
    //done
    float Mana
    {
        get { return mana; }
        set
        {
            mana = Mathf.Clamp(value, 0, 1);
            Debug.Log("Mana Working");
            if(onManaChangedCallback != null)
            {
                onManaChangedCallback.Invoke();
            }
            manaStorage.fillAmount = mana / 1;
            // mana not working
            Debug.Log("Mana Has Filled");
        }
    }
    //bug with mana

    protected void TigerStrikeSkill()
    {
        if(Input.GetButtonDown("TigerStrike") && mana >= 0.5f)
        {
            anim.SetTrigger("TigerStrike");
            Mana = mana - 0.5f;
            // add tiger strike skill for player
        }
    }
    void Heal()
    {
        if (Input.GetButton("Healing") && Health < maxHealth && mana > 0  && !playerState.jumping && !playerState.dashing)
        {
            playerState.healing = true;
            anim.SetBool("Healing", true);

            //healing
            healTimer += Time.deltaTime;
            if (healTimer >= timeToHeal)
            {
                Health++;
                healTimer = 0;
            }

            mana -= Time.deltaTime * manaDrainSpeed;
        }
        else
        {
            playerState.healing = false;
            anim.SetBool("Healing", false);
            healTimer = 0;
        }
    }

    void SlashEffectAtAngle(GameObject _slashEffect, int _effectAngle, Transform _attackTransform)
    {
        _slashEffect = Instantiate(_slashEffect, _attackTransform);
        _slashEffect.transform.eulerAngles = new Vector3(0, 0, _effectAngle);
        _slashEffect.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }
    // add slash effect at angle for player attack


    /// ////////////////////////////////////////////////////////////////////////


    public bool Grounded()

    {
        if (Physics2D.Raycast(GroundCheck.position, Vector2.down, groundcheckY, Whatground)
            || Physics2D.Raycast(GroundCheck.position + new Vector3(groundcheckX, 0, 0), Vector2.down, groundcheckY, Whatground)
            || Physics2D.Raycast(GroundCheck.position + new Vector3(-groundcheckX, 0, 0), Vector2.down, groundcheckY, Whatground))
        {
            return true;
        }
        else
        {
            return false;
        }

        // check conditional for grounded
        // if raycast hit ground return true and vice versa return false
        // use to create map layer for player
    }


    /// /////////////////////////////////////////////////////////////////////////

     protected void Jump()
    {
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);


            playerState.jumping = false;

            // change jump speed when player release jump button
        }
        if (!playerState.jumping)
        {

            if (JumpBufferCounter > 0 && Grounded())
            {

                rb.velocity = new Vector3(rb.velocity.x, JumpForceZ);

                playerState.jumping = true;

            }
            else if (!Grounded() && airJumpcounter < AirJumpMax && Input.GetButton("Jump"))
            {
                playerState.jumping = true;

                airJumpcounter++;

                rb.velocity = new Vector3(rb.velocity.x, JumpForceZ);


                // add conditional for air jump
            }
        }
        anim.SetBool("Jumping", !Grounded());
        // add animation for jump
    }
    //done

    /// ///////////////////////////////////////////////////////////////////////////////

     void UpdateJump()

    {
        if (Grounded())
        {
            playerState.jumping = false;// neu tren mat dat thi dat bang 0
            airJumpcounter = 0;
        }
        if (Input.GetButtonDown("Jump"))
        {
            JumpBufferCounter = JumpBufferFrame;
        }
        else
        {
            JumpBufferCounter--;
        }
        // add jump buffer frame to jump exacly
    }
    //done


    /////////////////////////////////////////////////////////////////////////////

    void Recoil()
    {
        if (playerState.recoilingX)
        {
            if (playerState.lookingRight)
            {
                rb.velocity = new Vector2(-recoilspeedX, 0);
            }
            else
            {
                rb.velocity = new Vector2(recoilspeedX, 0);
            }
        }


        if (playerState.recoilingY)
        {
            rb.gravityScale = 0;

            if (yAxis < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, recoilspeedY);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, -recoilspeedY);
            }
            airJumpcounter = 0;
        }
        else
        {
            rb.gravityScale = gravity;
        }
        if (playerState.recoilingX && stepsXRecoiled < recoilXSteps)
        {
            stepsXRecoiled++;
        }
        else
        {
            StopRecoilX();
        }
        if (playerState.recoilingY && stepsYRecoiled < recoilYSteps)
        {
            stepsYRecoiled++;
        }
        else
        {
            StopRecoilY();
        }

        if (Grounded())
        {
            StopRecoilY();
        }
        // add recoil function for player
    }


    /// /////////////////////////////////////////////////////////////////////

    void StopRecoilX()
    {
        stepsXRecoiled = 0;

        playerState.recoilingX = false;
    }
    void StopRecoilY()
    {
        stepsYRecoiled = 0;

        playerState.recoilingY = false;
    }
    // stop recoi function for player





    /// /////////////////////////////////////////////////////////////////////
    /// SKILL FOR PLAYER
    IEnumerator TigerStrike()
    {
        anim.SetBool("TigerStrike", true);
        yield return new WaitForSeconds(0.15f);

        //side cast
        if (yAxis == 0 || (yAxis < 0 && Grounded()))
        {
            GameObject _TigerStrike = Instantiate(sideSpellFireball, SideAttackTransform.position, Quaternion.identity);

            //flip fireball
            if (playerState.lookingRight)
            {
                _TigerStrike.transform.eulerAngles = Vector3.zero; // if facing right, fireball continues as per normal
            }
            else
            {
                _TigerStrike.transform.eulerAngles = new Vector2(_TigerStrike.transform.eulerAngles.x, 180);
                //if not facing right, rotate the fireball 180 deg
            }
            playerState.recoilingX = true;
        }

        //up cast
        else if (yAxis > 0)
        {
            Instantiate(upSpellExplosion, transform);
            rb.velocity = Vector2.zero;
        }

        //down cast
        else if (yAxis < 0 && !Grounded())
        {
            downSpellTigerStrike.SetActive(true);
        }

        Mana -= manaSpellCost;
        yield return new WaitForSeconds(0.35f);
        anim.SetBool("Casting", false);
        playerState.TigerStrikeSkill = false;
    }
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, downSpellTigerStrike);
        Gizmos.DrawWireSphere(transform.position, sideSpellTiger);
        Gizmos.DrawWireSphere (transform.position, upSpellExplosion);
        // default setting for tiger strike skill in unity
        // need setting ingame
    }
    protected void DumpSkillDeployed()
    {
        if( Input.GetButton("DumpSkill") && !Grounded() && Mana>= ManaToDeployedDumpSkill && TimeRecoilDumpSkill==0)
        {
            new Vector2(transform.position.x*10 , transform.position.y * 10);
            if(playerState.lookingRight)
            {
                rb.velocity = new Vector2(10, 0);
            }
            else
            {
                rb.velocity = new Vector2(-10, 0);
            }
            if(playerState.lookingRight)
            {
                Instantiate(dumpSkill, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(dumpSkill, transform.position, Quaternion.Euler(0, 0, 180));
            }
            if(CompareTag("Enermyscripts"))
            {
                Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);
            }
            TimeRecoilDumpSkill = TimeRecoilDumpSkill - Time.deltaTime;
        }
    }

public class FreezeSkill : MonoBehaviour
{
   [SerializeField] public float freezeDuration = 2.0f;
   [SerializeField] public float stunDuration = 3.0f;
    [SerializeField] public float damageMultiplier = 1.5f;
    [SerializeField] public float cooldown = 10.0f;

    private bool isOnCooldown = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isOnCooldown)
                // press F to de
        {
            Freeze();
        }

        if (isOnCooldown)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0.0f)
            {
                isOnCooldown = false;
            }
        }
    }

    private void Freeze()
    {
        // Lấy tất cả quái vật trong tầm nhìn
        Collider[] enemies = Physics.OverlapSphere(transform.position, 10.0f, LayerMask.GetMask("Enemies"));

        // Kích hoạt hiệu ứng đóng băng
        foreach (Collider enemy in enemies)
        {
            enemy.GetComponent<Enemy>().Freeze(freezeDuration);
        }

        // Kích hoạt hiệu ứng tê tái
        Invoke("StunEnemies", freezeDuration);

        // Kích hoạt cơ chế phản sát thương
        Invoke("ReflectDamage", freezeDuration + stunDuration);

        // Giảm thời gian hồi chiêu của các kỹ năng khác
        ReduceCooldown();

        isOnCooldown = true;
    }

    private void StunEnemies()
    {
        // Lấy tất cả quái vật trong tầm nhìn
        Collider[] enemies = Physics.OverlapSphere(transform.position, 10.0f, LayerMask.GetMask("Enemies"));

        // Kích hoạt hiệu ứng tê tái
        foreach (Collider enemy in enemies)
        {
            setbool(Enemy.State)= true;
            enemy.GetComponent<Enemy>().Stun(stunDuration);
            if( Enemy.CompareTag("Enermyscripts"))
            {
                CompareTag("Enemy").GetComponent<Animation> = 0
                SetAnim(0f);
                TimeDeployed -= Time.deltaTime;
                if( TimeDeployed <= 0){
                    SetAnim("Frezze") = 0.5f;
                    EnemyState.Frezze = false;
                }
                }
        }
    }

    private void ReflectDamage()
    {
        private float percentReflectDamage = async[];
        for( int i=0; i<= maxlevel; i++){
            percentReflectDamage = percentReflectDamage + deltaPercent;
        }
        Collider[] enemies = Physics.OverlapSphere(transform.position, 10.0f, LayerMask.GetMask("Enemies"));
        
        foreach (Collider enemy in enemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damageMultiplier * enemy.GetComponent<Enemy>().damage);
        }
    }

    private void ReduceCooldown()
    {

        Skill[] skills = GetComponentsInChildren<Skill>();

        foreach (Skill skill in skills)
        {
            if (skill != this)
            {
                skill.cooldown *= 0.5f;
            }
        }
    }
}

public class Enemy : MonoBehaviour
{
    public float health = 100.0f;
    public float damage = 10.0f;

    public void Freeze(float duration)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Animator>().enabled = false;
       



        Invoke("Unfreeze", duration);
    }

    private void Unfreeze()
    {

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Animator>().enabled = true;
    }

    public void Stun(float duration)
    {
        GetComponent<Animator>().speed *= 0.5f;

        Invoke("Unstun", duration);
    }
    // cơ chế skill freeze and stun cho player, quái vật chịu ảnh hưởng sẽ có khả năng bị đóng băng và tê tái, gây sát thương lên player sẽ phản sát thương

}



