using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.AI;
using Unity.VisualScripting;

public class BossController : MonoBehaviour
{
    // init variable for gameobject boss
    [Header("Boss Health")]
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject bossHealthBar;
    [SerializeField] private GameObject bossHealthBarFill;
    [SerializeField] private GameObject bossHealthBarText;
    [SerializeField] private GameObject bossHealthBarTextShadow;
    [SerializeField] private GameObject bossHealthBarTextOutline;
    [SerializeField] private GameObject bossHealthBarTextShadowOutline;
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
    void Update()
    {
        
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
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
    private void ShockWare()
    {
        // deployed skill1 here
    }
}
