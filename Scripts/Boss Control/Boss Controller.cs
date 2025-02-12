using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Microsoft.Win32.SafeHandles;
using UnityEngine.AI;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class BossController : MonoBehaviour
{
    // for global variable
    private BossStatus bS;
    private Rigidbody2D rbboss;
    private Animator animb;
    private float gravity = 10f;
    private Transform shockwaveRadius;
    public BossStatus bStatus;
    private PlayerController player;
    private SceneManager sceneManager;
    private SceneFader.FadeDirection fadeDirection;
    private LightEditor lightEditor;
    private int numAttack = 0; // need be removed in the future
    public event Action onHealthChangedCallback;
    
    [Space(10)]
    // init variable for gameobject boss
    [Header("Boss Health")]
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject bossHealthBar;
    [SerializeField] private GameObject bossHealthBarFill;
    [SerializeField] private GameObject bossHealthBarText;
    [SerializeField] private GameObject bossHealthBarTextShadow;
    [SerializeField] private GameObject bossHealthBarTextOutline;
    [SerializeField] private GameObject bossHealthBarTextShadowOutline;
    [SerializeField] private float bosshealth;
    [SerializeField] private float maxbossHealth;
    [Space(7)]


    // init variable for boss attack
    [Header("Boss Attack")]

    [SerializeField] private bool bossAttack;
    [SerializeField] private float bossAttackperSecond;
    [SerializeField] private GameObject bossAttackRange;
    [SerializeField] private float bossAttackSpeed;
    [SerializeField] private float bossAttackDamage;
    [SerializeField] private Type bossAttackDamageType;

    [Space(7)]

    [Header("Boss Movement")]
    [SerializeField] private float bossWalkSpeed;
    [SerializeField] private float bossJumpForce;
    [SerializeField] private float timetoFly;
    [Space(7)]
    // setting for skill 1
    [Header("Boss Skill1")]
    private float damageSkill1;
    private float skill1Cooldown;
    private float skill1Duration;
    private float skill1Range;
    // Weakness decrease 50% damage form player to boss
    private float weaknessDuration;
    // if player take dame form skill1, player will be slow 50% damage and speed in alpha second
    [Space(7)]

    //setting variable skill2


    [Header("Boss Skill2")]
    [SerializeField] private float damageSkill2;
    [SerializeField] private float skill2Cooldown;
    [SerializeField] private float skill2Duration;
    [SerializeField] private float skill2Range;
    [SerializeField] private float timeFreeze;
    // summon zombie
    [Space(7)]



    // setting variable skill3
    [Header("Boss Skill3")]
    [SerializeField] private float timeinvincible;
    [SerializeField] private float skill3Cooldown;
    [SerializeField] private float skill3Duration;
    [SerializeField] private int quantitySummon;
    [SerializeField] private Transform posisionSummon;
    [Space(7)]
    // summon zombie


    [ Header("Boss Position")]
    [SerializeField] private Transform BossSideAttackTransform; //the middle of the side attack area
    [SerializeField] private Vector2 BossSideAttackArea; //how large the area of side attack is
    [SerializeField] private Transform BossUpAttackTransform; //the middle of the up attack area
    [SerializeField] private Vector2 BossUpAttackArea; //how large the area of side attack is
    [SerializeField] private Transform BossDownAttackTransform; //the middle of the down attack area
    [SerializeField] private Vector2 BossDownAttackArea; //how large the area of down attack is
    [Space(7)]

    [Header("Boss Awake")]
    [SerializeField] private Transform BossAwakeside; // the middle of the side awake area
    [SerializeField] private Vector2 BossAwakeSide; // how large the area of side awake is
    [SerializeField] private Transform BossAwakeup; // the middle of the up awake area
    [SerializeField] private Vector2 BossAwakeUp; // how large the area of up awake is
    [SerializeField] private Transform BossAwakedown; // the middle of the down awake area
    [SerializeField] private Vector2 BossAwakeDown; // how large the area of down awake is
    // Update is called once per frame
    [Header("Shockwave settings")]
    [SerializeField] private float shockwaveDamage;
    [SerializeField] private float shockwaveForce;
    [SerializeField] private float shockwaveDuration;
    [SerializeField] private float shockwaveSpeed;

    [Header("Teleport settings")] 
    [SerializeField] private float teleport;
    [SerializeField] private int limitTeleport;
    [SerializeField] private float teleportSpeed;
    [SerializeField] private float teleportLength;
    
    
    
    

    void Update()
    {
        
    }
    private void OnBecameInvisible()
    {
       // take the boss will be invisible
       // dont take damage form player
       // lets the boss summon zombie
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(BossSideAttackTransform.position, BossSideAttackArea);
        Gizmos.DrawWireCube(BossUpAttackTransform.position, BossUpAttackArea);
        Gizmos.DrawWireCube(BossDownAttackTransform.position, BossDownAttackArea);
    }
    // identify the boss attack area
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(BossAwakeside.position, BossAwakeSide);
        Gizmos.DrawWireCube(BossAwakeup.position, BossAwakeUp);
        Gizmos.DrawWireCube(BossAwakedown.position, BossAwakeDown);
    }
    // identify the boss awake area
    // if player in the boss awake area, boss will attack player
    private void Awake()
    {
        bossHealthBar.SetActive(true);
        bossHealthBarFill.SetActive(true);
        bossHealthBarText.SetActive(true);
        bossHealthBarTextShadow.SetActive(true);
        bossHealthBarTextOutline.SetActive(true);
        bossHealthBarTextShadowOutline.SetActive(true);
        // get this prefab to display boss hitpoint current 
    }
    private void BossAttack()
    {
        if( CompareTag("Player") ){
            // if player in the boss awake area, boss will awake and attack player
            if (bossAttack)
            {
                if (bossAttackperSecond > 0)
                {
                    bossAttackperSecond -= Time.deltaTime;
                    numAttack++;
                    if (numAttack % 3 == 0)
                    {
                        BossDaze();
                    }
                }
                else
                {
                    bossAttackperSecond = 1;
                    bossAttackRange.SetActive(true);
                   // StartCoroutine(BossAttack());
                    // need coroutine bossattack add to animation
                }
            }
        }
    }
    private void Start()
    {
        // start, awake boss here
    }

    private void BossAwake()
    {
        // if player in the boss awake area, boss will awake and attack player
        if (CompareTag("Player"))
        {
            if (bossAttack)
            {
                if (bossAttackperSecond > 0)
                {
                    bossAttackperSecond -= Time.deltaTime;
                    numAttack++;
                    if (numAttack % 3 == 0)
                    {
                        BossDaze();
                    }
                }
                else
                {
                    bossAttackperSecond = 1;
                    bossAttackRange.SetActive(true);
                    // StartCoroutine(BossAttack());
                    // need coroutine bossattack add to animation
                }
            }
        }
    }

    IEnumerator BossDaze()
    {
        bS.teleport = false;
        yield return new WaitForSeconds(0.5f);
        animb.SetTrigger("BossDaze");
        rbboss.velocity = new Vector2();
        yield return new WaitForSeconds(1f);
        rbboss.gravityScale = 0;
        rbboss.velocity = new Vector2(transform.localPosition.x, teleportSpeed *(transform.localPosition.y));
        if (CompareTag("Grounded") == true)
        {
            Instantiate(boss, transform, false);
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerable movementBoss()
    {
        while (true)
        {
            if (bS.bossFlying)
            {
                rbboss.gravityScale = 0;
                rbboss.velocity = new Vector2(transform.localPosition.x, teleportSpeed * (transform.localPosition.y));
                if (CompareTag("Grounded") == true)
                {
                    Instantiate(boss, transform, false);
                    yield return new WaitForSeconds(1f);
                }
            }
            else
            {
                rbboss.gravityScale = gravity;
                // set default grativy =10f, can be setting on Trigger
            }
        }
    }
    public int Health
    {
        get { return (int)bosshealth; }
        set
        {
            if (bosshealth != value)
            {
                bosshealth = Mathf.Clamp(value, 0, maxbossHealth);

                if ( onHealthChangedCallback != null)
                {
                    onHealthChangedCallback.Invoke();
                    while(bosshealth <= 0)
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                        Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
                        bS.isDead = true;
                        Awake();
                    }
                }
            }
            
        }
        // for boss Awake
    }
    public void RecallAwake()
    
    {
        // change color for UX/UI boss awake
        // if player in the boss awake area, boss will awake and attack player
        void callFunnction()
        {
            /*
            bossHealthBar = new GameObject("Healthbar", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            bossHealthBar.GetComponent<Image>().color = Color.blue;
            
            bossHealthBarFill = new GameObject("Healthbar", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            bossHealthBar.GetComponent<Image>().color = Color.red;
            
            bossHealthBarText = new GameObject("Healthbar", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            bossHealthBar.GetComponent<Text>("unbelievable").color = Color.magenta;
            
            bossHealthBar = new GameObject("Healthbar", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            bossHealthBar.GetComponent<Image>().color = Color.red;
            
            bossHealthBar = new GameObject("Healthbar", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            bossHealthBar.GetComponent<Image>().color = Color.red;
            
            bossHealthBar = new GameObject("Healthbar", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            bossHealthBar.GetComponent<Image>().color = Color.red;
            
            bossHealthBar = new GameObject("Healthbar", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            bossHealthBar.GetComponent<Image>().color = Color.red;
            
            bossHealthBar = new GameObject("Healthbar", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            bossHealthBar.GetComponent<Image>().color = Color.red;
            
            */
            // need reference to the prefab
        }

        void Action()
        {
            while( onHealthChangedCallback != null)
            {
                onHealthChangedCallback.Invoke();
                while(bosshealth <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
                    bS.isDead = true;
                    Awake();
                }
            }
        }

        void skillwhenInvoke()
        {
            
        }
        // get this prefab to display boss hitpoint current 
    }
   
}
